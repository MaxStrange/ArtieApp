using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionSet;

namespace SearchAbstractDataTypes
{
    internal class DistributedNodeBehavior: NodeBehavior, ISearchNodeBehavior
    {
        public override void increaseNodeDepth()
        {
            base.caller.searchSpaceInformation.depth += 1;
        }
    }
}
