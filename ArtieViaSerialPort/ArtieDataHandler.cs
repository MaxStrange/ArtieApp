using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Search;
using SearchAbstractDataTypes;
using ActionSet;
using PerceptionSets;

namespace ArtieViaSerialPort
{
    public class ArtieDataHandler : IDataHandler
    {
//Private fields
        private ArtieMonitor artieMonitor = null;
        private ICalibrationRequester _calibrationRequester = null;
        private ICalibrationRequester calibrationRequester
        {
            get { return this._calibrationRequester; }
            set { this._calibrationRequester = value; }
        }
        private Queue<FormattedActionData> _compoundAction = new Queue<FormattedActionData>();
        private Queue<FormattedActionData> compoundAction
        {
            get { return this._compoundAction; }
            set { this._compoundAction = value; }
        }


//Constructors
        public ArtieDataHandler()
        {
            this.artieMonitor = new ArtieMonitor();
        }

        public ArtieDataHandler(ICalibrationRequester calibrationRequester, SerialPortController spController)
        {
            this.calibrationRequester = calibrationRequester;
            this.artieMonitor = new ArtieMonitor(null, spController);
        }

        public ArtieDataHandler(ICalibrationRequester calibrationRequester, 
            ISearchUpdate parentUI, SerialPortController spController, Sequence initialSequence)
        {
            this.calibrationRequester = calibrationRequester;
            this.artieMonitor = new ArtieMonitor(parentUI, spController, initialSequence);
        }


//Public methods
        public void receiveData(object receivedData)
        {
            decideWhatToDoWithDataBasedOnType(receivedData);
        }



//Private methods
        private void dealWithDataAsQueue(Queue<FormattedActionData> dataAsCompoundAction)
        {
            if (dataAsCompoundAction.Count <= 0)
                return;

            FormattedActionData nextActionInCompoundAction = dataAsCompoundAction.Peek();
            if (nextActionInCompoundAction.actionChar == ActionMap.SerialChars_Orders.getCalibrationDataFromBody)
            {
                //Hand off the calibration data to the requester. Then dump it.
                this.calibrationRequester.calibrationData = new CalibrationData(nextActionInCompoundAction.distanceTicks);
                return;
            }
            else if (nextActionInCompoundAction.actionChar == ActionMap.SerialChars_Orders.getCalibrationDataFromArm)
            {
                //Hand off the calibration data to the requester. Then dump it.
                this.calibrationRequester.calibrationData = new CalibrationData(nextActionInCompoundAction.distanceTicks);
                return;
            }
            else
            {
                this.compoundAction = dataAsCompoundAction;
                releaseData();
            }
        }

        private void decideWhatToDoWithDataBasedOnType(object data)
        {
            Queue<FormattedActionData> dataAsCompoundAction;
            if (data is PotValue)
            {
                return;
            }
            else if (data is Queue<FormattedActionData>)
            {
                dataAsCompoundAction = (data as Queue<FormattedActionData>);
                dealWithDataAsQueue(dataAsCompoundAction);
            }
            else if (data is string)
            {
                //data has already been printed. Just dump it.
                return;
            }
            else
            {
                throw new UnrecognizedDataException();
            }
        }
        
        private SearchNode compute(FormattedActionData data, SearchNode node)
        {
            switch (data.actionChar)
            {
                case ActionMap.ActionChars.BodyChars.driveBackwards:
                    node = this.artieMonitor.driveBackwards(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.BodyChars.driveForwards:
                    node = this.artieMonitor.driveForwards(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.BodyChars.turnTightLeft:
                    node = this.artieMonitor.turnTightLeft(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.BodyChars.turnTightRight:
                    node = this.artieMonitor.turnTightRight(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.BodyChars.turnWideLeft:
                    node = this.artieMonitor.turnWideLeft(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.BodyChars.turnWideRight:
                    node = this.artieMonitor.turnWideRight(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.ArmChars.driveAClockWise:
                    node = this.artieMonitor.driveJointA(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.ArmChars.driveACounterClockWise:
                    node = this.artieMonitor.driveJointABack(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.ArmChars.driveBClockWise:
                    node = this.artieMonitor.driveJointB(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.ArmChars.driveBCounterClockWise:
                    node = this.artieMonitor.driveJointBBack(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.ArmChars.driveCClockWise:
                    node = this.artieMonitor.driveJointC(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.ArmChars.driveCCounterClockWise:
                    node = this.artieMonitor.driveJointCBack(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.ArmChars.driveDClockWise:
                    node = this.artieMonitor.driveJointD(new CalibrationData(data.distanceTicks));
                    break;
                case ActionMap.ActionChars.ArmChars.driveDCounterClockWise:
                    node = this.artieMonitor.driveJointDBack(new CalibrationData(data.distanceTicks));
                    break;
            };
            return node;
        }

        private void releaseData()
        {
            SearchNode nextState = new SearchNode();
            DistanceTick distanceTicks = new DistanceTick(DistanceTick.neutralValue);
            foreach (FormattedActionData action in this.compoundAction)
            {
                nextState = compute(action, nextState);
                distanceTicks = action.distanceTicks;
            }

            //Now the compound action has been completed, check if the resultant state
            //is correct or if it needs corrections.
            this.artieMonitor.compare(nextState, distanceTicks);
        }
    }
}
