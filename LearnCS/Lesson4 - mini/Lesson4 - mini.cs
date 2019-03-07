using System;

class Program
{
    static void Main(string[] args)
    {
        string inputStr = "aaaxbbbbyyhwawiwjjjwwm";
        string example = "abcdefghijklm";
        int counter = 0;

        foreach (char chIn in inputStr)
        {
            foreach (char chEx in example)
            {
                bool chCompare = chIn.Equals(chEx);
                if (chCompare == true)
                {
                    counter++;
                }
            }
        }
        int errors = inputStr.Length - counter;
        Console.WriteLine(errors + "/" + inputStr.Length);
        Console.ReadKey();
    }
}