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
    public class P_S_Arm : P_Self
    {
//Constants
        //Joint A is taken to be at 0, 0, 0 (Artie coordinates).
        /// <summary>
        /// How much an arm joint get rotated in one action (radians) (estimated - depends
        /// on battery level).
        /// </summary>
        public const double ANGLE_OF_ROTATION_BY_ARMJOINT_A = 0.2235;
        /// <summary>
        /// How much an arm joint get rotated in one action (radians) (estimated - depends
        /// on battery level).
        /// </summary>
        public const double ANGLE_OF_ROTATION_BY_ARMJOINT_B = 0.2904;
        /// <summary>
        /// How much an arm joint get rotated in one action (radians) (estimated - depends
        /// on battery level).
        /// </summary>
        public const double ANGLE_OF_ROTATION_BY_ARMJOINT_C = 0.2201;
        /// <summary>
        /// How much an arm joint get rotated in one action (radians) (estimated - depends
        /// on battery level).
        /// </summary>
        public const double ANGLE_OF_ROTATION_BY_ARMJOINT_D = 0.3071;
        /// <summary>
        /// Length from the middle of motor_A to the middle of motor_B along the arm (meters)
        /// </summary>
        public const double LENGTH_FROM_A_TO_B = 0.065;
        /// <summary>
        /// Length from the middle of motor_B to the middle of motor_C along the arm (meters)
        /// </summary>
        public const double LENGTH_FROM_B_TO_C = 0.095;
        /// <summary>
        /// Length from the middle of motor_C to the middle of motor_D along the arm (meters)
        /// </summary>
        public const double LENGTH_FROM_C_TO_D = 0.12;
        /// <summary>
        /// Length from joint D (i.e. where the potentiometer is located) to the end of the
        /// LED (meters).
        /// </summary>
        public const double LENGTH_FROM_D_TO_GRIPPER = 0.07;
        /// <summary>
        /// Length of the base from the circle to the end (meters).
        /// </summary>
        public const double LENGTH_OF_BASE = 0.12;
        /// <summary>
        /// Radius of the circular part of the base (meters).
        /// </summary>
        public const double RADIUS_OF_BASE = 0.05;
        /// <summary>
        /// Height of the base (meters).
        /// </summary>
        public const double HEIGHT_OF_BASE = 0.045;
        /// <summary>
        /// Radius of arm at its thickest point in the segment from joint B to joint C
        /// (meters).
        /// </summary>
        public const double RADIUS_OF_ARM_B_TO_C = 0.025;
        /// <summary>
        /// Radius of arm at its thickest point in the segment from joint C to joint D
        /// (meters).
        /// </summary>
        public const double RADIUS_OF_ARM_C_TO_D = 0.025;

        private const double ANGLE_TRAVELED_PER_POT_VALUE_A = 0.0036;
        private const double ANGLE_TRAVELED_PER_POT_VALUE_B = 0.0036;
        private const double ANGLE_TRAVELED_PER_POT_VALUE_C = 0.0037;
        private const double ANGLE_TRAVELED_PER_POT_VALUE_D = 0.0036;


//Internal fields
        private ArmJoint _joint_A = null;
        public ArmJoint joint_A
        {
            get { return this._joint_A; }
            set { this._joint_A = value; }
        }

        private ArmJoint _joint_B = null;
        public ArmJoint joint_B
        {
            get { return this._joint_B; }
            set { this._joint_B = value; }
        }

        private ArmJoint _joint_C = null;
        public ArmJoint joint_C
        {
            get { return this._joint_C; }
            set { this._joint_C = value; }
        }

        private ArmJoint _joint_D = null;
        public ArmJoint joint_D
        {
            get { return this._joint_D; }
            set { this._joint_D = value; }
        }

        private Gripper _gripper = null;
        public Gripper gripper
        {
            get { return this._gripper; }
            set { this._gripper = value; }
        }


//Constructors
        public P_S_Arm(P_S_Body body)
        {
            initializeWithDefaultValues(body);
        }

        public P_S_Arm(Gripper gripper)
        {
            this.gripper = gripper;
        }

        public P_S_Arm(ArmCalibrationData armData)
        {
            this.joint_A = armData.jointA;
            this.joint_B = armData.jointB;
            this.joint_C = armData.jointC;
            this.joint_D = armData.jointD;
        }

        public P_S_Arm(ArmJoint A, ArmJoint B, ArmJoint C, ArmJoint D, Gripper gripper)
        {
            this.joint_A = A;
            this.joint_B = B;
            this.joint_C = C;
            this.joint_D = D;
            this.gripper = gripper;
        }


//Public methods
        public bool armIsInPotentiallyDamagingConfiguration(P_S_Body body)
        {
            if (baseRegionConflictsWithArm(body))
                return true;
            else if (armBCRegionConflictsWithArm())
                return true;
            else if (armCDRegionConflictsWithArm())
                return true;
            else
                return false;
        }

        public double computeNumericalDifferenceBetweenThisAnd(P_S_Arm otherArm, Percent percentError)
        {
            double oriComp = this.gripper.jointFrame.evaluationFunction(otherArm.gripper.jointFrame, percentError);
            double locationComp = this.gripper.location.evaluationFunction(otherArm.gripper.location, percentError);
            double slopeConstant = 0.5;
            double evaluationValue = slopeConstant * Math.Pow(locationComp + oriComp, 2);
            return evaluationValue;
        }

        public void driveAClockWise(DistanceTickFromArm distanceTicks)
        {
            double distanceTicksMag = Math.Abs(distanceTicks.distanceTicksValue);
            double angle = distanceTicks.valueIsNeutral() ? (-1.0 * ANGLE_OF_ROTATION_BY_ARMJOINT_A)
                : (-1.0 * distanceTicksMag * ANGLE_TRAVELED_PER_POT_VALUE_A);
            this.driveA(angle);
        }

        public void driveACounterClockWise(DistanceTickFromArm distanceTicks)
        {
            double distanceTicksMag = Math.Abs(distanceTicks.distanceTicksValue);
            double angle = distanceTicks.valueIsNeutral() ? ANGLE_OF_ROTATION_BY_ARMJOINT_A
                : (distanceTicksMag * ANGLE_TRAVELED_PER_POT_VALUE_A);
            this.driveA(angle);
        }

        public void driveBClockWise(DistanceTickFromArm distanceTicks)
        {
            double distanceTickMag = Math.Abs(distanceTicks.distanceTicksValue);
            double angle = distanceTicks.valueIsNeutral() ? (-1.0 * ANGLE_OF_ROTATION_BY_ARMJOINT_B)
                : (-1.0 * distanceTickMag * ANGLE_TRAVELED_PER_POT_VALUE_B);
            this.driveB(angle);
        }

        public void driveBCounterClockWise(DistanceTickFromArm distanceTicks)
        {
            double distanceTickMag = Math.Abs(distanceTicks.distanceTicksValue);
            double angle = distanceTicks.valueIsNeutral() ? ANGLE_OF_ROTATION_BY_ARMJOINT_B
                : (distanceTickMag * ANGLE_TRAVELED_PER_POT_VALUE_B);
            this.driveB(angle);
        }

        public void driveCClockWise(DistanceTickFromArm distanceTicks)
        {
            double distanceTickMag = Math.Abs(distanceTicks.distanceTicksValue);
            double angle = distanceTicks.valueIsNeutral() ? (-1.0 * ANGLE_OF_ROTATION_BY_ARMJOINT_C)
                : (-1.0 * distanceTickMag * ANGLE_TRAVELED_PER_POT_VALUE_C);
            this.driveC(angle);
        }

        public void driveCCounterClockWise(DistanceTickFromArm distanceTicks)
        {
            double distanceTickMag = Math.Abs(distanceTicks.distanceTicksValue);
            double angle = distanceTicks.valueIsNeutral() ? ANGLE_OF_ROTATION_BY_ARMJOINT_C
                : (distanceTickMag * ANGLE_TRAVELED_PER_POT_VALUE_C);
            this.driveC(angle);
        }

        public void driveDClockWise(DistanceTickFromArm distanceTicks)
        {
            double distanceTickMag = Math.Abs(distanceTicks.distanceTicksValue);
            double angle = distanceTicks.valueIsNeutral() ? (-1.0 * ANGLE_OF_ROTATION_BY_ARMJOINT_D)
                : (-1.0 * distanceTickMag * ANGLE_TRAVELED_PER_POT_VALUE_D);
            this.driveD(angle);
        }

        public void driveDCounterClockWise(DistanceTickFromArm distanceTicks)
        {
            double distanceTickMag = Math.Abs(distanceTicks.distanceTicksValue);
            double angle = distanceTicks.valueIsNeutral() ? ANGLE_OF_ROTATION_BY_ARMJOINT_D
                : (distanceTickMag * ANGLE_TRAVELED_PER_POT_VALUE_D);
            this.driveD(angle);
        }

        public void openGripper(DistanceTickFromArm distanceTicks)
        {
            this.gripper.openGripper();
        }

        public void closeGripper(DistanceTickFromArm distanceTicks)
        {
            this.gripper.closeGripper();
        }

        public void rotateWholeArm(Vector axisOfRotation, Point axisOrigin, double angle)
        {
            this.joint_A.location = MatrixMath.rotatePointAroundVectorGeneralForm(
                axisOfRotation, axisOrigin, angle, this.joint_A.location);
            this.joint_B.location = MatrixMath.rotatePointAroundVectorGeneralForm(
                axisOfRotation, axisOrigin, angle, this.joint_B.location);
            this.joint_C.location = MatrixMath.rotatePointAroundVectorGeneralForm(
                axisOfRotation, axisOrigin, angle, this.joint_C.location);
            this.joint_D.location = MatrixMath.rotatePointAroundVectorGeneralForm(
                axisOfRotation, axisOrigin, angle, this.joint_D.location);
            this.gripper.location = MatrixMath.rotatePointAroundVectorGeneralForm(
                axisOfRotation, axisOrigin, angle, this.gripper.location);

            this.joint_A.jointFrame = this.joint_A.jointFrame.rotateEachVectorAroundAnAxis(axisOfRotation, axisOrigin, angle);
            this.joint_B.jointFrame = this.joint_B.jointFrame.rotateEachVectorAroundAnAxis(axisOfRotation, axisOrigin, angle);
            this.joint_C.jointFrame = this.joint_C.jointFrame.rotateEachVectorAroundAnAxis(axisOfRotation, axisOrigin, angle);
            this.joint_D.jointFrame = this.joint_D.jointFrame.rotateEachVectorAroundAnAxis(axisOfRotation, axisOrigin, angle);
            this.gripper.jointFrame = this.gripper.jointFrame.rotateEachVectorAroundAnAxis(axisOfRotation, axisOrigin, angle);
        }

        public void setEqualToArmPosition(P_S_Arm arm)
        {
            this.joint_A = arm.joint_A;
            this.joint_B = arm.joint_B;
            this.joint_C = arm.joint_C;
            this.joint_D = arm.joint_D;
        }

        public void translateWholeArmPosition(Tuple<double, double, double> xyzDisplacement)
        {
            this.joint_A.location = MatrixMath.translatePoint(this.joint_A.location, xyzDisplacement);
            this.joint_B.location = MatrixMath.translatePoint(this.joint_B.location, xyzDisplacement);
            this.joint_C.location = MatrixMath.translatePoint(this.joint_C.location, xyzDisplacement);
            this.joint_D.location = MatrixMath.translatePoint(this.joint_D.location, xyzDisplacement);
            this.gripper.location = MatrixMath.translatePoint(this.gripper.location, xyzDisplacement);
        }

//Protected methods
        protected void initializeWithDefaultValues(P_S_Body body)
        {
            Point jointALocation = body.location;
            Orientation jointAOrientation = body.artieFrame;
            Vector jointARotationAxis = jointAOrientation.pVector;

            this.joint_A = new ArmJoint(jointALocation, jointARotationAxis, jointAOrientation);

            Point jointBLocation = MatrixMath.translatePoint(jointALocation, LENGTH_FROM_A_TO_B, jointAOrientation.pVector);
            Orientation jointBOrientation = body.artieFrame;
            Vector jointBRotationAxis = jointBOrientation.oVector;//!! Watch list: might have to make this negative !!

            this.joint_B = new ArmJoint(jointBLocation, jointBRotationAxis, jointBOrientation);

            Point jointCLocation = MatrixMath.translatePoint(jointBLocation, LENGTH_FROM_B_TO_C, jointBOrientation.pVector);
            Orientation jointCOrientation = body.artieFrame;
            Vector jointCRotationAxis = jointCOrientation.oVector;//!! Watch List: might have to make this negative !!

            this.joint_C = new ArmJoint(jointCLocation, jointCRotationAxis, jointCOrientation);

            Point jointDLocation = MatrixMath.translatePoint(jointCLocation, LENGTH_FROM_C_TO_D, jointCOrientation.nVector);
            Orientation jointDOrientation = body.artieFrame;
            Vector jointDRotationAxis = jointDOrientation.oVector;//!! Watch List: might have to make this negative !!
            
            this.joint_D = new ArmJoint(jointDLocation, jointDRotationAxis, jointDOrientation);

            Point gripperLocation = MatrixMath.translatePoint(jointDLocation, LENGTH_FROM_D_TO_GRIPPER, jointDOrientation.nVector);
            Orientation gripperOrientation = body.artieFrame;

            this.gripper = new Gripper(gripperLocation, gripperOrientation);
        }


//Private methods
        private bool armBCRegionConflictsWithArm()
        {
            RegionInSpace armBCRegion = calculateBCRegion();

            if (this.joint_D.fallsWithinRegionInSpace(armBCRegion))
                return true;
            else if (this.gripper.fallsWithinRegionInSpace(armBCRegion))
                return true;
            else
                return false;
        }

        private bool armCDRegionConflictsWithArm()
        {
            RegionInSpace armCDRegion = calculateCDRegion();

            if (this.joint_A.fallsWithinRegionInSpace(armCDRegion))
                return true;
            else if (this.joint_B.fallsWithinRegionInSpace(armCDRegion))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns a region in space that the base of the arm occupies.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        private RegionInSpace calculateBaseRegionPoints(P_S_Body body)
        {
            Point p1 = MatrixMath.translatePoint(body.location,
                P_S_Arm.LENGTH_OF_BASE, body.artieFrame.nVector.multiplyWithScalar(-1));
            Point p2 = MatrixMath.translatePoint(body.location,
                P_S_Arm.RADIUS_OF_BASE, body.artieFrame.nVector);
            Point p3 = MatrixMath.translatePoint(body.location,
                P_S_Arm.RADIUS_OF_BASE, body.artieFrame.oVector.multiplyWithScalar(-1));
            Point p4 = MatrixMath.translatePoint(body.location,
                P_S_Arm.RADIUS_OF_BASE, body.artieFrame.oVector);
            Point p5 = body.location;
            Point p6 = MatrixMath.translatePoint(body.location,
                P_S_Arm.HEIGHT_OF_BASE, body.artieFrame.pVector);
            List<Point> regionPoints = new List<Point>();
            regionPoints.Add(p1);
            regionPoints.Add(p2);
            regionPoints.Add(p3);
            regionPoints.Add(p4);
            regionPoints.Add(p5);
            regionPoints.Add(p6);
            return new RegionInSpace(regionPoints);
        }

        /// <summary>
        /// Returns a region in space that the BC arm segment occupies.
        /// </summary>
        /// <returns></returns>
        private RegionInSpace calculateBCRegion()
        {
            Point p1 = MatrixMath.translatePoint(this.joint_B.location,
                P_S_Arm.RADIUS_OF_ARM_B_TO_C, this.joint_B.jointFrame.nVector.multiplyWithScalar(-1));
            Point p2 = MatrixMath.translatePoint(this.joint_B.location,
                P_S_Arm.RADIUS_OF_ARM_B_TO_C, this.joint_B.jointFrame.nVector);
            Point p3 = MatrixMath.translatePoint(this.joint_B.location,
                P_S_Arm.RADIUS_OF_ARM_B_TO_C, this.joint_B.jointFrame.oVector.multiplyWithScalar(-1));
            Point p4 = MatrixMath.translatePoint(this.joint_B.location,
                P_S_Arm.RADIUS_OF_ARM_B_TO_C, this.joint_B.jointFrame.oVector);
            Point p5 = this.joint_B.location;
            Point p6 = MatrixMath.translatePoint(this.joint_B.location,
                P_S_Arm.LENGTH_FROM_B_TO_C, this.joint_B.jointFrame.pVector);
            List<Point> regionPoints = new List<Point>();
            regionPoints.Add(p1);
            regionPoints.Add(p2);
            regionPoints.Add(p3);
            regionPoints.Add(p4);
            regionPoints.Add(p5);
            regionPoints.Add(p6);
            return new RegionInSpace(regionPoints);
        }

        /// <summary>
        /// Returns a region in space that the CD arm segment occupies.
        /// </summary>
        /// <returns></returns>
        private RegionInSpace calculateCDRegion()
        {
            Point p1 = this.joint_C.location;
            Point p2 = MatrixMath.translatePoint(this.joint_C.location,
                P_S_Arm.LENGTH_FROM_C_TO_D, this.joint_C.jointFrame.nVector);
            Point p3 = MatrixMath.translatePoint(this.joint_C.location,
                P_S_Arm.RADIUS_OF_ARM_C_TO_D, this.joint_C.jointFrame.oVector.multiplyWithScalar(-1));
            Point p4 = MatrixMath.translatePoint(this.joint_C.location,
                P_S_Arm.RADIUS_OF_ARM_C_TO_D, this.joint_C.jointFrame.oVector);
            Point p5 = MatrixMath.translatePoint(this.joint_C.location,
                P_S_Arm.RADIUS_OF_ARM_C_TO_D, this.joint_C.jointFrame.pVector.multiplyWithScalar(-1));
            Point p6 = MatrixMath.translatePoint(this.joint_C.location,
                P_S_Arm.RADIUS_OF_ARM_C_TO_D, this.joint_C.jointFrame.pVector);
            List<Point> regionPoints = new List<Point>();
            regionPoints.Add(p1);
            regionPoints.Add(p2);
            regionPoints.Add(p3);
            regionPoints.Add(p4);
            regionPoints.Add(p5);
            regionPoints.Add(p6);
            return new RegionInSpace(regionPoints);
        }

        private bool baseRegionConflictsWithArm(P_S_Body body)
        {
            RegionInSpace baseRegion = calculateBaseRegionPoints(body);

            if (this.joint_C.fallsWithinRegionInSpace(baseRegion))
                return true;
            else if (this.joint_D.fallsWithinRegionInSpace(baseRegion))
                return true;
            else if (this.gripper.fallsWithinRegionInSpace(baseRegion))
                return true;
            else
                return false;
        }

        private void driveA(double angle)
        {
            rotateArmJointLocationD(this.joint_A, angle);
            rotateArmJointRotationAxisBCD(this.joint_A, angle);
            rotateArmJointFramesABCD(this.joint_A, angle);
        }

        private void driveB(double angle)
        {
            rotateArmJointLocationsCD(this.joint_B, angle);
            rotateArmJointRotationAxisCD(this.joint_B, angle);
            rotateArmJointFramesBCD(this.joint_B, angle);
        }

        private void driveC(double angle)
        {
            rotateArmJointLocationD(this.joint_C, angle);
            rotateArmJointRotationAxisD(this.joint_C, angle);
            rotateArmJointFramesCD(this.joint_C, angle);
        }

        private void driveD(double angle)
        {
            rotateArmJointFrameD(this.joint_D, angle);
            this.gripper.rotateComponentLocation(this.joint_D, angle);
        }

        private void rotateArmJointFramesABCD(ArmJoint jointToRotateAround, double angle)
        {
            this.joint_A.rotateComponentFrame(jointToRotateAround, angle);
            rotateArmJointFramesBCD(jointToRotateAround, angle);
        }

        private void rotateArmJointFramesBCD(ArmJoint jointToRotateAround, double angle)
        {
            this.joint_B.rotateComponentFrame(jointToRotateAround, angle);
            rotateArmJointFramesCD(jointToRotateAround, angle);
        }

        private void rotateArmJointFramesCD(ArmJoint jointToRotateAround, double angle)
        {
            this.joint_C.rotateComponentFrame(jointToRotateAround, angle);
            rotateArmJointFrameD(jointToRotateAround, angle);
        }

        private void rotateArmJointFrameD(ArmJoint jointToRotateAround, double angle)
        {
            this.joint_D.rotateComponentFrame(jointToRotateAround, angle);
            this.gripper.rotateComponentFrame(jointToRotateAround, angle);
        }

        private void rotateArmJointLocationsCD(ArmJoint jointToRotateAround, double angle)
        {
            this.joint_C.rotateComponentLocation(jointToRotateAround, angle);
            rotateArmJointLocationD(jointToRotateAround, angle);
        }

        private void rotateArmJointLocationD(ArmJoint jointToRotateAround, double angle)
        {
            this.joint_D.rotateComponentLocation(jointToRotateAround, angle);
            this.gripper.rotateComponentLocation(jointToRotateAround, angle);
        }

        private void rotateArmJointRotationAxisBCD(ArmJoint jointToRotateAround, double angle)
        {
            this.joint_B.rotateArmJointRotationAxis(jointToRotateAround, angle);
            rotateArmJointRotationAxisCD(jointToRotateAround, angle);
        }

        private void rotateArmJointRotationAxisCD(ArmJoint jointToRotateAround, double angle)
        {
            this.joint_C.rotateArmJointRotationAxis(jointToRotateAround, angle);
            rotateArmJointRotationAxisD(jointToRotateAround, angle);
        }

        private void rotateArmJointRotationAxisD(ArmJoint jointToRotateAround, double angle)
        {
            this.joint_D.rotateArmJointRotationAxis(jointToRotateAround, angle);
        }

        private void rotateGripperFrame(ArmJoint jointToRotateAround, double angle)
        {
            this.gripper.rotateComponentFrame(jointToRotateAround, angle);
        }

        private void rotateGripperLocation(ArmJoint jointToRotateAround, double angle)
        {
            this.gripper.rotateComponentLocation(jointToRotateAround, angle);
        }
    }
}
