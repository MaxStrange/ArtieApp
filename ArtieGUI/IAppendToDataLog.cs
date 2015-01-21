using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;

namespace ArtieViaSerialPort
{
    public interface IAppendToDataLog
    {
        void appendToDataLog(string text);

        void appendToDebugLog_ToArduino(string text);

        void appendToDebugLog_FromArduino(string text);

        bool keepSearchPollingFlag
        {
            get;
            set;
        }
        void refreshControlTextByPolling(ref SerialPortPartitionToGUI sppg);

        SerialPortPartitionToGUI serialPortPartitionToGUI
        {
            get;
            set;
        }

        P_S_Arm currentArmPosition
        {
            get;
        }

        P_S_Body currentPosition
        {
            get;
        }
    }
}
