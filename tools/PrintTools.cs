//using reddit_scraper.Tools;
//using System;
//using System.Linq;

//namespace reddit_scraper.tools
//{
//    public class PrintTools
//    {
//        private readonly string _subreddit_target;
//        private readonly string _output_directory;
//        private int TotalDays;
//        private int CurrentDateIdx = 0;
//        private int NumPostsInDate = 0;
//        private int NumCommentsInDate = 0;
//        private int NumRoundsInDate = 0;
//        private Func<int, int, string> GetInitString;
//        public PrintTools(
//            int totalDays,
//            int before,
//            int after
//        ) {
//            TotalDays = totalDays;
//            PrintDefaultInfo(before, after);
//        }
//        void PrintDefaultInfo(int before, int after)
//        {
//            var firstLine = $"Parsing posts & comments for subreddit - {_subreddit_target} after {DateRange.UnixTimeStampToDateTime(after).ToShortDateString()} and before {DateRange.UnixTimeStampToDateTime(before).ToShortDateString()}";
//            var secondLine = $"Files will be written to {AppDomain.CurrentDomain.BaseDirectory}{_output_directory}";
//            var stars = string.Join("", Enumerable.Repeat("*", firstLine.Length));
//            GetInitString = (Func<int, int, string>)((int dateIdx, int totalDates) => $"{stars}\n{firstLine}\n{secondLine}\n{stars}\n\nDate: {dateIdx}/{totalDates}");
//        }
//        public void NextSubredditDetails()
//        {
//            Console.Clear();
//            Console.WriteLine(_init_string);
//            Console.WriteLine($"\n{string.Join("", Enumerable.Repeat("/", 44))}\n");
//            Console.Write("Date: ");
//            Console.ForegroundColor = ConsoleColor.Red;
//            Console.WriteLine($"{dateScope.Before.ToShortDateString()}");
//            Console.ResetColor();
//            Console.Write("Round: ");
//            Console.ForegroundColor = ConsoleColor.Blue;
//            Console.WriteLine($"#{numIters}");
//            Console.ResetColor();
//            Console.Write($"Posts: ");
//            Console.ForegroundColor = ConsoleColor.Green;
//            Console.WriteLine($"{postTotal}");
//            Console.ResetColor();
//            Console.Write($"Comments: ");
//            Console.ForegroundColor = ConsoleColor.Green;
//            Console.WriteLine($"{commentTotal}");
//            Console.ResetColor();
//            Console.WriteLine($"\n{string.Join("", Enumerable.Repeat("/", 44))}\n");
//        }
//        public void NextDay()
//        {
//            CurrentDateIdx += 1;
//            NumPostsInDate = 0;
//            NumCommentsInDate = 0;
//            NumRoundsInDate = 0;
//        }
//    }
//}
