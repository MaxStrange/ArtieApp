using System.Collections.Generic;
using SearchAbstractDataTypes;
using ActionSet;
using UsefulStaticMethods;

namespace Search
{
    public abstract class SearchController
    {
//Protected fields
        private SearchNode _bestSoFar = null;
        protected SearchNode bestSoFar
        {
            get 
            {
                if (this.openQueue.firstNode != null)
                    this._bestSoFar = this.openQueue.firstNode;

                return this._bestSoFar;
            }
        }
        private PrioritySearchNodeSet _closedNodeSet = null;
        protected PrioritySearchNodeSet closedNodeSet
        {
            get { return this._closedNodeSet; }
            set { this._closedNodeSet = value; }
        }
        protected SearchNode _goalNode = null;
        protected SearchNode goalNode
        {
            get { return this._goalNode; }
            set { this._goalNode = goalNode; }
        }
        private int _numberOfSearchIterations = 0;
        protected int searchIterations
        {
            get { return this._numberOfSearchIterations; }
            set { this._numberOfSearchIterations = value; }
        }
        private PrioritySearchNodeQueue _openQueue = null;
        protected PrioritySearchNodeQueue openQueue
        {
            get { return this._openQueue; }
            private set { this._openQueue = value; }
        }
        protected SearchPartitionToGUI searchToGui = null;
        protected SearchNode _startNode = null;
        protected SearchNode startNode
        {
            get { return this._startNode; }
        }
        private bool _stopRequestedFlag = false;
        protected bool stopRequestedFlag
        {
            get { return this._stopRequestedFlag; }
            set { this._stopRequestedFlag = value; }
        }
        
        
//Private fields        
        private CalibrationData _calibrationData = null;
        private CalibrationData calibrationData
        {
            get { return this._calibrationData; }
        }

        private List<IDirective> _listOfDirectives = new List<IDirective>();
        private List<IDirective> listOfDirectives
        {
            get { return this._listOfDirectives; }
            set { this._listOfDirectives = value; }
        }
        


//Public methods
        virtual public void AStarSearch()
        {
        }



//Protected methods
        protected void alertUISolutionHasBeenFound(SearchNode solutionNode)
        {
            alertUI(solutionNode);
        }

        protected void alertUISolutionHasNotBeenFound(string failureExplanation)
        {
            this.stopRequestedFlag = true;
            alertUI(null, failureExplanation);
        }

        protected void alertUISearchHasStopped()
        {
            this.searchToGui.searching = false;
        }

        protected void expandNodeAndAddDaughtersToOpen(SearchNode parent)
        {
            List<SearchNode> daughtersOfN = nodeExpand(parent);
            foreach (SearchNode daughter in daughtersOfN)
            {
                if (this.closedNodeSet.contains(daughter))
                {
                    this.closedNodeSet.add(daughter);//Add even if you have seen before,
                    //since matches are not exact.
                    continue;//We have already examined this node before.
                }
                else if (nodeIsDisallowedByDirective(daughter))
                {
                    this.closedNodeSet.add(daughter);
                    continue;
                }
                else
                    this.openQueue.add(daughter);
            }
            updateUIBestSoFarNode();
        }

        protected void initializeOpenAndClosed()
        {
            initiateBestSoFarQueue();
            this.closedNodeSet = new PrioritySearchNodeSet(this.goalNode);
        }

        protected void initializeSearch(SearchNode startNode, SearchNode goalNode,
            ref SearchPartitionToGUI spg, CalibrationData calData = null)
        {
            this.listOfDirectives.Add(new Directive_NoSelfHarm());
            initializeSearch(startNode, goalNode);
            this.startNode.guiCommunicator = spg;
            this.goalNode.guiCommunicator = spg;
            this.searchToGui = spg;
            this._calibrationData = calData;
            updateUIGoalNode();
        }

        protected SearchNode moveFirstNodeOnOpenToClosedAndAlertUI()
        {
            SearchNode n = this.openQueue.pop();
            n.stringExpandForGUICommunicator();
            this.closedNodeSet.add(n);

            return n;
        }

        protected virtual List<SearchNode> nodeExpand(SearchNode parent)
        {
            List<SearchNode> daughters = new List<SearchNode>();

            foreach (ElementaryAction action in ActionMap.allowedArtieActions)
            {
                SearchNode daughter = parent.deriveDaughterNode(action, this.calibrationData);
                daughters.Add(daughter);
            }
            return daughters;
        }

        protected bool openQueueContainsANode()
        {
            if (this.openQueue.count > 0)
                return true;
            else
                return false;
        }

        protected void pruneOpenQueue()
        {
            this.openQueue.prune();
        }

        protected bool stopIsRequested()
        {
            if (this.searchToGui.stopRequested)
            {
                alertUISolutionHasNotBeenFound("Search was stopped, showing best match.");
                return true;
            }
            return false;
        }


//Private methods
        private void alertUI(SearchNode n = null, string failureExplanation = null)
        {
            if (n == null)
                n = this.bestSoFar;

            Sequence solutionSeq = new Sequence();
            solutionSeq.buildFromSearchNode(n);
            sendSolutionToArduino(solutionSeq, failureExplanation);

            this.searchToGui.solutionFound = true;

            stringExpandForGUICommunicator(n, failureExplanation);

            alertUISearchHasStopped();
        }

        private void initiateBestSoFarQueue()
        {
            this.openQueue = new PrioritySearchNodeQueue(this.goalNode);

            SearchNode startCopy = new SearchNode(ref this.searchToGui);
            startCopy.copy(this.startNode);
            
            this.openQueue.add(startCopy);
        }

        private void initializeSearch(SearchNode startNode, SearchNode goalNode)
        {
            this._startNode = startNode;
            this._goalNode = goalNode;
        }

        private bool nodeIsDisallowedByDirective(SearchNode n)
        {
            foreach (IDirective directive in this.listOfDirectives)
            {
                if (directive.disallow(n))
                    return true;
            }
            return false;
        }

        private void sendSolutionToArduino(Sequence solutionSeq, string failureExplanation)
        {
            if (failureExplanation != null)
                this.searchToGui.sendSolutionToArduino(solutionSeq, failureExplanation);
            else
                this.searchToGui.sendSolutionToArduino(solutionSeq);
        }

        private void showBestMatch(string UIText, SearchNode seedNode)
        {
            this.searchToGui.refreshTextLog = UIText + seedNode.solutionExpand();
        }

        private void stringExpandForGUICommunicator(SearchNode n, string failureExplanation)
        {
            if (n == null)
                return;
            else
                n.stringExpandForGUICommunicator(failureExplanation);
        }

        private void updateUIBestSoFarNode()
        {
            this.searchToGui.bestSoFar = this.bestSoFar;
        }

        private void updateUIGoalNode()
        {
            this.searchToGui.goalNode = this.goalNode;
        }
    }
}
