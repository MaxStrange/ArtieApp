using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;
using Matrices;

namespace PerceptionSets
{
    /// <summary>
    /// Save this class to a file to save arm joint data from previous sessions.
    /// </summary>
    public class ArmCalibrationData
    {
//Internal fields
        private ArmJoint _jointA = null;
        internal ArmJoint jointA
        {
            get { return this._jointA; }
            private set { this._jointA = value; }
        }
        private ArmJoint _jointB = null;
        internal ArmJoint jointB
        {
            get { return this._jointB; }
            private set { this._jointB = value; }
        }
        private ArmJoint _jointC = null;
        internal ArmJoint jointC
        {
            get { return this._jointC; }
            private set { this._jointC = value; }
        }
        private ArmJoint _jointD = null;
        internal ArmJoint jointD
        {
            get { return this._jointD; }
            private set { this._jointD = value; }
        }


//Constructors
        public ArmCalibrationData(P_S_Body artiePosition)
        {
            initializeJointA(artiePosition);
            initializeJointB(artiePosition);
            initializeJointC(artiePosition);
            initializeJointD(artiePosition);
        }



//Private methods
        private void initializeJointA(P_S_Body artiePosition)
        {
            Point locA = artiePosition.location;
            Orientation frameA = artiePosition.artieFrame;
            Vector axisA = frameA.pVector;
            this._jointA = new ArmJoint(locA, axisA, frameA);
        }

        private void initializeJointB(P_S_Body artiePosition)
        {
            Point locB = MatrixMath.translatePoint(this.jointA.location,
             P_S_Arm.LENGTH_FROM_A_TO_B, this.jointA.rotationAxis);
            Orientation frameB = artiePosition.artieFrame;
            Vector axisB = frameB.oVector;
            this._jointB = new ArmJoint(locB, axisB, frameB);
        }

        private void initializeJointC(P_S_Body artiePosition)
        {
            Point locC = MatrixMath.translatePoint(this.jointB.location,
             P_S_Arm.LENGTH_FROM_B_TO_C, this.jointB.jointFrame.pVector);
            Orientation frameC = artiePosition.artieFrame;
            Vector axisC = frameC.oVector;
            this._jointC = new ArmJoint(locC, axisC, frameC);
        }

        private void initializeJointD(P_S_Body artiePosition)
        {
            Point locD = MatrixMath.translatePoint(this.jointC.location,
                   P_S_Arm.LENGTH_FROM_C_TO_D, this.jointC.jointFrame.nVector);
            Orientation frameD = artiePosition.artieFrame;
            Vector axisD = frameD.oVector;
            this._jointD = new ArmJoint(locD, axisD, frameD);
        }
    }
}
