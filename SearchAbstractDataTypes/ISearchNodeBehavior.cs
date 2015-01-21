using ActionSet;
using SearchAbstractDataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAbstractDataTypes
{
    internal interface ISearchNodeBehavior
    {
        Stack<CompoundAction> buildMethodStack(SearchNode expansionLimit);
        
        void increaseNodeDepth();

        bool matches(SearchNode m);

        string solutionExpand(SearchNode expansionLimit);
    }
}
