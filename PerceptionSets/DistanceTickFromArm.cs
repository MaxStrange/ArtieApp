using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptionSets
{
    [Serializable()]
    public class DistanceTickFromArm : DistanceTick
    {
        //Cast operator
        static public implicit operator DistanceTickFromArm(int value)
        {
            return new DistanceTickFromArm(value);
        }

        static public implicit operator int(DistanceTickFromArm d)
        {
            return d.distanceTicksValue;
        }



        //Constructors
        public DistanceTickFromArm()
        {
            base._distanceTicksValue = DistanceTick.neutralValue;
        }

        public DistanceTickFromArm(int value)
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
