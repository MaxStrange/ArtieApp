using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAbstractDataTypes;
using PerceptionSets;

namespace ArtieViaSerialPort
{
    [Serializable()]
    internal class FormattedActionData
    {
//Private fields
        private char _actionChar;
        public char actionChar
        {
            get { return this._actionChar; }
        }
        private DistanceTick _distanceTicks;
        public DistanceTick distanceTicks
        {
            get { return this._distanceTicks; }
        }



//Cast overrides
        static public explicit operator FormattedActionData(Tuple<char, DistanceTick> tup)
        {
            return new FormattedActionData(tup.Item1, tup.Item2);
        }


//Constructors
        public FormattedActionData(char actionChar, DistanceTick distanceTicks)
        {
            this._actionChar = actionChar;
            this._distanceTicks = distanceTicks;
        }


//Public methods
        public override string ToString()
        {
            char action = this.actionChar;
            if (action == '\0')
                action = 'u';
            StringBuilder sb = new StringBuilder();
            sb.Append("Action: ").Append(action.ToString());
            sb.Append(", Ticks: ").Append(this.distanceTicks.ToString());

            return sb.ToString();
        }
    }
}
