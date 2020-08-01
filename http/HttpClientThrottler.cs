using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using reddit_scraper.DataHolders;
using reddit_scraper.http;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace reddit_scraper.Http
{
    public class Lock
    {
        public bool Value { get; set; } = false;
    }
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
        private readonly Lock _locker = new Lock();
        public readonly SemaphoreSlim _per_second_throttler;
        private readonly bool _verbosity = false;
        private UserAgent _user_agent;
        public HttpClientThrottler(IServiceProvider provider)
        {
            _user_agent = new UserAgent(provider);
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
            await _per_second_throttler.WaitAsync();
            while (_locker.Value) {
                Thread.Sleep(1000);
            }
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", _user_agent.Agent);
            var time = DateTime.Now;
            var task = client.GetAsync(url);
            _ = task.ContinueWith(async s =>
            {
                var totalMs = 2000 - (DateTime.Now - time).TotalMilliseconds;
                if (totalMs > 0) {
                    await Task.Delay((int)totalMs);
                }
                if (_verbosity) {
                    Console.WriteLine($"\t\t {url} waiting");
                }
                _per_second_throttler.Release();
            });
            try {
                var response = await task;
                if (response.IsSuccessStatusCode) {
                    return await response.Content.ReadAsStringAsync();
                } else if (response.StatusCode == HttpStatusCode.TooManyRequests) {
                    Console.WriteLine($"\n\nPushshift is complaining about too many requests so now we sleep for a few minutes...");
                    lock (_locker) {
                        _locker.Value = true;
                        lock (_user_agent) {
                            _user_agent.GetNewUserAgent();
                        }
                        Thread.Sleep(120000);
                        _locker.Value = false;
                    }
                    Console.WriteLine($"Trying again @ {url}...");
                    return await MakeRequestAsync(url);
                }
                if (_verbosity) {
                    Console.WriteLine($"{url}");
                }
                throw new HttpRequestException(response.ReasonPhrase);
            } catch (HttpRequestException e) {
                if (_verbosity) {
                    Console.WriteLine($"\t\t\t {e} error out");
                }
                throw e;
            }
        }
    }
}
