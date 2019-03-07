using System;

class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine(Math.Log(1.5 / 3.0, 0.66));
        Console.WriteLine(BouncingBall.bouncingBall(30.0, 0.66, 1.5));
        Console.ReadKey();
    }
}
public class BouncingBall
{
    public static int bouncingBall(double h, double bounce, double window)
    {
        //It's cery cheating solution!
        //Why?
        //Function Math.Log works with double values. And this return not precise value.
        //We, can't say what it's work perfectly always!
        //If this function return double, with basic params (3.0, 0.66, 1.5), result = 4,33632451276056 - not 3!
        double result = Math.Log(window / h, bounce);
        //It's written how if (double.IsNaN(result) || result <= 0) {return -1} else {return (int)result * 2 + 1}
        return double.IsNaN(result) || result <= 0 ? -1 : (int)result * 2 + 1;
    }
}
