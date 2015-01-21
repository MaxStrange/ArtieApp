using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrices
{
    [Serializable]
    public class Matrix
    {

//Public fields
        protected double[] _listOfvalues;
        //READONLY!
        virtual public double[] listOfvalues
        {
            get
            {
                this.computeValues();
                return this._listOfvalues;
            }
        }
        protected int _numberOfColumns;
        virtual public int numberOfColumns
        {
            get { return this._numberOfColumns; }
        }
        protected int _numberOfRows;
        virtual public int numberOfRows
        {
            get { return this._numberOfRows; }
        }
        protected int _size;
        virtual public int size
        {
            get { return (this.numberOfRows * this.numberOfColumns); }
        }

        
        
//Private fields   
        //Row, Column
        private double[,] _values;
        private double[,] values
        {
            get { return this._values; }
            set
            {
                if (this.numberOfRows != value.GetLength(0))
                    throw new RowWrongSizeException();
                if (this.numberOfColumns != value.GetLength(1))
                    throw new ColumnWrongSizeException();

                this._values = value;
                this.computeValues();
            }
        }
        

//Constructors        
        public Matrix()
        {
        }

        public Matrix(int numberOfRows, int numberOfColumns)
        {
            this._numberOfRows = numberOfRows;
            this._numberOfColumns = numberOfColumns;

            this._listOfvalues = new double[this.numberOfRows * this.numberOfColumns];
            this.values = new double[this.numberOfRows, this.numberOfColumns];
        }


//Operator Overrides
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            if (m2 is Vector)
                m2 = Matrix.convertVectorToMatrix(m2 as Vector);
            
            m1.checkForIncompatibleMatrixException(m2);

            Matrix newMatrix = new Matrix(m1.numberOfRows, m1.numberOfColumns);
            newMatrix.populateEachValue((int i, int j) => m1.values[i, j] + m2.values[i, j]);
            
            return newMatrix;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m2 is Vector)
                m2 = Matrix.convertVectorToMatrix(m2 as Vector);
            
            m1.checkForIncompatibleMatrixException(m2);

            Matrix newMatrix = new Matrix(m1.numberOfRows, m1.numberOfColumns);
            newMatrix.populateEachValue((int i, int j) => m1.values[i, j] - m2.values[i, j]);
            
            return newMatrix;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            //this Matrix * secondMatrix (not the other way around)

            if (m1.numberOfColumns != m2.numberOfRows)
                throw new IncompatibleMatrixException();
            if (m2 is Vector)
                m2 = Matrix.convertVectorToMatrix(m2 as Vector);
            
            Matrix resultantMatrix = new Matrix(m1.numberOfRows, m2.numberOfColumns);
            resultantMatrix.populateEachValue((int i, int j) =>
                m1.convertRowToVector(i).computeDotProduct(m2.convertColumnToVector(j)));
            
            return resultantMatrix;
        }


//Public methods
        public bool checkIfEqualTo(Matrix matrixToCompareWith)
        {
            //Two matrices are equal if and only if they are identical

            if (this.numberOfColumns != matrixToCompareWith.numberOfColumns)
                return false;
            if (this.numberOfRows != matrixToCompareWith.numberOfRows)
                return false;

            for (int i = 0; i < this.listOfvalues.Length; i++)
            {
                if (this.listOfvalues[i] != matrixToCompareWith.listOfvalues[i])
                    return false;
            }

            return true;
        }

        public Vector convertColumnToVector(int columnNumberToConvert)
        {
            double[] column = new double[this.numberOfRows];
            for (int i = 0; i < this.numberOfRows; i++)
            {
                column[i] = this.values[i, columnNumberToConvert];
            }
            Vector columnAsVector = new Vector(this.numberOfRows, column);

            return columnAsVector;
        }

        public Vector convertRowToVector(int rowNumberToConvert)
        {
            double[] row = new double[this.numberOfColumns];
            for (int i = 0; i < this.numberOfColumns; i++)
            {
                row[i] = this.values[rowNumberToConvert, i];
            }
            Vector rowAsVector = new Vector(this.numberOfColumns, row);

            return rowAsVector;
        }

        public double getValueAtIndex(int rowIndex, int columnIndex)
        {
            return this.values[rowIndex, columnIndex];
        }

        public Matrix multiplyWithScalar(double scalar)
        {
            Matrix S = new Matrix(this.numberOfRows, this.numberOfColumns);
            S.populateEachValue((int i, int j) => this.values[i, j] * scalar);
            return S;
        }

        public void setValueAtIndex(int rowIndex, int columnIndex, double d)
        {
            this.values[rowIndex, columnIndex] = d;
        }


//Internal methods
        internal void populateRow(int rowNumberToPopulate, double[] rowValues)
        {
            if (this.numberOfColumns != rowValues.Length)
                throw new RowWrongSizeException();

            for (int i = 0; i < rowValues.Length; i++)
            {
                this.values[rowNumberToPopulate, i] = rowValues[i];
            }
        }

        
        
//Private methods
        private void checkForIncompatibleMatrixException(Matrix otherMatrix)
        {
            if (this.numberOfColumns != otherMatrix.numberOfColumns)
                throw new IncompatibleMatrixException();
            if (this.numberOfRows != otherMatrix.numberOfRows)
                throw new IncompatibleMatrixException();
        }

        private void computeValues()
        {
            this._listOfvalues = new double[this.numberOfRows * this.numberOfColumns];
            int k = 0;
            for (int i = 0; i < this.numberOfRows; i++)
            {
                for (int j = 0; j < this.numberOfColumns; j++)
                {
                    this._listOfvalues[k] = this.values[i, j];
                    k++;
                }
            }
        }

        private static Matrix convertVectorToMatrix(Vector v)
        {
            double[] values = v.listOfvalues;
            Matrix m = new Matrix(v.numberOfRows, 1);
            m.populateColumn(0, values);

            return m;
        }

        private void populateColumn(int columnNumberToPopulate, double[] columnValues)
        {
            if (this.numberOfRows != columnValues.Length)
                throw new ColumnWrongSizeException();

            for (int i = 0; i < columnValues.Length; i++)
            {
                this.values[i, columnNumberToPopulate] = columnValues[i];
            }
        }

        private void populateEachValue(Func<int, int, double> algorithmToProduce_ith_jth_value)
        {
            for (int i = 0; i < this.numberOfRows; i++)
            {
                double[] rowValues = new double[this.numberOfColumns];
                for (int j = 0; j < rowValues.Length; j++)
                {
                    rowValues[j] = algorithmToProduce_ith_jth_value(i, j);
                }
                this.populateRow(i, rowValues);
            }
        }
    }
}
