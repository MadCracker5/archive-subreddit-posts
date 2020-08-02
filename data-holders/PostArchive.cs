using reddit_scraper.DataHolders.CommentResponseParser;
using System.Collections.Generic;

namespace reddit_scraper.DataHolders
{
    public class PostArchiveHolder
    {
        public IEnumerable<PostArchive> Posts { get; set; }
        public Metadata Metadata { get; set; }
    }
    public class PostArchive
    {
        public Post Post { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
    public class UnresolvedPostArchive : PostArchive
    {
        public IEnumerable<string> CommentIds { get; set; }
    }
}
