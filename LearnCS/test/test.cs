using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class test
    {
        static void Main(string[] args)
        {
            double num = 2.7;
            double baseNum = 2;
            double x = Math.Log(num,baseNum);

            Console.WriteLine(x);
            Console.WriteLine(Math.Log(2.71828182845905));
            Console.WriteLine(Math.E);
            Console.WriteLine(0 % 10);
            Console.ReadKey();
        }
    }
}
