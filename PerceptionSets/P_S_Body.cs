using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrices;
using AbstractDataClasses;
using UsefulStaticMethods;

namespace PerceptionSets
{
    [Serializable]
    /// <summary>
    /// Represents a perception of Artie's body position and orientation.
    /// Parent is P_Self. Children are P_H_S_Body and P_A_S_Body.
    /// Encapsulates a location and orientation and provides methods for initializing and
    /// manipulating them.
    /// </summary>
    public class P_S_Body : P_Self
    {
//Public fields
        private Point _location = new Point();
        virtual public Point location
        {
            get { return this._location; }
            set { this._location = value; }
        }

        private Orientation _artieFrame = new Orientation();
        virtual public Orientation artieFrame
        {
            get { return this._artieFrame; }
            set { this._artieFrame = value; }
        }


//Private fields
        private P_S_BodyMathSlave _mathSlave = null;
        private P_S_BodyMathSlave mathSlave
        {
            get { return this._mathSlave; }
            set { this._mathSlave = value; }
        }


        public P_S_Body()
        {
            this.artieFrame.alignVectorAlongAxis(Orientation.Vectors.nVector, Vector.Components.x);
            this.artieFrame.alignVectorAlongAxis(Orientation.Vectors.oVector, Vector.Components.y);
            this.artieFrame.alignVectorAlongAxis(Orientation.Vectors.pVector, Vector.Components.z);
            this._mathSlave = new P_S_BodyMathSlave(this);
        }


//Public methods
        public double computeNumericalDifferenceBetweenThisAnd(P_S_Body otherBody, Percent percentError)
        {
            double oriComp = this.artieFrame.evaluationFunction(otherBody.artieFrame, percentError);
            double locationComp = this.location.evaluationFunction(otherBody.location, percentError);
            double slopeConstant = 0.5;
            double evaluationValue = slopeConstant * Math.Pow(locationComp + oriComp, 2);
            return evaluationValue;
        }

        /// <summary>
        /// Returns true if both locations and artieFrames .Equals() one another.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            P_S_Body otherBodyState = (P_S_Body)obj;

            if (this.location.Equals(otherBodyState.location) && this.artieFrame.Equals(otherBodyState.artieFrame))
                return true;
            else
                return false;
        }

        public bool locationMatches(Point otherLocation, Percent precisionAsPercent)
        {
            if (this.location.matches(otherLocation, precisionAsPercent))
                return true;
            else
                return false;
        }

        public bool orientationMatches(Orientation otherOrientation, Percent precisionAsPercent)
        {
            if (this.artieFrame.matches(otherOrientation, precisionAsPercent))
                return true;
            else
                return false;
        }

        public virtual void refreshOrientation()
        {
            initializeOrientation();
        }

        public virtual void refreshPosition()
        {
            initializePosition();
        }

        public void turnTightLeft(DistanceTickFromBody distanceTicks, P_S_Arm arm)
        {
            double angle = this.mathSlave.decideHowToCalculateTightAngle(distanceTicks);
            this.mathSlave.tightTurn(angle, arm);
        }

        public void turnTightRight(DistanceTickFromBody distanceTicks, P_S_Arm arm)
        {
            double angle = (-1.0) * this.mathSlave.decideHowToCalculateTightAngle(distanceTicks);
            this.mathSlave.tightTurn(angle, arm);
        }

        public void turnWideLeft(DistanceTickFromBody distanceTicks, P_S_Arm arm)
        {
            double angle = this.mathSlave.decideHowToCalculateWideAngle(distanceTicks);
            bool rightTurn = false;
            this.mathSlave.wideTurn(rightTurn, angle, arm);
        }

        public void turnWideRight(DistanceTickFromBody distanceTicks, P_S_Arm arm)
        {
            double angle = (-1.0) * this.mathSlave.decideHowToCalculateWideAngle(distanceTicks);
            bool rightTurn = true;
            this.mathSlave.wideTurn(rightTurn, angle, arm);
        }

        public void driveForwards(DistanceTickFromBody distanceTicks, P_S_Arm arm)
        {
            double distance = this.mathSlave.decideHowToCalculateDistance(distanceTicks);
            this.mathSlave.driveForwards(distance, arm);
        }

        public void driveBackwards(DistanceTickFromBody distanceTicks, P_S_Arm arm)
        {
            double distance = this.mathSlave.decideHowToCalculateDistance(distanceTicks);
            this.mathSlave.driveBackwards(distance, arm);
        }

        public void setEqualToBodyPosition(P_S_Body psbody)
        {
            this.location = psbody.location;
            this.artieFrame = psbody.artieFrame;
        }


//Protected methods
        protected void initialize()
        {
            initializePosition();
            initializeOrientation();
            this.mathSlave = new P_S_BodyMathSlave(this);
        }


//Private methods
        private void initializeNVector()
        {
            this.artieFrame = this.artieFrame.alignVectorAlongAxis(
                Orientation.Vectors.nVector, Vector.Components.x);
        }

        private void initializeOVector()
        {
            this.artieFrame = this.artieFrame.alignVectorAlongAxis(
                Orientation.Vectors.oVector, Vector.Components.y);
        }

        private void initializePVector()
        {
            this.artieFrame = this.artieFrame.alignVectorAlongAxis(Orientation.Vectors.pVector,
                Vector.Components.z); 
        }

        private void initializeOrientation()
        {
            initializeNVector();
            initializeOVector();
            initializePVector();
        }

        private void initializePosition()
        {
            this.location.x = 0;
            this.location.y = 0;
            this.location.z = 0;
        }
    }
}
