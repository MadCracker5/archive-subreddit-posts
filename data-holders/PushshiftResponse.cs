using System;
using System.Collections.Generic;
using System.Text;

namespace reddit_scraper.DataHolders
{
    class PushshiftResponse<T>
    {
        public T[] Data { get; set; }
    }
}
