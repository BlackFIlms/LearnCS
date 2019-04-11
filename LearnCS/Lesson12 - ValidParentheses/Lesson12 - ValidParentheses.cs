using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(ValidParentheses( "(" ));
        Console.ReadKey();
    }
    public static bool ValidParentheses(string input)
    {
        List<int> oB = new List<int>();
        List<int> cB = new List<int>();

        int i = 0;
        foreach (var item in input)
        {
            if (item == '(')
                oB.Add(i);
            i++;
        }

        int j = 0;
        foreach (var item in input)
        {
            if (item == ')')
                cB.Add(j);
            j++;
        }

        if ((cB.Count == 0 && oB.Count > 0) || (cB.Count > 0 && oB.Count == 0))
            return false;
        else if (cB.Count == 0 && oB.Count == 0)
            return true;
        else if ((cB[0] < oB[0]) || (oB.Last() > cB.Last()) || (oB.Count != cB.Count))
            return false;
        else
            return true;
    }
}
