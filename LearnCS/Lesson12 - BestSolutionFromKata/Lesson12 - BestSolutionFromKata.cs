using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson12___BestSolutionFromKata
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ValidParentheses("("));
            Console.ReadKey();
        }
        public static bool ValidParentheses(string input)
        {
            int c = 0;
            return !input.Select(i => c += i == '(' ? 1 : i == ')' ? -1 : 0).Any(i => i < 0) && c == 0;
        }
    }
}
