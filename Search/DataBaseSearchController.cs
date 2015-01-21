using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using PerceptionSets;
using DataBase;
using System.Threading;
using UsefulStaticMethods;
using SearchAbstractDataTypes;
using ActionSet;

namespace Search
{
    public class DataBaseSearchController : Distributed_OR_DataBaseSearchController, IDataBaseSearch
    {
//Constructors
        public DataBaseSearchController(DataBaseAccessor connection, int precision,
            ref SearchPartitionToGUI spg, SearchNode start, SearchNode goal)
        {
            initializeSearch(start, goal, ref spg);
            base.connection = new SearchDataBaseAccessor(connection);
        }


//Public methods
        public void AStarWithThread()
        {
            Thread t = ThreadMethods.createNewBackgroundThread(AStarSearch, "DBSearchThread");
            t.Start();
        }

        public override void AStarSearch()
        {
            checkForKnownSolution();

            initializeOpenAndClosed();
            while (openQueueContainsANode())
            {
                if (stopIsRequested())
                    return;

                SearchNode n = moveFirstNodeOnOpenToClosedAndAlertUI();

                if (n.matches(base.goalNode))
                {
                    alertUISolutionHasBeenFound(n);
                    return;
                }

                expandNodeAndAddDaughtersToOpen(n);
            }
            alertUISolutionHasNotBeenFound("Solution could not be located, returning best match: ");
            return;
        }



//Protected methods
        override protected List<SearchNode> nodeExpand(SearchNode parent)
        {
            //TODO : you should load in the acceptable methods for deriving daughter nodes
            //at initialization of search and then use that list every iteration, rather
            //than accessing the database every time nodeExpand gets called.
            List<List<CompoundAction>> actionsFromActionSet =
                this.connection.CollectActions();
            string action = "";
            List<SearchNode> expansionOfNode = new List<SearchNode>();
            foreach (List<CompoundAction> elementOfActionSet in actionsFromActionSet)
            {
                SearchNode daughter = new SearchNode(ref this.searchToGui);
                action = elementOfActionSet[0];
                daughter = new SearchNode();//parent.deriveDaughterNode(action);
                for (int i = 1; i < elementOfActionSet.Count; i++)
                {
                    daughter.modifyNodeAccordingToAction(elementOfActionSet[i]);
                }
                expansionOfNode.Add(daughter);
            }
            return expansionOfNode;
        }



//Private methods
        private Sequence checkForKnownSolution()
        {
            Sequence knownSolution = base.connection.checkDataBaseForSolution(base.goalNode);
            if (knownSolution.Count > 0)
            {
                SearchNode bestMatch = findClosestNode(knownSolution);
                base.goalNode = bestMatch;
                base.solutionTailAsString = knownSolution.endNode.solutionExpand(bestMatch);
                base.solutionTail = knownSolution;
                dealWithAssociations();
            }
            return knownSolution;
        }

        private bool checkIfKnownSolutionStartsAtCurrentNode()
        {
            if (this.startNode.matches(knownSolution.startNode))
                return true;
            else
                return false;
        }

        private void dealWithAssociations()
        {
        }

        private void decideWhichNodeIsGoal()
        {
            Sequence knownSolution = new Sequence();
            knownSolution = checkForKnownSolution();
            if (knownSolution.Count > 0)
                this.goalNode = knownSolution.startNode;
        }

        private void deriveDaughtersToSearch(SearchNode parent)
        {
            List<SearchNode> tempList = nodeExpand(parent);
            foreach (SearchNode daughter in tempList)
            {
                bool daughterOnClosed = false;
                foreach (SearchNode m in closedSet)
                {
                    if (m.matches(daughter))
                        daughterOnClosed = true;
                }

                if (daughterOnClosed) continue;
                openQueue.add(daughter);
            }
        }

        private SearchNode findClosestNode(Sequence seqToCheck)
        {
            SearchNode bestSoFar = new SearchNode(ref this.searchToGui);
            bestSoFar = seqToCheck.startNode;
            foreach (SearchNode n in seqToCheck)
            {
                if (n.computeDifferenceBetweenThisNodeAnd(this.startNode) <
                    bestSoFar.computeDifferenceBetweenThisNodeAnd(this.startNode))
                {
                    bestSoFar = n;
                }
            }
            return bestSoFar;
        }

        private Sequence intersect(Sequence a, Sequence b)
        {
            Sequence result = new Sequence();

            int startNodeNumberA = -1;
            int startNodeNumberB = -1;

            for (int i = 0; i < a.Count; i++)
            {
                //find if/where the sequences start to overlap
                for (int j = 0; j < b.Count; j++)
                {
                    if (a[i].matches(b[j]))
                    {
                        startNodeNumberA = i;
                        startNodeNumberB = j;
                        break;//from j's for loop
                    }
                    if (startNodeNumberA != (-1)) break;//from i's loop
                }
            }

            if (startNodeNumberA != -1)
            {
                result.Add(a[startNodeNumberA]);
                startNodeNumberA++;
                startNodeNumberB++;
                bool stillMatching = true;

                while ((startNodeNumberA <= a.Count) &&
                    (startNodeNumberB <= b.Count) && stillMatching)
                {
                    stillMatching = false;
                    if (a[startNodeNumberA].matches(b[startNodeNumberB]))
                    {
                        stillMatching = true;
                        result.Add(a[startNodeNumberA]);
                    }
                    startNodeNumberA++;
                    startNodeNumberB++;
                }
            }
            return result;
        }

        private void modifySearchViaAssociationHeuristics()//List<Association> associations)
        {
            //List<Association> firedAssociations = new List<Association>();
            //foreach (Association ascn in associations)
            //{
            //    if (ascn.maybeFireAssociation())
            //        firedAssociations.Add(ascn);
            //}
            //foreach (Association ascn in firedAssociations)
            //{
            //    if (ascn is NodeToPreceedingActionAssociation)
            //    {

            //    }
            //    else if (ascn is NodeToNodeAssociation)
            //    {
            //    }
            //}
        }


        private void solutionHasBeenFound(SearchNode solutionNode)
        {
            Sequence solutionSeq = new Sequence();
            solutionSeq.buildFromSearchNode(solutionNode);
            solutionSeq.appendSequence(this.solutionTail);
            alertUISolutionHasBeenFound(solutionSeq.startNode);
            this.connection.solutionSave(solutionSeq);
        }
    }
}
