using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            double percentage = 30 / 100d;

            Console.WriteLine("Main thread: starting a timer");
            Timer t = new Timer(ComputeBoundOp, 1, 0, 1000);
            Console.WriteLine("Main thread: Doing other work here...");
            Thread.Sleep(3000); // Simulating other work (10 seconds)
            t.Dispose(); // Cancel the timer now

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Percentage 30 of 100: " + percentage);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(2%1);
            Console.WriteLine(x);
            Console.WriteLine(Math.Log(2.71828182845905));
            Console.WriteLine(Math.E);
            Console.WriteLine(0 % 10);
            Console.ReadKey();
        }

        private static void ComputeBoundOp(Object state)
        {
            // This method is executed by a thread pool thread 
            Console.WriteLine("In ComputeBoundOp: state={0}", state);
            Thread.Sleep(1000); // Simulates other work (1 second)
                                // When this method returns, the thread goes back 
                                // to the pool and waits for another task 
        }
    }
}
