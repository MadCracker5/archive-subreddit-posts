
using reddit_scraper.DataHolders.CommentResponseParser;
using reddit_scraper.DataHolders.PostResponseParser;
using System.Collections.Generic;

namespace reddit_scraper.DataHolders
{
    public class PostArchive
    {
        public Post Post { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
    public class UnresolvedPostArhive : PostArchive
    {
        public IEnumerable<string> CommentIds { get; set; }
    }
}
