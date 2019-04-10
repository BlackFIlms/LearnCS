using System;
using System.Linq;

namespace Lesson4___BestSolutionFromKata
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "aaaxbbbbyyhwawiwjjjwwm";

            string z = s.Where(chi => chi > 'm').Count() + "/" + s.Length;

            int num = s.Select(c => IsValid(c)).Sum();
            string z1 = String.Format("{0}/{1}", num, s.Length);

            Console.WriteLine(z);
            Console.WriteLine(z1);
            Console.ReadKey();
        }
        public static int IsValid(char chi)
        {
            if ('a' <= chi && 'm' >= chi)
                return 0;
            else
                return 1;
        }
    }
}
