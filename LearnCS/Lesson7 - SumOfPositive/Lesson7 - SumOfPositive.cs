/*
You get an array of numbers, return the sum of all of the positives ones.

Example [1,-4,7,12] => 1 + 7 + 12 = 20

Note: if there is nothing to sum, the sum is default to 0.
*/

using System;

class Program
{
    static void Main(string[] args)
    {
        int[] a = { 1, -2, 3, 4, 5 }; //result 13

        int sum = 0;
        foreach (int item in a)
        {
            sum = (item > 0) ? sum += item : sum += 0;
        }
        Console.WriteLine(sum);
        Console.ReadKey();
    }
}
