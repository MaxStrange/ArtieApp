using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractDataClasses
{
    [Serializable]
    /// <summary>
    /// Wrapper class for a double. Represents a percent. IMPORTANT: this class is designed
    /// to be immutable! So all api methods that change the value actually return a new
    /// Percent object.
    /// </summary>
    public class Percent
    {
        //Public fields
        private double _value = double.NaN;
        public double value
        {
            get { return this._value; }
            private set { this._value = value; }
        }


        //Constructors
        /// <summary>
        /// If the value is already in percentage form, provide "true" for the second
        /// argument. Otherwise the value will be converted to a percentage (by multiplying
        /// by 100) - e.g., 0.3 would be converted to 30 percent.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueIsAlreadyPercentage"></param>
        public Percent(double value, bool valueIsAlreadyPercentage = false)
        {
            if (valueIsAlreadyPercentage)
                this.value = value;
            else
                this.value = (value * 100);
        }
    }
}
