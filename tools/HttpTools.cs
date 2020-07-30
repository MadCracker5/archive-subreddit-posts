using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace reddit_scraper
{
    public class HttpTools
    {
        private List<DateTime> _active_requests;
        public HttpTools() =>
            _active_requests = new List<DateTime>();
        private void UpdateActiveRequests() =>
            _active_requests = _active_requests
                .Where(x => (DateTime.Now - x).TotalSeconds < 60)
                .Select(x => x)
                .ToList();
        private async Task<T> Throttler<T>(Func<Task<T>> getFn)
        {
            UpdateActiveRequests();
            while (_active_requests.Count() >= 200) {
                await Task.Delay(500);
                UpdateActiveRequests();
            }
            _active_requests.Add(DateTime.Now);
            var response = await getFn();
            return response;
        }

        public async Task<string> Get(string url) =>
             await Throttler(async () =>
             {
                 using var client = new HttpClient();
                 try {
                     return await client.GetStringAsync(url);
                 } catch (HttpRequestException e) {
                     Console.WriteLine("Pushshift API is not available right now because - {0}", e.Message);
                     throw e;
                 }
             });

    }
}
