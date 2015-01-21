using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtieGUI
{
    public partial class TCP_IP_ClientStartGUI : Form
    {
        private TCP_IP_ClientController parent = null;

        public TCP_IP_ClientStartGUI(TCP_IP_ClientController parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void setEP_and_ConnectToComputer()
        {
            this.parent.setEP_and_ConnectToComputer();
        }

        private void setIPAddress_AutoDetect_Michelle()
        {
            try
            {
                this.parent.setIPAddress_AutoDetect_Michelle();
            }
            catch (SocketException)
            {
                this.parent.parentUI.appendToDataLog("Socket Exception: Is Michelle's computer on?");
                return;
            }
            setEP_and_ConnectToComputer();
        }

        private void setIPAddress_AutoDetect_Self()
        {
            try
            {
                this.parent.setIPAddress_AutoDetect_Self();
            }
            catch (SocketException)
            {
                this.parent.parentUI.appendToDataLog("Socket Exception: Not sure what's wrong.");
                return;
            }
            setEP_and_ConnectToComputer();
        }

        private void setIPAddress_Manual()
        {
            IPAddress ip = IPAddress.Parse(this.ipTextBox.Text);
            this.parent.setIPAddress_Manual(ip);
            setEP_and_ConnectToComputer();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (this.autoDetectRadioButton.Checked)
            {
                setIPAddress_AutoDetect_Michelle();
                this.Close();
            }
            else if (this.autoDetectSelfRadioButton.Checked)
            {
                setIPAddress_AutoDetect_Self();
                this.Close();
            }
            else if (this.manualDetectRadioButton.Checked)
            {
                setIPAddress_Manual();
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
