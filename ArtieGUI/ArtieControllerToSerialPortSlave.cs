using ActionSet;
using ArtieViaSerialPort;
using SearchAbstractDataTypes;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsefulStaticMethods;

namespace ArtieGUI
{
    /// <summary>
    /// A slave class for ArtieController to handle all serial data and communications.
    /// Any access to this class should probably be through ArtieController, but just in
    /// case, I have made it its own class, rather than one that is inside ArtieController.
    /// </summary>
    internal class ArtieControllerToSerialPortSlave : ICalibrationRequester
    {
//Private fields
        private ArtieController _parent = null;
        private ArtieController parent
        {
            get { return this._parent; }
            set { this._parent = value; }
        }
        private SerialPortController _spController = null;
        private SerialPortController spController
        {
            get { return this._spController; }
            set { this._spController = value; }
        }
        

//Public fields
        private CalibrationData _calibrationData = null;
        public CalibrationData calibrationData
        {
            get { return this._calibrationData; }
            set { this._calibrationData = value; }
        }


//Constructors
        internal ArtieControllerToSerialPortSlave(ArtieController parent)
        {
            this._parent = parent;
        }


//Internal methods
        internal void close()
        {
            this.spController.close();
        }

        /// <summary>
        /// Everytime this method gets called, it starts a new text-polling thread. So if you
        /// ever use it in another place besides during the Form1 construction, make sure
        /// you deal with that.
        /// </summary>
        internal void connectToArduino()
        {
            constructAndInitializeTextPollingThread();
            checkIfArduinoIsOnSerialPort();
            alertUserAboutSerialPortCondition();
            this.parent.setArmSafety();
            checkArtieBatteryLevel();
        }

        internal void disconnectFromArduino()
        {
            this.spController.close();
            this.spController = null;
        }

        internal void sendSolution(Sequence solutionSequence, ISearchUpdate parentUI)
        {
            this.spController.dataHandlerBehavior =
                new ArtieDataHandler(this, parentUI, this.spController, solutionSequence);
        }

        internal bool testArduinoConnection()
        {
            if (somethingIsConnectedToPort())
            {
                writeToPort("" + ActionMap.SerialChars_CommunicationProtocol.test);
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void writeToPort(string portWrite)
        {
            this.spController.dataHandlerBehavior =
                new ArtieDataHandler(this, this.spController);
            if (somethingIsConnectedToPort()) 
                this.spController.write(portWrite);
        }


//Private methods
        private void alertUserAboutSerialPortCondition()
        {
            if (!somethingIsConnectedToPort())
                MessageBox.Show("No Arduino detected. Arduino will be unnavailable.");
            else
                this.parent.arduinoConnected = true;
        }

        private void checkArtieBatteryLevel()
        {
            if (somethingIsConnectedToPort())
            {
                writeToPort("" + ActionMap.SerialChars_Orders.getCalibrationDataFromBody);
            }
        }

        private void checkIfArduinoIsOnSerialPort()
        {
            string[] portList = SerialPort.GetPortNames();
            foreach (string portNumber in portList)
            {
                if (this.spController.openComPort(portNumber))
                    break;
            }
        }

        private void constructAndInitializeTextPollingThread()
        {
            SerialPortPartitionToGUI sppg = new SerialPortPartitionToGUI();
            this.parent.gui.serialPortPartitionToGUI = sppg;
            this.spController = new SerialPortController(ref sppg, new ArtieDataHandler(), new ArtieCommunicationProtocol());
            startTextPollingThread(ref sppg);
        }
        
        private bool somethingIsConnectedToPort()
        {
            if (this.spController != null && this.spController.somethingIsConnectedToPort)
                return true;
            else
                return false;
        }

        private void startTextPollingThread(ref SerialPortPartitionToGUI sppg)
        {
            SerialPortPartitionToGUI s = sppg;
            Thread textPollingThread = ThreadMethods.createNewBackgroundThread(() => textPollingMethod(ref s), "TextPollingThread");
            textPollingThread.Start();
        }

        private void streamSolutionToArduino(char[] solutionActionCharSequence)
        {
            foreach (char action in solutionActionCharSequence)
            {
                if (action == '\0')
                    continue;
                writeToPort("" + action);
            }
        }

        private void textPollingMethod(ref SerialPortPartitionToGUI sppg)
        {
            this.parent.gui.keepSearchPollingFlag = true;
            while (this.parent.gui.keepSearchPollingFlag)
            {
                Thread.Sleep(80);
                this.parent.gui.refreshControlTextByPolling(ref sppg);
            }
        }
    }
}
