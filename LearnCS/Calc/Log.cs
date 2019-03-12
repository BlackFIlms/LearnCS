using System;

namespace Calc
{
    class Log
    {
        public const double e = 2.71828182845905;
        public const double pi = 3.14159265358979;

        static void Main(string[] args)
        {
            string input = "";
            double log = 0;
            double logBase = 0;

            Console.WriteLine("Input your number for log:");
            input = Console.ReadLine();
            if (input == "e")
                log = e;
            else if (input == "pi")
                log = pi;
            else
                log = Double.Parse(input);

            Console.WriteLine("Input your base for log:");
            input = Console.ReadLine();
            if (input == "e")
                logBase = 2.71828182845905;
            else if (input == "pi")
                logBase = 3.14159265358979;
            else if (String.IsNullOrEmpty(input) != true)
                logBase = Double.Parse(input);

            if (logBase == 0)
                Console.WriteLine("ln = {0}", MathLog(log));
            else
                Console.WriteLine("log = {0}", MathLog(log, logBase));

            if (logBase == 0)
                Console.WriteLine("ln = {0}", Math.Log(log));
            else
                Console.WriteLine("log = {0}", Math.Log(log, logBase));

            Console.ReadKey();
        }


        public static double MathLog(double log)
        {
            double pow = 0;

            if (log <= e)
            {
                pow = MathLogSingle(log);
            }
            else if (log > e)
            {
                int count = 0;
                while (log > e)
                {
                    log /= e;
                    count++;
                }
                pow = count + MathLogSingle(log);
            }

            return pow;
        }

        public static double MathLog(double log, double logBase)
        {
            double pow = 0;

            if (log <= e && logBase <= e)
            {
                pow = MathLogSingle(log) / MathLogSingle(logBase);
            }
            else
            {
                pow = MathLog(log) / MathLog(logBase);
            }

            return pow;
        }

        public static double MathLogSingle(double log)
        {
            double x = 0;
            double pow = 0;
            int rangeValue = 40;
            double percentage = rangeValue / 100d;

            if (log > 0 && log != 1)
            {
                x = (log - 1) / (1 + log);

                for (int i = 1; i < rangeValue; i += 2)
                {
                    pow += 2 * ((Math.Pow(x, i) / i));
                }
            }

            return pow;
        }
    }
}
