using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;
using Search;
using System.Threading;
using SearchAbstractDataTypes;
using ArtieViaSerialPort;
using ActionSet;

namespace ArtieGUI
{
    /// <summary>
    /// Responds to user events on the control panel. Part of this duty is keeping track of
    /// the user-defined path on the control panel. Anything that involves physical
    /// Artie is delegated to the ArtieController class.
    /// </summary>
    public class ControlPanelController
    {
//Private fields
        private Sequence _userDefinedArtiePath = new Sequence();
        private Sequence userDefinedArtiePath
        {
            get { return this._userDefinedArtiePath; }
            set { this._userDefinedArtiePath = value; }
        }
        private IControlPanelParent _parentUI;
        private IControlPanelParent parentUI
        {
            get { return this._parentUI; }
            set { this._parentUI = value; }
        }
        private ArtieController _artieController = null;
        private ArtieController artieController
        {
            get { return this._artieController; }
            set { this._artieController = value; }
        }




//Constructors
        public ControlPanelController(IControlPanelParent parentUI, ArtieController artieController)
        {
            this.parentUI = parentUI;
            this.artieController = artieController;
            startPath();
        }



//Internal methods
        internal void clearPath()
        {
            this.userDefinedArtiePath.Clear();
            startPath();
            displayTextOnControlPanel(this.userDefinedArtiePath.startNode.stringExpandForControlPanel());
        }

        internal void driveBackwards()
        {
            ThreadPool.QueueUserWorkItem(this.ThreadPoolCallBackDriveBackwards);
        }

        internal void driveForwards()
        {
            ThreadPool.QueueUserWorkItem(this.ThreadPoolCallBackDriveForwards);
        }

        internal void getPotValues()
        {
            this.artieController.getPotValues();
        }

        internal void recalibrate()
        {
            this.artieController.recalibrate();
        }

        internal void refreshCoordinates()
        {
            ThreadPool.QueueUserWorkItem(this.ThreadPoolCallBackRefreshCoordinates);
        }

        internal void refreshOrientation()
        {
            ThreadPool.QueueUserWorkItem(this.ThreadPoolCallBackRefreshOrientation);
        }

        internal void retrieveMemory()
        {
            this.artieController.retrieveMemory();
        }

        internal void send(ISearchUpdate parentUI)
        {
            Sequence path = new Sequence();
            path.buildFromSearchNode(this.userDefinedArtiePath.endNode);
            this.artieController.sendSolution(path, parentUI);
        }

        internal void stopDriving()
        {
            this.artieController.stopDriving();
        }

        internal void turnTightLeft()
        {
            ThreadPool.QueueUserWorkItem(this.ThreadPoolCallBackTurnTightLeft);
        }

        internal void turnTightRight()
        {
            ThreadPool.QueueUserWorkItem(this.ThreadPoolCallBackTurnTightRight);
        }

        internal void turnWideLeft()
        {
            ThreadPool.QueueUserWorkItem(this.ThreadPoolCallBackTurnWideLeft);
        }

        internal void turnWideRight()
        {
            ThreadPool.QueueUserWorkItem(this.ThreadPoolCallBackTurnWideRight);
        }

        internal void undo()
        {
            if (this.userDefinedArtiePath.Count > 1)
            {
                this.userDefinedArtiePath.Remove(this.userDefinedArtiePath.endNode);
                displayTextOnControlPanel(this.userDefinedArtiePath.endNode.stringExpandForControlPanel());
            }
        }


//Private methods
        private void addNewNodeToPath(CompoundAction methodName)
        {
            SearchNode newNode = new SearchNode();
            newNode = userDefinedArtiePath.endNode.deriveDaughterNode(methodName);
            this.userDefinedArtiePath.Add(newNode);
            displayTextOnControlPanel(newNode.stringExpandForControlPanel());
        }

        private void displayTextOnControlPanel(string textToDisplay)
        {
            this.parentUI.refreshControlPanelText(textToDisplay);
        }

        private void P_A_SChanged()
        {
            //TODO : Make sure the sequence gets changed according to how the start node has changed
        }

        private void startPath()
        {
            SearchNode start = new SearchNode();
            start.setBodyPositionEqualToAnotherBodyPosition(this.parentUI.currentPosition);
            start.methodUsedToDeriveNodeFromParent = ElementaryAction.START;
            this.userDefinedArtiePath.Add(start);
        }

        private void ThreadPoolCallBackDriveBackwards(object state)
        {
            addNewNodeToPath(ElementaryAction.DRIVE_BACKWARDS);
            this.artieController.driveBackwards();
        }

        private void ThreadPoolCallBackDriveForwards(object state)
        {
            addNewNodeToPath(ElementaryAction.DRIVE_FORWARDS);
            this.artieController.driveForwards();
        }

        private void ThreadPoolCallBackRefreshCoordinates(object state)
        {
            this.parentUI.currentPosition.refreshPosition();
            P_A_SChanged();
            this.parentUI.refreshLabels();
        }

        private void ThreadPoolCallBackRefreshOrientation(object state)
        {
            this.parentUI.currentPosition.refreshOrientation();
            P_A_SChanged();
            this.parentUI.refreshLabels();
        }

        private void ThreadPoolCallBackTurnTightLeft(object state)
        {
            addNewNodeToPath(ElementaryAction.TURN_TIGHT_LEFT);
            this.artieController.turnTightLeft();
        }

        private void ThreadPoolCallBackTurnTightRight(object state)
        {
            addNewNodeToPath(ElementaryAction.TURN_TIGHT_RIGHT);
            this.artieController.turnTightRight();
        }

        private void ThreadPoolCallBackTurnWideLeft(object state)
        {
            addNewNodeToPath(ElementaryAction.TURN_WIDE_LEFT);
            this.artieController.turnWideLeft();
        }

        private void ThreadPoolCallBackTurnWideRight(object state)
        {
            addNewNodeToPath(ElementaryAction.TURN_WIDE_RIGHT);
            this.artieController.turnWideRight();
        }
    }
}