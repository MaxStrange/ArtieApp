using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;
using BeowulfCluster;
using DataBase;
using UsefulStaticMethods;
using System.Threading;
using SearchAbstractDataTypes;
using ActionSet;

namespace Search
{
    public class DistributedDataBaseSearchController : Distributed_OR_DataBaseSearchController, IDistributedSearch, IDataBaseSearch
    {

        public DistributedDataBaseSearchController(int precision, SearchNode start, SearchNode goal,
            DataBaseAccessor DBAccess,
            ref SearchPartitionToGUI spg, TCP_IP_Client michelleComp)
        {
            this.initializeSearch(start, goal, ref spg);
            this.connection = new SearchDataBaseAccessor(DBAccess);
            this.searchToCluster = new SearchToCluster(michelleComp);

///            buildGoalNode();
///            this.listOfSequencesContainingGoalNode =
///                this.DBAccessor.reconstituteAllSequencesContainingNode(this.goalNode);
///            this.listOfGoalNodeAssociations =
///                this.ascnManager.retrieveAssociations(this.goalNode);
///            buildAssociationPyramid();
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
            Thread t = ThreadMethods.createNewBackgroundThread(depthFirstSearch, "DistributedDBSearch");
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

        protected override void spawn(SearchNode n)
        {

            throw new NotImplementedException();
        }
    }
}
