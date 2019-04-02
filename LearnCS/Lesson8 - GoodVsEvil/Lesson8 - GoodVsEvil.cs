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
        return Rand.RandCount(6);
    }
}

static class Evil
{
    public static string RandCountOfEachRace()
    {
        //example: "1 1 1 1 1 1 1"
        return Rand.RandCount(7);
    }
}

static class Rand
{
    //If create object in function RandCount, then this create new object  instance every time, when this function has been called.
    //And function Random, create similar values.
    static Random rand = new Random();

    public static string RandCount(int count)
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