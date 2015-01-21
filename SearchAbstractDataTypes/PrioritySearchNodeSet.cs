using AbstractDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulStaticMethods;

namespace SearchAbstractDataTypes
{
    public class PrioritySearchNodeSet : PriorityCollection
    {
        private int _maxIndex;
        private int maxIndex
        {
            get { return this._maxIndex; }
            set { this._maxIndex = value; }
        }
        private int _minIndex = 0;
        private int minIndex
        {
            get { return this._minIndex; }
            set { this._minIndex = value; }
        }


//Constructors
        public PrioritySearchNodeSet(SearchNode goalNode)
        {
            base.goalNode = goalNode;
        }


//Public methods
        public override void add(SearchNode n)
        {
            base.add(n);
            this.maxIndex = base.list.Count - 1;
        }

        public bool contains(SearchNode n)
        {
            if (n.searchSpaceInformation.valueIsNotComputed())
                n.computeValue(base.goalNode);

            while (this.minIndex <= this.maxIndex)
            {
                int middle = (int)((this.minIndex + this.maxIndex) / 2);
                if (NumberMethods.A_FallsWithinPercentOf_B(n.searchSpaceInformation.value,
                    base.list[middle].searchSpaceInformation.value, n.precision.value))
                {
                    return rangeAroundHit(n);
                }
                else if (n.searchSpaceInformation.value <
                    base.list[middle].searchSpaceInformation.value)
                {
                    this.maxIndex = middle - 1;
                }
                else
                {
                    this.minIndex = middle + 1;
                }
            }
            resetMinAndMaxIndexes();
            return false;
        }


//Private methods
        private bool rangeAroundHit(SearchNode n)
        {
            for (int i = this.minIndex; i <= this.maxIndex; i++)
            {
                if (n.matches(base.list[i], new Percent(0.085)))
                {
                    resetMinAndMaxIndexes();
                    return true;
                }
            }
            resetMinAndMaxIndexes();
            return false;
        }

        private void resetMinAndMaxIndexes()
        {
            this.minIndex = 0;
            this.maxIndex = base.list.Count - 1;
        }
    }
}
