using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase;
using BeowulfCluster;
using UsefulStaticMethods;

namespace ArtieGUI
{
    public class ToolStripController
    {
//Internal fields
        private ArtieController _artieController = null;
        internal ArtieController artieController
        {
            get { return this._artieController; }
            set { this._artieController = value; }
        }
        private bool _calibrateArmDialogIsShowing = false;
        internal bool calibrateArmDialogIsShowing
        {
            get { return this._calibrateArmDialogIsShowing; }
            set { this._calibrateArmDialogIsShowing = value; }
        }

//Private fields
        private IToolStripParent _parentUI = null;
        private IToolStripParent parentUI
        {
            get { return this._parentUI; }
            set { this._parentUI = value; }
        }


//Constructors
        public ToolStripController(IToolStripParent parentUI, ArtieController artieController)
        {
            this.parentUI = parentUI;
            this.artieController = artieController;
        }


//Internal methods
        internal void calibrateArm()
        {
            if (!this.calibrateArmDialogIsShowing)
            {
                CalibrateArmGUI calibrateArmDialog = new CalibrateArmGUI(this);
                calibrateArmDialog.Show();
                this.calibrateArmDialogIsShowing = true;
            }
        }

        internal void connectToArduino()
        {
            this.artieController.connectToArduino();
        }

        internal void connectToANode()
        {
            this.parentUI.michelleClient = new TCP_IP_Client();
        }

        internal void disconnectFromArduino()
        {
            this.artieController.disconnectFromArduino();
        }

        internal void disconnectFromAllNodes()
        {
            if (this.parentUI.michelleClient != null)
            {
                this.parentUI.michelleClient.closeClient();
                this.parentUI.michelleClient = null;
            }
        }

        internal void endConnectionToDB()
        {
            try
            {
                this.parentUI.DBAccess.closeConnection();
                appendToSerialLogText("Disconnected from Database");
            }
            catch (NullReferenceException)
            {
            }
        }

        internal void startConnectionToDB()
        {
            this.parentUI.DBAccess = new DataBaseAccessor();
            if (this.parentUI.DBAccess.connected)
                appendToSerialLogText("Connected to Database!");
            else
                appendToSerialLogText("Failed to connect to Database. MySqlException thrown.");
        }

        internal void testArduino()
        {
            if (!testArduinoConnection())
                appendToSerialLogText("No Arduino connected.");
        }


//Private methods
        private void appendToSerialLogText(string text)
        {
            this.parentUI.appendToDataLog(text);
        }

        private bool testArduinoConnection()
        {
            if (this.artieController.testArduinoConnection())
                return true;
            else
                return false;
        }
    }
}
