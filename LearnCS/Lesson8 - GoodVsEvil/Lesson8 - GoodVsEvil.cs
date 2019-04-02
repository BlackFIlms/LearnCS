using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        //Print Values of each side.
        Console.WriteLine(Good.RandCountOfEachRace());
        Console.WriteLine(Evil.RandCountOfEachRace());

        Console.ReadKey();
    }
}

static class Good
{
    public static string RandCountOfEachRace()
    {
        //example: "1 1 1 1 1 1"
        Random randGood = new Random();
        return Rand.RandCount(6, randGood);
    }
}

static class Evil
{
    public static string RandCountOfEachRace()
    {
        //example: "1 1 1 1 1 1 1"
        Random randEvil = new Random();
        return Rand.RandCount(7, randEvil);
    }
}

static class Rand
{
    public static string RandCount(int count, Random rand)
    {
        string res = "";
        for (int i = 0; i < count; i++)
        {
            if (i < count - 1)
                res += rand.Next(0, 100) + " ";
            else
                res += rand.Next(0, 100);
        }
        return res;
    }
}