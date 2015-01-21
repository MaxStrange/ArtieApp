using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Search;
using BeowulfCluster;
using System.Threading;
using System.Windows.Forms;
using UsefulStaticMethods;
using SearchAbstractDataTypes;
using AbstractDataClasses;
using ActionSet;

namespace ArtieGUI
{
    /// <summary>
    /// Responds to a SearchButtonPushed delegation by preparing and initiating a search.
    /// </summary>
    public class SearchSlave
    {
        private class PreparedSearchData
        {
            private SearchNode _start;
            internal SearchNode start
            {
                get { return this._start; }
            }
            private SearchNode _goal;
            internal SearchNode goal
            {
                get { return this._goal; }
            }
            private SearchPartitionToGUI _spg;
            internal SearchPartitionToGUI spg
            {
                get { return this._spg; }
            }

            internal PreparedSearchData(SearchNode start, SearchNode goal, SearchPartitionToGUI spg)
            {
                this._start = start;
                this._goal = goal;
                this._spg = spg;
            }
        }

//Private fields
        private ISearchButtonParent _parentUI = null;
        private ISearchButtonParent parentUI
        {
            get { return this._parentUI; }
            set { this._parentUI = value; }
        }


//Constructors
        public SearchSlave(ISearchButtonParent parentUI)
        {
            this.parentUI = parentUI;   
        }


//Internal methods
        internal void searchButtonPushed()
        {
            if (!this.parentUI.searching)
            {
                setParentSearchFlagsToSearchingConfiguration();
                decideWhichSearchShouldBeUsed();
            }
        }


//Private methods
        private void dataBaseSearchTask()
        {
            if (this.parentUI.DBAccess == null)
            {
                displaySearchStartError("No database connection.");
                return;
            }
            PreparedSearchData searchData = prepareSearchTask();
            SearchPartitionToGUI spg = searchData.spg;
            startTextPollingThread(ref spg);

            DataBaseSearchController controller = new DataBaseSearchController(
                this.parentUI.DBAccess, 0, ref spg, searchData.start, searchData.goal);
            controller.AStarWithThread();
        }

        private void decideBetweenDistributedDB_OR_SimpleDBSearch()
        {
            if (this.parentUI.DistributeToolStripMenuItem.Checked)
            {
                distributedDataBaseSearchTask();
            }
            else
            {
                dataBaseSearchTask();
            }
        }

        private void decideBetweenDistributed_OR_DumbSearch()
        {
            if (this.parentUI.DistributeToolStripMenuItem.Checked)
            {
                distributedSearchTask();
            }
            else
            {
                dumbSearchTask();
            }
        }

        private void decideWhichSearchShouldBeUsed()
        {
            if (this.parentUI.UseDataBaseToolStripMenuItem.Checked)
            {
                decideBetweenDistributedDB_OR_SimpleDBSearch();
            }
            else
            {
                decideBetweenDistributed_OR_DumbSearch();
            }
        }

        private void displaySearchStartError(string errorMessage)
        {
            MessageBox.Show(errorMessage);
            this.parentUI.searching = false;
        }

        private void distributedDataBaseSearchTask()
        {
            if (this.parentUI.michelleClient == null)
            {
                displaySearchStartError("Michelle's computer not detected.");
                return;
            }
            if (this.parentUI.DBAccess == null)
            {
                displaySearchStartError("No database connection.");
                return;
            }
            PreparedSearchData searchData = prepareSearchTask();
            SearchPartitionToGUI spg = searchData.spg;
            startTextPollingThread(ref spg);

            DistributedDataBaseSearchController controller = new DistributedDataBaseSearchController(
                0, searchData.start, searchData.goal,
                this.parentUI.DBAccess, ref spg, this.parentUI.michelleClient);
            controller.depthFirstWithThread();
        }

        private void distributedSearchTask()
        {
            if (this.parentUI.michelleClient == null)
            {
                displaySearchStartError("Michelle's computer not detected.");
                return;
            }
            PreparedSearchData searchData = prepareSearchTask();
            SearchPartitionToGUI spg = searchData.spg;
            startTextPollingThread(ref spg);

            DistributedSearchController controller = new DistributedSearchController(
                0, searchData.start, searchData.goal, this.parentUI.michelleClient, ref spg);
            controller.depthFirstWithThread();
        }

        private void dumbSearchTask()
        {
            PreparedSearchData searchData = prepareSearchTask();
            SearchPartitionToGUI spg = searchData.spg;
            startTextPollingThread(ref spg);

            DumbSearchController controller = new DumbSearchController(searchData.start,
                searchData.goal, ref spg, this.parentUI.calibrationData);
            controller.AStarWithThread();
        }

        private SearchNode prepareGoalNode(Percent precision)
        {
            double posX, posY, posZ, nx, ny, nz, ox, oy, oz, px, py, pz, armX, armY, armZ,
                armNX, armNY, armNZ, armOX, armOY, armOZ, armPX, armPY, armPZ;
            posX = posY = posZ = nx = ny = nz = ox = oy = oz = px = py = pz = armX = armY = armZ = armNX =
                armNY = armNZ = armOX = armOY = armOZ = armPX = armPY = armPZ = Double.NaN;

            double[] textBoxValues = new double[] { posX, posY, posZ, nx, ny, armX, armY,
                armZ, armNX, armNY, armNZ, armOX, armOY, armOZ, armPX, armPY, armPZ };
            TextBox[] textBoxes = new TextBox[] {this.parentUI.PosXTextBox, this.parentUI.PosYTextBox,
                this.parentUI.PosZTextBox, this.parentUI.NxTextBox, this.parentUI.NyTextBox,
                this.parentUI.armXPosTextBox, this.parentUI.armYPosTextBox, this.parentUI.armZPosTextBox,
                this.parentUI.armNXTextBox, this.parentUI.armNYTextBox, this.parentUI.armNZTextBox,
                this.parentUI.armOXTextBox, this.parentUI.armOYTextBox, this.parentUI.armOZTextBox,
                this.parentUI.armPXTextBox, this.parentUI.armPYTextBox, this.parentUI.armPZTextBox };

            for (int i = 0; i < textBoxValues.Length; i++)
            {
                textBoxValues[i] = setAs_NaN_or_valueFromTextBox(textBoxes[i]);
            }


            Matrices.Point location = new Matrices.Point(textBoxValues[0], textBoxValues[1], textBoxValues[2]);
            Matrices.Vector nVector = new Matrices.Vector(3, new double[] { textBoxValues[3], textBoxValues[4], nz });
            Matrices.Vector oVector = new Matrices.Vector(3, new double[] { ox, oy, oz });
            Matrices.Vector pVector = new Matrices.Vector(3, new double[] { px, py, pz });
            Matrices.Orientation artieFrame = new Matrices.Orientation(nVector, oVector, pVector);

            Matrices.Point gripperLocation = new Matrices.Point(textBoxValues[5], textBoxValues[6], textBoxValues[7]);
            Matrices.Vector gripperNVector = new Matrices.Vector(3, new double[] { textBoxValues[8], textBoxValues[9], textBoxValues[10] });
            Matrices.Vector gripperOVector = new Matrices.Vector(3, new double[] { textBoxValues[11], textBoxValues[12], textBoxValues[13] });
            Matrices.Vector gripperPVector = new Matrices.Vector(3, new double[] { textBoxValues[14], textBoxValues[15], textBoxValues[16] });
            Matrices.Orientation gripperFrame = new Matrices.Orientation(gripperNVector, gripperOVector, gripperPVector);
            PerceptionSets.Gripper gripper = new PerceptionSets.Gripper(gripperLocation, gripperFrame); 

            SearchPartitionToGUI spg = new SearchPartitionToGUI();
            this.parentUI.searchPartitionToGUI = spg;

            return new SearchNode(ref spg, location, artieFrame, precision, gripper);
        }

        private Percent preparePrecision()
        {
            return new Percent(this.parentUI.TrackBar1.Value, true);
        }

        private SearchNode prepareStartNode(Percent precision)
        {
            SearchNode start = new SearchNode(precision);
            start.methodUsedToDeriveNodeFromParent = ElementaryAction.START;
            start.setBodyPositionEqualToAnotherBodyPosition(this.parentUI.currentPosition);
            start.setArmPositionEqualToAnotherArmPosition(this.parentUI.currentArmPosition);

            return start;
        }

        private PreparedSearchData prepareSearchTask()
        {
            Percent precision = preparePrecision();
            SearchNode start = prepareStartNode(precision);
            SearchNode goal = prepareGoalNode(precision);

            return new PreparedSearchData(start, goal, this.parentUI.searchPartitionToGUI);
        }

        /// <summary>
        /// Returns NaN if the textBox is empty or if the function is not passed a textBox.
        /// </summary>
        /// <param name="txtBox"></param>
        /// <returns></returns>
        private double setAs_NaN_or_valueFromTextBox(TextBox txtBox = null)
        {
            //TODO : make it so that the user cannot input anything other than a number
            //into the text boxes
            if (txtBox == null)
            {
                return Double.NaN;
            }
            else if (txtBox.Text == "")
            {
                return Double.NaN;
            }
            else
            {
                return double.Parse(txtBox.Text);
            }
        }

        private void setParentSearchFlagsToSearchingConfiguration()
        {
            this.parentUI.searchStopRequested = false;
            this.parentUI.searching = true;
        }

        private void startTextPollingThread(ref SearchPartitionToGUI spg)
        {
            SearchPartitionToGUI s = spg;
            Thread textPollingThread = ThreadMethods.createNewBackgroundThread(() => textPollingMethod(ref s), "TextPollingThread");
            textPollingThread.Start();
        }

        private void textPollingMethod(ref SearchPartitionToGUI spg)
        {
            this.parentUI.keepSearchPollingFlag = true;
            while (this.parentUI.keepSearchPollingFlag)
            {
                Thread.Sleep(80);
                this.parentUI.refreshSearchTextByPolling(ref spg);
            }
        }
    }
}
