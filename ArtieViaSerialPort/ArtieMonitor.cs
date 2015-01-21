using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Search;
using PerceptionSets;
using SearchAbstractDataTypes;
using ArtieViaSerialPort;
using UsefulStaticMethods;
using System.Threading;
using ActionSet;
using AbstractDataClasses;

namespace ArtieViaSerialPort
{
    internal class ArtieMonitor
    {
//Private fields
        private SequenceAsStack currentSequenceToDo
        {
            get { return this.toDoStack.Peek(); }
        }
        private SearchNode nextArtieState
        {
            get { return this.currentSequenceToDo.Peek(); }
        }
        private SearchNode _previousArtieState = new SearchNode();
        private SearchNode previousArtieState
        {
            get { return this._previousArtieState; }
            set { this._previousArtieState = value; }
        }
        private SerialPortController _spController = null;
        private SerialPortController spController
        {
            get { return this._spController; }
            set { this._spController = value; }
        }
        private Stack<SequenceAsStack> _toDoStack = new Stack<SequenceAsStack>();//Watch list:
        //Maybe make Stack<SequenceAsStack> its own class if you find yourself adding methods
        //to this class that deal with it explicitly (you already have one - the pop method).
        private Stack<SequenceAsStack> toDoStack
        {
            get { return this._toDoStack; }
            set { this._toDoStack = value; }
        }
        private ISearchUpdate _parentUI = null;
        private ISearchUpdate parentUI
        {
            get { return this._parentUI; }
            set { this._parentUI = value; }
        }

        private int correctionCounter = 0;
        private int correctionLimit = 2;


//Constructors
        /// <summary>
        /// If an initialSequence is given, the ArtieMonitor will start sending it to the
        /// Arduino.
        /// </summary>
        /// <param name="parentUI"></param>
        /// <param name="spController"></param>
        /// <param name="initialSequence"></param>
        internal ArtieMonitor(ISearchUpdate parentUI = null,
            SerialPortController spController = null, Sequence initialSequence = null)
        {
            this.spController = spController;
            this.parentUI = parentUI;
            
            if (initialSequenceIsvalid(initialSequence))
                initiateTheSequenceGiven(initialSequence);
        }


//Internal methods
        /// <summary>
        /// Checks if Artie is at the state he is supposed to be at. If he is not, it asks
        /// for corrections.
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="distanceTicks"></param>
        internal void compare(SearchNode currentState, DistanceTick distanceTicks)
        {
            //Only check for corrections if a solution sequence was sent, not when sending
            //single actions.
            if (this.toDoStack.Count > 0)
                checkForCorrections(currentState, distanceTicks);
        }

        internal SearchNode driveBackwards(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction driveBackwards = ElementaryAction.DRIVE_BACKWARDS;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(driveBackwards, calData)
                : node.deriveDaughterNode(driveBackwards, calData);

            return n;
        }

        internal SearchNode driveForwards(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction driveForwards = ElementaryAction.DRIVE_FORWARDS;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(driveForwards, calData)
                : node.deriveDaughterNode(driveForwards, calData);

            return n;
        }

        internal SearchNode turnTightLeft(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction turnTightLeft = ElementaryAction.TURN_TIGHT_LEFT;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(turnTightLeft, calData)
                : node.deriveDaughterNode(turnTightLeft, calData);

            return n;
        }

        internal SearchNode turnTightRight(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction turnTightLeft = ElementaryAction.TURN_TIGHT_LEFT;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(turnTightLeft, calData)
                : node.deriveDaughterNode(turnTightLeft, calData);

            return n;
        }

        internal SearchNode turnWideLeft(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction turnWideLeft = ElementaryAction.TURN_WIDE_LEFT;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(turnWideLeft, calData)
                : node.deriveDaughterNode(turnWideLeft, calData);

            return n;
        }

        internal SearchNode turnWideRight(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction turnWideRight = ElementaryAction.TURN_WIDE_RIGHT;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(turnWideRight, calData)
                : node.deriveDaughterNode(turnWideRight, calData);

            return n;
        }


        internal SearchNode driveJointA(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction driveJointA = ElementaryAction.DRIVE_A_CLOCKWISE;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(driveJointA, calData)
                : node.deriveDaughterNode(driveJointA, calData);

            return n;
        }

        internal SearchNode driveJointABack(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction driveJointABack = ElementaryAction.DRIVE_A_COUNTERCLOCKWISE;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(driveJointABack, calData)
                : node.deriveDaughterNode(driveJointABack, calData);

            return n;
        }

        internal SearchNode driveJointB(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction driveJointB = ElementaryAction.DRIVE_B_CLOCKWISE;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(driveJointB, calData)
                : node.deriveDaughterNode(driveJointB, calData);

            return n;
        }

        internal SearchNode driveJointBBack(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction driveJointBBack = ElementaryAction.DRIVE_B_COUNTERCLOCKWISE;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(driveJointBBack, calData)
                : node.deriveDaughterNode(driveJointBBack, calData);

            return n;
        }

        internal SearchNode driveJointC(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction driveJointC = ElementaryAction.DRIVE_C_CLOCKWISE;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(driveJointC, calData)
                : node.deriveDaughterNode(driveJointC, calData);

            return n;
        }

        internal SearchNode driveJointCBack(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction driveJointCBack = ElementaryAction.DRIVE_C_COUNTERCLOCKWISE;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(driveJointCBack, calData)
                : node.deriveDaughterNode(driveJointCBack, calData);

            return n;
        }

        internal SearchNode driveJointD(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction driveJointD = ElementaryAction.DRIVE_D_CLOCKWISE;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(driveJointD, calData)
                : node.deriveDaughterNode(driveJointD, calData);

            return n;
        }

        internal SearchNode driveJointDBack(CalibrationData calData, SearchNode node = null)
        {
            ElementaryAction driveJointDBack = ElementaryAction.DRIVE_D_COUNTERCLOCKWISE;

            SearchNode n = (node == null) ?
                this.previousArtieState.deriveDaughterNode(driveJointDBack, calData)
                : node.deriveDaughterNode(driveJointDBack, calData);

            return n;
        }



//Private methods
        private void addCorrectionSequenceToTheToDoStack(Sequence correctionToPath)
        {
            SequenceAsStack correctionToPathAsStack = new SequenceAsStack(correctionToPath);
            correctionToPathAsStack.Pop();//pop off the start search node cap
            this.toDoStack.Push(correctionToPathAsStack);
        }

        private void checkForCorrections(SearchNode currentState, DistanceTick distanceTicks)
        {
            SearchNode nextState = this.currentSequenceToDo.Peek();
            SearchNode currentGoalNode = this.currentSequenceToDo.endNode;

            if (currentState.matches(currentGoalNode, new Percent(10, true)))
            {
                //Regardless of how we got here, the current sequence to do is done. Get
                //rid of it.
                popTheToDoStack();
            }
            else if (this.currentSequenceToDo.Count == 1)
            {
                //The sequence is done, but we didn't match. Correct and increase the
                //correction counter.
                if (this.correctionCounter >= this.correctionLimit)
                    popTheToDoStack();//Give up
                else
                    correctPath(currentState, distanceTicks);//Try to correct
                this.correctionCounter++;
            }
            else
            {
                setUpCurrentState(currentState);
            }

            //if (currentState.matches(currentGoalNode, new Percent(10, true)))
            //{
            //    //Regardless of how we got here, the current sequence to do is done. Get
            //    //rid of it.
            //    popTheToDoStack();
            //}
            //else if (currentState.matches(nextState, new Percent(10, true)))
            //{
            //    //We are on track.
            //    setUpCurrentState(currentState);
            //}
            //else
            //{
            //    //Need corrections to path to get back to current nextState.
            //    correctPath(currentState, distanceTicks);
            //}
            releaseNextActionOrFinish();
        }

        private void correctPath(SearchNode currentState, DistanceTick distanceTicks)
        {
            Sequence correctionToPath = searchForCorrectionToPath(currentState, distanceTicks);
            addCorrectionSequenceToTheToDoStack(correctionToPath);
        }

        private bool initialSequenceIsvalid(Sequence initialSequence)
        {
            if ((initialSequence != null) && (initialSequence.Count > 1))
                return true;
            else
                return false;
        }

        private void initiateTheSequenceGiven(Sequence initialSequence)
        {
            this.toDoStack.Push(new SequenceAsStack(initialSequence));
            this.previousArtieState = this.currentSequenceToDo.Pop();

            releaseNextActionOrFinish();
        }

        /// <summary>
        /// Applies pop to this.currentSequenceToDo. Then, if this.currentSequenceToDo
        /// is empty, it pops it off of the toDoStack.
        /// </summary>
        private void popCurrentSequenceAndCheckIfItIsEmpty()
        {
            if (this.toDoStack.Count == 0)
                return; //there is nothing to pop.

            this.currentSequenceToDo.Pop();
            if (this.currentSequenceToDo.Count == 0)
                popTheToDoStack();
        }

        /// <summary>
        /// Calls this.toDoStack.Pop() and then, since the last node in a current sequence
        /// is the first node in the next sequence down, pop that node too.
        /// </summary>
        private void popTheToDoStack()
        {
            this.toDoStack.Pop();
            popCurrentSequenceAndCheckIfItIsEmpty();
        }

        /// <summary>
        /// Sends the next compound action to Artie for him to do.
        /// </summary>
        private void releaseNextActionOrFinish()
        {
            bool finished = this.toDoStack.Count <= 0;
            if (finished)
                return;//We are at the goal and are therefore done.
            else
                releaseNextAction();
        }

        private void releaseNextAction()
        {
            string compoundActionUsedToDeriveNextNodeInSequence =
                StringMethods.convertCharArrayToString(this.nextArtieState.buildArrayOfActionCharactersFromNode());
            this.spController.write(compoundActionUsedToDeriveNextNodeInSequence);
        }

        private Sequence searchForCorrectionToPath(SearchNode currentState, DistanceTick distanceTicks)
        {
            SearchNode nextState = this.currentSequenceToDo.Peek();
            currentState.parent = null;

            SearchPartitionToGUI spg = new SearchPartitionToGUI();
            startTextPollingThread(ref spg);

            DumbSearchController correctionSearch =
                new DumbSearchController(currentState, nextState, ref spg, new CalibrationData(distanceTicks));

            return correctionSearch.AStarSearchForSequenceReturn();
        }

        private void setUpCurrentState(SearchNode currentState)
        {
            this.previousArtieState = currentState;
            popCurrentSequenceAndCheckIfItIsEmpty();
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
