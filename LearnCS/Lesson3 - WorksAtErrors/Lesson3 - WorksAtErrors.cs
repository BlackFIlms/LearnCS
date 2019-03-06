using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Kata.FindSmallestInt(new int[] { 78, 56, 232, 12, 11, 43 }));
        Console.WriteLine(Kata.FindSmallestInt(new int[] { 78, 56, -2, 12, 8, -33 }));
        Console.ReadKey();
    }
}

public class Kata
{
    public static int[] args1 = new int[0];
    public static int[] args2 = new int[0];

    public static int FindSmallestInt(int[] args)
    {
        Array.Resize(ref args1, 0);
        Array.Resize(ref args2, 0);
        Array.Resize(ref args1, args.Length);
        Array.Copy(args, 0, args1, 0, args.Length);
        int getNum = args1[Array.FindIndex(GetSmallestNum(), SetInt)];
        Console.WriteLine("Array:");
        PrintIndexAndValues(args1);
        Console.WriteLine("Press any key");
        Console.ReadKey();
        Console.WriteLine("Smallest num is:");
        return getNum;
    }


    public static int[] GetSmallestNum()
    {
        for (int i = 0; i < args1.Length; i++)
        {
            for (int t = 0; t < args1.Length; t++)
            {
                if (args1[i] > args1[t])
                {
                    Array.Resize(ref args2, args1.Length);
                    args2[i] = i + 1;
                }
            }
        }
        return args2;
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

    public static void PrintIndexAndValues(Array myArray)
    {
        for (int i = myArray.GetLowerBound(0); i <= myArray.GetUpperBound(0); i++)
            Console.WriteLine("\t[{0}]:\t{1}", i, myArray.GetValue(i));
    }
}
