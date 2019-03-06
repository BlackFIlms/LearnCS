using System;

namespace Lesson3___Smallest_number_from_Array
{
    class Program
    {
        public static int[] args1 = new int[] { 78, 56, 232, 12, 11, 43 };
        public static int[] args2 = new int[args1.Length];

        static void Main(string[] args)
        {
            //I came up with this fowl.
            //Array.Find(args1, );
            Console.WriteLine("Index of smallest number into basic array:");
            Console.WriteLine(Array.FindIndex(GetSmallestNum(), SetInt));
            Console.WriteLine("Value of that index:");
            Console.WriteLine(args1[Array.FindIndex(GetSmallestNum(), SetInt)]);
            Console.WriteLine("Array of bigger indexes:");
            PrintIndexAndValues(GetSmallestNum());
            Console.ReadKey();
        }

        public static int[] GetSmallestNum()
        {
            for (int i = 0; i < args1.Length; i++)
            {
                for (int t = 0; t < args1.Length; t++)
                {
                    if (args1[i] > args1[t])
                    {
                        args2[i] = i + 1;
                    }
                }
            }
            return args2;
        }

        public static void PrintIndexAndValues(Array myArray)
        {
            for (int i = myArray.GetLowerBound(0); i <= myArray.GetUpperBound(0); i++)
                Console.WriteLine("\t[{0}]:\t{1}", i, myArray.GetValue(i));
        }

        public static bool SetInt(int f)
        {
            if (f == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
