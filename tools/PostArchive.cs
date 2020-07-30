using System.Collections.Generic;

namespace reddit_scraper
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
