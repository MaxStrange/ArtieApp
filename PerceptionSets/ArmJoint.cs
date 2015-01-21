using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrices;
using AbstractDataClasses;

namespace PerceptionSets
{
    [Serializable]
    public class ArmJoint : ArmComponent
    {
//Public fields
        private Vector _rotationAxis;
        /// <summary>
        /// RotationAxis is a vector whose origin is at the armJoint's location and which
        /// is of unit length. It is the axis around which the arm joint rotates. Its
        /// components are the amount of the vector which point along each axis in the
        /// World coordinates. So, rotationAxis.listOfvalues[Vector.Components.x] is the
        /// amount of the rotation axis that lies along the World's x-axis.
        /// </summary>
        public Vector rotationAxis
        {
            get { return this._rotationAxis; }
            set { this._rotationAxis = value; }
        }


//Constructors
        public ArmJoint()
        {
            base.location = new Point(0, 0, 0);
            base.jointFrame = new Orientation();
            base.jointFrame.alignVectorAlongAxis(Orientation.Vectors.nVector, Vector.Components.x);
            base.jointFrame.alignVectorAlongAxis(Orientation.Vectors.oVector, Vector.Components.y);
            base.jointFrame.alignVectorAlongAxis(Orientation.Vectors.pVector, Vector.Components.z);
        }

        public ArmJoint(Point location, Vector rotationAxis, Orientation frame)
        {
            base.location = location;
            this.rotationAxis = rotationAxis;
            base.jointFrame = frame;
        }


//Public Methods
        public bool locationMatches(Point otherLocation, Percent precisionAsPercent)
        {
            if (base.location.matches(otherLocation, precisionAsPercent))
                return true;
            else
                return false;
        }

        public bool orientationMatches(Orientation otherOrientation, Percent precisionAsPercent)
        {
            if (base.jointFrame.matches(otherOrientation, precisionAsPercent))
                return true;
            else
                return false;
        }

//Internal Methods
        internal void rotateArmJointRotationAxis(ArmJoint armJointToRotateAround, double angle)
        {
            Vector axis = armJointToRotateAround.rotationAxis;
            Point axisOrigin = armJointToRotateAround.location;

            this.rotationAxis = MatrixMath.rotateVectorAroundVectorGeneralForm(
                axis, axisOrigin, angle, this.rotationAxis);
        }
    }
}
