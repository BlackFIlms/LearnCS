﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Variable declaration.
            string itemsInFunction = "Text:";
            string outDigitsString = "";
            int outDigits = 0;

            //Get all items from List.
            foreach (var item in SquareDigits(9119))
            {
                outDigitsString = outDigitsString + item;
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

        //Function for get square of two digits. Output the List.
        public static List<int> SquareDigits(int n)
        {
            List<int> output = new List<int>();
            foreach (var item in GetDigits(n))
            {
                output.Add(item * item);
            }
            return output;
        }

        //This function get every digit from number and put in Enumerable"List".
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