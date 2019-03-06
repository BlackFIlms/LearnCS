using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Smallest nums is:");
        Console.WriteLine(Kata.FindSmallestInt(new int[] { 78, 56, 232, 12, 11, 43 }));
        Console.WriteLine(Kata.FindSmallestInt(new int[] { 78, 56, -2, 12, 8, -33 }));
        Console.ReadKey();
    }
}

public class Kata
{
    public static int FindSmallestInt(int[] args)
    {
        int a = args.Length;
        int min = args[0];

        for (int i = 1; i < a; i++)
        {
            if (args[i] < min)
                min = args[i];
        }
        return min;
    }
}
