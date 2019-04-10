using System;

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
        Console.WriteLine("Press any key");
        Console.WriteLine("Smallest num is:");
        Console.WriteLine(getNum);
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
}