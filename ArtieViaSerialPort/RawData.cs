using ActionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulStaticMethods;

namespace ArtieViaSerialPort
{
    /// <summary>
    /// Represents raw data drawn from the serial port one char at a time.
    /// !Important! This class is designed to be immutable!
    /// </summary>
    public class RawData
    {
        private int _data = 0;
        private int data
        {
            get { return this._data; }
            set { this._data = value; }
        }
        private char dataAsChar
        {
            get { return CharMethods.convertIntToChar(this.data); }
        }



//Constructors
        internal RawData(int data)
        {
            this.data = data;
        }


        
//Public methods        
        public bool dataIsActionCharID_Arm()
        {
            if (ActionMap.ActionChars.ArmChars.armChars.Contains<char>(this.dataAsChar)
                || (this.dataAsChar == ActionMap.SerialChars_CommunicationProtocol.frameEndChar))
                return true;
            else
                return false;
        }

        public bool dataIsActionCharID_Body()
        {
            if (ActionMap.ActionChars.BodyChars.bodyChars.Contains<char>(this.dataAsChar)
                || (this.dataAsChar == ActionMap.SerialChars_CommunicationProtocol.frameEndChar))
                return true;
            else
                return false;
        }

        public bool dataIsCalibrateArmSignal()
        {
            if (this.dataAsChar == ActionMap.SerialChars_Orders.getCalibrationDataFromArm)
                return true;
            else
                return false;
        }

        public bool dataIsCalibrateBodySignal()
        {
            if (this.dataAsChar == ActionMap.SerialChars_Orders.getCalibrationDataFromBody)
                return true;
            else
                return false;
        }

        public bool dataIsConnectedSignal()
        {
            if (this.dataAsChar == 'X')
                return true;
            else
                return false;
        }

        public bool dataIsFormatFlag_Number()
        {
            if (this.dataAsChar == ActionMap.SerialChars_CommunicationProtocol.formatFlag_Number)
                return true;
            else
                return false;
        }

        public bool dataIsInt()
        {
            if (this.dataAsChar >= 0)
                return true;
            else
                return false;
        }

        public bool dataIsPotValueSignal()
        {
            if (this.dataAsChar == 'Y')
                return true;
            else
                return false;
        }

        public bool dataIsVerticalTabChar()
        {
            if (this.dataAsChar == '\v')
                return true;
            else
                return false;
        }

        public char toChar()
        {
            if (dataIsVerticalTabChar())
                return (char)0;
            else
                return CharMethods.convertIntToChar(this.data);
        }

        public override string ToString()
        {
            return this.dataAsChar.ToString();
        }
    }
}
