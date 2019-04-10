using System;

public class Printer
{
    public static string PrinterError(String s)
    {
        string example = "abcdefghijklm";
        bool chCompare;
        int counter = 0;
        int errors = 0;
        int lenghtInputStr = s.Length;
        string errorsOfSymbols = "";

        foreach (char chIn in s)
        {
            foreach (char chEx in example)
            {
                if (Char.IsLetter(chIn))
                {
                    chCompare = chIn.Equals(chEx);
                }
                else
                {
                    Console.WriteLine("You have not only chars on input!");
                    Console.WriteLine("Problem symbol:" + chIn);
                    chCompare = false;
                }
                if (chCompare == true)
                {
                    counter++;
                }
            }
        }

        errors = lenghtInputStr - counter;
        errorsOfSymbols = errors + "/" + lenghtInputStr;
        Console.WriteLine(s);
        Console.WriteLine(errorsOfSymbols);
        return errorsOfSymbols;
    }
}