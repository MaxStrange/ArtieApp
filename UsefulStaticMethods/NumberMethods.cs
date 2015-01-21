using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulStaticMethods
{
    public class NumberMethods
    {
        /// <summary>
        /// Checks if A falls within precision % of B. Both numbers must be positive.
        /// </summary>
        /// <param name="positiveA"></param>
        /// <param name="positiveB"></param>
        /// <param name="precision">This is the number in percentage form.</param>
        /// <returns></returns>
        public static bool A_FallsWithinPercentOf_B(double positiveA, double positiveB, double precisionAsPercent)
        {
            double precisionAsDecimal = (double)precisionAsPercent / (double)100;

            if ((positiveA <= (positiveB + (positiveB * precisionAsDecimal))) && (positiveA >= (positiveB - (positiveB * precisionAsDecimal))))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the difference between a and b is less than 0.00001.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool doublesRepresentTheSameNumber(double a, double b)
        {
            if (doublesRepresentTheSameNumber(a, b, 0.00001))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the difference between a and b is less than epsilon.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool doublesRepresentTheSameNumber(double a, double b, double epsilon)
        {
            if (Math.Abs(a - b) < epsilon)
                return true;
            else
                return false;
        }

        public static int roundDoubleToClosestInt(double d)
        {
            return ((int)(d + 0.5));
        }

        /// <summary>
        /// Returns true if both doubles are the same sign or if both are NaN.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool sameSign(double a, double b)
        {
            if (double.IsNaN(a) && double.IsNaN(b))
                return true;

            if (((a < 0) && (b > 0)) || ((a > 0) && (b < 0)))
                return false;
            else 
                return true;
        }

        public static void setBothNumbersToZeroIfEitherIsNaN(ref double a, ref double b)
        {
            if (Double.IsNaN(a) || Double.IsNaN(b))
            {
                a = 0;
                b = 0;
            }
        }
    }
}
