//Task
/*In a factory a printer prints labels for boxes. For one kind of boxes the printer has to use colors which, for the sake of simplicity, are named with letters from a to m.

The colors used by the printer are recorded in a control string. For example a "good" control string would be aaabbbbhaijjjm meaning that the printer used three times color a, four times color b, one time color h then one time color a...

Sometimes there are problems: lack of colors, technical malfunction and a "bad" control string is produced e.g. aaaxbbbbyyhwawiwjjjwwm with letters not from a to m.

You have to write a function printer_error which given a string will output the error rate of the printer as a string representing a rational whose numerator is the number of errors and the denominator the length of the control string. Don't reduce this fraction to a simpler expression.

The string has a length greater or equal to one and contains only letters from ato z.

#Examples:

s="aaabbbbhaijjjm"
error_printer(s) => "0/14"

s="aaaxbbbbyyhwawiwjjjwwm"
error_printer(s) => "8/22"
*/

using System;

namespace Lesson4___PrinterErrors
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputStr = "aaaxbbbbyyhwawiwjjjwwm";

            //This basic string for compare to a good char.
            string example = "abcdefghijklm";
            //Define vars.
            bool chCompare;
            int counter = 0;
            int errors = 0;
            int lenghtInputStr = inputStr.Length;

            foreach (char chIn in inputStr)
            {
                foreach (char chEx in example)
                {
                    //This checks whether characters in inputStr are letters.
                    if (Char.IsLetter(chIn))
                    {
                        //Compare with example letters.
                        chCompare = chIn.Equals(chEx);
                    }
                    else
                    {
                        Console.WriteLine("You have not only chars on input!");
                        Console.WriteLine("Problem symbol:" + chIn);
                        chCompare = false;
                    }
                    //If you have troubles, this counter will count errors.
                    if (chCompare == true)
                    {
                        counter++;
                    }
                }
            }
            errors = lenghtInputStr - counter;
            Console.WriteLine("In string:" + "\n" + inputStr + "\n" + "contain symbols errors / all symbols:");
            Console.WriteLine(errors + "/" + lenghtInputStr);
            Console.ReadKey();
        }
    }
}
