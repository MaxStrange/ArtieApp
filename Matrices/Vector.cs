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
    public class Vector : Matrix
    {
        public enum Components
        {
            x = 0,
            y = 1,
            z = 2
        };


//Public fields
        public override double[] listOfvalues
        {
            get
            {
                this.computeValues();
                return this._listOfvalues;
            }
        }

        private double _length;
        public double length
        {
            get
            {
                this.computeLength();
                return this._length;
            }
        }

        public int dimension
        {
            get
            {
                return this.values.Length;
            }
        }

        private double[] _values;
        private double[] values
        {
            get { return this._values; }
            set { this._values = value; }
        }


//Constructors
        public Vector(int dimension)
        {
            initialize(dimension);
        }

        public Vector(int dimension, double[] components)
        {
            initialize(dimension);
            this.values = components;
        }


//Overidden operators
        public static Vector operator +(Vector v1, Vector v2)
        {
            v1.checkIfVectorsHaveSameDimension(v2);
            
            Vector v3 = new Vector(v1.dimension);
            for (int i = 0; i < v1.values.Length; i++)
            {
                double d = v1.getValueAtIndex(i) + v2.getValueAtIndex(i);
                if (Double.IsNaN(d))
                    d = 0;

                v3.setValueAtIndex(i, d);
            }

            return v3;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            v1.checkIfVectorsHaveSameDimension(v2);

            Vector v3 = new Vector(v1.dimension);
            for (int i = 0; i < v1.values.Length; i++)
            {
                double d = v1.getValueAtIndex(i) + v2.getValueAtIndex(i);
                if (Double.IsNaN(d))
                    d = 0;

                v3.setValueAtIndex(i, d);
            }

            return v3;
        }


//Public methods
        public Tuple<double, double, double> convertXYZComponentsToTuple()
        {
            return new Tuple<double, double, double>(this.values[0], this.values[1], this.values[2]);
        }

        /// <summary>
        /// Returns true if both vectors are the same dimension and vector A's ith value
        /// matches (via NumberMethods.doublesRepresentTheSameNumber) vectorB's ith value
        /// for all values of i. Otherwise, returns false.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Vector otherVector = (Vector)obj;

            if (this.dimension != otherVector.dimension)
                return false;

            for (int i = 0; i < this.dimension; i++)
            {
                if (!this.listOfvalues[i].doublesRepresentTheSameNumber(otherVector.listOfvalues[i]))
                    return false;
            }
            return true;
        }

        public double evaluationFunction(Vector v2, Percent percent)
        {
            double xComp = Math.Pow(this.listOfvalues[(int)Components.x] - v2.listOfvalues[(int)Components.x], 2);
            xComp = double.IsNaN(xComp) ? 0.0 : xComp;
            double yComp = Math.Pow(this.listOfvalues[(int)Components.y] - v2.listOfvalues[(int)Components.y], 2);
            yComp = double.IsNaN(yComp) ? 0.0 : yComp;
            double zComp = Math.Pow(this.listOfvalues[(int)Components.z] - v2.listOfvalues[(int)Components.z], 2);
            zComp = double.IsNaN(zComp) ? 0.0 : zComp;
            double evalValue = Math.Sqrt(xComp + yComp + zComp);
            return this.matches(v2, percent) ? 0.0 : evalValue;
        }

        public double getValueAtIndex(int index)
        {
            return this.values[index];
        }

        public bool matches(Vector otherVector, Percent precisionAsPercent)
        {
            for (int i = 0; i < this.dimension; i++)
            {
                double thisComp = this.values[i];
                double otherComp = otherVector.values[i];

                if (!NumberMethods.sameSign(thisComp, otherComp))
                    return false;
                
                absoluteOrSetToZeroBothNumbers(ref thisComp, ref otherComp);
                
                if (!NumberMethods.A_FallsWithinPercentOf_B(thisComp, otherComp, precisionAsPercent.value))
                    return false;
            }
            return true;
        }

        new public Vector multiplyWithScalar(double scalar)
        {
            Vector S = new Vector(this.dimension);
            S.populateEachValue((int i) => this.values[i] * scalar);
            return S;
        }

        public void setValueAtIndex(int index, double d)
        {
            this.values[index] = d;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(").Append(String.Format("{0:0.00}", this.values[(int)Components.x])).Append(", ").Append(String.Format("{0:0.00}", this.values[(int)Components.y])).Append(", ").Append(String.Format("{0:0.00}", this.values[(int)Components.z])).Append(")");
            return sb.ToString();
        }


//Internal methods
        internal double computeDotProduct(Vector otherVector)
        {
            double dotProduct = 0.0;
            for (int i = 0; i < this.dimension; i++)
            {
                dotProduct += (this.values[i] * otherVector.values[i]);
            }

            return dotProduct;
        }

        internal Point convertToPoint()
        {
            Point p = new Point(this.values[0], this.values[1], this.values[2]);
            return p;
        }

        /// <summary>
        /// Converts the vector into a vector of dimension (this.dimension + 1) whose extra
        /// element is a 1.0, thus allowing a three dimensional vector to be translated by
        /// a square 4x4 translation matrix
        /// </summary>
        /// <returns></returns>
        internal Vector convertToTranslateableVector()
        {
            double[] newVectorValues = new double[this.dimension + 1];
            for (int i = 0; i < this.dimension; i++)
            {
                newVectorValues[i] = this.values[i];
            }
            newVectorValues[this.dimension] = 1.0;
            Vector newVector = new Vector(this.dimension + 1, newVectorValues);

            return newVector;
        }

        internal Vector convertFromTranslateableVector()
        {
            double[] newVectorValues = new double[this.dimension - 1];
            for (int i = 0; i < newVectorValues.Length; i++)
            {
                newVectorValues[i] = this.values[i];
            }
            Vector newVector = new Vector(this.dimension - 1, newVectorValues);

            return newVector;
        }

        internal void normalizeLengthOrSetAllValuesToNAN()
        {
            List<int> valuesThatWereNAN = changeAllNANValuesToZeros();

            if (NumberMethods.doublesRepresentTheSameNumber(this.length, 0))
            {
                changeAllValuesToNAN();
                return;
            }
            else
            {
                normalizeLength(valuesThatWereNAN);
            }
        }


//Private methods
        private void absoluteOrSetToZeroBothNumbers(ref double a, ref double b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            NumberMethods.setBothNumbersToZeroIfEitherIsNaN(ref a, ref b);
        }

        /// <summary>
        /// Returns a list of the indeces of the values that were NAN.
        /// </summary>
        /// <returns></returns>
        private List<int> changeAllNANValuesToZeros()
        {
            List<int> valuesThatWereNAN = new List<int>();

            for (int i = 0; i < this.dimension; i++)
            {
                if (double.IsNaN(this.values[i]))
                {
                    valuesThatWereNAN.Add(i);
                    this.values[i] = 0;
                }
            }
            return valuesThatWereNAN;
        }

        private void changeAllValuesToNAN()
        {
            for (int i = 0; i < this.dimension; i++)
            {
                this.values[i] = double.NaN;
            }
        }

        private void checkIfVectorsHaveSameDimension(Vector otherV)
        {
            if (this.dimension != otherV.dimension)
                throw new IncompatibleMatrixException();
        }
        
        private void computeLength()
        {
            double sum = 0.0;
            foreach (double d in this.values)
            {
                sum += (d * d);
            }
            this._length =  Math.Sqrt(sum);
        }

        private void computeValues()
        {
            this._listOfvalues = new double[this.dimension];
            for (int i = 0; i < this.dimension; i++)
            {
                this._listOfvalues[i] = this.values[i];
            }
        }

        private void initialize(int dimension)
        {
            this._numberOfColumns = 1;
            this._numberOfRows = dimension;
            this._listOfvalues = new double[this.numberOfRows];
            this.values = new double[this.numberOfRows];
        }

        /// <summary>
        /// Takes a list of indeces of that values that should be changed to NAN after
        /// normalizing the length.
        /// </summary>
        /// <param name="valuesThatShouldBeChangedToNAN"></param>
        private void normalizeLength(List<int> valuesThatShouldBeChangedToNAN)
        {
            double currentLength = this.length;

            for (int i = 0; i < this.dimension; i++)
            {
                this.values[i] = (this.values[i] / currentLength);
            }

            foreach (int i in valuesThatShouldBeChangedToNAN)
            {
                this.values[i] = double.NaN;
            }
        }

        private void populateEachValue(Func<int, double> algorithmToProduce_ith_value)
        {
            for (int i = 0; i < this.dimension; i++)
            {
                this.values[i] = algorithmToProduce_ith_value(i);
            }
        }
    }
}
