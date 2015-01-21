using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;
using ActionSet;
using System.Diagnostics;

namespace SearchAbstractDataTypes
{
    [Serializable]
    public class Sequence : List<SearchNode>
    {
//Public fields
        private SearchNode _endNode = new SearchNode();
        public SearchNode endNode
        {
            get
            {
                if (this.Count > 0)
                {
                    this._endNode = this[this.Count - 1];
                    return this._endNode;
                }
                else
                {
                    throw new SequenceNotBuiltException();
                }
            }
        }
        private SearchNode _startNode = new SearchNode();
        public SearchNode startNode
        {
            get
            {
                if (this.Count > 0)
                {
                    this._startNode = this[0];
                    return this._startNode;
                }
                else
                {
                    throw new SequenceNotBuiltException();
                }
            }
        }


//Public methods
        public void appendSequence(Sequence seqToAppend)
        {
            seqToAppend.startNode.parent = this._endNode;
            buildFromSearchNode(seqToAppend.endNode);
        }

        public void buildFromSearchNode(SearchNode seed)
        {
            this.Clear();
            this.Add(seed);
            while (seed.parent != null)
            {
                this.Insert(0, seed.parent);
                seed = seed.parent;
            }
        }

        public override string ToString()
        {
            ElementaryAction[] elementaryActionArray = this.toElementaryActionList().ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (ElementaryAction action in elementaryActionArray)
            {
                sb.Append(" ").Append(action.ToString());
            }

            return sb.ToString();
        }



//Internal methods
        internal void reverseSequence()
        {
            SearchNode[] tempArray = new SearchNode[this.Count];
            for (int i = 0; i < tempArray.Length; i++)
            {
                tempArray[i] = this[Count - (i + 1)];
            }
            for (int j = 0; j < tempArray.Length; j++)
            {
                this[j] = tempArray[j];
            }
        }



//Private methods
        private List<CompoundAction> toCompoundActionList()
        {
            List<CompoundAction> compoundActionList = new List<CompoundAction>();
            foreach (SearchNode n in this)
            {
                compoundActionList.Add(n.methodUsedToDeriveNodeFromParent);
            }

            return compoundActionList;
        }

        private List<ElementaryAction> toElementaryActionList()
        {
            List<CompoundAction> compoundActionList = this.toCompoundActionList();
            List<ElementaryAction> elementaryActionList = new List<ElementaryAction>();
            
            foreach (CompoundAction compoundAction in compoundActionList)
            {
                List<ElementaryAction> actionName = compoundAction.parseCompoundMethodIntoElementaryActions();
                elementaryActionList.AddRange(actionName);
            }

            return elementaryActionList;
        }
    }
}
