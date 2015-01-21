using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptionSets;
using Matrices;
using UsefulStaticMethods;
using AbstractDataClasses;
using ActionSet;

namespace SearchAbstractDataTypes
{
    [Serializable]
    /// <summary>
    /// SearchNode encapsulates a perception - a state that Artie has experienced, either
    /// actual or hypothetical, and it provides the means to construct and connect together 
    /// sequences of those states.
    /// </summary>
    public class SearchNode
    {
//Public fields
        private SearchPartitionToGUI _guiCommunicator = null;
        public SearchPartitionToGUI guiCommunicator
        {
            get { return this._guiCommunicator; }
            set { this._guiCommunicator = value; }
        }
        
        private CompoundAction _methodUsedToDeriveNodeFromParent;
        public CompoundAction methodUsedToDeriveNodeFromParent
        {
            get { return this._methodUsedToDeriveNodeFromParent; }
            set { this._methodUsedToDeriveNodeFromParent = value; }
        }

        private SearchNodeGraphInformation _searchSpaceInformation = new SearchNodeGraphInformation();
        public SearchNodeGraphInformation searchSpaceInformation
        {
            get { return this._searchSpaceInformation; }
            private set { this._searchSpaceInformation = value; }
        }

        private SearchNode _parent;
        public SearchNode parent
        {
            get { return this._parent; }
            set
            {
                this._parent = value;
                this.nodeAsString.parentNode = this;
                if (this._parent != null)
                    this.searchSpaceInformation.parentID =
                        this._parent.searchSpaceInformation.nodeID;
            }
        }

        private P_S_Arm _perceptionState_Arm = null;
        public P_S_Arm perceptionState_Arm
        {
            get { return this._perceptionState_Arm; }
            set { this._perceptionState_Arm = value; }
        }

        private P_S_Body _perceptionState_Body = null;
        public P_S_Body perceptionState_Body
        {
            get { return this._perceptionState_Body; }
            set { this._perceptionState_Body = value; }
        }

        private Percent _precision;
        public Percent precision
        {
            get { return this._precision; }
            private set { this._precision = value; }
        }


//Internal fields
        private ISearchNodeBehavior _behavior;
        internal ISearchNodeBehavior behavior
        {
            get { return this._behavior; }
            private set { this._behavior = value; }
        }

        private SearchNodeAsString _nodeAsString = null;
        private SearchNodeAsString nodeAsString
        {
            get { return this._nodeAsString; }
            set { this._nodeAsString = value; }
        }

        
//Private fields
        private bool _whichPartsAreInUseIsUnkown = true;
        private bool whichPartsAreInUseIsUnkown
        {
            get { return this._whichPartsAreInUseIsUnkown; }
            set { this._whichPartsAreInUseIsUnkown = value; }
        }
        private bool _armIsUsed = false;
        private bool armIsUsed
        {
            get { return this._armIsUsed; }
            set { this._armIsUsed = value; }
        }
        private bool _bodyIsUsed = false;
        private bool bodyIsUsed
        {
            get { return this._bodyIsUsed; }
            set { this._bodyIsUsed = value; }
        }

        

//Constructors
        public SearchNode(Percent precision = null, CompoundAction methodName = null,
            P_S_Body position = null, P_S_Arm arm = null)
        {
            this.nodeAsString = new SearchNodeAsString(this);

            this.precision = precision;
            
            this.methodUsedToDeriveNodeFromParent = methodName;
            
            if (position == null)
                this.perceptionState_Body = new P_S_Body();
            else
                this.perceptionState_Body.setEqualToBodyPosition(position);
            
            if (arm == null)
                this.perceptionState_Arm = new P_S_Arm(this.perceptionState_Body);
            else
                this.perceptionState_Arm.setEqualToArmPosition(arm);
        }

        public SearchNode(ref SearchPartitionToGUI guiComm)
        {
            this.nodeAsString = new SearchNodeAsString(this);

            this.perceptionState_Body = new P_S_Body();
            this.perceptionState_Arm = new P_S_Arm(this.perceptionState_Body);
            this._guiCommunicator = guiComm;
        }

        public SearchNode(ref SearchPartitionToGUI guiComm, Point location,
            Orientation artieFrame, Percent precision, Gripper gripper)
        {
            this.nodeAsString = new SearchNodeAsString(this);

            this.perceptionState_Body = new P_S_Body();
            this.perceptionState_Body.location = location;
            this.perceptionState_Body.artieFrame = artieFrame;
            this._guiCommunicator = guiComm;
            this.precision = precision;

            this.perceptionState_Arm = new P_S_Arm(gripper);
        }


//Public methods
        public bool armIsInPotentiallyDamagingConfiguration()
        {
            if (this.perceptionState_Arm.armIsInPotentiallyDamagingConfiguration(this.perceptionState_Body))
                return true;
            else
                return false;
        }

        public char[] buildArrayOfActionCharactersFromNode()
        {
            char[] solutionActionCharSequenceAsString = this.nodeAsString.parseMethodUsedToDeriveNodeToCharArray(this);

            return solutionActionCharSequenceAsString;
        }

        public void computeValue(SearchNode goalNode)
        {
            double value = computeDifferenceBetweenThisNodeAnd(goalNode);
            this.searchSpaceInformation.value = value;
        }

        public double computeDifferenceBetweenThisNodeAnd(SearchNode m)
        {
            double bodyDif = this.perceptionState_Body.computeNumericalDifferenceBetweenThisAnd(m.perceptionState_Body, m.precision);
            double armDif = this.perceptionState_Arm.computeNumericalDifferenceBetweenThisAnd(m.perceptionState_Arm, m.precision);

            return bodyDif + armDif;
        }

        public void copy(SearchNode n)
        {
            this.perceptionState_Body.artieFrame = n.perceptionState_Body.artieFrame;
            this.perceptionState_Body.location = n.perceptionState_Body.location;
            this.perceptionState_Arm.gripper = n.perceptionState_Arm.gripper.DeepClone();
            this.perceptionState_Arm.joint_A = n.perceptionState_Arm.joint_A.DeepClone();
            this.perceptionState_Arm.joint_B = n.perceptionState_Arm.joint_B.DeepClone();
            this.perceptionState_Arm.joint_C = n.perceptionState_Arm.joint_C.DeepClone();
            this.perceptionState_Arm.joint_D = n.perceptionState_Arm.joint_D.DeepClone();
            this.parent = n.parent;
            this.methodUsedToDeriveNodeFromParent = n.methodUsedToDeriveNodeFromParent;
            this.searchSpaceInformation.value = n.searchSpaceInformation.value;
            this.searchSpaceInformation.depth = n.searchSpaceInformation.depth;
            this._guiCommunicator = n.guiCommunicator;
            this.precision = n.precision;
            this.nodeAsString = n.nodeAsString;
        }

        public SearchNode deriveDaughterNode(CompoundAction actionName, CalibrationData calData = null)
        {
            SearchNode n = new SearchNode();
            n.copy(this);

            n.parent = this;
            n.modifyNodeAccordingToAction(actionName, calData);
            n.searchSpaceInformation.setValueToNotComputed();
            return n;
        }

        /// <summary>
        /// Returns true if both nodes' perceptionState_Bodys match. Otherwise, returns
        /// false. Currently, this does nothing with the perceptionState_Arm.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            SearchNode otherNode = (SearchNode)obj;

            if (this.perceptionState_Body.Equals(otherNode.perceptionState_Body))
                return true;
            else
                return false;
        }

        //distributed
        public void increaseNodeDepth()
        {
            this.behavior.increaseNodeDepth();
        }

        public bool matches(SearchNode m)
        {
            bool bodyLocMatches = bodyLocationMatches(m.perceptionState_Body.location);
            bool bodyOriMatches = bodyOrientationMatches(m.perceptionState_Body.artieFrame);
            bool gripperLocMatches = gripperLocationMatches(m.perceptionState_Arm.gripper.location);
            bool gripperOriMatches = true;// gripperOrientationMatches(m.perceptionState_Arm.gripper.jointFrame);

            bool valueIsNotComputed = this.searchSpaceInformation.valueIsNotComputed();
            bool valueIsComputed = !valueIsNotComputed;

            double valueThreshold = 0.02;//if both arm and body are in use

            if (this.whichPartsAreInUseIsUnkown)
            {
                this.armIsUsed = armIsInUse(gripperLocMatches, gripperOriMatches);
                this.bodyIsUsed = bodyIsInUse(bodyLocMatches, bodyOriMatches);
                this.whichPartsAreInUseIsUnkown = false;//now we know
            }
            //if only body is in use: val threshold = 0.05
            if (bodyIsUsed && !armIsUsed)
                valueThreshold = 0.05;
            //if only arm is in use: val threshold = 0.001
            if (!bodyIsUsed && armIsUsed)
                valueThreshold = 0.001;

            if (bodyLocMatches && bodyOriMatches && gripperLocMatches && gripperOriMatches)
                return true;
            else if ((valueIsComputed) && (this.searchSpaceInformation.value < valueThreshold))
                return true;
            else
                return false;
        }

        public bool matches(SearchNode m, Percent allowableError)
        {
            bool bodyLocMatches = bodyLocationMatches(m.perceptionState_Body.location, allowableError);
            bool bodyOriMatches = bodyOrientationMatches(m.perceptionState_Body.artieFrame, allowableError);
            bool gripperLocMatches = gripperLocationMatches(m.perceptionState_Arm.gripper.location, allowableError);
            bool gripperOriMatches = true;//gripperOrientationMatches(m.perceptionState_Arm.gripper.jointFrame, allowableError);

            bool valueIsNotComputed = this.searchSpaceInformation.valueIsNotComputed();
            bool valueIsComputed = !valueIsNotComputed;

            double valueThreshold = 0.02;//if both arm and body are in use

            if (this.whichPartsAreInUseIsUnkown)
            {
                this.armIsUsed = armIsInUse(gripperLocMatches, gripperOriMatches);
                this.bodyIsUsed = bodyIsInUse(bodyLocMatches, bodyOriMatches);
                this.whichPartsAreInUseIsUnkown = false;//now we know
            }
            //if only body is in use: val threshold = 0.05
            if (bodyIsUsed && !armIsUsed)
                valueThreshold = 0.05;
            //if only arm is in use: val threshold = 0.001
            if (!bodyIsUsed && armIsUsed)
                valueThreshold = 0.001;

            if (bodyLocMatches && bodyOriMatches && gripperLocMatches && gripperOriMatches)
                return true;
            else if ((valueIsComputed) && (this.searchSpaceInformation.value < valueThreshold))
                return true;
            else
                return false;
        }

        public void modifyNodeAccordingToAction(CompoundAction action, CalibrationData calData = null)
        {
            this.methodUsedToDeriveNodeFromParent = action;

            List<ElementaryAction> compoundActionAsList = action.parseCompoundMethodIntoElementaryActions();
            foreach (ElementaryAction a in compoundActionAsList)
            {
                applyElementaryActionToNode(a, calData);
            }
        }

        public void setArmPositionEqualToAnotherArmPosition(P_S_Arm otherArmPosition)
        {
            this.perceptionState_Arm.setEqualToArmPosition(otherArmPosition);
        }

        public void setBodyPositionEqualToAnotherBodyPosition(P_S_Body otherBodyPosition)
        {
            this.perceptionState_Body.setEqualToBodyPosition(otherBodyPosition);
        }

        public Stack<SearchNodeInformation> solutionExpand()
        {
            Stack<SearchNodeInformation> solutionAsStack = this.nodeAsString.deriveSearchTreeUpToThisNodeAsString(this);
            
            return solutionAsStack;
        }

        //database
        public string solutionExpand(SearchNode expansionLimit)
        {
            return this.behavior.solutionExpand(expansionLimit);
        }

        public void stringExpandForGUICommunicator(string failureExplanation = "\n")
        {
            Stack<SearchNodeInformation> solutionAsStack = 
                this.nodeAsString.deriveSearchTreeUpToThisNodeAsString(this);
            string solution = StringMethods.buildStringFromStack(solutionAsStack);
            this.guiCommunicator.refreshTextLog = failureExplanation + solution;
        }

        public string stringExpandForControlPanel()
        {
            Stack<SearchNodeInformation> solutionAsStack =
                this.nodeAsString.deriveSearchTreeUpToThisNodeAsString(this);
            string solution = StringMethods.buildStringFromStack(solutionAsStack);
            return solution;
        }

        public override string ToString()
        {
            return new SearchNodeInformation(this).ToString();
        }


//Internal methods
        internal void absoluteOrSetToZeroBothNumbersIfEitherIsNAN(ref double a, ref double b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            NumberMethods.setBothNumbersToZeroIfEitherIsNaN(ref a, ref b);
        }

        
//Private methods
        private bool armIsInUse(bool armLocMatches, bool armOriMatches)
        {
            //if either one fails to match, then the arm is in use
            if (!(armLocMatches && armOriMatches))
                return true;
            else
                return false;
        }

        private void applyElementaryActionToNode(ElementaryAction action, CalibrationData calData)
        {
            int calDataAsInt;
            if (action == ElementaryAction.DRIVE_FORWARDS)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.bodyIsNull() ? DistanceTick.neutralValue : calData.bodyDataTicks;
                this.perceptionState_Body.driveForwards(calDataAsInt, this.perceptionState_Arm);
            }
            else if (action == ElementaryAction.DRIVE_BACKWARDS)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.bodyIsNull() ? DistanceTick.neutralValue : calData.bodyDataTicks;
                this.perceptionState_Body.driveBackwards(calDataAsInt, this.perceptionState_Arm);
            }
            else if (action == ElementaryAction.TURN_WIDE_LEFT)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.bodyIsNull() ? DistanceTick.neutralValue : calData.bodyDataTicks;
                this.perceptionState_Body.turnWideLeft(calDataAsInt, this.perceptionState_Arm);
            }
            else if (action == ElementaryAction.TURN_WIDE_RIGHT)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.bodyIsNull() ? DistanceTick.neutralValue : calData.bodyDataTicks;
                this.perceptionState_Body.turnWideRight(calDataAsInt, this.perceptionState_Arm);
            }
            else if (action == ElementaryAction.TURN_TIGHT_LEFT)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.bodyIsNull() ? DistanceTick.neutralValue : calData.bodyDataTicks;
                this.perceptionState_Body.turnTightLeft(calDataAsInt, this.perceptionState_Arm);
            }
            else if (action == ElementaryAction.TURN_TIGHT_RIGHT)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.bodyIsNull() ? DistanceTick.neutralValue : calData.bodyDataTicks;
                this.perceptionState_Body.turnTightRight(calDataAsInt, this.perceptionState_Arm);
            }
            else if (action == ElementaryAction.DRIVE_A_CLOCKWISE)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.armIsNull() ? DistanceTick.neutralValue : calData.armDataTicks;
                this.perceptionState_Arm.driveAClockWise(calDataAsInt);
            }
            else if (action == ElementaryAction.DRIVE_A_COUNTERCLOCKWISE)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.armIsNull() ? DistanceTick.neutralValue : calData.armDataTicks;
                this.perceptionState_Arm.driveACounterClockWise(calDataAsInt);
            }
            else if (action == ElementaryAction.DRIVE_B_CLOCKWISE)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.armIsNull() ? DistanceTick.neutralValue : calData.armDataTicks;
                this.perceptionState_Arm.driveBClockWise(calDataAsInt);
            }
            else if (action == ElementaryAction.DRIVE_B_COUNTERCLOCKWISE)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.armIsNull() ? DistanceTick.neutralValue : calData.armDataTicks;
                this.perceptionState_Arm.driveBCounterClockWise(calDataAsInt);
            }
            else if (action == ElementaryAction.DRIVE_C_CLOCKWISE)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.armIsNull() ? DistanceTick.neutralValue : calData.armDataTicks;
                this.perceptionState_Arm.driveCClockWise(calDataAsInt);
            }
            else if (action == ElementaryAction.DRIVE_C_COUNTERCLOCKWISE)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.armIsNull() ? DistanceTick.neutralValue : calData.armDataTicks;
                this.perceptionState_Arm.driveCCounterClockWise(calDataAsInt);
            }
            else if (action == ElementaryAction.DRIVE_D_CLOCKWISE)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.armIsNull() ? DistanceTick.neutralValue : calData.armDataTicks;
                this.perceptionState_Arm.driveDClockWise(calDataAsInt);
            }
            else if (action == ElementaryAction.DRIVE_D_COUNTERCLOCKWISE)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.armIsNull() ? DistanceTick.neutralValue : calData.armDataTicks;
                this.perceptionState_Arm.driveDCounterClockWise(calDataAsInt);
            }
            else if (action == ElementaryAction.OPEN_CLAW)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.armIsNull() ? DistanceTick.neutralValue : calData.armDataTicks;
                this.perceptionState_Arm.openGripper(calDataAsInt);
            }
            else if (action == ElementaryAction.CLOSE_CLAW)
            {
                if (calData == null)
                    calDataAsInt = DistanceTick.neutralValue;
                else
                    calDataAsInt = calData.armIsNull() ? DistanceTick.neutralValue : calData.armDataTicks;
                this.perceptionState_Arm.closeGripper(calDataAsInt);
            }
        }

        private bool bodyIsInUse(bool bodyLocMatches, bool bodyOriMatches)
        {
            //if either one fails to match, then the body is in use
            if (!(bodyLocMatches && bodyOriMatches))
                return true;
            else
                return false;
        }

        private bool bodyOrientationMatches(Orientation otherOrientation)
        {
            if (this.perceptionState_Body.orientationMatches(otherOrientation, this.precision))
                return true;
            else
                return false;
        }

        private bool bodyOrientationMatches(Orientation otherOrientation, Percent allowedError)
        {
            if (this.perceptionState_Body.orientationMatches(otherOrientation, allowedError))
                return true;
            else
                return false;
        }

        private bool bodyLocationMatches(Point otherLoc)
        {
            if (this.perceptionState_Body.locationMatches(otherLoc, this.precision))
                return true;
            else
                return false;
        }

        private bool bodyLocationMatches(Point otherLoc, Percent allowedError)
        {
            if (this.perceptionState_Body.locationMatches(otherLoc, allowedError))
                return true;
            else
                return false;
        }

        private bool gripperLocationMatches(Point otherLoc)
        {
            if (this.perceptionState_Arm.gripper.locationMatches(otherLoc, this.precision))
                return true;
            else
                return false;
        }

        private bool gripperLocationMatches(Point otherLoc, Percent allowedError)
        {
            if (this.perceptionState_Arm.gripper.locationMatches(otherLoc, allowedError))
                return true;
            else
                return false;
        }

        private bool gripperOrientationMatches(Orientation otherOri)
        {
            if (this.perceptionState_Arm.gripper.orientationMatches(otherOri, this.precision))
                return true;
            else
                return false;
        }

        private bool gripperOrientationMatches(Orientation otherOri, Percent allowedError)
        {
            if (this.perceptionState_Arm.gripper.orientationMatches(otherOri, allowedError))
                return true;
            else
                return false;
        }
        
        //database
        private bool matchesDB(SearchNode m)
        {
            return this.behavior.matches(m);
        }
    }
}
