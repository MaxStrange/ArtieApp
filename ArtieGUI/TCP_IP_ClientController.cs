using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using BeowulfCluster;
using ArtieViaSerialPort;
using ArtieGUI;

namespace ArtieGUI
{
    public class TCP_IP_ClientController
    {
        private IAppendToDataLog _parentUI = null;
        public IAppendToDataLog parentUI
        {
            get { return this._parentUI; }
        }

        private TCP_IP_Client _tcpIPClient = null;
        private TCP_IP_Client tcpIPClient
        {
            get { return this._tcpIPClient; }
            set { this._tcpIPClient = value; }
        }


        private bool waitingForConnection = false;


        public TCP_IP_ClientController(IAppendToDataLog parentUI)
        {
            this._parentUI = parentUI;
            this.tcpIPClient = new TCP_IP_Client();
        }


        private void connectSocketToEndPoint()
        {
            this.tcpIPClient.connectSocketToEndPoint();
            this._parentUI.appendToDataLog("Socket connected to end point.");
        }

        public void connectToComputer()
        {
            try
            {
                this.waitingForConnection = true;
                ThreadPool.QueueUserWorkItem(this.ThreadPoolCallBackShowPleaseWaitAnimation);
                this.tcpIPClient.connectToComputer();
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Failed to connect: socket exception. Are you sure there is an "
                    + "endpoint program running?");
                this._parentUI.appendToDataLog(ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect.");
                this._parentUI.appendToDataLog(ex.ToString());
            }
            finally
            {
                ///signal the pleaseWaitAnimationThread that it should die
                this.waitingForConnection = false;
            }
        }

        public void setEP_and_ConnectToComputer()
        {
            this.tcpIPClient.setEndPoint();
            this.connectToComputer();
        }

        public void setIPAddress_AutoDetect_Michelle()
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry("fattylumpkin");
            this.tcpIPClient.ipAddress = ipHostEntry.AddressList[0];
///                this.parent.ipAddress = Dns.GetHostAddresses("fattylumpkin")[0];
        }

        public void setIPAddress_AutoDetect_Self()
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            this.tcpIPClient.ipAddress = ipHostEntry.AddressList[0];
///                this.tcpIPClient.ipAddress = Dns.GetHostAddresses("localhost")[0];
        }

        public void setIPAddress_Manual(IPAddress ip)
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry(ip);
            this.tcpIPClient.ipAddress = ip;
        }

        public void StartClient()
        {
            TCP_IP_ClientStartGUI startDialog = new TCP_IP_ClientStartGUI(this);
            startDialog.Show();
        }

        private void ThreadPoolCallBackShowPleaseWaitAnimation(object state)
        {
            while (this.waitingForConnection)
            {
                //TODO : Get this to be cleaner/nicer (also animated) - probably don't use a 
                //message box - make your own form

 ///               MessageBox.Show("Attempting to connect to Michelle's computer.\n" +
 ///                   "Please wait...");
            }
        }
    }
}
