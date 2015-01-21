using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptionSets
{
    [Serializable()]
    public class DistanceTick
    {
        protected int _distanceTicksValue;
        public int distanceTicksValue
        {
            get { return this._distanceTicksValue; }
            protected set { this._distanceTicksValue = value; }
        }
        public const int neutralValue = -1;

//Constructors
        public DistanceTick()
        {
            this.distanceTicksValue = DistanceTick.neutralValue;
        }

        public DistanceTick(int value)
        {
            this.distanceTicksValue = value;
        }


//Public methods
        public void setValueToNeutral()
        {
            this.distanceTicksValue = DistanceTick.neutralValue;
        }

        public override string ToString()
        {
            return this.distanceTicksValue.ToString();
        }

        public bool valueIsNeutral()
        {
            if (this.distanceTicksValue == DistanceTick.neutralValue)
                return true;
            else
                return false;
        }
    }
}
