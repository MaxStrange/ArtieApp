using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulStaticMethods
{
    public class CharMethods
    {
        public static char convertIntToChar(int i)
        {
            return char.Parse(char.ConvertFromUtf32(i));
        }
    }
}
