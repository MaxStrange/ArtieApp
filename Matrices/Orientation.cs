using AbstractDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulStaticMethods;

namespace Matrices
{
    [Serializable]
    /// <summary>
    /// An Orientation is an object that represents the orientation of a physical entity.
    /// It contains three vectors:
    /// The n vector faces "forwards" - so if the entity has a front, the n vector should
    /// be used to represent the direction the front is facing.
    /// The o vector faces along the positive y-axis of the World when the n vector points
    /// along the positive x-axis of the World.
    /// The p vector points towards you if you are looking down at the World coordinates
    /// where x points right and y points up when n and o are aligned along the x and y
    /// respectively.
    /// IMPORTANT: Orientation has been designed to be immutable - anytime it is changed
    /// from the outside, it returns a new Orientation object that reflects those changes.
    /// </summary>
    public class Orientation
    {
        public enum Vectors
        {
            nVector = 0,
            oVector = 1,
            pVector = 2
        };

//Public methods
        private Vector _nVector = new Vector(3);
        public Vector nVector
        {
            get { return this._nVector; }
            internal set 
            { 
                this._nVector = value;
                this._nVector.normalizeLengthOrSetAllValuesToNAN();
            }
        }

        private Vector _oVector = new Vector(3);
        public Vector oVector
        {
            get { return this._oVector; }
            internal set 
            {
                this._oVector = value;
                this._oVector.normalizeLengthOrSetAllValuesToNAN();
            }
        }

        private Vector _pVector = new Vector(3);
        public Vector pVector
        {
            get { return this._pVector; }
            internal set 
            {
                this._pVector = value;
                this._pVector.normalizeLengthOrSetAllValuesToNAN();
            }
        }


//Constructors
        public Orientation()
        {
        }

        public Orientation(Vector nVector, Vector oVector, Vector pVector)
        {
            this.nVector = nVector;
            this.oVector = oVector;
            this.pVector = pVector;
        }


//Overridden operators
        public static Orientation operator +(Orientation or1, Orientation or2)
        {
            return new Orientation(or1.nVector + or2.nVector,
                or1.oVector + or2.oVector, or1.pVector + or2.pVector);  
        }

        public static Orientation operator -(Orientation or1, Orientation or2)
        {
            return new Orientation(or1.nVector - or2.nVector,
                or1.oVector - or2.oVector, or1.pVector - or2.pVector);
        }


//Public methods
        public Orientation alignVectorAlongAxis(Orientation.Vectors v, Vector.Components axis)
        {
            double[] components = unityAlongAxis(axis);
            switch (v)
            {
                case Vectors.nVector:
                    this.nVector = new Vector(3, components);
                    return this;
                case Vectors.oVector:
                    this.oVector = new Vector(3, components);
                    return this;
                case Vectors.pVector:
                    this.pVector = new Vector(3, components);
                    return this;
                default:
                    return this;
            }
        }

        public double evaluationFunction(Orientation ori2, Percent percent)
        {
            return this.nVector.evaluationFunction(ori2.nVector, percent) +
                this.oVector.evaluationFunction(ori2.oVector, percent) +
                this.pVector.evaluationFunction(ori2.pVector, percent);
        }

        /// <summary>
        /// Returns true if this.vector_i.Equals(obj.vector_i) for all values of i. Otherwise,
        /// returns false.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Orientation otherOri = (Orientation)obj;

            if (this.nVector.Equals(otherOri.nVector) && this.oVector.Equals(otherOri.oVector)
                && this.pVector.Equals(otherOri.pVector))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the x-component of Artie's forward-facing vector (the n-vector)
        /// is 0.9 or more.
        /// </summary>
        /// <returns></returns>
        public bool isFacingInXDirection()
        {
            if (this.nVector.getValueAtIndex((int)Vector.Components.x) > 0.9)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the y-component of Artie's forward-facing vector (the n-vector)
        /// is 0.9 or more.
        /// </summary>
        /// <returns></returns>
        public bool isFacingInYDirection()
        {
            if (this.nVector.getValueAtIndex((int)Vector.Components.y) > 0.9)
                return true;
            else
                return false;
        }

        public bool matches(Orientation otherOrientation, Percent precisionAsPercent)
        {
            if (this.nVector.matches(otherOrientation.nVector, precisionAsPercent)
                && this.oVector.matches(otherOrientation.oVector, precisionAsPercent)
                && this.pVector.matches(otherOrientation.pVector, precisionAsPercent))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns a new orientation which has been derived from the old one by rotating
        /// each vector around the same axis. Of course, if one of the vectors is perfectly
        /// aligned with the axisOfRotation, it will not be changed in the new orientation.
        /// </summary>
        /// <param name="axisOfRotation"></param>
        /// <param name="axisOrigin"></param>
        /// <param name="angleOfRotation"></param>
        /// <returns></returns>
        public Orientation rotateEachVectorAroundAnAxis(Vector axisOfRotation, Point axisOrigin, double angleOfRotation)
        {
            Orientation ori = new Orientation(this.nVector, this.oVector, this.pVector);
            ori.applyAlgorithmToEachOrientationVector((Vector v) =>
                 MatrixMath.rotateVectorAroundVectorGeneralForm(axisOfRotation, axisOrigin,
                     angleOfRotation, v));

            return ori;
        }


//Private methods
        /// <summary>
        /// This applies the passed algorithm to each vector in the orientation, but does
        /// not return a new orientation. The algorithm takes each vector as its argument
        /// and applies the algorithm to the vector.
        /// </summary>
        /// <param name="algorithmToApplyToEachVector"></param>
        private void applyAlgorithmToEachOrientationVector(Action<Vector> algorithmToApplyToEachVector)
        {
            algorithmToApplyToEachVector(this.nVector);
            algorithmToApplyToEachVector(this.oVector);
            algorithmToApplyToEachVector(this.pVector);
        }

        /// <summary>
        /// This applies the passed algorithm to each vector in the orientation. The
        /// algorithm takes no arguments and returns a vector. The algorithm is used to
        /// generate each orientation vector.
        /// </summary>
        /// <param name="algorithmForProducingEachVector"></param>
        /// <returns></returns>
        private void applyAlgorithmToEachOrientationVector(Func<Vector> algorithmForProducingEachVector)
        {
            Vector v = algorithmForProducingEachVector();
            
            this.nVector = v;
            this.oVector = v;
            this.pVector = v;
        }

        /// <summary>
        /// This applies the passed algorithm to each vector in the orientation. The algorithm
        /// takes each orientation vector in turn and returns a vector that then replaces it.
        /// </summary>
        /// <param name="algorithmToProduce_ith_Vector"></param>
        /// <returns></returns>
        private void applyAlgorithmToEachOrientationVector(Func<Vector, Vector> algorithmToProduce_ith_Vector)
        {
            this.nVector = algorithmToProduce_ith_Vector(this.nVector);
            this.oVector = algorithmToProduce_ith_Vector(this.oVector);
            this.pVector = algorithmToProduce_ith_Vector(this.pVector);
        }

        private double[] unityAlongAxis(Vector.Components axis)
        {
            switch (axis)
            {
                case Vector.Components.x:
                    return new double[] { 1, 0, 0 };
                case Vector.Components.y:
                    return new double[] { 0, 1, 0 };
                case Vector.Components.z:
                    return new double[] { 0, 0, 1 };
                default:
                    throw new Exception("Axis value must be from Vector.Components enum");
            }
        }
    }
}
