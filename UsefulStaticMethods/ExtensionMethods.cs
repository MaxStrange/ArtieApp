using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UsefulStaticMethods
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Deep clone, so that modifying anything in the resulting object does not modify the original in any way.
        /// Warning: This method is SLOW. Don't use this if speed is a necessity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }


//Array extension methods
        /// <summary>
        /// Copies the array, producing a new array of the same type and length, whose elements are:
        /// In the case of reference types, the exact same. So don't bother using this method with an array of reference types.
        /// In the case of value types, copies. So modifying this array will not affect the original array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisArray"></param>
        /// <returns></returns>
        public static T[] Copy<T>(this T[] thisArray)
        {
            T[] copy = new T[thisArray.Length];

            for (int i = 0; i < copy.Length; i++)
            {
                copy[i] = thisArray[i];
            }

            return copy;
        }


//Bool extension methods
        public static bool Toggle(this bool tf)
        {
            return UsefulStaticMethods.BoolMethods.toggle(tf);
        }


//Dictionary extension methods
        
        /// <summary>
        /// Inverts a Dictionary. That is, takes each key value pair in the dictionary and produces a new Dictionary such
        /// that those are swapped. In the case of a bijective Dictionary (a Dictionary where each key AND each value is
        /// unique), this cannot cause problems. But in other cases, this function will produce a Dictionary by overwriting
        /// each time it encounters a new non-unique value from the original dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisDict"></param>
        /// <returns></returns>
        public static Dictionary<V, T> Inverse<T, V>(this Dictionary<T, V> thisDict)
        {
            Dictionary<V, T> inverted = new Dictionary<V, T>();
            
            foreach (T key in thisDict.Keys)
            {
                V value = thisDict[key];
                inverted.Add(value, key);
            }
            return inverted;
        }


//Double extension methods

        /// <summary>
        /// Returns true if the difference between a and b is less than 0.00001.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool DoublesRepresentTheSameNumber(this double a, double b)
        {
            if (DoublesRepresentTheSameNumber(a, b, 0.00001))
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
        public static bool DoublesRepresentTheSameNumber(this double a, double b, double epsilon)
        {
            if (!SameSign(a, b))
                return false;

            if (Math.Abs(a - b) < epsilon)
                return true;
            else
                return false;
        }

        public static int RoundDoubleToClosestInt(this double d)
        {
            return ((int)(d + 0.5));
        }

        /// <summary>
        /// Returns true if doubles are the same sign or if both are NaN.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool SameSign(this double a, double b)
        {
            if (double.IsNaN(a) && double.IsNaN(b))
                return true;

            if (((a < 0) && (b > 0)) || ((a > 0) && (b < 0)))
                return false;
            else
                return true;
        }



//List exetension methods
        public static List<T> Add<T>(this List<T> thisOne, params T[] parameters)
        {
            foreach (T item in parameters)
            {
                thisOne.Add(item);
            }
            return thisOne;
        }

        /// <summary>
        /// Returns the Difference between the two lists, found by comparing the references in each list. So the resulting
        /// List is composed of references to the items not found in both lists.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static List<T> Difference<T>(this List<T> thisOne, List<T> other)
        {
            List<T> difference = new List<T>();

            foreach (T t in other)
            {
                if (!thisOne.Contains(t))
                    difference.Add(t);
            }
            return difference;
        }

        /// <summary>
        /// Splits the list into a collection of smaller lists of sizes equal to the number of items in the list
        /// divided by the number of chunks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="numberOfChunks"></param>
        /// <returns></returns>
        public static List<List<T>> Split<T>(this List<T> list, int numberOfChunks)
        {
            List<List<T>> listOfChunks = new List<List<T>>();
            for (int i = 0; i < numberOfChunks; i++)
            {
                listOfChunks.Add(new List<T>());
            }


            int chunkSize = list.Count / numberOfChunks;


            int j = 0;
            foreach (List<T> chunk in listOfChunks)
            {
                int beginning = j * chunkSize;
                int end = (j == numberOfChunks - 1) ? list.Count : (j + 1) * chunkSize;
                for (int i = beginning; i < end; i++)
                {
                    chunk.Add(list[i]);
                }
                j++;
            }


            return listOfChunks;
        }




//RichTextBox extension methods
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
