using ActionSet;
using SearchAbstractDataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulStaticMethods;

namespace SearchAbstractDataTypes
{
    abstract internal class NodeBehavior : ISearchNodeBehavior
    {
        protected SearchNode _caller = null;
        protected SearchNode caller
        {
            get { return this._caller; }
            set { this._caller = value; }
        }

        public virtual Stack<CompoundAction> buildMethodStack(SearchNode expansionLimit)
        {
            throw new NotImplementedException();
        }

        public virtual void increaseNodeDepth()
        {
            throw new NotImplementedException();
        }

        public virtual bool matches(SearchNode m)
        {
            throw new NotImplementedException();
        }

        public virtual string solutionExpand(SearchNode expansionLimit)
        {
            throw new NotImplementedException();
        }
    }
}
