using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Contents
{
    public class Card
    {
        private string patternStr, numberStr;
        private int pattern, number;

        public string PatternStr => patternStr;
        public string NumberStr => numberStr;
        public int Number => number;
        public int Pattern => pattern;

        public Card(string patternStr, string numberStr, int pattern, int number)
        {
            this.patternStr = patternStr;
            this.numberStr = numberStr;
            this.pattern = pattern;
            this.number = number;
        }

        public void Print(int drawX, int drawY)
        {
            Console.SetCursorPosition(drawX, drawY);
            Console.Write($"┌{"".PadRight(11, '─')}┐");

            Console.SetCursorPosition(drawX, drawY + 1);
            string temp = (Number == 10) ? "      │" : "       │";
            Console.Write($"│ {PatternStr + NumberStr}{temp}");

            Console.SetCursorPosition(drawX, drawY + 2);
            Console.Write("│           │");

            Console.SetCursorPosition(drawX, drawY + 3);
            Console.Write("│           │");

            Console.SetCursorPosition(drawX, drawY + 4);
            Console.Write($"│     {PatternStr}    │");

            Console.SetCursorPosition(drawX, drawY + 5);
            Console.Write("│           │");

            Console.SetCursorPosition(drawX, drawY + 6);
            Console.Write("│           │");

            Console.SetCursorPosition(drawX, drawY + 7);
            temp = (Number == 10) ? "│       " : "│        ";
            Console.Write($"{temp}{NumberStr + PatternStr}│");

            Console.SetCursorPosition(drawX, drawY + 8);
            Console.Write($"└{"".PadRight(11, '─')}┘");
        }
    }
}
