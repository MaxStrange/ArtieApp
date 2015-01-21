using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase;
using MySql;
using MySql.Data.MySqlClient;
using SearchAbstractDataTypes;
using ActionSet;

namespace Search
{
    public class SearchDataBaseAccessor
    {
        private DataBaseAccessor _dbAccessor = null;
        private DataBaseAccessor dbAccessor
        {
            get { return this._dbAccessor; }
            set { this._dbAccessor = value; }
        }

        public SearchDataBaseAccessor(DataBaseAccessor dbAccessor)
        {
            this.dbAccessor = dbAccessor;
        }

        private double calculateActionSaveProbability(int numberOfTimesOccurrs)
        {
            if (numberOfTimesOccurrs < 1) numberOfTimesOccurrs = 1;
            double tanhDouble = ((double)numberOfTimesOccurrs / (double)10);
            double P = Math.Tanh(tanhDouble);
            return P;
        }

        private double calculateDeletionProbability(int n)
        {
            double deletionP = ((n == 1) ? 0.3 : (0.5 *
                calculateDeletionProbability(n - 1)));
            return deletionP;
        }

        private bool checkForAction(CompoundAction action)
        {
            List<CompoundAction> actionsInDB = retrieveAllActionNames();
            foreach (string dbAction in actionsInDB)
            {
                if (action.Equals(dbAction))
                {
                    return true;
                }
            }
            return false;
        }

        public bool checkIfInDB(Sequence seq)
        {
            List<Sequence> listOfSeqs = reconstituteAllSequences();
            foreach (Sequence sequence in listOfSeqs)
            {
                int a = 0;
                int b = 0;
                //find where seq and sequence start to match
                for (int i = 0; i < sequence.Count; i++)
                {
                    for (int j = 0; j < seq.Count; j++)
                    {
                        if (sequence[i].matches(seq[j]))
                        {
                            //a match has been found.
                            //see if the rest of seq matches
                            a = j;
                            b = i;
                            while (seq[a].matches(sequence[b]))
                            {
                                a--;
                                b++;
                                if (b >= sequence.Count) break;//from while
                                if (a < 0) return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void checkDBForActionsToDelete(List<Sequence> listOfSequences)
        {
            List<string> actionNames = retrieveNonElementaryActionNames();

            foreach (string a in actionNames)
            {
                updateOrDeleteAction(a, listOfSequences);
            }
        }

        public Sequence checkDataBaseForSolution(SearchNode goalNode)
        {
            Sequence solution = new Sequence();
            List<Sequence> seqsFromDB = reconstituteAllSequences();
            bool match = false;
            foreach (Sequence solutionFromDB in seqsFromDB)
            {
                foreach (SearchNode s in solutionFromDB)
                {
                    if (s.matches(goalNode))
                    {
                        match = true;
                        solution.buildFromSearchNode(s);
                    }
                    if (match) break;//from foreach
                }
                if (match) break;//from i's for loop
            }
            return solution;
        }

        private void checkSequenceForNewActions(Sequence candidate, List<Sequence> listOfSequences)
        {
            List<List<Tuple<CompoundAction, CompoundAction>>> listOflistsOfDBPairs = new List<List<Tuple<CompoundAction, CompoundAction>>>();
            List<Tuple<CompoundAction, CompoundAction>> listOfPairs = createSequencePairs(candidate);

            foreach (Sequence seq in listOfSequences)
            {
                List<Tuple<CompoundAction, CompoundAction>> listOfPairsDB = createSequencePairs(candidate);
                listOflistsOfDBPairs.Add(listOfPairsDB);
            }

            for (int i = 0; i < listOfPairs.Count; i++)
            {
                //for each pair, check if it should be a new action
                createNewAction(listOfPairs[i], listOflistsOfDBPairs);
            }
        }

        public List<List<CompoundAction>> CollectActions()
        {
            string cmdText = "SELECT actionName FROM actions";
            List<string> listOfActionNames = executeQuery(cmdText);

            List<List<CompoundAction>> actionsFromActionSet = new List<List<CompoundAction>>();
            for (int i = 0; i < listOfActionNames.Count; i++)
            {
                string actionName = listOfActionNames[i];
                List<ElementaryAction> elementOfActionSet = new List<ElementaryAction>();

                elementOfActionSet.AddRange(new CompoundAction(actionName).parseCompoundMethodIntoElementaryActions());
            }
            return actionsFromActionSet;
        }

        private CompoundAction composeAction(Tuple<CompoundAction, CompoundAction> actionPair)
        {
            List<CompoundAction> tupleAsList = new List<CompoundAction>();
            tupleAsList.Add(actionPair.Item1);
            tupleAsList.Add(actionPair.Item2);
            CompoundAction action = new CompoundAction(tupleAsList);
            return action;
        }

        private int countNumberOfTimesActionIsInDB(string action, List<Sequence> listOfSequences)
        {
            int numberOfTimesObserved = 0;
            foreach (Sequence s in listOfSequences)
            {
                foreach (SearchNode node in s)
                {
                    if (node.methodUsedToDeriveNodeFromParent.Equals(action))
                        numberOfTimesObserved++;
                }
            }
            return numberOfTimesObserved;
        }

        private int countNumberOfTimesActionPairIsInDB(List<List<Tuple<CompoundAction, CompoundAction>>> listOflistsOfDBPairs,
            string action)
        {
            int numberOfTimesOccurrs = 0;
            foreach (List<Tuple<CompoundAction, CompoundAction>> dbSequence in
                listOflistsOfDBPairs)
            {
                foreach (Tuple<CompoundAction, CompoundAction> dbPair in dbSequence)
                {
                    if (action.Equals(dbPair.Item1 + "_" +
                        dbPair.Item2)) numberOfTimesOccurrs++;
                }
            }
            return numberOfTimesOccurrs;
        }

        private void createNewAction(Tuple<CompoundAction, CompoundAction> pair, List<List<Tuple<CompoundAction, CompoundAction>>> listOflistsOfDBPairs)
        {
            CompoundAction action = composeAction(pair);
            bool alreadyInDB = checkForAction(action);
            if (alreadyInDB) return;
            int numberOfTimesOccurrs = countNumberOfTimesActionPairIsInDB(listOflistsOfDBPairs, action);
            double P = calculateActionSaveProbability(numberOfTimesOccurrs);
            maybeSaveAction(P, action);
        }

        private List<Tuple<CompoundAction, CompoundAction>> createSequencePairs(Sequence seq)
        {
            List<Tuple<CompoundAction, CompoundAction>> listOfPairs = new List<Tuple<CompoundAction, CompoundAction>>();
            for (int i = 0; i < (seq.Count - 1); i++)
            {
                //don't save any that have the start node in them.
                if (seq[i].methodUsedToDeriveNodeFromParent.Equals(ElementaryAction.START)
                    || seq[i + 1].methodUsedToDeriveNodeFromParent.Equals(ElementaryAction.START))
                    continue;
                Tuple<CompoundAction, CompoundAction> pair = new Tuple<CompoundAction, CompoundAction>(seq[i].methodUsedToDeriveNodeFromParent, seq[i + 1].methodUsedToDeriveNodeFromParent);
                listOfPairs.Add(pair);
            }
            return listOfPairs;
        }

        private void executeNonQuery(string commandText)
        {
            this._dbAccessor.executeNonQuery(commandText);
        }

        private List<string> executeQuery(string commandText)
        {
            return this._dbAccessor.executeQuery(commandText);
        }

        private bool maybeDeleteAction(double P, string action)
        {
            Random randomGenerator = new Random();

            bool deleted = false;

            P = 100 * P;
            int dp = randomGenerator.Next(100);

            string cText = "";

            if (dp < P)
            {
                cText = "DELETE FROM actions WHERE actionName = '" + action + "'";
                
                executeNonQuery(cText);
                deleted = true;
            }
            return deleted;
        }

        private bool maybeSaveAction(double P, string action)
        {
            Random randomGenerator = new Random();

            bool saved = false;

            P = P * 100;
            int p = randomGenerator.Next(100);
            if (p <= P)
            {
                string commandText = "INSERT INTO actions " +
                    "(actionName, isElementary) VALUES ('"
                    + action + "', 0)";
                
                executeNonQuery(commandText);
                saved = true;
            }
            return saved;
        }

        public void Overwrite(Sequence seqToSave, Sequence seqToRemove)
        {
            saveSequence(seqToSave);
            removeSequence(seqToRemove);
        }

        public List<Sequence> reconstituteAllSequences()
        {
            string cmdText = "SELECT nodeID FROM listOfNodes "
             + "WHERE isStartNode = " + 1;
            List<string> startNodeIDs = executeQuery(cmdText);

            List<Sequence> listOfSeqs = new List<Sequence>();
            foreach (string ID in startNodeIDs)
            {
                listOfSeqs.Add(
                    reconstituteSequence(reconstituteNode(ID)));
            }
            return listOfSeqs;
        }

        ///        public List<Sequence> reconstituteAllSequencesContainingNode(SearchNode n)
        ///        {

        ///        }

        public SearchNode reconstituteNode(string nodeID)
        {
            SearchNode n = new SearchNode();

            string cmdText = "SELECT nodeID, parentID, " +
            "daughterID, methodName, thetaNHat, xPos, yPos, " +
            "zPos, nx, ny, nz, ox, oy, oz, px, py, pz " +
            "FROM ListOfNodes WHERE nodeID = " + nodeID;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(cmdText,
                this._dbAccessor.connection);
            reader = command.ExecuteReader();
            reader.Read();

            string parentID = reader.GetString(1);
            string daughterID = reader.GetString(2);
            string methodName = reader.GetString(3);
            string thetaNHat = reader.GetString(4);
            string xPos = reader.GetString(5);
            string yPos = reader.GetString(6);
            string zPos = reader.GetString(7);
            string nx = reader.GetString(8);
            string ny = reader.GetString(9);
            string nz = reader.GetString(10);
            string ox = reader.GetString(11);
            string oy = reader.GetString(12);
            string oz = reader.GetString(13);
            string px = reader.GetString(14);
            string py = reader.GetString(15);
            string pz = reader.GetString(16);

            reader.Close();

            //n.nodeID = int.Parse(nodeID);
            //n.parentID = int.Parse(parentID);
            //n.daughterID = int.Parse(daughterID);
            //n.methodUsedToDeriveNodeFromParent = methodName;
            //n.perceptionState_Body.location = new Matrices.Point(double.Parse(xPos), double.Parse(yPos), double.Parse(zPos));
            //n.perceptionState_Body.artieFrame.nVector.x = double.Parse(nx);
            //n.perceptionState_Body.artieFrame.nVector.y = double.Parse(ny);
            //n.perceptionState_Body.artieFrame.oVector.x = double.Parse(ox);
            //n.perceptionState_Body.artieFrame.oVector.y = double.Parse(oy);

            return n;
        }

        public void removeSequence(Sequence seq)
        {
            List<string> nodeIDs = new List<string>();
            foreach (SearchNode n in seq)
            {
                nodeIDs.Add("" + n.searchSpaceInformation.nodeID);
            }

            for (int i = 0; i < nodeIDs.Count; i++)
            {
                string cmdText = "DELETE FROM listOfNodes WHERE " +
                    "nodeID = " + nodeIDs[i];
                executeNonQuery(cmdText);
            }
        }

        private List<CompoundAction> retrieveAllActionNames()
        {
            string cmndText = "SELECT actionName FROM actions";
            List<string> actionsInDBAsStrings = executeQuery(cmndText);
            List<CompoundAction> actionsInDB = new List<CompoundAction>();
            foreach (string s in actionsInDBAsStrings)
            {
                actionsInDB.Add(new CompoundAction(s));
            }

            return actionsInDB;
        }

        private List<string> retrieveNonElementaryActionNames()
        {
            string commText = "SELECT actionName FROM actions WHERE " +
                "isElementary = 0";
            List<string> actionNames = executeQuery(commText);
            return actionNames;
        }

        private List<string> retrieveStartNodeIDs()
        {
            string cmndText = "SELECT nodeID FROM listOfNodes WHERE " +
                "isStartNode = 1";
            List<string> startNodeIDs = executeQuery(cmndText);
            return startNodeIDs;
        }

        public void saveSequence(Sequence seq)
        {
            //string cmdText = "";

            //int parentID = -1;
            //int daughterID = -1;

            //foreach (SearchNode seqNode in seq)
            //{
            //    int isStartNode = 0;
            //    if (seqNode.parent == null)
            //    {
            //        isStartNode = 1;
            //        parentID = -1;
            //    }

            //    cmdText = "INSERT INTO listOfNodes(" +
            //        "daughterID, methodName, thetaNHat, xPos, yPos, " +
            //        "zPos, nx, ny, nz, ox, oy, oz, px, py, pz, " +
            //        "isStartNode) " +
            //        "VALUES(" +
            //        daughterID + ", " + "'" + seqNode.methodUsedToDeriveNodeFromParent +
            //        "'" + ", " + 0.0 + ", " +
            //        seqNode.perceptionState_Body.location.x + ", " + seqNode.perceptionState_Body.location.y
            //        + ", " + seqNode.perceptionState_Body.location.z + ", " +
            //        seqNode.perceptionState_Body.artieFrame.nVector.x + ", " + seqNode.perceptionState_Body.artieFrame.nVector.y +
            //        ", " + seqNode.perceptionState_Body.artieFrame.nVector.z + ", " + seqNode.perceptionState_Body.artieFrame.oVector.x + ", " +
            //        seqNode.perceptionState_Body.artieFrame.oVector.y + ", " + seqNode.perceptionState_Body.artieFrame.oVector.z + ", " +
            //        seqNode.perceptionState_Body.artieFrame.pVector.x + ", " + seqNode.perceptionState_Body.artieFrame.pVector.y + ", " +
            //        seqNode.perceptionState_Body.artieFrame.pVector.z + ", " + isStartNode + ")";
            //    executeNonQuery(cmdText);

            //    cmdText = "SELECT last_insert_id()";
            //    daughterID = int.Parse(executeQuery(cmdText)[0]);
            //    parentID = 1 + daughterID; //at this point, daughterID
            //    //is equal to nodeID.

            //    if (seqNode.parent == null)
            //    {
            //        parentID = -1;
            //    }
            //    cmdText = "UPDATE listOfNodes SET parentID = " +
            //        parentID + " WHERE nodeID = " + daughterID;
            //    executeNonQuery(cmdText);
            //}
        }

        public Sequence reconstituteSequence(SearchNode seedNode)
        {
            Sequence nodes = new Sequence();
            int daughterID = seedNode.searchSpaceInformation.daughterID;

            nodes.Add(seedNode);
            while (daughterID != (-1))
            {
                //reconstitute seed's daughter and assign seed to it
                seedNode = reconstituteNode("" + daughterID);
                nodes.Add(seedNode);
                daughterID = seedNode.searchSpaceInformation.daughterID;
            }

            //set the parents and the daughters of each node in the list
            for (int i = 0; i < nodes.Count; i++)
            {
                if (i > 0) nodes[i].parent = nodes[i - 1];
            }

            //assign each node's id
            nodes[0].searchSpaceInformation.nodeID =
                nodes[1].searchSpaceInformation.parentID;
            for (int i = 1; i < nodes.Count; i++)
            {
                nodes[i].searchSpaceInformation.nodeID =
                    nodes[i - 1].searchSpaceInformation.daughterID;
            }

            return nodes;
        }

        public void solutionSave(Sequence candidate)
        {
            if (checkIfInDB(candidate)) return;

            List<Sequence> listOfSequences = reconstituteAllSequences();
            checkSequenceForNewActions(candidate, listOfSequences);
            checkDBForActionsToDelete(listOfSequences);

            saveSequence(candidate);
        }

        private void updateNumberOfTimesActionObserved(string action, int numberOfTimesObserved)
        {
            string cText = "UPDATE actions SET n = " +
                        numberOfTimesObserved + " WHERE actionName = '" + action + "'";
            executeNonQuery(cText);
        }

        private void updateOrDeleteAction(string action, List<Sequence> listOfSequences)
        {
            int numberOfTimesObserved = countNumberOfTimesActionIsInDB(action, listOfSequences);
            if (numberOfTimesObserved == 0) numberOfTimesObserved = 1;
            double deletionProb = calculateDeletionProbability(numberOfTimesObserved);
            if (!maybeDeleteAction(deletionProb, action))
                updateNumberOfTimesActionObserved(action, numberOfTimesObserved);
        }
    }
}
