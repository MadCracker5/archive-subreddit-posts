using reddit_scraper.Tools;
using System;
using System.Linq;

namespace reddit_scraper.tools
{
    public static class PrintTools
    {
        public static void NextSubredditDetails(string _init_string, DateRange dateScope, int postTotal = 0, int commentTotal = 0, int numIters = 0)
        {
            Console.Clear();
            Console.WriteLine(_init_string);
            Console.WriteLine($"\n{string.Join("", Enumerable.Repeat("/", 44))}\n");
            Console.Write("Date: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{dateScope.Before.ToShortDateString()}");
            Console.ResetColor();
            Console.Write("Round: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"#{numIters}");
            Console.ResetColor();
            Console.Write($"Posts: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{postTotal}");
            Console.ResetColor();
            Console.Write($"Comments: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{commentTotal}");
            Console.ResetColor();
            Console.WriteLine($"\n{string.Join("", Enumerable.Repeat("/", 44))}\n");
        }
    }
}
