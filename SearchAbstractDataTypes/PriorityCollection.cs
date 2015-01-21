using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulStaticMethods;

namespace SearchAbstractDataTypes
{
    public class PriorityCollection
    {
//Protected fields
        private SearchNode _goalNode = null;
        protected SearchNode goalNode
        {
            get { return this._goalNode; }
            set { this._goalNode = value; }
        }
        private List<SearchNode> _list = new List<SearchNode>();
        protected List<SearchNode> list
        {
            get { return this._list; }
            set { this._list = value; }
        }


//Private fields
        private int _tailIndex = 0;
        private int tailIndex
        {
            get { return (this.list.Count - 1); }
        }


//Public methods
        public virtual void add(SearchNode n)
        {
            if (valueIsNotComputed(n))
            {
                n.computeValue(this.goalNode);
                checkToPriorityAdd(n);
                return;
            }
            else
            {
                checkToPriorityAdd(n);
                return;
            }
        }


//Private methods
        /// <summary>
        /// Adds SearchNode n to the list such that n is in front of all nodes with
        /// a greater value.
        /// Throws a NegativeValueException if passed a searchNode whose "value"
        /// is negative.
        /// </summary>
        /// <param name="n"></param>
        private void checkToPriorityAdd(SearchNode n)
        {
            if (valueIsNotComputed(n))
                throw new ValueNotCalculatedException();

            if (collectionIsEmpty())
                this.list.Add(n);
            else
                priorityAdd(n);
        }

        private bool collectionIsEmpty()
        {
            if (this.list.Count <= 0)
                return true;
            else
                return false;
        }

        private void negativeInsert(SearchNode n, int index)
        {
            if (index == this.tailIndex)
            {
                this.list.Add(n);
                return;
            }

            while (valuesOfAdjacentNodesAreSame(index))
            {
                ++index;
                if (index == this.tailIndex)
                {
                    this.list.Add(n);
                    return;
                }
            }
            this.list.Insert(index + 1, n);
            return;
        }

        private bool nodeValuesAreTheSame(SearchNode n, SearchNode m)
        {
            double nValue = n.searchSpaceInformation.value;
            double mValue = m.searchSpaceInformation.value;

            if (nValue.doublesRepresentTheSameNumber(mValue))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Inserts a SearchNode n in front of any node that has the same value as it
        /// starting at index.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="index"></param>
        private void positiveInsert(SearchNode n, int index)
        {
            if (index == 0)
            {
                this.list.Insert(index, n);
                return;
            }

            while (valuesOfAdjacentNodesAreSame(index, false))
            {
                --index;
                if (index == 0)
                {
                    this.list.Insert(index, n);
                    return;
                }
            }
            this.list.Insert(index, n);
            return;
        }

        private void priorityAdd(SearchNode n)
        {
            int lowBound = 0;
            int highBound = this.tailIndex;
            int middle = (int)((lowBound + highBound) / 2);
            
            while (lowBound <= highBound)
            {
                if (nodeValuesAreTheSame(n, this.list[middle]))
                {
                    this.list.Insert(middle + 1, n);
                    return;
                }
                else if (n.searchSpaceInformation.value <
                    this.list[middle].searchSpaceInformation.value)
                {
                    highBound = middle - 1;
                }
                else
                {
                    lowBound = middle + 1;
                }
                middle = (int)((lowBound + highBound) / 2);
            }
            priorityInsert(n, middle);
        }

        private void priorityInsert(SearchNode n, int index)
        {
            if (index < 0)
            {
                this.list.Insert(0, n);
                return;
            }

            if (this.list[index].searchSpaceInformation.value <
                n.searchSpaceInformation.value)
            {
                negativeInsert(n, index);
                return;
            }
            else
            {
                positiveInsert(n, index);
                return;
            }
        }

        private bool valueIsNotComputed(SearchNode n)
        {
            if (n.searchSpaceInformation.valueIsNotComputed())
                return true;
            else
                return false;
        }

        /// <summary>
        /// Compares the value of the node at the specified index with either the node
        /// immediately after or the node immediately before it. To compare with the node
        /// that comes before the node at index, pass false as the second argument.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="checkIndexPlusOne"></param>
        /// <returns></returns>
        private bool valuesOfAdjacentNodesAreSame(int index, bool checkIndexPlusOne = true)
        {
            switch (checkIndexPlusOne)
            {
                case (true):
                    return nodeValuesAreTheSame(this.list[index], this.list[index + 1]);
                case (false):
                    return nodeValuesAreTheSame(this.list[index], this.list[index - 1]);
                default:
                    throw new NotImplementedException();//This method should never get here,
                //if it has, this is not implemented correctly.
            };
        }
    }
}
