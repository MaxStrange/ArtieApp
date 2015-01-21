using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAbstractDataTypes;

namespace Search
{
    internal class Directive_NoSelfHarm : IDirective
    {
        public bool disallow(SearchNode n)
        {
            if (n.armIsInPotentiallyDamagingConfiguration())
                return true;//disallow this node
            else
                return false;
        }
    }
}
