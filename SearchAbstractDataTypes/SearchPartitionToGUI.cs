using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAbstractDataTypes
{
    [Serializable]
    public class SearchPartitionToGUI
    {
//Public fields
        private SearchNode _bestSoFar = null;
        public SearchNode bestSoFar
        {
            get { return this._bestSoFar; }
            set { this._bestSoFar = value; }
        }

        private string _failureExplanation = null;
        public string failureExplanation
        {
            get { return this._failureExplanation; }
            set { this._failureExplanation = value; }
        }

        private SearchNode _goalNode = null;
        public SearchNode goalNode
        {
            get { return this._goalNode; }
            set { this._goalNode = value; }
        }

        private string _refreshTextLog = "";
        public string refreshTextLog
        {
            get { return this._refreshTextLog; }
            set { this._refreshTextLog = value; }
        }

        private bool _searching = false;
        public bool searching
        {
            get { return this._searching; }
            set { this._searching = value; }
        }

        private bool _solutionFound = false;
        public bool solutionFound
        {
            get { return this._solutionFound; }
            set { this._solutionFound = value; }
        }

        private Sequence _solutionSequence = null;
        public Sequence solutionSequence
        {
            get { return this._solutionSequence; }
            private set { this._solutionSequence = value; }
        }

        private bool _stopRequested = false;
        public bool stopRequested
        {
            get { return this._stopRequested; }
            set { this._stopRequested = value; }
        }



//Constructors
        public SearchPartitionToGUI()
        {
        }



//Public methods
        public void sendSolutionToArduino(Sequence solutionSeq, string failureExplanation = null)
        {
            this.failureExplanation = failureExplanation;
            this.solutionSequence = solutionSeq;
        }
    }
}
