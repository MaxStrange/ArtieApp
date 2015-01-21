using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;
using System.Diagnostics;
using UsefulStaticMethods;
using System.Threading;
using SearchAbstractDataTypes;

namespace Search
{
    public class DumbSearchController : SearchController
    {
//Constructors
        public DumbSearchController(SearchNode startNode, SearchNode goalNode,
            ref SearchPartitionToGUI spg)
        {
            initializeSearch(startNode, goalNode, ref spg);
        }

        /// <summary>
        /// If you have distance tick data, you can put that in - it should be the number
        /// of distance ticks counted during a normal action length. The search will derive
        /// daughter nodes by using the provided data rather than an estimated distance
        /// per action length.
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="goalNode"></param>
        /// <param name="spg"></param>
        /// <param name="calData"></param>
        public DumbSearchController(SearchNode startNode, SearchNode goalNode,
            ref SearchPartitionToGUI spg, CalibrationData calData)
        {
            initializeSearch(startNode, goalNode, ref spg, calData);
        }


//Public methods
        public void AStarWithThread()
        {
            Thread t = ThreadMethods.createNewBackgroundThread(AStarSearch, "DumbSearchThread");
            t.Start();
        }

        public Sequence AStarSearchForSequenceReturn()
        {
            initializeOpenAndClosed();
            while (openQueueContainsANode())
            {
                SearchNode n = moveFirstNodeOnOpenToClosedAndAlertUI();

                if (n.matches(base.goalNode, new AbstractDataClasses.Percent(0.25)))
                {
                    alertUISolutionHasBeenFound(n);
                    Sequence solutionSeq = new Sequence();
                    solutionSeq.buildFromSearchNode(n);
                    return solutionSeq;
                }

                expandNodeAndAddDaughtersToOpen(n);
            }
            alertUISolutionHasNotBeenFound("Solution could not be located, showing best match: ");
            return null;
        }

        override public void AStarSearch()
        {
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

//Private methods
        private void maybePruneOpenList()
        {
            base.searchIterations++;
            if (base.searchIterations > 100)
            {
                pruneOpenQueue();
                base.searchIterations = 0;
            }
        }
    }
}
