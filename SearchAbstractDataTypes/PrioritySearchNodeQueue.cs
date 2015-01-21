using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;
using UsefulStaticMethods;

namespace SearchAbstractDataTypes
{
    public class ValueNotCalculatedException : Exception
    {
    }

    public class PrioritySearchNodeQueue : PriorityCollection
    {
//Public fields
        public int count
        {
            get { return base.list.Count; }
        }
        public SearchNode firstNode
        {
            get 
            {
                if (base.list.Count > 0)
                    return base.list[0];
                else
                    return null;
            }
        }
        

//Private fields        
        private SearchNode lastNode
        {
            get { return base.list[base.list.Count - 1]; }
        }


        
//Constructors
        public PrioritySearchNodeQueue(SearchNode goalNode)
        {
            base.goalNode = goalNode;
        }


//Public methods
        public SearchNode pop()
        {
            SearchNode n = this.firstNode;
            base.list.Remove(this.firstNode);
            return n;
        }

        /// <summary>
        /// Removes the last 90% of the queue.
        /// </summary>
        public void prune()
        {
            int oneTenthTheWay = (base.list.Count * 0.1).roundDoubleToClosestInt();
            if (oneTenthTheWay <= 0)
                oneTenthTheWay = 1;//Always leave at least one node.
            
            int numberOfNodesToRemove = base.list.Count - oneTenthTheWay;

            base.list.RemoveRange(oneTenthTheWay, numberOfNodesToRemove);
        }
    }
}