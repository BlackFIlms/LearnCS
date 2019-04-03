using System;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        GoodVsEvil("111111","1111111");
    }
    public static string GoodVsEvil(string good, string evil)
    {
        var gWorth = new[] { 1, 2, 3, 3, 4, 10 };
        var eWorth = new[] { 1, 2, 2, 2, 3, 5, 10 };
        var g = good.Split(' ').Select(int.Parse).Zip(gWorth, (f, s) => f * s).Sum();
        var b = evil.Split(' ').Select(int.Parse).Zip(eWorth, (f, s) => f * s).Sum();
        return (g > b) ? "Battle Result: Good triumphs over Evil" : ((g == b) ? "Battle Result: No victor on this battle field" : "Battle Result: Evil eradicates all trace of Good");
    }
}
