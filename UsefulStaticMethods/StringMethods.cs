using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefulStaticMethods
{
    public class StringMethods
    {
        public static string appendCharArrayToString(char[] charArray, string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in charArray)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static string appendCharArrayToString(char[] charArray, ref StringBuilder sb)
        {
            foreach (char c in charArray)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a string built by concatenating each item.ToString() in the stack 
        /// popped off the top one at a time.
        /// </summary>
        /// <param name="stringStack"></param>
        /// <returns></returns>
        public static string buildStringFromStack<T>(Stack<T> stringStack)
        {
            if (stringStack.Count <= 0)
                return "Already there.";
            
            StringBuilder sb = new StringBuilder();
            foreach (T pop in stringStack)
            {
                sb.Append(pop).Append(" ");
            }
            return sb.ToString();
        }

        public static string convertCharArrayToString(char[] charArray)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in charArray)
            {
                sb.Append(c);
            }

            return sb.ToString();
        }

        public static string parsePortionOfStringBeforeUnderscore(string s)
        {
            if (s.Contains("_"))
                return s.Substring(0, s.IndexOf('_'));
            else
                return "";
        }

        public static void parseStringIntoTwoSubStringsFromBeforeAndAfterUnderscore(out string firstHalf, out string secondHalf, string stringToParse)
        {
            firstHalf =
                StringMethods.parsePortionOfStringBeforeUnderscore(stringToParse);
            secondHalf =
                stringToParse.Substring(1 + stringToParse.IndexOf('_'));
        }

        public static bool stringsAreTheSame(string s1, string s2)
        {
            if (s1.CompareTo(s2) == 0)
                return true;
            else
                return false;
        }
    }
}
