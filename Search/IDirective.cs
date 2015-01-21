using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAbstractDataTypes;

namespace Search
{
    internal interface IDirective
    {
        bool disallow(SearchNode n);
    }
}
