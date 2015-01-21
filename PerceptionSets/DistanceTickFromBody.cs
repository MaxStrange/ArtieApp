using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptionSets
{
    [Serializable()]
    public class DistanceTickFromBody : DistanceTick
    {
//Cast operator
        static public implicit operator DistanceTickFromBody(int value)
        {
            return new DistanceTickFromBody(value);
        }

        static public implicit operator int(DistanceTickFromBody d)
        {
            return d.distanceTicksValue;
        }



//Constructors
        public DistanceTickFromBody()
        {
            base._distanceTicksValue = DistanceTick.neutralValue;
        }

        public DistanceTickFromBody(int value)
        {
            base._distanceTicksValue = value;
        }



//Public methods
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
