//Task
/*
Everyone knows passphrases. One can choose passphrases from poems, songs, movies names and so on but frequently they can be guessed due to common cultural references. You can get your passphrases stronger by different means. One is the following:

choose a text in capital letters including or not digits and non alphabetic characters,

    1.shift each letter by a given number but the transformed letter must be a letter (circular shift),
    2.replace each digit by its complement to 9,
    3.keep such as non alphabetic and non digit characters,
    4.downcase each letter in odd position, upcase each letter in even position (the first character is in position 0),
    5.reverse the whole result.

#Example:

your text: "BORN IN 2015!", shift 1

1 + 2 + 3 -> "CPSO JO 7984!"

4 "CpSo jO 7984!"

5 "!4897 Oj oSpC"

With longer passphrases it's better to have a small and easy program. Would you write it?

https://en.wikipedia.org/wiki/Passphrase

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static string example = "BOY! YOU WANTED TO SEE HIM? IT'S YOUR FATHER:-)";
    static int shift = 15;

    static void Main(string[] args)
    {
        Console.WriteLine(BuildPassphrase(example, shift));
        Console.WriteLine(")-:gTwIpU GjDn h'iX ?bXw tTh dI StIcPl jDn !NdQ");
        Console.ReadKey();
    }

    static string BuildPassphrase(string s, int n)
    {
        // Shift each letter by a given number but the transformed letter must be a letter (circular shift).
        string first = "";
        Func<int, int, int> CircleShift = (c, i) => (((c + i) >= 65 && (c + i) <= 90 && 65 < c && c < 90) || ((c + i) >= 97 && (c + i) <= 122 && 97 < c && c < 122)) ? (c + i) : (((c + i) > 90 && c >= 65 && c <= 90) || ((c + i) > 122 && c >= 65 && c <= 90)) ? (c+i) - 26 : (c+i);
        s.Select(c => (char.IsLetter(c)) ? Convert.ToChar(CircleShift(Convert.ToInt32(c),n)) : c).ToList().ForEach(c => { first += c; });
        // Replace each digit by its complement to 9.
        string second = "";
        first.Select(c => (char.IsDigit(c)) ? Convert.ToChar((Math.Abs(Convert.ToInt32(c.ToString()) - 9)).ToString()) : c).ToList().ForEach(c => { second += c; });
        // Downcase each letter in odd position, upcase each letter in even position.
        string third = "";
        second.Select((c,i) => (char.IsLetter(c) && i%2==0) ? char.ToUpper(c) : (char.IsLetter(c) && i%2!=0) ? char.ToLower(c) : c).ToList().ForEach(c => { third += c; });
        // Reverse the whole result.
        string fourth = "";
        third.Reverse().ToList().ForEach(c => { fourth += c; });

        return fourth;
    }
}
