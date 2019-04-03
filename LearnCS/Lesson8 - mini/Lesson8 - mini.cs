using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        Side Good = new Side();
        Side Evil = new Side();
        Good.ForcesCount = 6;
        Good.Worth = "1 2 3 3 4 10";
        Good.Forces = Logic.RandCount(Good.ForcesCount);
        Evil.ForcesCount = 7;
        Evil.Worth = "1 2 2 2 3 5 10";
        Evil.Forces = Logic.RandCount(Evil.ForcesCount);
        Console.WriteLine(Logic.BattleResult(Good, Evil));

        Console.ReadKey();
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
    {
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
        public static Array Parser(string z)
        {
            string pattern = @"(\d+)\s?";
            MatchCollection matches = Regex.Matches(z, pattern);

            if (matches.Count > 0)
            {
                int[] res = new int[matches.Count];
                int i = 0;
                foreach (Match item in matches)
                {
                    res[i] = int.Parse(Regex.Replace(item.Value, @"\s", ""));
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
            obj.WorthParsed = new int[obj.ForcesCount];
            obj.ForcesParsed = new int[obj.ForcesCount];
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
        public static Array AssocWorthAndForce(Side obj)
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
            int goodPower = 0;
            for (int i = 0; i < AssocWorthAndForce(Good).GetLength(0); i++)
            {
                for (int j = 0; j < AssocWorthAndForce(Good).GetLength(1); j++)
                {
                    goodPower += Convert.ToInt32(AssocWorthAndForce(Good).GetValue(0,i)) * Convert.ToInt32(AssocWorthAndForce(Good).GetValue(1,j));
                }
            }
            int evilPower = 0;
            for (int i = 0; i < AssocWorthAndForce(Evil).GetLength(0); i++)
            {
                for (int j = 0; j < AssocWorthAndForce(Evil).GetLength(1); j++)
                {
                    evilPower += Convert.ToInt32(AssocWorthAndForce(Evil).GetValue(0,i)) * Convert.ToInt32(AssocWorthAndForce(Evil).GetValue(1,j));
                }
            }
            if (goodPower > evilPower)
                return "Battle Result: Good triumphs over Evil";
            else if (goodPower < evilPower)
                return "Battle Result: Evil eradicates all trace of Good";
            else
                return "Battle Result: No victor on this battle field";
        }
    }
}