using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;

namespace SearchAbstractDataTypes
{
    public class SequenceNotBuiltException : Exception
    {
        public SequenceNotBuiltException()
        {
        }

        public SequenceNotBuiltException(string message)
        : base(message)
        {
        }

        public SequenceNotBuiltException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
