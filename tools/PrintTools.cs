//using reddit_scraper.Tools;
//using System;
//using System.Linq;

//namespace reddit_scraper.tools
//{
//    public class PrintTools
//    {
//        private readonly string _subredditTarget;
//        private readonly string _outputDirectory;
//        private readonly int _totalDays;
//        private DateTime CurrentDate;
//        private int CurrentDateIdx = 0;
//        private int NumPostsInDate = 0;
//        private int NumCommentsInDate = 0;
//        private int NumRoundsInDate = 0;
//        private Func<int, int, string> GetInitString;
//        public PrintTools(
//            string subredditTarget,
//            string outputDirectory,
//            int totalDays,
//            long before,
//            long after,
//            long startingDate
//        )
//        {
//            _subredditTarget = subredditTarget;
//            _outputDirectory = outputDirectory;
//            _totalDays = totalDays;
//            CurrentDate = DateRange.UnixTimeStampToDateTime(startingDate);
//            PrintDefaultInfo(before, after);
//            NextSubredditDetails();
//        }
//        void PrintDefaultInfo(long before, long after)
//        {
//            var firstLine = $"Parsing posts & comments for subreddit - {_subredditTarget} after {DateRange.UnixTimeStampToDateTime(after).ToShortDateString()} and before {DateRange.UnixTimeStampToDateTime(before).ToShortDateString()}";
//            var secondLine = $"Files will be written to {AppDomain.CurrentDomain.BaseDirectory}{_outputDirectory}";
//            var stars = string.Join("", Enumerable.Repeat("*", firstLine.Length));
//            GetInitString = (Func<int, int, string>)((int dateIdx, int totalDates) => $"{stars}\n{firstLine}\n{secondLine}\n{stars}\n\nDate Progress: #{dateIdx}/{totalDates}");
//        }
//        public void NextSubredditDetails()
//        {
//            Console.Clear();
//            Console.WriteLine(GetInitString(CurrentDateIdx, _totalDays));
//            Console.WriteLine($"\n{string.Join("", Enumerable.Repeat("/", 44))}\n");
//            Console.Write("Date: ");
//            Console.ForegroundColor = ConsoleColor.Red;
//            Console.WriteLine($"{CurrentDate.ToShortDateString()}");
//            Console.ResetColor();
//            Console.Write("Round: ");
//            Console.ForegroundColor = ConsoleColor.Blue;
//            Console.WriteLine($"#{NumRoundsInDate}");
//            Console.ResetColor();
//            Console.Write($"Posts: ");
//            Console.ForegroundColor = ConsoleColor.Green;
//            Console.WriteLine(NumPostsInDate);
//            Console.ResetColor();
//            Console.Write($"Comments: ");
//            Console.ForegroundColor = ConsoleColor.Green;
//            Console.WriteLine(NumCommentsInDate);
//            Console.ResetColor();
//            Console.WriteLine($"\n{string.Join("", Enumerable.Repeat("/", 44))}\n");
//        }
//        public void SearchingForPosts() =>
//                Console.Write($"\nSearching for posts...\t");
//        public void PostsFoundUpdate(int numPosts)
//        {
//            Console.Write($"{numPosts} posts found.\n");
//            NumPostsInDate += numPosts;
//            Console.Write($"\nFinding comment ids for posts...\t");
//        }
//        public void CommentIdsInPosts(int numComments, int numPosts)
//        {
//            Console.WriteLine($"\n{numComments} total comment ids found in {numPosts} posts.");
//            NumCommentsInDate += numComments;
//            Console.Write("\nComment Ids -> Comments...\t");
//        }
//        public void DayResolution(string fileName)
//        {
//            Console.WriteLine($"\n\nWrote {NumPostsInDate} posts and {NumCommentsInDate} comments in an archive to {fileName}");
//        }
//        public void NextDay(int currentDateIdx, DateTime currentDate)
//        {
//            CurrentDateIdx = currentDateIdx;
//            CurrentDate = currentDate;
//            NumPostsInDate = 0;
//            NumCommentsInDate = 0;
//            NumRoundsInDate = 0;
//        }
//    }
//}
