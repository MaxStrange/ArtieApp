using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtieViaSerialPort
{
    struct PotValue
    {
        private int _value;
        private int value
        {
            get { return this._value; }
        }
        private char _pot;
        private char pot
        {
            get { return this._pot; }
        }

//Cast operator
        static public explicit operator PotValue(int value)
        {
            return new PotValue(value, 'U');
        }

        static public implicit operator int(PotValue pv)
        {
            return pv.value;
        }

//Constructor
        public PotValue(int value, char pot)
        {
            this._value = value;
            this._pot = pot;
        }

//Public methods
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.pot.ToString()).Append(": ").Append(this.value.ToString());
            return sb.ToString();
        }
    }
}
