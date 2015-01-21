using Search;
using SearchAbstractDataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulStaticMethods;
using ActionSet;
using PerceptionSets;

namespace ArtieViaSerialPort
{
    internal class PotProtocolSlave
    {
        private bool _expectPotValue = false;
        internal bool expectPotValue
        {
            get { return this._expectPotValue; }
            set { this._expectPotValue = value; }
        }
        private char _potID = 'U';
        internal char potID
        {
            get
            {
                switch (this._potID)
                {
                    case 'U':
                        this._potID = 'A';
                        return this._potID;
                    case 'A':
                        this._potID = 'B';
                        return this._potID;
                    case 'B':
                        this._potID = 'C';
                        return this._potID;
                    case 'C':
                        this._potID = 'D';
                        return this._potID;
                    case 'D':
                        this._potID = 'E';
                        return this._potID;
                    case 'E':
                        this._potID = 'A';
                        return this._potID;
                    default:
                        return this._potID;
                };
            }
        }
        private int _potIndexForPrintingPotValues = 0;
        internal int potIndexForPrintingPotValues
        {
            get { return this._potIndexForPrintingPotValues; }
            set
            {
                if (value > 4)
                    value = 0;
                this._potIndexForPrintingPotValues = value;
            }
        }
    }

    internal class NumberProtocolSlave
    {
        private StringBuilder _incomingNumber = new StringBuilder(0);
        internal StringBuilder incomingNumber
        {
            get { return this._incomingNumber; }
            set { this._incomingNumber = value; }
        }
        private int _numberOfTimesPassHasBeenSeen = 0;
        internal int numberOfTimesPassHasBeenSeen
        {
            get { return this._numberOfTimesPassHasBeenSeen; }
            set { this._numberOfTimesPassHasBeenSeen = value; }
        }
        internal const int numberOfTimesPassShouldBeSeen = 3;
        internal PotProtocolSlave potSlave = new PotProtocolSlave();
        private bool _treatIncomingDataAsNumber = false;
        internal bool treatIncomingDataAsNumber
        {
            get { return this._treatIncomingDataAsNumber; }
            set { this._treatIncomingDataAsNumber = value; }
        }

        internal bool expectPotValue
        {
            get { return this.potSlave.expectPotValue; }
            set { this.potSlave.expectPotValue = value; }
        }

        internal void appendDataAndAllFalsePassesToIncomingNumber(RawData dataAsRaw)
        {
            if (this.numberOfTimesPassHasBeenSeen > 0)
                appendAllFalsePassesToIncomingNumber();

            this.numberOfTimesPassHasBeenSeen = 0;

            char dataChar = dataAsRaw.toChar();

            this.incomingNumber.Append(dataChar);
        }

        internal object collectFirstHalfOfPasses()
        {
            this.numberOfTimesPassHasBeenSeen++;

            if (this.numberOfTimesPassHasBeenSeen >= NumberProtocolSlave.numberOfTimesPassShouldBeSeen)
            {
                this.treatIncomingDataAsNumber = true;
                this.numberOfTimesPassHasBeenSeen = 0;
            }
            return null;
        }


        private void appendAllFalsePassesToIncomingNumber()
        {
            for (int i = 0; i < this.numberOfTimesPassHasBeenSeen; i++)
            {
                this.incomingNumber.Append(int.Parse(ActionMap.SerialChars_CommunicationProtocol.formatFlag_Number.ToString()));
            }
        }
    }

    internal class FormattedActionDataProtocolSlave
    {
        private Queue<FormattedActionData> _dataQueue = new Queue<FormattedActionData>();
        internal Queue<FormattedActionData> dataQueue
        {
            get { return this._dataQueue; }
            set { this._dataQueue = value; }
        }
        private bool _distanceTicksAreFromArm = false;
        internal bool distanceTicksAreFromArm
        {
            get { return this._distanceTicksAreFromArm; }
            set { this._distanceTicksAreFromArm = value; }
        }
        private char _pendingActionIdentifier;
        internal char pendingActionIdentifier
        {
            get { return this._pendingActionIdentifier; }
            set { this._pendingActionIdentifier = value; }
        }
        private DistanceTick _pendingDistanceTicks = new DistanceTick(DistanceTick.neutralValue);
        internal DistanceTick pendingDistanceTicks
        {
            get { return this._pendingDistanceTicks; }
            set { this._pendingDistanceTicks = value; }
        }
        private bool _waitingForCalibrationData_Arm = false;
        internal bool waitingForCalibrationData_Arm
        {
            get { return this._waitingForCalibrationData_Arm; }
            set { this._waitingForCalibrationData_Arm = value; }
        }
        private bool _waitingForCalibrationData_Body = false;
        internal bool waitingForCalibrationData_Body
        {
            get { return this._waitingForCalibrationData_Body; }
            set { this._waitingForCalibrationData_Body = value; }
        }

        internal object formatData(int value)
        {
            if (this.distanceTicksAreFromArm)
                this.pendingDistanceTicks = (DistanceTickFromArm)value;
            else
                this.pendingDistanceTicks = (DistanceTickFromBody)value;

            return nullOrPackageDataAndQueueIt();
        }

        internal object formatEndOfActionData()
        {
            this.pendingDistanceTicks.setValueToNeutral();
            Queue<FormattedActionData> dataToReturn = this.dataQueue.DeepClone();
            this.dataQueue.Clear();
            return dataToReturn;
        }

        internal object waitOrFormatEndOfActionData(char actionID)
        {
            if (actionID == ActionMap.SerialChars_CommunicationProtocol.frameEndChar)
                return this.formatEndOfActionData();

            if (this.waitingForCalibrationData_Arm)
            {
                this.pendingActionIdentifier = ActionMap.SerialChars_Orders.getCalibrationDataFromArm;
                this.waitingForCalibrationData_Arm = false;
                return null;
            }
            else if (this.waitingForCalibrationData_Body)
            {
                this.pendingActionIdentifier = ActionMap.SerialChars_Orders.getCalibrationDataFromBody;
                this.waitingForCalibrationData_Body = false;
                return null;
            }
            else
            {
                this.pendingActionIdentifier = actionID;
                return null;
            }
        }

        private object nullOrPackageDataAndQueueIt()
        {
            if (!this.pendingDistanceTicks.valueIsNeutral())
                packageDataAndQueueIt();

            return null;
        }

        private void packageDataAndQueueIt()
        {
            FormattedActionData formattedData = new FormattedActionData(this.pendingActionIdentifier, this.pendingDistanceTicks);
            FormattedActionData formattedDataToQueue = formattedData.DeepClone();
            this.dataQueue.Enqueue(formattedDataToQueue);
            this.pendingDistanceTicks.setValueToNeutral();
        }
    }

    public class ArtieCommunicationProtocol : IProtocol
    {
//Private fields
        //TODO : refactor into different behaviors: arm behavior and body behavior
        private FormattedActionDataProtocolSlave formattedActionDataSlave = new FormattedActionDataProtocolSlave();
        private NumberProtocolSlave numberSlave = new NumberProtocolSlave();   


//Public methods
        public object formatDataReceived(RawData dataAsRaw)
        {
            //TODO : see what you can refactor into the RawData class

            char dataChar = dataAsRaw.toChar();//for debug
            
            if (this.numberSlave.treatIncomingDataAsNumber)
                return dealWithDataAsANumber(dataAsRaw);
            else if (dataAsRaw.dataIsFormatFlag_Number())
                return this.numberSlave.collectFirstHalfOfPasses();
            else
                this.numberSlave.numberOfTimesPassHasBeenSeen = 0;

            if (dataAsRaw.dataIsCalibrateArmSignal())
            {
                Console.Out.WriteLine("Calibration signal received: " + dataAsRaw.toChar());
                this.formattedActionDataSlave.waitingForCalibrationData_Arm = true;
                this.formattedActionDataSlave.distanceTicksAreFromArm = true;
                return this.formattedActionDataSlave.waitOrFormatEndOfActionData(dataAsRaw.toChar());
            }
            else if (dataAsRaw.dataIsCalibrateBodySignal())
            {
                Console.Out.WriteLine("Calibration signal received: " + dataAsRaw.toChar());
                this.formattedActionDataSlave.waitingForCalibrationData_Body = true;
                this.formattedActionDataSlave.distanceTicksAreFromArm = false;
                return this.formattedActionDataSlave.waitOrFormatEndOfActionData(dataAsRaw.toChar());
            }
            else if (dataAsRaw.dataIsConnectedSignal())
            {
                Console.Out.WriteLine("Connected signal received: " + dataAsRaw.toChar());
                return "Connected!";
            }
            else if (dataAsRaw.dataIsPotValueSignal())
            {
                Console.Out.WriteLine("Incoming Pot value.");
                this.numberSlave.expectPotValue = true;
                return null;
            }
            else if (dataAsRaw.dataIsActionCharID_Arm())
            {
                Console.Out.WriteLine("Action char_arm received: " + dataAsRaw.toChar());
                this.formattedActionDataSlave.distanceTicksAreFromArm = true;
                return this.formattedActionDataSlave.waitOrFormatEndOfActionData(dataAsRaw.toChar());
            }
            else if (dataAsRaw.dataIsActionCharID_Body())
            {
                Console.Out.WriteLine("Action char_body received: " + dataAsRaw.toChar());
                this.formattedActionDataSlave.distanceTicksAreFromArm = false;
                return this.formattedActionDataSlave.waitOrFormatEndOfActionData(dataAsRaw.toChar());
            }
            else
            {
                Console.Out.WriteLine("Unkown data received: " + dataAsRaw.toChar());
                throw new UnrecognizedDataException();
            }
        }

        public string formatDataToSend(object data)
        {
            return (data as string);
        }

        public string handShakeSend()
        {
            string handShake = ActionMap.SerialChars_CommunicationProtocol.idTestArduino.ToString();
            return handShake;
        }

        public bool handShakeReceive(char data)
        {
            if (data == 'x')
            {
                return true;
            }
            else if (data == 'y')
            {
                return true;
            }
            else
            {
                return false;
            }
        }


//Private methods
        private object collectPassesAndMaybePackageAndSendNumber()
        {
            this.numberSlave.numberOfTimesPassHasBeenSeen++;
            if (this.numberSlave.numberOfTimesPassHasBeenSeen >= NumberProtocolSlave.numberOfTimesPassShouldBeSeen)
                return packageAndSendDataAsInt();
            else
                return null;
        }
        
        private object dealWithDataAsANumber(RawData dataAsRaw)
        {
            if (dataAsRaw.dataIsFormatFlag_Number())
            {
                return collectPassesAndMaybePackageAndSendNumber();
            }
            else
            {
                this.numberSlave.appendDataAndAllFalsePassesToIncomingNumber(dataAsRaw);
                return null;
            }
        }

        private object packageAndSendDataAsInt()
        {
            int number;
            string snumber = this.numberSlave.incomingNumber.ToString();
            this.numberSlave.incomingNumber.Clear();
            int inumber;
            if (int.TryParse(snumber, out inumber))
                number = inumber;
            else
                number = 0;
            Console.Out.WriteLine("Int received: " + number);
            this.numberSlave.treatIncomingDataAsNumber = false;
            this.numberSlave.numberOfTimesPassHasBeenSeen = 0;

            return returnStringOrFormatDataAsNumber(number);
        }

        private object returnStringOrFormatDataAsNumber(int dataAsNumber)
        {
            if (this.numberSlave.expectPotValue)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Pot ").Append(this.numberSlave.potSlave.potIndexForPrintingPotValues);
                sb.Append(" Value: ").Append(dataAsNumber);
                this.numberSlave.potSlave.potIndexForPrintingPotValues++;

                string strSbAsString = sb.ToString();
                this.numberSlave.incomingNumber.Clear();

                this.numberSlave.expectPotValue = false;

                System.Console.Out.WriteLine(strSbAsString);

                return new PotValue(dataAsNumber, this.numberSlave.potSlave.potID);
            }
            else
                return this.formattedActionDataSlave.formatData(dataAsNumber);
        }
    }
}
