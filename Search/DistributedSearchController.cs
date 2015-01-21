using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;
using BeowulfCluster;
using UsefulStaticMethods;
using System.Threading;
using SearchAbstractDataTypes;
using ActionSet;

namespace Search
{
    public class DistributedSearchController : Distributed_OR_DataBaseSearchController, IDistributedSearch
    {
        public DistributedSearchController(int precision, SearchNode startNode,
            SearchNode goalNode, TCP_IP_Client michelleComp, ref SearchPartitionToGUI spg)
        {
            this.initializeSearch(startNode, goalNode, ref spg);
            this.searchToCluster = new SearchToCluster(michelleComp);
        }

        public bool closedSetContainsNode(SearchNode daughter)
        {
            foreach (SearchNode n in this.closedSet)
            {
                if (n.matches(daughter))
                {
                    return true;
                }
            }
            return false;
        }

        public void depthFirstWithThread()
        {
            Thread t = ThreadMethods.createNewBackgroundThread(depthFirstSearch, "DistributedSearch");
            t.Start();
        }

        public void depthFirstSearch()
        {
            if (this.startNode.matches(this.goalNode))
            {
                alertUISolutionHasBeenFound(this.startNode);
                return;
            }

            initiateOpenStack();
            
            while (thereIsStillANodeToCheck(this))
            {
                if (stopIsRequested())
                    return;

                SearchNode n = moveFirstNodeFromOpenStackToClosedAndReturnIt();

                List<SearchNode> daughtersOfN = nodeExpand(n);
                foreach (SearchNode daughter in daughtersOfN)
                {
                    if (!closedSetContainsNode(daughter))
                    {
                        daughter.increaseNodeDepth();
                        
                        if (daughter.matches(this.goalNode))
                        {
                            alertUISolutionHasBeenFound(daughter);
                            return;
                        }
                        decideWhetherToAddNodeToOpenStack(daughter);
                        maybeSpawn(daughter);
                    }
                }
            }
            alertUI("No solution found");
            return;
        }

        override protected List<SearchNode> nodeExpand(SearchNode parent)
        {
            List<SearchNode> daughters = new List<SearchNode>();

            foreach (ElementaryAction methodName in ActionMap.allowedArtieActions)
            {
                SearchNode daughter = parent.deriveDaughterNode(methodName);
                daughter.computeValue(this.goalNode);
                daughters.Add(daughter);
            }
            return daughters;
        }
    }
}
