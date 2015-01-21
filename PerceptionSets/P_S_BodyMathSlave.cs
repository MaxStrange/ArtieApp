using Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptionSets
{
    [Serializable]
    internal class P_S_BodyMathSlave
    {
//Constants
        /// <summary>
        /// The axel length (meters)
        /// </summary>
        public const double ARTIE_AXEL_LENGTH = 0.17;
        /// <summary>
        /// Angular velocity magnitude of Artie's wheels (rad/sec) (estimated - depends on
        /// battery power)
        /// </summary>
        public const double OMEGA_WHEEL = 2.75;
        /// <summary>
        /// Change in time per action (seconds)
        /// </summary>
        public const int DT = 2;
        /// <summary>
        /// The radius of Artie's drive wheels (meters)
        /// </summary>
        public const double WHEEL_RAD = 0.04;
        /// <summary>
        /// Circumfrance of Artie's drive wheels (meters)
        /// </summary>
        public const double WHEEL_CIRC = 2.0 * Math.PI * WHEEL_RAD;
        /// <summary>
        /// The number of tic marks that the sensor counts per full rotation of a wheel
        /// </summary>
        public const int TICKS = 4;

        
//Private fields
        private P_S_Body _parent = null;
        private P_S_Body parent
        {
            get { return this._parent; }
            set { this._parent = value; }
        }


//Constructors
        internal P_S_BodyMathSlave(P_S_Body parent)
        {
            this._parent = parent;
        }


//Internal methods
        internal void driveBackwards(double distanceMagnitude, P_S_Arm arm)
        {
            Tuple<double, double, double> xyzDisplacement = calculateBackwardsDisplacementValues(distanceMagnitude);
            this.parent.location = MatrixMath.translatePoint(this.parent.location, xyzDisplacement);
            arm.translateWholeArmPosition(xyzDisplacement);
        }

        internal void driveForwards(double distanceMagnitude, P_S_Arm arm)
        {
            Tuple<double, double, double> xyzDisplacement = calculateForwardDisplacementValues(distanceMagnitude);
            this.parent.location = MatrixMath.translatePoint(this.parent.location, xyzDisplacement);
            arm.translateWholeArmPosition(xyzDisplacement);
        }

        internal double decideHowToCalculateDistance(DistanceTickFromBody distanceTicks)
        {
            double distance = distanceTicks.valueIsNeutral() ? calculateDistance() : calculateDistance(distanceTicks);
            return distance;
        }

        internal double decideHowToCalculateTightAngle(DistanceTickFromBody distanceTicks)
        {
            double angle = distanceTicks.valueIsNeutral() ? calculateTightAngle() : calculateTightAngle(distanceTicks);
            return angle;
        }

        internal double decideHowToCalculateWideAngle(DistanceTickFromBody distanceTicks)
        {
            double angle = distanceTicks.valueIsNeutral() ? ((-1.0) * calculateWideAngle()) : ((-1.0) * calculateWideAngle(distanceTicks));
            return angle;
        }

        internal void tightTurn(double angle, P_S_Arm arm)
        {
            Vector axis = this.parent.artieFrame.pVector;
            Point axisOrigin = this.parent.location;

            turnArtie(axis, axisOrigin, angle, arm);
        }

        internal void wideTurn(bool rightTurn, double angle, P_S_Arm arm)
        {
            Tuple<double, double, double> displacementValues = rightTurn ?
                calculateTurnWideRightDisplacementValues() : calculateTurnWideLeftDisplacementValues();
            Vector axis = this.parent.artieFrame.pVector;
            Point axisOrigin = MatrixMath.translatePoint(this.parent.location, displacementValues);

            turnArtie(axis, axisOrigin, angle, arm);
        }



//Private methods
        private Tuple<double, double, double> calculateBackwardsDisplacementValues(double displacementMag)
        {
            Tuple<double, double, double> forwardValues = calculateForwardDisplacementValues(displacementMag);

            return MatrixMath.multiplyThroughTuple(forwardValues, (-1.0));
        }

        private double calculateDistance(DistanceTickFromBody distanceTicks)
        {
            double distance = (WHEEL_CIRC * distanceTicks.distanceTicksValue) / (TICKS);
            return distance;
        }

        private double calculateDistance()
        {
            double distance = OMEGA_WHEEL * DT * WHEEL_RAD;
            return distance;
        }

        private Tuple<double, double, double> calculateForwardDisplacementValues(double displacementMag)
        {
            Vector nVector = this.parent.artieFrame.nVector;
            Vector nAfterDisplacement = nVector.multiplyWithScalar(displacementMag);

            return nAfterDisplacement.convertXYZComponentsToTuple();
        }

        private double calculateTightAngle(DistanceTickFromBody distanceTicks)
        {
            double angle = (2.0 * (double)distanceTicks.distanceTicksValue * WHEEL_CIRC) / (TICKS * ARTIE_AXEL_LENGTH);
            return angle;
        }

        private double calculateTightAngle()
        {
            double angle = (4.0 * OMEGA_WHEEL * DT * WHEEL_RAD) / ARTIE_AXEL_LENGTH;
            return angle;
        }

        private Tuple<double, double, double> calculateTurnWideRightDisplacementValues()
        {
            Tuple<double, double, double> leftValues = calculateTurnWideLeftDisplacementValues();

            return MatrixMath.multiplyThroughTuple(leftValues, (-1.0));
        }

        private Tuple<double, double, double> calculateTurnWideLeftDisplacementValues()
        {
            Vector oVector = this.parent.artieFrame.oVector;
            Vector oAfterDisplacement = oVector.multiplyWithScalar((0.5) * ARTIE_AXEL_LENGTH);

            return oAfterDisplacement.convertXYZComponentsToTuple();
        }

        private double calculateWideAngle(DistanceTickFromBody distanceTicks)
        {
            double angle = (((double)distanceTicks.distanceTicksValue * WHEEL_CIRC) / (TICKS * ARTIE_AXEL_LENGTH));
            return angle;
        }

        private double calculateWideAngle()
        {
            double angle = (OMEGA_WHEEL * DT * WHEEL_RAD) / (ARTIE_AXEL_LENGTH);
            return angle;
        }

        private void turnArtie(Vector axis, Point axisOrigin, double angle, P_S_Arm arm)
        {
            this.parent.location = MatrixMath.rotatePointAroundVectorGeneralForm(
             axis, axisOrigin, angle, this.parent.location);
            this.parent.artieFrame = this.parent.artieFrame.rotateEachVectorAroundAnAxis(axis, axisOrigin, angle);

            arm.rotateWholeArm(axis, axisOrigin, angle);
        }
    }
}
