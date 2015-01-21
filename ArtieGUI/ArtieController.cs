using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Search;
using UsefulStaticMethods;
using System.Threading;
using ArtieViaSerialPort;
using SearchAbstractDataTypes;
using ActionSet;
using PerceptionSets;

namespace ArtieGUI
{
    /// <summary>
    /// ArtieController is a delegate class - it interfaces with the control panel controller
    /// whenever contact with Artie is required. To contact Artie, ArtieController uses
    /// a slave class, ArtieControllerToSerialPortSlave.
    /// </summary>
    public class ArtieController
    {
//internal fields
        private bool _arduinoConnected = false;
        internal bool arduinoConnected
        {
            get { return this._arduinoConnected; }
            set { this._arduinoConnected = value; }
        }
        private IAppendToDataLog _gui = null;
        internal IAppendToDataLog gui
        {
            get { return this._gui; }
            private set { this._gui = value; }
        }
        private ArtieControllerToSerialPortSlave _serialPortSlave = null;
        internal ArtieControllerToSerialPortSlave serialPortSlave
        {
            get { return this._serialPortSlave; }
            set { this._serialPortSlave = value; }
        }
        /// <summary>
        /// This is Artie's actual perception as the computer understands it. It changes
        /// everytime Artie is moved or rotated etc. It reflects actual sensor data if such
        /// data is available, if not, default or calibration data is used to calculate
        /// the perception.
        /// </summary>
        private SearchNode _perception_Actual = null;
        internal SearchNode perception_Actual
        {
            get { return this._perception_Actual; }
            set { this._perception_Actual = value; }
        }

        

//Constructors
        internal ArtieController(IAppendToDataLog gui)
        {
            this.gui = gui;
            this.serialPortSlave = new ArtieControllerToSerialPortSlave(this);
            this.perception_Actual = new SearchNode();
        }


//Internal Methods
        internal void close()
        {
            setArmSafety();
            this.serialPortSlave.close();
        }

        internal void closeGripper()
        {
            string portWrite = ActionMap.ActionChars.ArmChars.closeGripper.ToString();
            writeToPort(portWrite);
        }

        internal void connectToArduino()
        {
            this.serialPortSlave.connectToArduino();
        }

        internal void disconnectFromArduino()
        {
            this.serialPortSlave.disconnectFromArduino();
        }

        internal void driveBackwards()
        {
            string portWrite = ActionMap.ActionChars.BodyChars.driveBackwards.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.DRIVE_BACKWARDS);
        }

        internal void driveForwards()
        {
            string portWrite = ActionMap.ActionChars.BodyChars.driveForwards.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.DRIVE_FORWARDS);
        }

        internal void driveJointAClockwise()
        {
            string portWrite = ActionMap.ActionChars.ArmChars.driveAClockWise.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.DRIVE_A_CLOCKWISE);
        }

        internal void driveJointACounterClockwise()
        {
            string portWrite = ActionMap.ActionChars.ArmChars.driveACounterClockWise.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.DRIVE_A_COUNTERCLOCKWISE);
        }

        internal void driveJointBClockwise()
        {
            string portWrite = ActionMap.ActionChars.ArmChars.driveBClockWise.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.DRIVE_B_CLOCKWISE);
        }

        internal void driveJointBCounterClockwise()
        {
            string portWrite = ActionMap.ActionChars.ArmChars.driveBCounterClockWise.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.DRIVE_B_COUNTERCLOCKWISE);
        }

        internal void driveJointCClockwise()
        {
            string portWrite = ActionMap.ActionChars.ArmChars.driveCClockWise.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.DRIVE_C_CLOCKWISE);
        }

        internal void driveJointCCounterClockwise()
        {
            string portWrite = ActionMap.ActionChars.ArmChars.driveCCounterClockWise.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.DRIVE_C_COUNTERCLOCKWISE);
        }

        internal void driveJointDClockwise()
        {
            string portWrite = ActionMap.ActionChars.ArmChars.driveDClockWise.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.DRIVE_D_CLOCKWISE);
        }

        internal void driveJointDCounterClockwise()
        {
            string portWrite = ActionMap.ActionChars.ArmChars.driveDCounterClockWise.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.DRIVE_D_COUNTERCLOCKWISE);
        }

        internal void getPotValues()
        {
            string portWrite = ActionMap.SerialChars_Orders.getPotValues.ToString();
            writeToPort(portWrite);
        }

        internal void openGripper()
        {
            string portWrite = ActionMap.ActionChars.ArmChars.openGripper.ToString();
            writeToPort(portWrite);
        }

        internal void recalibrate()
        {
            string portWrite = ActionMap.SerialChars_Orders.getCalibrationDataFromBody.ToString();
            writeToPort(portWrite);
        }

        internal void removeArmSafetyForCalibration()
        {
            string portWrite = ActionMap.SerialChars_Orders.removeArmSafetyForCalibration.ToString();
            writeToPort(portWrite);
        }

        internal void retrieveMemory()
        {
            string portWrite = ActionMap.SerialChars_Orders.getFreeMemory.ToString();
            writeToPort(portWrite);
        }

        internal void requestCalibrationData()
        {
            string portWrite = ActionMap.SerialChars_Orders.getCalibrationDataFromBody.ToString();
            writeToPort(portWrite);
        }

        internal void sendSolution(Sequence solutionSequence, ISearchUpdate parentUI)
        {
            this.serialPortSlave.sendSolution(solutionSequence, parentUI);
        }

        internal void setArmSafety()
        {
            string portWrite = ActionMap.SerialChars_Orders.setArmSafety.ToString();
            writeToPort(portWrite);
        }

        internal bool testArduinoConnection()
        {
            return this.serialPortSlave.testArduinoConnection();
        }

        internal void turnTightLeft()
        {
            string portWrite = ActionMap.ActionChars.BodyChars.turnTightLeft.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.TURN_TIGHT_LEFT);
        }

        internal void turnTightRight()
        {
            string portWrite = ActionMap.ActionChars.BodyChars.turnTightRight.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.TURN_TIGHT_RIGHT);
        }

        internal void turnWideLeft()
        {
            string portWrite = ActionMap.ActionChars.BodyChars.turnWideLeft.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.TURN_WIDE_LEFT);
        }

        internal void turnWideRight()
        {
            string portWrite = ActionMap.ActionChars.BodyChars.turnWideRight.ToString();
            writeToPort(portWrite);
            this.perception_Actual = this.perception_Actual.deriveDaughterNode(ElementaryAction.TURN_WIDE_RIGHT);
        }

        internal void stopDriving()
        {
            string portWrite = ActionMap.ActionChars.stopDriving.ToString();
            writeToPort(portWrite);
        }


//Private Methods
        private void writeToPort(string portWrite)
        {
            this.serialPortSlave.writeToPort(portWrite);
        }
    }
}
