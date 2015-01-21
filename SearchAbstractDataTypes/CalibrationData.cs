using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;

namespace SearchAbstractDataTypes
{
    /// <summary>
    /// This class is designed to be immutable!
    /// Provides a means of accessing the calibration data for all things on Artie
    /// that need calibration.
    /// </summary>
    public class CalibrationData
    {
        private class ArmData
        {
            private int _armJoint_A_Data = NEUTRAL_VALUE;
            internal int armJoint_A_Data
            {
                get { return this._armJoint_A_Data; }
                private set { this._armJoint_A_Data = value; }
            }

            internal ArmData(DistanceTickFromArm d)
            {
                this.armJoint_A_Data = d;
            }
        }

        private class BodyData
        {
            private int _bodyData = NEUTRAL_VALUE;
            internal int bodyData
            {
                get { return this._bodyData; }
                private set { this._bodyData = value; }
            }

            internal BodyData(DistanceTickFromBody d)
            {
                this.bodyData = d;
            }
        }


        private ArmData _armData = null;
        public int armDataTicks
        {
            get { return this._armData.armJoint_A_Data; }
        }
        private BodyData _bodyData = null;
        public int bodyDataTicks
        {
            get { return this._bodyData.bodyData; }
        }
        internal const int NEUTRAL_VALUE = -1000;

        public CalibrationData()
        {
            this._armData = new ArmData((DistanceTickFromArm)NEUTRAL_VALUE);
            this._bodyData = new BodyData((DistanceTickFromBody)NEUTRAL_VALUE);
        }

        public CalibrationData(DistanceTick d)
        {
            if (d is DistanceTickFromBody)
            {
                this._armData = new ArmData((DistanceTickFromArm)NEUTRAL_VALUE);
                this._bodyData = new BodyData((DistanceTickFromBody)d);
            }
            else
            {
                this._armData = new ArmData((DistanceTickFromArm)d);
                this._bodyData = new BodyData((DistanceTickFromBody)NEUTRAL_VALUE);
            }
        }



        public bool armIsNull()
        {
            if (this.armDataTicks == NEUTRAL_VALUE)
                return true;
            else
                return false;
        }

        public bool bodyIsNull()
        {
            if (this.bodyDataTicks == NEUTRAL_VALUE)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Arm: ").Append(this.armDataTicks.ToString());
            sb.Append(", Body: ").Append(this.bodyDataTicks.ToString());
            return sb.ToString();
        }
    }
}
