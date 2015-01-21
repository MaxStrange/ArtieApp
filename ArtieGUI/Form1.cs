using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO.Ports;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using PerceptionSets;
using Search;
using BeowulfCluster;
using DataBase;
using MySql;
using UsefulStaticMethods;
using System.Text;
using SearchAbstractDataTypes;
using ArtieViaSerialPort;
using ActionSet;

//C:\Users\Max\Documents\Visual Studio 2012\Projects\ArtieDLL\Debug
//Remember that you must compile the ArtieDLL project and take the
//resulting .dll file, copy it and put the copy in the folder that
//contains this project's .exe file.

namespace ArtieGUI
{
    public partial class Form1 : Form, IAppendToDataLog, IControlPanelParent, IToolStripParent,
        ISearchButtonParent, ISearchUpdate
    {
//DLLs
        [DllImport("ArtieDLL.dll", EntryPoint = "retrieveTest")]
        public static extern IntPtr retrieveTest(int n);

        [DllImport("ArtieDLL.dll", EntryPoint = "openCV")]
        public static extern void openCV();


//Public fields
        public TextBox armXPosTextBox
        {
            get { return this.armPosXTextBox; }
        }
        public TextBox armYPosTextBox
        {
            get { return this.armYTextBox; }
        }
        public TextBox armZPosTextBox
        {
            get { return this.armZTextBox; }
        }
        public TextBox armNXTextBox
        {
            get { return this._armNXTextBox; }
        }
        public TextBox armNYTextBox
        {
            get { return this._armNYTextBox; }
        }
        public TextBox armNZTextBox
        {
            get { return this._armNZTextBox; }
        }
        public TextBox armOXTextBox
        {
            get { return this._armOXTextBox; }
        }
        public TextBox armOYTextBox
        {
            get { return this._armOYTextBox; }
        }
        public TextBox armOZTextBox
        {
            get { return this._armOZTextBox; }
        }
        public TextBox armPXTextBox
        {
            get { return this._armPXTextBox; }
        }
        public TextBox armPYTextBox
        {
            get { return this._armPYTextBox; }
        }
        public TextBox armPZTextBox
        {
            get { return this._armPZTextBox; }
        }
        public CalibrationData calibrationData
        {
            get { return this.artieController.serialPortSlave.calibrationData; }
        }
        private P_S_Arm _currentArmPosition = null;
        public P_S_Arm currentArmPosition
        {
            get { return this.artieController.perception_Actual.perceptionState_Arm; }
        }
        private P_S_Body _currentPosition = null;
        public P_S_Body currentPosition
        {
            get { return this.artieController.perception_Actual.perceptionState_Body; }
        }
        private DataBaseAccessor _DBAccess;
        public DataBaseAccessor DBAccess
        {
            get { return this._DBAccess; }
            set { this._DBAccess = value; }
        }
        private TCP_IP_Client _michelleClient = null;
        public TCP_IP_Client michelleClient
        {
            get { return this._michelleClient; }
            set { this._michelleClient = value; }
        }
        private bool _searching = false;
        public bool searching
        {
            get 
            {
                if (this.searchPartitionToGUI != null)
                    this._searching = this.searchPartitionToGUI.searching;
                return this._searching;
            }
            set 
            {
                if (this.searchPartitionToGUI != null)
                    this.searchPartitionToGUI.searching = value;
                this._searching = value;
            }
        }
        private bool _searchStopRequested = false;
        public bool searchStopRequested
        {
            get { return this._searchStopRequested; }
            set 
            { 
                this._searchStopRequested = value;
                
            }
        }
        public ToolStripMenuItem UseDataBaseToolStripMenuItem
        {
            get { return this.useDataBaseToolStripMenuItem; }
        }
        public ToolStripMenuItem DistributeToolStripMenuItem
        {
            get { return this.distributeToolStripMenuItem; }
        }
        public TextBox PosXTextBox
        {
            get { return this.posXTextBox; }
        }
        public TextBox PosYTextBox
        {
            get { return this.posYTextBox; }
        }
        public TextBox PosZTextBox
        {
            get { return this.posZTextBox; }
        }
        public TextBox NxTextBox
        {
            get { return this.nxTextBox; }
        }
        public TextBox NyTextBox
        {
            get { return this.nyTextBox; }
        }
        public TrackBar TrackBar1
        {
            get { return this.trackBar1; }
        }
        private bool _keepSearchPollingFlag = false;
        public bool keepSearchPollingFlag
        {
            get { return this._keepSearchPollingFlag; }
            set { this._keepSearchPollingFlag = value; }
        }
        private bool _keepSerialPollingFlag = false;
        private bool keepSerialPollingFlag
        {
            get { return this._keepSerialPollingFlag; }
            set { this._keepSerialPollingFlag = value; }
        }
        private SearchPartitionToGUI _searchPartitionToGUI = null;
        public SearchPartitionToGUI searchPartitionToGUI
        {
            get { return this._searchPartitionToGUI; }
            set { this._searchPartitionToGUI = value; }
        }
        private SerialPortPartitionToGUI _serialPortPartitionToGUI = null;
        public SerialPortPartitionToGUI serialPortPartitionToGUI
        {
            get { return this._serialPortPartitionToGUI; }
            set { this._serialPortPartitionToGUI = value; }
        }


//Private fields
        private ArtieController _artieController = null;
        private ArtieController artieController
        {
            get { return this._artieController; }
            set { this._artieController = value; }
        }
        private ControlPanelController _cpController = null;
        private ControlPanelController cpController
        {
            get { return this._cpController; }
            set { this._cpController = value; }
        }
        private Point lastPoint = new Point(9999, 9999);
        private bool painting = false;
        private SearchButtonController _sbController = null;
        private SearchButtonController sbController
        {
            get { return this._sbController; }
            set { this._sbController = value; }
        }
        private bool _searchIsUserInitiated = false;
        private bool searchIsUserInitiated
        {
            get { return this._searchIsUserInitiated; }
            set { this._searchIsUserInitiated = value; }
        }
        private bool _solutionFound = false;
        private bool solutionFound
        {
            get { return this._solutionFound; }
            set { this._solutionFound = value; }
        }
        private ToolStripController _tsController = null;
        private ToolStripController tsController
        {
            get { return this._tsController; }
            set { this._tsController = value; }
        }
        

//Private delegates
        private delegate void refreshSerialTextByPollingDelegate(ref SerialPortPartitionToGUI sppg);
        private delegate void refreshSearchTextByPollingDelegate(ref SearchPartitionToGUI spg);
        private delegate void searchFunctionDelegate(DataBaseSearchController search);
        private delegate void searchDelegate(SearchNode start, SearchNode goal, int precision);
        private delegate void RefreshTextDelegate(string text);
        private delegate void RefreshLabelsDelegate();
        private delegate void AppendToDataLogDelegate(string text);
        private delegate void RefreshControlPanelTextDelegate(string text);
        private delegate void RefreshOutputTextDelegate(string text);


//Constructors        
        public Form1()
        {
            InitializeComponent();
            
            //ActionMap A = new ActionMap();
            ActionMap.initialize();

            this._currentPosition = new P_S_Body();
            this._currentArmPosition = new P_S_Arm(this._currentPosition);
            
            this.artieController = new ArtieController(this);
            this.artieController.connectToArduino();
            
            this.tsController = new ToolStripController(this, this.artieController);
            
            this.cpController = new ControlPanelController(this, this.artieController);
            
            this.sbController = new SearchButtonController(this);
        }



//Control Panel Actions
        private void controlPanelArduinoPositionButton_Click(object sender, EventArgs e)
        {
            this.cpController.recalibrate();
        }

        private void controlPanelClearButton_Click(object sender, EventArgs e)
        {
            this.cpController.clearPath();
        }

        private void controlPanelGetPotValuesButton_Click(object sender, EventArgs e)
        {
            this.cpController.getPotValues();
        }

        private void controlPanelRetrieveMemoryButton_Click(object sender, EventArgs e)
        {
            this.cpController.retrieveMemory();
        }

        private void controlPanelSendButton_Click(object sender, EventArgs e)
        {
            this.cpController.send(this);
        }

        private void controlPanelUndoButton_Click(object sender, EventArgs e)
        {
            this.cpController.undo();
        }

        private void driveBackwardsButtonPushed(object sender, EventArgs e)
        {
            this.cpController.driveBackwards();
        }

        private void driveForwardsButtonPushed(object sender, EventArgs e)
        {
            this.cpController.driveForwards();
        }

        private void refreshCoordinatesButton_Click(object sender, EventArgs e)
        {
            this.cpController.refreshCoordinates();
        }

        private void refreshOrientationButtonPushed(object sender, EventArgs e)
        {
            this.cpController.refreshOrientation();
        }

        private void stopDrivingButtonPushed(object sender, EventArgs e)
        {
            this.cpController.stopDriving();
        }

        private void turnTightLeftButtonPushed(object sender, EventArgs e)
        {
            this.cpController.turnTightLeft();
        }

        private void turnTightRightButtonPushed(object sender, EventArgs e)
        {
            this.cpController.turnTightRight();
        }

        private void turnWideLeftButtonPushed(object sender, EventArgs e)
        {
            this.cpController.turnWideLeft();
        }
        
        private void turnWideRightButtonPushed(object sender, EventArgs e)
        {
            this.cpController.turnWideRight();
        }



//ControlPanel text methods
        public void appendToDebugLog_FromArduino(string text)
        {
            AppendToDataLogDelegate appendText = new AppendToDataLogDelegate(appendToDebugLog_FromArduino);

            if (!this.InvokeRequired)
            {
                this.controlPanelTextBox_FromArduino.AppendText(" " + text);
                this.controlPanelTextBox_FromArduino.Refresh();
            }
            else
            {
                tryInvoke(appendText, text);
            }
        }

        public void appendToDebugLog_ToArduino(string text)
        {
            AppendToDataLogDelegate appendText = new AppendToDataLogDelegate(appendToDebugLog_ToArduino);

            if (!this.InvokeRequired)
            {
                this.controlPanelTextBox_ToArduino.AppendText(" " + text);
                this.controlPanelTextBox_ToArduino.Refresh();
            }
            else
            {
                tryInvoke(appendText, text);
            }
        }

        //TODO : should be pass by value I think. Same for the other refreshTextByPolling method.
        public void refreshControlTextByPolling(ref SerialPortPartitionToGUI sppg)
        {
            if (sppg == null)
                return;

            refreshSerialTextByPollingDelegate refText = new refreshSerialTextByPollingDelegate(refreshControlTextByPolling);

            if (!this.InvokeRequired)
            {
                this.dataLogTextBox.Text = sppg.dataLogText;
                this.controlPanelTextBox_FromArduino.Text = sppg.debugLogFromArduino;
                this.controlPanelTextBox_CharsToArduino.Text = sppg.debugLogToArduino;
            }
            else
            {
                tryInvoke(refText, sppg);
            }
        }

        public void refreshControlPanelText(string text)
        {
            RefreshControlPanelTextDelegate refText = new RefreshControlPanelTextDelegate(refreshControlPanelText);

            if (!this.InvokeRequired)
            {
                this.controlPanelTextBox_ToArduino.Text = text;
                this.dataLogTextBox.Refresh();
            }
            else
            {
                tryInvoke(refText, text);
            }
        }

        

//Vision Tab actions and associated code
        /// <summary>
        /// Checks if DLL is working
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DLLCheckButtonPushed(object sender, EventArgs e)
        {
            IntPtr retrievedArray = retrieveTest(3);
            appendToDataLog(Marshal.PtrToStringAuto(retrievedArray));
        }

        private void openCVButtonPushed(object sender, EventArgs e)
        {
            if (this.pictureBox2.Image == null) return;

            //save the img into a file where the openCV function will
            //read it

            this.pictureBox2.Image.Save("C:\\Users\\Max\\Desktop\\image.JPG",
                System.Drawing.Imaging.ImageFormat.Jpeg);

            //call openCV function from the dll
            openCV();
        }

        private void openImageButtonPushed(object sender, EventArgs e)
        {
            OpenFileDialog openImageDialog = new OpenFileDialog();
            openImageDialog.Filter = "Image Files (*.bmp, *.jpg)|*.bmp;*.jpg";
            openImageDialog.FilterIndex = 1;
            openImageDialog.Multiselect = false;

            DialogResult imageDialogRes = openImageDialog.ShowDialog();
            if (imageDialogRes == DialogResult.OK)
            {
                this.pictureBox2.Image = new Bitmap(openImageDialog.FileName);
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            this.painting = true;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.painting && pictureBox2.Image != null) 
            {
                if ((this.lastPoint.X != 9999) && (this.lastPoint.Y != 9999))
                {
                    //TODO all graphics stuff should be done through my own graphics library
                    
                    SolidBrush color = new SolidBrush(Color.White);
                    Pen p = new Pen(color);

                    GraphicsPath gPath = new GraphicsPath();
                    gPath.AddLine(this.lastPoint, new Point(e.X, e.Y));

                    Bitmap img = new Bitmap(pictureBox2.Image);
                    Graphics g = Graphics.FromImage(img);
                    g.DrawPath(p, gPath);

                    this.pictureBox2.Image = img;
                }
                this.lastPoint.X = e.X;
                this.lastPoint.Y = e.Y;
            }
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            this.painting = false;
        }



//DataLog Tab Text Methods
        public void appendToDataLog(string text)
        {
            AppendToDataLogDelegate appendText = new AppendToDataLogDelegate(appendToDataLog);

            if (!this.InvokeRequired)
            {
                this.dataLogTextBox.AppendText(" " + text);
                this.dataLogTextBox.Refresh();
            }
            else
            {
                tryInvoke(appendText, text);
            }
        }

        

//Toolstrip Actions
        //Cluster items
        private void connectToANodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tsController.connectToANode();
        }

        private void disconnectFromAllNodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tsController.disconnectFromAllNodes();
        }


        //Database items
        private void connectToDataBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tsController.startConnectionToDB();
        }

        private void disconnectFromDataBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tsController.endConnectionToDB();
        }


        //XBee items
        private void connectToXBeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tsController.connectToArduino();
        }

        private void disconnectFromXBeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tsController.disconnectFromArduino();
        }

        private void testXBeeConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tsController.testArduino();
        }
        

        //Search items
        private void distributeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.distributeToolStripMenuItem.Checked = BoolMethods.toggle(this.distributeToolStripMenuItem.Checked);
        }
        
        private void useDataBaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dataBaseToolStripMenuItem.Checked = BoolMethods.toggle(this.dataBaseToolStripMenuItem.Checked);
        }


        //Arm items
        private void calibrateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tsController.calibrateArm();   
        }

        private void orientationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.orientationToolStripMenuItem.Checked = BoolMethods.toggle(this.orientationToolStripMenuItem.Checked);
            this.positionToolStripMenuItem.Checked = false;
        }

        private void positionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.positionToolStripMenuItem.Checked = BoolMethods.toggle(this.positionToolStripMenuItem.Checked);
            this.orientationToolStripMenuItem.Checked = false;
        }



//PictureBox
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = e.Graphics;
            //SolidBrush b = new SolidBrush(Color.Black);
            //Pen p = new Pen(b);

            //g.DrawLine(p, new Point(pictureBox1.Size.Width / 2, 0),
            //    new Point(pictureBox1.Size.Width / 2,
            //        pictureBox1.Size.Height));
            //g.DrawLine(p, new Point(0, pictureBox1.Size.Height / 2),
            //    new Point(pictureBox1.Size.Width,
            //        pictureBox1.Size.Height / 2));
            //b.Color = Color.Red;
            //p.Brush = b;
            //Point origin = new Point(pictureBox1.Size.Width / 2,
            //    pictureBox1.Size.Height / 2);
            //int SF = 50;//scale factor for lines
            //Point nHat = new Point((int)(origin.X + (SF * this.currentPosition.artieFrame.nVector.x + 0.5)),
            //    (int)(origin.Y - (SF * this.currentPosition.artieFrame.nVector.y + 0.5)));
            //Point oHat = new Point((int)(origin.X + (SF * this.currentPosition.artieFrame.oVector.x + 0.5)),
            //    (int)(origin.Y - (SF * this.currentPosition.artieFrame.oVector.y + 0.5)));
            //g.DrawLine(p, nHat, origin);
            //b.Color = Color.Blue;
            //p.Brush = b;
            //g.DrawLine(p, oHat, origin);
        }



//Misc front page
        public void refreshLabels()
        {
            RefreshLabelsDelegate labelDel = new RefreshLabelsDelegate(refreshLabels);

            if (!this.InvokeRequired)
            {
                label3.Text = "Position vector (x): " + this.currentPosition.location.x;
                label4.Text = "Position vector (y): " + this.currentPosition.location.y;
                label5.Text = "Position vector (z): " + this.currentPosition.location.z;
                //TODO : refactor so that GUI does not depend on matrices project.
                //label6.Text = "n_x: " + this.currentPosition.artieFrame.nVector.x;
                //label7.Text = "n_y: " + this.currentPosition.artieFrame.nVector.y;
                //label8.Text = "n_z: " + this.currentPosition.artieFrame.nVector.z;
                //label9.Text = "o_x: " + this.currentPosition.artieFrame.oVector.x;
                //label10.Text = "o_y: " + this.currentPosition.artieFrame.oVector.y;
                //label11.Text = "o_z: " + this.currentPosition.artieFrame.oVector.z;
                //label12.Text = "p_x: " + this.currentPosition.artieFrame.pVector.x;
                //label13.Text = "p_y: " + this.currentPosition.artieFrame.pVector.y;
                //label14.Text = "p_z: " + this.currentPosition.artieFrame.pVector.z;
                artieOrientationPictureBox.Refresh();
            }
            else
            {
                tryInvoke(labelDel);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        { 
            label24.Text = "Allowed Error: " + trackBar1.Value;
        }



//Search Button Actions
        private void checkIfUserWantsToSendSolution(SearchPartitionToGUI spg)
        {
            //Decide if the user should be asked at all: (e.g. if it is a search created
            //by the serial port in communication with Artie)
            if (!this.searchIsUserInitiated)
                return;

            if (this.artieController.arduinoConnected)
            {
                DialogResult sendSolution = displaySendSolutionDialog(ref spg);
                if (sendSolution == DialogResult.Yes)
                {
                    this.artieController.sendSolution(spg.solutionSequence, this);
                }
            }
            //Set this flag back to default configuration.
            this.searchIsUserInitiated = false;
        }

        private DialogResult displaySendSolutionDialog(ref SearchPartitionToGUI spg)
        {
            string message = (spg.failureExplanation == null) ?
                "Arduino detected. Would you like to send the solution?" : spg.failureExplanation;
            spg.failureExplanation = null;
            return MessageBox.Show(message, "Send to Arduino?", MessageBoxButtons.YesNo);
        }

        private void nxTextBox_TextChanged(object sender, EventArgs e)
        {
            this.sbController.nxTextBox_TextChanged((TextBox)sender);
        }

        private void nyTextBox_TextChanged(object sender, EventArgs e)
        {
            this.sbController.nyTextBox_TextChanged((TextBox)sender);
        }

        private void searchButtonPushed(object sender, EventArgs e)
        {
            this.outPutListBox.Items.Clear();
            this.solutionTextBox.Clear();
            this.sbController.searchButtonPushed();
            this.searchIsUserInitiated = true;
        }

        public void refreshSearchTextByPolling(ref SearchPartitionToGUI spg)
        {
            refreshSearchTextByPollingDelegate refText = new refreshSearchTextByPollingDelegate(refreshSearchTextByPolling);

            if (!this.InvokeRequired)
                refreshSearchText(ref spg);
            else
                tryInvoke(refText, spg);
        }

        private void refreshSearchText(ref SearchPartitionToGUI spg)
        {
            if (spg.solutionFound)
            {
                spg.solutionFound = false;
                updateOutPutSearchBoxes(ref spg);
                this.solutionTextBox.Text = spg.refreshTextLog;
                this.solutionTextBox.Refresh();
                this.keepSearchPollingFlag = false;
                checkIfUserWantsToSendSolution(spg);
            }
            else
            {
                updateOutPutSearchBoxes(ref spg);
            }
        }

        private void replaceSearchNodeInListBox(SearchNode n, ref ListBox box)
        {
            if (n == null)
                return;

            box.BeginUpdate();
            if (box.Items.Count > 0)
                box.Items.RemoveAt(0);
            box.Items.Add(n.ToString());
            box.Refresh();
            box.EndUpdate();
        }

        private void stopButtonPushed(object sender, EventArgs e)
        {
            this.searchStopRequested = BoolMethods.toggle(this.searchStopRequested);
            this.searchPartitionToGUI.stopRequested = true;
            this.keepSearchPollingFlag = false;
            this.searchIsUserInitiated = false;
        }

        private void updateOutPutListBox(string text)
        {
            this.outPutListBox.BeginUpdate();
            this.outPutListBox.Items.Add(text);
            this.outPutListBox.Refresh();
            this.outPutListBox.EndUpdate();
        }

        private void updateOutPutSearchBoxes(ref SearchPartitionToGUI spg)
        {
            updateOutPutListBox(spg.refreshTextLog);
            replaceSearchNodeInListBox(spg.bestSoFar, ref this.bestSoFarListBox);
            replaceSearchNodeInListBox(spg.goalNode, ref this.goalNodeListBox);
        }

//Misc
        private void tryInvoke(Delegate method)
        {
            try
            {
                this.Invoke(method, new object[] { });
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private void tryInvoke(Delegate method, object param)
        {
            try
            {
                this.Invoke(method, new object[] { param });
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            wrapUp();
        }

        private void wrapUp()
        {
            this.searching = false;
            this.keepSearchPollingFlag = false;
            this.artieController.close();
            if (this.DBAccess.connection != null)
            {
                this.DBAccess.closeConnection();
            }
        }
    }
}
