using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        //Print Values of each side.
        Console.WriteLine("Forces of the good side:");
        Console.WriteLine(goodSideForces);
        Console.WriteLine("Forces of the evil side:");
        Console.WriteLine(evilSideForces);
        foreach (var item in Parse.Parser(goodSideForces))
        {
            Console.WriteLine(item);
        }
        foreach (var item in Parse.Parser(evilSideForces))
        {
            Console.WriteLine(item);
        }

        Console.ReadKey();
    }
}

class Good
{
    private string goodSideForces = Rand.RandCount(6);
    public string GoodSideForces
    {
        get { return goodSideForces; }
    }
}

class Evil
{
    private string evilSideForces = Rand.RandCount(7);
    public string EvilSideForces
    {
        get { return evilSideForces; }
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

static class Parse
{
    public static Array Parser(string z)
    {
        string pattern = @"(\d+)\s?";
        MatchCollection matches = Regex.Matches(z, pattern); //Scan input string with pattern.
        
        if (matches.Count > 0)
        {
            int[] res = new int[matches.Count];
            int i = 0;
            foreach (Match item in matches)
            {
                res[i] = int.Parse(Regex.Replace(item.Value, @"\s", "")); //Format and Put into Array each match value.
                i++;
            }
            return res;
        }
        else
        {
            int[] res = { 0 };
            return res;
        }
    }
}