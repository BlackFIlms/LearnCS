//Task
/*Description

Middle Earth is about to go to war. The forces of good will have many battles with the forces of evil. Different races will certainly be involved. Each race has a certain worth when battling against others. On the side of good we have the following races, with their associated worth:

    Hobbits: 1
    Men: 2
    Elves: 3
    Dwarves: 3
    Eagles: 4
    Wizards: 10

On the side of evil we have:

    Orcs: 1
    Men: 2
    Wargs: 2
    Goblins: 2
    Uruk Hai: 3
    Trolls: 5
    Wizards: 10

Although weather, location, supplies and valor play a part in any battle, if you add up the worth of the side of good and compare it with the worth of the side of evil, the side with the larger worth will tend to win.

Thus, given the count of each of the races on the side of good, followed by the count of each of the races on the side of evil, determine which side wins.
Input:

The function will be given two parameters. Each parameter will be a string separated by a single space. Each string will contain the count of each race on the side of good and evil.

The first parameter will contain the count of each race on the side of good in the following order:

    Hobbits, Men, Elves, Dwarves, Eagles, Wizards.

The second parameter will contain the count of each race on the side of evil in the following order:

    Orcs, Men, Wargs, Goblins, Uruk Hai, Trolls, Wizards.

All values are non-negative integers. The resulting sum of the worth for each side will not exceed the limit of a 32-bit integer.
Output:

Return "Battle Result: Good triumphs over Evil" if good wins, "Battle Result: Evil eradicates all trace of Good" if evil wins, or "Battle Result: No victor on this battle field" if it ends in a tie.
*/

using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        char control = 'z';
        while (true)
        {
            //Create each side.
            Side Good = new Side();
            Side Evil = new Side();

            //Set worth and get and set force for each side.
            Good.ForcesCount = 6;
            Good.Worth = "1 2 3 3 4 10";
            Good.Forces = Logic.RandCount(Good.ForcesCount);
            Evil.ForcesCount = 7;
            Evil.Worth = "1 2 2 2 3 5 10";
            Evil.Forces = Logic.RandCount(Evil.ForcesCount);

            //Print all variables for each side.
            Console.WriteLine(Good.Worth);
            Console.WriteLine(Good.Forces);
            Console.WriteLine();
            Console.WriteLine(Evil.Worth);
            Console.WriteLine(Evil.Forces);
            Console.WriteLine();

            //Get and print battle result.
            Console.WriteLine(Logic.BattleResult(Good, Evil, out int goodPower, out int evilPower));
            Console.WriteLine("Sides power:" + "\r\n" + "Good - {0}" + "\r\n" + "Evil - {1}", goodPower, evilPower);

            control = ConsoleRestart();

            if (control == 'x')
            {
                break;
            }
        }

        Console.ReadKey();
    }

    private static char ConsoleRestart()
    {
        Char xKey = 'z';
        while (xKey != 'r' || xKey != 'x')
        {
            xKey = Console.ReadKey().KeyChar;
            Console.WriteLine();
            Console.WriteLine(xKey);
            if (xKey == 'r')
            {
                return xKey;
            }
            else if (xKey == 'x')
            {
                return xKey;
            }
        }
        return 'x';
    }
}

class Side
{
    //Set force and worth values of side.
    private int forcesCount = 0;
    private string worth = "";
    private string forces = "";

    //Create private array for all values.
    private int[] worthParsed;
    private int[] forcesParsed;

    //Let's make it possible to get the values of these variables from outside.
    public int ForcesCount
    {
        get { return forcesCount; }
        set { forcesCount = value; }
    }
    public string Worth
    {
        get { return worth; }
        set { worth = value; }
    }
    public string Forces
    {
        get { return forces; }
        set { forces = value; }
    }
    public int[] WorthParsed
    {
        get { return worthParsed; }
        set { worthParsed = value; }
    }
    public int[] ForcesParsed
    {
        get { return forcesParsed; }
        set { forcesParsed = value; }
    }
}

static class Logic
//Contains all functions for calculate battle result.
{
    //If create object in function RandCount, then this create new object  instance every time, when this function has been called.
    //And function Random, create similar values.
    static Random rand = new Random();

    public static string RandCount(int count) //Makes a string with any number of numeric values separated by spaces.
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


    public static Array Parser(string z) //Parses strings with numbers separated by spaces. If it does, then inserts the values into the array.
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

    private static void DefineArrays(Side obj)
    {
        //Define Arrays.
        obj.WorthParsed = new int[obj.ForcesCount];
        obj.ForcesParsed = new int[obj.ForcesCount];

        //Put all values in private defined arrays.
        int j = 0;
        foreach (int item in Parser(obj.Worth))
        {
            obj.WorthParsed[j] = item;
            j++;
        }
        int k = 0;
        foreach (int item in Parser(obj.Forces))
        {
            obj.ForcesParsed[k] = item;
            k++;
        }
    }

    public static Array AssocWorthAndForce(Side obj)//Create 2 dimensions array and insert values of side there.
    {
        DefineArrays(obj);
        int[,] assocWorthAndForce = new int[obj.ForcesParsed.Length, obj.WorthParsed.Length];
        for (int i = 0; i < obj.WorthParsed.Length; i++)
        {
            assocWorthAndForce[i, 1] = Convert.ToInt32(obj.WorthParsed.GetValue(i));
        }
        for (int i = 0; i < obj.ForcesParsed.Length; i++)
        {
            assocWorthAndForce[i, 2] = Convert.ToInt32(obj.ForcesParsed.GetValue(i));
        }

        return assocWorthAndForce;
    }

    public static string BattleResult(Side Good, Side Evil)
    {
        //Get power for Good side.
        int goodPower = 0;
        for (int i = 0; i < AssocWorthAndForce(Good).GetLength(0); i++)
        {
            for (int j = 0; j < AssocWorthAndForce(Good).GetLength(1); j++)
            {
                goodPower += Convert.ToInt32(AssocWorthAndForce(Good).GetValue(0,i)) * Convert.ToInt32(AssocWorthAndForce(Good).GetValue(1,j));
            }
        }

        //Get power for Evil side.
        int evilPower = 0;
        for (int i = 0; i < AssocWorthAndForce(Evil).GetLength(0); i++)
        {
            for (int j = 0; j < AssocWorthAndForce(Evil).GetLength(1); j++)
            {
                evilPower += Convert.ToInt32(AssocWorthAndForce(Evil).GetValue(0,i)) * Convert.ToInt32(AssocWorthAndForce(Evil).GetValue(1,j));
            }
        }

        //Compare powers and return result.
        if (goodPower > evilPower)
        {
            return "Battle Result: Good triumphs over Evil";
        }
        else if (goodPower < evilPower)
        {
            return "Battle Result: Evil eradicates all trace of Good";
        }
        else
        {
            return "Battle Result: No victor on this battle field";
        }
    }

    public static string BattleResult(Side Good, Side Evil, out int goodPower, out int evilPower)
    {
        //Get power for Good side.
        goodPower = 0;
        for (int i = 0; i < AssocWorthAndForce(Good).GetLength(0); i++)
        {
            for (int j = 0; j < AssocWorthAndForce(Good).GetLength(1); j++)
            {
                goodPower += Convert.ToInt32(AssocWorthAndForce(Good).GetValue(0,i)) * Convert.ToInt32(AssocWorthAndForce(Good).GetValue(1,j));
            }
        }

        //Get power for Evil side.
        evilPower = 0;
        for (int i = 0; i < AssocWorthAndForce(Evil).GetLength(0); i++)
        {
            for (int j = 0; j < AssocWorthAndForce(Evil).GetLength(1); j++)
            {
                evilPower += Convert.ToInt32(AssocWorthAndForce(Evil).GetValue(0,i)) * Convert.ToInt32(AssocWorthAndForce(Evil).GetValue(1,j));
            }
        }

        //Compare powers and return result.
        if (goodPower > evilPower)
        {
            return "Battle Result: Good triumphs over Evil";
        }
        else if (goodPower < evilPower)
        {
            return "Battle Result: Evil eradicates all trace of Good";
        }
        else
        {
            return "Battle Result: No victor on this battle field";
        }
    }
}