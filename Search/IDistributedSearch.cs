using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;
using SearchAbstractDataTypes;

namespace Search
{
    public interface IDistributedSearch
    {
        bool closedSetContainsNode(SearchNode daughter);

        void depthFirstSearch();

//        List<SearchNode> nodeExpand(SearchNode parent);

        //TODO : make sure that all methods common to all distributed searches are listed here
    }
}
