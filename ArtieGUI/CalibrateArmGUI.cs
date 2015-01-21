using PerceptionSets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtieGUI
{
    public partial class CalibrateArmGUI : Form
    {
        private P_S_Arm arm
        {
            get { return this.parent.artieController.perception_Actual.perceptionState_Arm; }
        }
        private ToolStripController _parent = null;
        private ToolStripController parent
        {
            get { return this._parent; }
            set { this._parent = value; }
        }

//Constructor
        public CalibrateArmGUI(ToolStripController parent)
        {
            InitializeComponent();
            this.parent = parent;
            changeArmValuesInUI();
        }



//Private methods
        private void changeArmValuesInUI()
        {
            setClawLabels(this.arm.gripper);
            setJoint_A_Labels(this.arm.joint_A);
            setJoint_B_Labels(this.arm.joint_B);
            setJoint_C_Labels(this.arm.joint_C);
            setJoint_D_Labels(this.arm.joint_D);
        }

        private void jointAClockwiseButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.parent.artieController.removeArmSafetyForCalibration();
            this.parent.artieController.driveJointAClockwise();
        }

        private void jointAClockwiseButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.parent.artieController.stopDriving();
            changeArmValuesInUI();
        }

        private void jointACounterClockwiseButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.parent.artieController.removeArmSafetyForCalibration();
            this.parent.artieController.driveJointACounterClockwise();
        }

        private void jointACounterClockwiseButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.parent.artieController.stopDriving();
            changeArmValuesInUI();
        }

        private void jointBClockWiseButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.parent.artieController.removeArmSafetyForCalibration();
            this.parent.artieController.driveJointBClockwise();
        }

        private void jointBClockWiseButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.parent.artieController.stopDriving();
            changeArmValuesInUI();
        }

        private void jointBCounterClockwiseButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.parent.artieController.removeArmSafetyForCalibration();
            this.parent.artieController.driveJointBCounterClockwise();
        }

        private void jointBCounterClockwiseButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.parent.artieController.stopDriving();
            changeArmValuesInUI();
        }

        private void jointCClockwiseButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.parent.artieController.removeArmSafetyForCalibration();
            this.parent.artieController.driveJointCClockwise();
        }

        private void jointCClockwiseButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.parent.artieController.stopDriving();
            changeArmValuesInUI();
        }

        private void jointCCounterClockwiseButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.parent.artieController.removeArmSafetyForCalibration();
            this.parent.artieController.driveJointCCounterClockwise();
        }

        private void jointCCounterClockwiseButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.parent.artieController.stopDriving();
            changeArmValuesInUI();
        }

        private void jointDClockwiseButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.parent.artieController.removeArmSafetyForCalibration();
            this.parent.artieController.driveJointDClockwise();
        }

        private void jointDClockwiseButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.parent.artieController.stopDriving();
            changeArmValuesInUI();
        }

        private void jointDCounterClockwiseButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.parent.artieController.removeArmSafetyForCalibration();
            this.parent.artieController.driveJointDCounterClockwise();
        }

        private void jointDCounterClockwiseButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.parent.artieController.stopDriving();
            changeArmValuesInUI();
        }

        private void gripperOpenButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.parent.artieController.removeArmSafetyForCalibration();
            this.parent.artieController.openGripper();
        }

        private void gripperOpenButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.parent.artieController.stopDriving();
            changeArmValuesInUI();
        }

        private void gripperCloseButton_MouseDown(object sender, MouseEventArgs e)
        {
            this.parent.artieController.removeArmSafetyForCalibration();
            this.parent.artieController.closeGripper();
        }

        private void gripperCloseButton_MouseUp(object sender, MouseEventArgs e)
        {
            this.parent.artieController.stopDriving();
            changeArmValuesInUI();
        }

        private void CalibrateArmGUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.parent.artieController.setArmSafety();
            this.parent.calibrateArmDialogIsShowing = false;
        }

        private void setClawLabels(Gripper claw)
        {
            this.clawLocationLabel.Text = claw.location.ToString();
            this.clawNVectorLabel.Text = claw.jointFrame.nVector.ToString();
            this.clawOVectorLabel.Text = claw.jointFrame.oVector.ToString();
            this.clawPVectorLabel.Text = claw.jointFrame.pVector.ToString();
        }

        private void setJoint_A_Labels(ArmJoint joint_A)
        {
            this.jointALocationLabel.Text = joint_A.location.ToString();
            this.jointA_nVectorLabel.Text = joint_A.jointFrame.nVector.ToString();
            this.jointA_oVectorLabel.Text = joint_A.jointFrame.oVector.ToString();
            this.jointA_pVectorLabel.Text = joint_A.jointFrame.pVector.ToString();
        }

        private void setJoint_B_Labels(ArmJoint joint_B)
        {
            this.jointBLocationLabel.Text = joint_B.location.ToString();
            this.jointB_nVectorLabel.Text = joint_B.jointFrame.nVector.ToString();
            this.jointB_oVectorLabel.Text = joint_B.jointFrame.oVector.ToString();
            this.jointB_pVectorLabel.Text = joint_B.jointFrame.pVector.ToString();
        }

        private void setJoint_C_Labels(ArmJoint joint_C)
        {
            this.jointCLocationLabel.Text = joint_C.location.ToString();
            this.jointC_nVectorLabel.Text = joint_C.jointFrame.nVector.ToString();
            this.jointC_oVectorLabel.Text = joint_C.jointFrame.oVector.ToString();
            this.jointC_pVectorLabel.Text = joint_C.jointFrame.pVector.ToString();
        }

        private void setJoint_D_Labels(ArmJoint joint_D)
        {
            this.jointDLocationLabel.Text = joint_D.location.ToString();
            this.jointD_nVectorLabel.Text = joint_D.jointFrame.nVector.ToString();
            this.jointD_oVectorLabel.Text = joint_D.jointFrame.oVector.ToString();
            this.jointD_pVectorLabel.Text = joint_D.jointFrame.pVector.ToString();
        }
    }
}
