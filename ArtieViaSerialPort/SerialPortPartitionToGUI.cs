using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtieViaSerialPort
{
    public class SerialPortPartitionToGUI
    {
        private string _dataLogText = "";
        public string dataLogText
        {
            get { return this._dataLogText; }
            private set { this._dataLogText = value; }
        }
        private string _debugLogFromArduino = "";
        public string debugLogFromArduino
        {
            get { return this._debugLogFromArduino; }
            private set { this._debugLogFromArduino = value; }
        }
        private string _debugLogToArduino = "";
        public string debugLogToArduino
        {
            get { return this._debugLogToArduino; }
            private set { this._debugLogToArduino = value; }
        }


//Internal methods
        internal void passToUIDebugLog_ToArduino(string data)
        {
            this.debugLogToArduino += data;
        }

        internal void updateUILogs(object formattedData)
        {
            if (formattedData is IEnumerable<object>)
            {
                IEnumerable<object> enumerableData = (formattedData as IEnumerable<object>);
                updateUILogsWithEnumeration(enumerableData);
            }
            else
            {
                passToAllUILogs(formattedData.ToString());
            }
        }


//Private methods
        private void passToAllUILogs(string dataToGiveUILogs)
        {
            passToUIDataLog(dataToGiveUILogs);
            passToUIDebugLog_FromArduino(dataToGiveUILogs);
        }

        private void passToUIDataLog(string data)
        {
            string dataWithTab = "\t" + data;
            this.dataLogText += dataWithTab;
        }

        private void passToUIDebugLog_FromArduino(string data)
        {
            string dataWithTab = "\t" + data;
            this.debugLogFromArduino += dataWithTab;
        }

        private void updateUILogsWithEnumeration(IEnumerable<object> enumerableData)
        {
            foreach (object obj in enumerableData)
            {
                passToAllUILogs(obj.ToString());
            }
        }
    }
}
