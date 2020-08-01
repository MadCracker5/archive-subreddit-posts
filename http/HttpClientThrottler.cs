using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using reddit_scraper.DataHolders;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace reddit_scraper.Http
{
    /// <summary>
    /// Finds the rate limit we are allowed from Pushshift's API and ensures
    /// that concurrently scheduled requests stay within this limit.
    /// </summary>
    public interface IHttpClientThrottler
    {
        public Task<string> MakeRequestAsync(string url);
    }
    public class HttpClientThrottler : IHttpClientThrottler
    {
        public readonly SemaphoreSlim _per_second_throttler;
        private readonly bool _verbosity = false;
        public HttpClientThrottler(IServiceProvider provider)
        {
            var verbosity = provider.GetRequiredService<Microsoft.Extensions.Configuration.IConfigurationRoot>().GetSection("verbosity").Value;
            if (int.TryParse(verbosity, out int verbosityInt)) {
                _verbosity = verbosityInt != 0;
            }
            using var client = new HttpClient();
            var res = client.GetStringAsync("https://api.pushshift.io/meta").GetAwaiter().GetResult();
            var serverRatePerMinute = JsonConvert.DeserializeObject<PushshiftMetaInfo>(res).ServerRatelimitPerMinute;
            var per_second = (int)Math.Round((decimal)(serverRatePerMinute / 60));
            _per_second_throttler = new SemaphoreSlim(per_second);
        }
        public async Task<string> MakeRequestAsync(string url)
        {
            using var client = new HttpClient();
            await _per_second_throttler.WaitAsync();
            var time = DateTime.Now;
            var task = client.GetStringAsync(url);
            _ = task.ContinueWith(async s =>
            {
                var totalMs = 1000 - (DateTime.Now - time).TotalMilliseconds;
                if (totalMs > 0) {
                    await Task.Delay((int)totalMs);
                }
                if (_verbosity) {
                    Console.WriteLine($"\t\t {url} waiting");
                }
                _per_second_throttler.Release();
            });
            string result;
            try {
                result = await task;
                if (_verbosity) {
                    Console.WriteLine($"{url}");
                }
            } catch (HttpRequestException e) {
                if (_verbosity) {
                    Console.WriteLine($"\t\t\t {e} error out");
                }
                throw e;
            }
            return result;
        }
    }
}
