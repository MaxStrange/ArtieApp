using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;
using SearchAbstractDataTypes;
using UsefulStaticMethods;

namespace Search
{
    public class Distributed_OR_DataBaseSearchController : SearchController
    {
        //Distributed Search
        protected SearchToCluster searchToCluster = null;
        protected Stack<SearchNode> openStack = new Stack<SearchNode>();
        protected HashSet<SearchNode> closedSet = new HashSet<SearchNode>();
        public const int maxDepth = 10;
        protected List<Sequence> listOfSequencesContainingGoalNode = null;
        public const int maxSpawnings = 4;
        protected int currentNumberOfSpawnedSearchesStillAlive = 0;
        public const int minimumSpawnProbability = 35;
        public const int maximumSpawnProbability = 95;
        protected int spawnProbability = minimumSpawnProbability;
        private Random spawnGenerator = new Random();



        //DataBase Search
        protected SearchDataBaseAccessor connection = null;
        protected bool solutionFound = false;
        protected string solutionTailAsString = null;
        protected Sequence solutionTail = null;
        protected Sequence knownSolution = null;


        protected void alertUI(string failureExplanation)
        {
            this.stopRequestedFlag = true;
            this.searchToGui.refreshTextLog = failureExplanation;
            alertUISearchHasStopped();
        }

        protected bool thereIsStillANodeToCheck(SearchController s)
        {
            if (s is IDistributedSearch)
            {
                return doesOpenStackContainAnyNodes();
            }
            else
            {
                return (base.openQueue.count > 1) ? true : false;
            }
        }


        //Distributed Search
        protected bool checkIfWeSpawn()
        {
            int p_spawn = this.spawnGenerator.Next(100);
            if (p_spawn < this.spawnProbability)
            {
                this.spawnProbability = minimumSpawnProbability;
                return true;
            }
            else
            {
                this.spawnProbability += 20;
                if (this.spawnProbability > maximumSpawnProbability)
                    this.spawnProbability = maximumSpawnProbability;
                return false;
            }
        }

        protected void decideWhetherToAddNodeToOpenStack(SearchNode n)
        {
            if (n.searchSpaceInformation.depth < maxDepth)
                this.openStack.Push(n);
        }

        private bool doesOpenStackContainAnyNodes()
        {
            if (this.openStack.Count > 0)
                return true;
            return false;
        }

        protected void initiateOpenStack()
        {
            this.startNode.searchSpaceInformation.depth = 0;
            SearchNode startCopy = new SearchNode(ref this.searchToGui);
            startCopy.copy(this.startNode);
            this.openStack.Push(startCopy);
        }

        protected void maybeSpawn(SearchNode n)
        {
            if (this.currentNumberOfSpawnedSearchesStillAlive >= maxSpawnings)
                return;

            if (checkIfWeSpawn())
                spawn(n);
        }

        protected SearchNode moveFirstNodeFromOpenStackToClosedAndReturnIt()
        {
            SearchNode n = this.openStack.Pop();
            this.closedSet.Add(n);

            n.stringExpandForGUICommunicator();

            return n;
        }

        virtual protected void spawn(SearchNode n)
        {
            return;
            throw new NotImplementedException();
        }
    }
}
