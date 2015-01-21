using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Search;
using System.Windows.Forms;
using Erlang;
using ArtieViaSerialPort;
using System.Threading;
using UsefulStaticMethods;

namespace ArtieViaSerialPort
{
    /// <summary>
    /// SerialPortController is an encapsulation of a SerialPort object. It requires a
    /// communication protocol behavior of type IProtocol, which is responsible for formatting
    /// all data sent through and received through this port. Without an IProtocol object
    /// specified, it will not format data sent or received, instead all data will be raw.
    /// It also requires a data handling behavior of type IDataHandler, which is handed 
    /// all data received through this port once it has been formatted. Without specifying
    /// this behavior, the SerialPortController object will discard all data received after
    /// any formatting.
    /// Before a handshake has been completed, the port will silently discard any data
    /// received.
    /// </summary>
    public class SerialPortController
    {
//Public fields
        private bool _somethingIsConnectedToPort = false;
        public bool somethingIsConnectedToPort
        {
            get { return _somethingIsConnectedToPort; }
            private set { this._somethingIsConnectedToPort = value; }
        }
        private IDataHandler _dataHandlerBehavior = null;
        public IDataHandler dataHandlerBehavior
        {
            get { return this._dataHandlerBehavior; }
            set { this._dataHandlerBehavior = value; }
        }


//Private fields
        private IProtocol protocolBehavior = null;
        private SerialPort _port = null;
        private SerialPort port
        {
            get { return this._port; }
            set { this._port = value; }
        }
        private bool _portHandHasBeenShaken = false;
        private bool portHandHasBeenShaken
        {
            get { return this._portHandHasBeenShaken; }
            set { this._portHandHasBeenShaken = value; }
        }
        private SerialPortPartitionToGUI _serialPartitionToGUI = null;
        private SerialPortPartitionToGUI serialPartitionToGUI
        {
            get { return this._serialPartitionToGUI; }
            set { this._serialPartitionToGUI = value; }
        }



//Constructors
        public SerialPortController(ref SerialPortPartitionToGUI sppg, IDataHandler dataHandlerBehavior=null, IProtocol protocolBehavior=null)
        {
            this.serialPartitionToGUI = sppg;
            this.dataHandlerBehavior = dataHandlerBehavior;
            this.protocolBehavior = protocolBehavior;
        }


//Public methods
        public void close()
        {
            try
            {
                this.port.Close();
            }
            catch (Exception)
            {
                //Ignore any complaints, just close the port.
            }
        }

        public bool openComPort(string comNumber)
        {
            try
            {
                initializePort(comNumber);
                return true;
            }
            catch (System.IO.IOException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public void write(string data)
        {
            string formattedData = this.protocolBehavior.formatDataToSend(data);

            Console.Out.Write("Attempting to send: ");
            Console.Out.WriteLine(formattedData);

            if (portIsUseable())
                writeAndUpdateDebugLog(formattedData);    
        }
        


//Private methods 
        private void assignPortID(RawData data)
        {
            char idOfThingThatIsOnOtherEndOfPort = data.toChar();
            System.Console.Out.Write("Raw data: ");
            System.Console.Out.Write(data);
            System.Console.Out.Write(" formatted as: ");
            System.Console.Out.WriteLine(idOfThingThatIsOnOtherEndOfPort);
            if (this.protocolBehavior.handShakeReceive(idOfThingThatIsOnOtherEndOfPort))
                setSomethingConnectedToPort();
        }

        private void checkTheReceivedData(RawData data)
        {
            if (this.portHandHasBeenShaken)
                readData(data);
            else
                assignPortID(data);
        }

        private void checkPortIdentity()
        {
            string handShakeSend = this.protocolBehavior.handShakeSend();
            write(handShakeSend);
            waitForHandShakeReturn();
        }

        private void initializePort(string comNumber)
        {
            this.port = new SerialPort(comNumber) { BaudRate = 9600 };
            this.port.DataReceived += port_DataReceived;
            this.port.Open();
            this.port.ReadTimeout = 3000;
            checkPortIdentity();
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (this.port != null)
                readAllTheBytesInThePort();
        }

        private bool portIsUseable()
        {
            if ((this.port != null) && (this.port.IsOpen))
                return true;
            else
                return false;
        }

        private void readAllTheBytesInThePort()
        {
            while (this.port.BytesToRead > 0)
            {
                int data = this.port.ReadChar();
                char dChar = (char)data;//For debug purposes
                System.Console.Out.Write("Unformatted, raw data received: ");
                System.Console.Out.WriteLine(dChar);
                checkTheReceivedData(new RawData(data));
            }
        }

        private void readData(RawData data)
        {
            object formattedData = this.protocolBehavior.formatDataReceived(data);

            if (formattedData == null)
                return;

            System.Console.Out.Write("Formatted data: ");
            System.Console.Out.WriteLine(formattedData);

            this.serialPartitionToGUI.updateUILogs(formattedData);
            this.dataHandlerBehavior.receiveData(formattedData);
        }

        private void setSomethingConnectedToPort()
        {
            this.somethingIsConnectedToPort = true;
            this.portHandHasBeenShaken = true;
            System.Console.Out.WriteLine("Handshake complete.");
        }

        private void showTimeoutErrorMessage()
        {
            string errorMessage = "Attempt to establish communication with the Arduino timed out.";
            MessageBox.Show(errorMessage);
        }

        private void waitForHandShakeReturn()
        {
            Thread.Sleep(this.port.ReadTimeout);

            if (!this.somethingIsConnectedToPort)
                showTimeoutErrorMessage();
        }

        private void writeAndUpdateDebugLog(string formattedData)
        {
            try
            {
                this.port.Write(formattedData);
                Console.Out.Write("Successfully sent: ");
                Console.Out.WriteLine(formattedData);
            }
            catch (System.IO.IOException)
            {
                //Interrupted communication
            }
            this.serialPartitionToGUI.passToUIDebugLog_ToArduino(formattedData);
        }
    }
}