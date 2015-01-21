using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulStaticMethods
{
    public class BoolMethods
    {
        /// <summary>
        /// If true, returns false. If currently false, returns true.
        /// </summary>
        /// <param name="tf"></param>
        /// <returns></returns>
        public static bool toggle(bool tf)
        {
            if (tf)
                return false;
            else
                return true;
        }

        /// <summary>
        /// If the bool is true, it is set to false. If false, it is set to true.
        /// </summary>
        /// <param name="tf"></param>
        public static void toggle(ref bool tf)
        {
            if (tf)
                tf = false;
            else
                tf = true;
        }
    }
}
