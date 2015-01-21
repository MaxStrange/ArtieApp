using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrices
{
    public static class MatrixMath
    {
        //The first index is the row number starting from zero
        //The second index is the column number starting from zero

        //TODO : refactor into Vector, Point, and Matrix classes.

//Public and internal methods
        /// <summary>
        /// Multiplies each component of v by (-1), thus giving a vector which, if added to
        /// v, will result in the zero vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector computeNegativeVector(Vector v)
        {
            Vector v_neg = v.multiplyWithScalar(-1.0);
            return v_neg;
        }

        /// <summary>
        /// Multiplies each component of p by (-1), thus giving a vector which, if added to
        /// p, will result in the zero vector.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Vector computeNegativeVector(Point p)
        {
            Vector v_neg = p.convertToVector().multiplyWithScalar(-1.0);
            return v_neg;
        }

        internal static double[] convertComponentArrayToDoubleArray(Component[] compArray)
        {
            double[] doubleArray = new double[compArray.Length];
            for (int i = 0; i < compArray.Length; i++)
            {
                doubleArray[i] = compArray[i].componentValue;
            }
            return doubleArray;
        }

        internal static Tuple<double, double, double> multiplyThroughTuple(Tuple<Component, Component, Component> tup, double d)
        {
            return new Tuple<double, double, double>(tup.Item1.componentValue * d, tup.Item2.componentValue * d, tup.Item3.componentValue * d);
        }

        public static Tuple<double, double, double> multiplyThroughTuple(Tuple<double, double, double> tup, double d)
        {
            return new Tuple<double, double, double>(tup.Item1 * d, tup.Item2 * d, tup.Item3 * d);
        }

        public static Point rotatePoint(Vector axisOfRotationAsUnitVectorThroughOrigin, double angleOfRotation, Point pointToRotate)
        {
            Matrix rotationMatrix = computeRotationMatrix(axisOfRotationAsUnitVectorThroughOrigin, angleOfRotation);
            Matrix rotatedPointAsMatrix = rotationMatrix * pointToRotate.convertToVector();
            return rotatedPointAsMatrix.convertColumnToVector(0).convertToPoint();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axisOfRotation">
        /// The axis vector as components, irrespective of where it is in space.
        /// </param>
        /// <param name="axisOrigin">
        /// The point that the axis vector stems from. Or, if the axis is a line, it is a
        /// point through which the axis passes.
        /// </param>
        /// <param name="angleOfRotation"></param>
        /// <param name="pointToRotate"></param>
        /// <returns></returns>
        public static Point rotatePointAroundVectorGeneralForm(Vector axisOfRotation, Point axisOrigin, double angleOfRotation, Point pointToRotate)
        {
            //Translate point such that it is centered around the origin now, rather than around the
            //axis of rotation.
            Tuple<Component, Component, Component> positiveDisplacementVectorAsTuple = axisOrigin.convertToTuple();
            Tuple<double, double, double> displacementVectorAsTuple = multiplyThroughTuple(positiveDisplacementVectorAsTuple, (-1.0));

            axisOfRotation.normalizeLengthOrSetAllValuesToNAN();
            
            Point point_translated = MatrixMath.translatePoint(pointToRotate, displacementVectorAsTuple);

            Point pointTranslatedRotated = MatrixMath.rotatePoint(
                axisOfRotation, angleOfRotation, point_translated);

            //Reverse the translation after rotation is complete
            Tuple<double, double, double> reverseDisplacement = multiplyThroughTuple(
                displacementVectorAsTuple, (-1.0));

            Point point_Rotated = MatrixMath.translatePoint(pointTranslatedRotated,
                reverseDisplacement);

            return point_Rotated;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="axisOfRotation">
        /// The axis vector as components, irrespective of where it is in space.
        /// </param>
        /// <param name="axisOrigin">
        /// The point that the axis vector stems from. Or, if the axis is a line, it is a
        /// point through which the axis passes.
        /// </param>
        /// <param name="angleOfRotation"></param>
        /// <param name="vectorToRotate">
        /// Vector components, irrespective of location or origin.
        /// </param>
        /// <returns></returns>
        public static Vector rotateVectorAroundVectorGeneralForm(Vector axisOfRotation, Point axisOrigin, double angleOfRotation, Vector vectorToRotate)
        {
            Point vectorRotatedAsPoint = rotatePoint(axisOfRotation, angleOfRotation,
                vectorToRotate.convertToPoint());
            Vector vectorRotated = vectorRotatedAsPoint.convertToVector();

            return vectorRotated;
        }

        internal static Point translatePoint(Point p, Tuple<Component, Component, Component> dxdydz)
        {
            Vector p_translateable = p.convertToVector().convertToTranslateableVector();

            Point translatedPoint = computeTranslatedVector(p_translateable, dxdydz).convertToPoint();

            return translatedPoint;
        }
        
        public static Point translatePoint(Point p, Tuple<double, double, double> dxdydz)
        {
            Tuple<Component, Component, Component> compTuple = convertComponentTupleToDoubleTuple(dxdydz);
            return translatePoint(p, compTuple);
        }

        public static Point translatePoint(Point p, double distanceToTranslate, Vector directionInWhichToTranslate)
        {
            Tuple<double, double, double> dxdydz = directionInWhichToTranslate.multiplyWithScalar(
                distanceToTranslate).convertXYZComponentsToTuple();
            return translatePoint(p, dxdydz);
        }

        internal static Vector translateVector(Vector v, Tuple<Component, Component, Component> dxdydz)
        {
            Vector v_translateable = v.convertToTranslateableVector();

            Vector translatedVector = computeTranslatedVector(v_translateable, dxdydz);

            return translatedVector.convertFromTranslateableVector();
        }

        public static Vector translateVector(Vector v, Tuple<double, double, double> dxdydz)
        {
            Tuple<Component, Component, Component> compTuple = convertComponentTupleToDoubleTuple(dxdydz);
            return translateVector(v, compTuple);
        }


//Private methods
        private static Matrix computeRotationMatrix(Vector axisOfRotationAsUnitVector, double angle)
        {
            double nx = axisOfRotationAsUnitVector.getValueAtIndex((int)Vector.Components.x);
            double ny = axisOfRotationAsUnitVector.getValueAtIndex((int)Vector.Components.y);
            double nz = axisOfRotationAsUnitVector.getValueAtIndex((int)Vector.Components.z);

            double A = 1 - (2 * ((ny * ny) + (nz * nz)) * Math.Sin(0.5 * angle) * Math.Sin(0.5 * angle));
            double B = ((-1) * nz * Math.Sin(angle)) + (2 * nx * ny * Math.Sin(0.5 * angle) * Math.Sin(0.5 * angle));
            double C = (ny * Math.Sin(angle)) + (2 * nz * nx * Math.Sin(0.5 * angle) * Math.Sin(0.5 * angle));
            double D = (nz * Math.Sin(angle)) + (2 * nx * ny * Math.Sin(0.5 * angle) * Math.Sin(0.5 * angle));
            double E = 1 - (2 * ((nz * nz) + (nx * nx)) * Math.Sin(0.5 * angle) * Math.Sin(0.5 * angle));
            double F = ((-1) * nx * Math.Sin(angle)) + (2 * ny * nz * Math.Sin(0.5 * angle) * Math.Sin(0.5 * angle));
            double G = ((-1) * ny * Math.Sin(angle)) + (2 * nz * nx * Math.Sin(0.5 * angle) * Math.Sin(0.5 * angle));
            double H = (nx * Math.Sin(angle)) + (2 * ny * nz * Math.Sin(0.5 * angle) * Math.Sin(0.5 * angle));
            double I = 1 - (2 * ((nx * nx) + (ny * ny)) * Math.Sin(0.5 * angle) * Math.Sin(0.5 * angle));

            Matrix rotationMatrix = new Matrix(3, 3);
            rotationMatrix.populateRow(0, new double[] { A, B, C });
            rotationMatrix.populateRow(1, new double[] { D, E, F });
            rotationMatrix.populateRow(2, new double[] { G, H, I });

            return rotationMatrix;
        }

        private static Matrix computeTranslationMatrix(Tuple<Component, Component, Component> dxdydz)
        {
            Matrix translationMatrix = new Matrix(4, 4);
            translationMatrix.populateRow(0, new double[] { 1.0, 0, 0, dxdydz.Item1.componentValue });
            translationMatrix.populateRow(1, new double[] { 0, 1.0, 0, dxdydz.Item2.componentValue });
            translationMatrix.populateRow(2, new double[] { 0, 0, 1.0, dxdydz.Item3.componentValue });
            translationMatrix.populateRow(3, new double[] { 0, 0, 0, 1.0 });

            return translationMatrix;
        }

        private static Vector computeTranslatedVector(Vector transleateableVector, Tuple<Component, Component, Component> dxdydz)
        {
            Matrix T = computeTranslationMatrix(dxdydz);
            Matrix Tv_translateable = T * transleateableVector;
            Vector translatedV = Tv_translateable.convertColumnToVector(0);

            return translatedV;
        }

        private static Tuple<Component, Component, Component> convertComponentTupleToDoubleTuple(Tuple<double, double, double> dubTup)
        {
            return new Tuple<Component, Component, Component>(new Component(dubTup.Item1),
                new Component(dubTup.Item2), new Component(dubTup.Item3));
        }
    }
}
