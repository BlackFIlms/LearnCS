using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson10___BestSolutionFromKata
{
    class Program
    {
        static string example = "BOY! YOU WANTED TO SEE HIM? IT'S YOUR FATHER:-)";
        static int shift = 15;

        static void Main(string[] args)
        {
            Console.WriteLine(playPass(example, shift));
            Console.WriteLine(")-:gTwIpU GjDn h'iX ?bXw tTh dI StIcPl jDn !NdQ");
            Console.ReadKey();
        }
        public static string playPass(string s, int n)
        {
            return String.Join("", s.ToLower()//.ToArray() - It's excess.
              .Select(c =>
                Char.IsDigit(c) ? (9 - char.GetNumericValue(c)).ToString()[0] :
                Char.IsLetter(c) ? (char)(((int)c - (int)'a' + n) % 26 + (int)'a') :
                c)
              .Select((c, i) => i % 2 == 0 ? char.ToUpper(c) : char.ToLower(c))
              .Reverse());
        }
    }
}
