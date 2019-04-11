//Task
/*
Write a function that takes an integer as input, and returns the number of bits that are equal to one in the binary representation of that number.
You can guarantee that input is non-negative.

Example: The binary representation of 1234 is 10011010010, so the function should return 5 in this case

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(CountBits(1234));
        Console.ReadKey();
    }
    public static int CountBits(int n)
    {
        string toBin = Convert.ToString(n,2);
        return toBin.Count(c => (c == '1') ? true : false);
    }
}
