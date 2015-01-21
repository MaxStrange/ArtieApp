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
    /// Wrapper for doubles used for components in the Point class.
    /// IMPORTANT! This class is designed to be immutable.
    /// </summary>
    internal class Component
    {
//Internal fields
        private double _componentValue = 0.0;
        internal double componentValue
        {
            get { return this._componentValue; }
            set { this._componentValue = value; }
        }



//Constructors
        public Component()
        {
        }

        public Component(double value)
        {
            this.componentValue = value;
        }

        
//Overridden operators

        //TODO : override all operators that doubles have. This should essentially be
        //interchangeable with double except have some helper methods.
       
        public static Component operator +(Component c1, Component c2)
        {
            Component c3 = new Component(c1.componentValue + c2.componentValue);
            return c3;
        }

        public static Component operator -(Component c1, Component c2)
        {
            Component c3 = new Component(c1.componentValue - c2.componentValue);
            return c3;
        }

        public static Component operator *(Component c1, Component c2)
        {
            Component c3 = new Component(c1.componentValue * c2.componentValue);
            return c3;
        }

        public static Component operator /(Component c1, Component c2)
        {
            Component c3 = new Component(c1.componentValue - c2.componentValue);
            return c3;
        }


//Public methods
        public Component absolute()
        {
            return new Component(Math.Abs(this.componentValue));
        }

        /// <summary>
        /// Returns true if the two components' values represent the same number - that is,
        /// if UsefulStaticMethods.NumberMethods.doublesRepresentTheSameNumber returns true
        /// for them.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Component otherComp = (Component)obj;
            if (this.componentValue.doublesRepresentTheSameNumber(otherComp.componentValue))
                return true;
            else
                return false;
        }
        
        public bool matches(Component otherComp, Percent precisionAsPercent)
        {
            double c = this.componentValue;
            double otherC = otherComp.componentValue;

            if (!NumberMethods.sameSign(c, otherC))
                return false;

            absoluteOrSetToZeroBothNumbers(ref c, ref otherC);

            if (NumberMethods.A_FallsWithinPercentOf_B(c, otherC, precisionAsPercent.value))
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return this.componentValue.ToString();
        }

//Private methods
        private void absoluteOrSetToZeroBothNumbers(ref double a, ref double b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            NumberMethods.setBothNumbersToZeroIfEitherIsNaN(ref a, ref b);
        }
    }
}
