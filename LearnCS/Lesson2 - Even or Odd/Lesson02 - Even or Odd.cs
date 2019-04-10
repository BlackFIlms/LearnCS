using System;

namespace Lesson2___Even_or_Odd
{
    class Program
    {
        static void Main(string[] args)
        {
            int number = 478;
            int reminder = number % 2;
            string evenOdd = "";
            if (reminder == 0)
            {
                evenOdd = "Even";
            }
            else
            {
                evenOdd = "Odd";
            }
            Console.WriteLine(reminder + "\n" + evenOdd);
            Console.ReadKey();
        }
    }
}
