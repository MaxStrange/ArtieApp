using AbstractDataClasses;
using Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptionSets
{
    [Serializable]
    /// <summary>
    /// Either a joint or the gripper
    /// </summary>
    public class ArmComponent
    {
        private Orientation _jointFrame;
        public Orientation jointFrame
        {
            get { return this._jointFrame; }
            set { this._jointFrame = value; }
        }
        private Point _location;
        public Point location
        {
            get { return this._location; }
            set { this._location = value; }
        }



//Public methods
        public bool locationMatches(Point otherLocation, Percent precisionAsPercent)
        {
            if (this.location.matches(otherLocation, precisionAsPercent))
                return true;
            else
                return false;
        }

        public bool orientationMatches(Orientation otherOrientation, Percent precisionAsPercent)
        {
            if (this.jointFrame.matches(otherOrientation, precisionAsPercent))
                return true;
            else
                return false;
        }

//Internal methods
        internal bool fallsWithinRegionInSpace(RegionInSpace region)
        {
            if (this.location.fallsWithinRegion(region))
                return true;
            else
                return false;
        }

        internal void rotateComponentLocation(ArmJoint armJointToRotateAround, double angle)
        {
            Vector axis = armJointToRotateAround.rotationAxis;
            Point axisOrigin = armJointToRotateAround.location;

            this.location = MatrixMath.rotatePointAroundVectorGeneralForm(
                axis, axisOrigin, angle, this.location);
        }

        internal void rotateComponentFrame(ArmJoint armJointToRotateAround, double angle)
        {
            Vector axis = armJointToRotateAround.rotationAxis;
            Point axisOrigin = armJointToRotateAround.location;

            this.jointFrame = this.jointFrame.rotateEachVectorAroundAnAxis(axis, axisOrigin,
                angle);
        }
    }
}
