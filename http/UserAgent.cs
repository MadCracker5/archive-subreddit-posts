using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace reddit_scraper.http
{
    public class UserAgent
    {
        public string Agent;
        private readonly IServiceProvider _provider;
        private readonly Random _rng = new Random();
        public UserAgent(IServiceProvider provider)
        {
            _provider = provider;
            GetNewUserAgent();
        }
        public void GetNewUserAgent()
        {
            var userAgents = _provider.GetRequiredService<IConfigurationRoot>().GetSection("user_agents").Get<string[]>();
            var nextAgent = userAgents[_rng.Next(userAgents.Length)];
            while (Agent != null && Agent == nextAgent) {
                nextAgent = userAgents[_rng.Next(userAgents.Length)];
            }
            Agent = nextAgent;
        }
    }
}
