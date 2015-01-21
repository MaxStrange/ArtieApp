using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Search;
using SearchAbstractDataTypes;

namespace ArtieViaSerialPort
{
    /// <summary>
    /// Convenience class for implementing a Sequence as a stack. The first node in the
    /// Sequence is the top of the stack.
    /// </summary>
    internal class SequenceAsStack : Stack<SearchNode>
    {
//Private fields
        private Sequence _sequence = null;
        private Sequence sequence
        {
            get { return this._sequence; }
        }

//Internal Fields
        internal SearchNode endNode
        {
            get { return this.sequence.endNode; }
        }

//Constructors
        internal SequenceAsStack(Sequence sequence)
        {
            sequence.Reverse();
            foreach (SearchNode n in sequence)
            {
                this.Push(n);
            }
            sequence.Reverse();
            this._sequence = sequence;
        }
    }
}
