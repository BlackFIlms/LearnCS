//Task
/*
Make log Function.
Log - Get x pow for a num, to get b num.
*/

/* This shit works, not perfectly...
//Define vars.
double logBase = 4;
double num = 2;
double num2 = 0;
double pow = 0;
long count = 0;

        while (num2 != logBase)
        {
            count++;
            
            if ((count % 10) != 0)
            {
                if (num2<logBase)
                {
                    pow += 0.000000000000001;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.000000000000001;
                }
            }
            else if ((count % 10) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.000000000000010;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.000000000000010;
                }
                count += 10;
            }
            else if ((count % 100) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.000000000000100;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.000000000000100;
                }
                count += 100;
            }
            else if ((count % 1000) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.000000000001000;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.000000000001000;
                }
                count += 1000;
            }
            else if ((count % 10000) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.000000000010000;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.000000000010000;
                }
                count += 10000;
            }
            else if ((count % 100000) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.000000000100000;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.000000000100000;
                }
                count += 100000;
            }
            else if ((count % 1000000) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.000000001000000;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.000000001000000;
                }
                count += 1000000;
            }
            else if ((count % 10000000) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.000000010000000;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.000000010000000;
                }
                count += 10000000;
            }
            else if ((count % 100000000) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.000000100000000;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.000000100000000;
                }
                count += 100000000;
            }
            else if ((count % 1000000000) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.000001000000000;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.000001000000000;
                }
                count += 1000000000;
            }
            else if ((count % 10000000000) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.000010000000000;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.000010000000000;
                }
                count += 10000000000;
            }
            else if ((count % 100000000000) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.000100000000000;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.000100000000000;
                }
                count += 100000000000;
            }
            else if ((count % 1000000000000) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.001000000000000;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.001000000000000;
                }
                count += 1000000000000;
            }
            else if ((count % 10000000000000) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.010000000000000;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.010000000000000;
                }
                count += 10000000000000;
            }
            else if ((count % 100000000000000) == 0)
            {
                if (num2<logBase)
                {
                    pow += 0.100000000000000;
                }
                else if (num2 > logBase)
                {
                    pow -= 0.100000000000000;
                }
                count += 100000000000000;
            }

            num2 = Math.Pow(num, pow);
            Console.WriteLine("{0} : {1}", num2, pow);
        }

        Console.WriteLine("Result is: {0} : {1}", num2, pow);
        Console.ReadKey();*/

/*
 * Therefore, we will calculate by the formula of an infinite series for the natural logarithm.
 * Natural log - log(e)(z) = ln(z)
 * e = 2.71828182845905
 * ln(z)=ln((1+x)/(1-x))=2(x+(x^3/3)+(x^5/5)+...)
*/

/*
* How find x:
*  log(z) = log(1+x)/(1-x)
*  z = (1+x)/(1-x)
*  ( z = (1+x)/(1-x) ) * (1-x)
*  z(1-x) = (1+x)*(1-x)/(1-x)
*  z(1-x) = (1+x)
*  (1*z)-(z*x) = (1+x)
*  ( (1*z)-1 )-(z*x) = x
*  ( (1*z)-1 ) = ( (1+z) * x)
*  (z-1)/(1+z) = x
*/

using System;


class Program
{
    static void Main(string[] args)
    {
        string input = "";
        double log = 0;
        double logBase = 0;
        double x = 0;
        double x2 = 0;
        double resultPow = 0;
        double resultPow2 = 0;

        Console.WriteLine("Input your number for log:");
        input = Console.ReadLine();
        if (input == "e")
        {
            log = 2.71828182845905;
        }
        else if (input == "pi")
        {
            log = 3.14159265358979;
        }
        else
        {
            log = Double.Parse(input);
        }
            
        Console.WriteLine("Input your base for log:");
        input = Console.ReadLine();
        if (input == "e")
        {
            logBase = 2.71828182845905;
        }
        else if (input == "pi")
        {
            logBase = 3.14159265358979;
        }
        else
        {
            logBase = Double.Parse(input);
        }

        //Find x:
        x = (log - 1) / (1 + log);

        //Use formula for calculate logarithm:
        resultPow = 2 * (x + (Math.Pow(x, 3) / 3) + (Math.Pow(x, 5) / 5) + (Math.Pow(x, 7) / 7) + (Math.Pow(x, 9) / 9) + (Math.Pow(x, 11) / 11) + (Math.Pow(x, 13) / 13) + (Math.Pow(x, 15) / 15) + (Math.Pow(x, 17) / 17) + (Math.Pow(x, 19) / 19) + (Math.Pow(x, 21) / 21) + (Math.Pow(x, 23) / 23) + (Math.Pow(x, 25) / 25) + (Math.Pow(x, 27) / 27) + (Math.Pow(x, 29) / 29) + (Math.Pow(x, 31) / 31) + (Math.Pow(x, 33) / 33) + (Math.Pow(x, 35) / 35) + (Math.Pow(x, 37) / 37) + (Math.Pow(x, 39) / 39) + (Math.Pow(x, 41) / 41) + (Math.Pow(x, 43) / 43) + (Math.Pow(x, 45) / 45) + (Math.Pow(x, 47) / 47) + (Math.Pow(x, 49) / 49) + (Math.Pow(x, 51) / 51) + (Math.Pow(x, 53) / 53) + (Math.Pow(x, 55) / 55) + (Math.Pow(x, 57) / 57) + (Math.Pow(x, 59) / 59));
        
        Console.WriteLine("ln{0} = {1}", log, resultPow);
        Console.WriteLine("Check this(c# method): ln{0} = {1}", log, Math.Log(log));

        if (logBase != 0)
        {
            //Find x2:
            x2 = (logBase - 1) / (1 + logBase);

            //Use formula for calculate logarithm:
            resultPow2 = 2 * (x2 + (Math.Pow(x2, 3) / 3) + (Math.Pow(x2, 5) / 5) + (Math.Pow(x2, 7) / 7) + (Math.Pow(x2, 9) / 9) + (Math.Pow(x2, 11) / 11) + (Math.Pow(x2, 13) / 13) + (Math.Pow(x2, 15) / 15) + (Math.Pow(x2, 17) / 17) + (Math.Pow(x2, 19) / 19) + (Math.Pow(x2, 21) / 21) + (Math.Pow(x2, 23) / 23) + (Math.Pow(x2, 25) / 25) + (Math.Pow(x2, 27) / 27) + (Math.Pow(x2, 29) / 29) + (Math.Pow(x2, 31) / 31) + (Math.Pow(x2, 33) / 33) + (Math.Pow(x2, 35) / 35) + (Math.Pow(x2, 37) / 37) + (Math.Pow(x2, 39) / 39) + (Math.Pow(x2, 41) / 41) + (Math.Pow(x2, 43) / 43) + (Math.Pow(x2, 45) / 45) + (Math.Pow(x2, 47) / 47) + (Math.Pow(x2, 49) / 49) + (Math.Pow(x2, 51) / 51) + (Math.Pow(x2, 53) / 53) + (Math.Pow(x2, 55) / 55) + (Math.Pow(x2, 57) / 57) + (Math.Pow(x2, 59) / 59));

            Console.WriteLine("ln{0} = {1}", logBase, resultPow2);
            Console.WriteLine("Check this(c# method): ln{0} = {1}", logBase, Math.Log(logBase));
            Console.WriteLine("log({0}){1} = {2}", logBase, log, resultPow/resultPow2);
            Console.WriteLine("Check this(c# method): log({0}){1} = {2}", logBase, log, Math.Log(log,logBase));
        }
        else
        {
            Console.WriteLine("ln0 = 0");
        }

        Console.ReadKey();
    }
}
