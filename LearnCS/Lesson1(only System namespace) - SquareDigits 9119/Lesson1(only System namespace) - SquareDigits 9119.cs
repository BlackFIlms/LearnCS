using System;

namespace Lesson1_only_System_namespace____SquareDigits_9119
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console out.
            Console.WriteLine(SquareDigits(9119));
            Console.ReadKey();
        }

        public static int SquareDigits(int n)
        {
            string output = "";
            foreach (char c in n.ToString())
            {
                int square = int.Parse(c.ToString());
                output += (square * square);
            }
            return int.Parse(output);
        }
    }
}
