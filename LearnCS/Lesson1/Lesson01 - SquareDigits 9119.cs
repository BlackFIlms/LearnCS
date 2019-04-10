using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1
{
    class Program
    {
        //Variable declaration.
        public static string itemsInFunction = "Text:";
        public static string outDigitsString = "";
        public static int outDigits = 0;

        static void Main(string[] args)
        {
            //Get all items from List.
            foreach (var item in SquareDigits(9119))
            {
                Program.outDigitsString = outDigitsString + item;
            }
            //Convert string to int.
            Int32.TryParse(outDigitsString, out outDigits);

            //Get all items from type IEnumerable, for the next output on console.
            foreach (var item in GetDigits(9119))
            {
                itemsInFunction = itemsInFunction + "\n" + item;
            }

            //Console out.
            Console.WriteLine(outDigits);
            Console.WriteLine(itemsInFunction);
            Console.ReadKey();
        }

        //The function is designed to get a square of two digits.Output the List.
        public static List<int> SquareDigits(int n)
        {
            List<int> output = new List<int>();
            foreach (var item in GetDigits(n))
            {
                output.Add(item * item);
            }
            return output;
        }

        //This function gets every digit from number and puts in Enumerable"List".
        public static IEnumerable<int> GetDigits(int source)
        {
            while (source > 0)
            {
                var digit = source % 10;
                source /= 10;
                yield return digit;
            }
        }
    }
}
