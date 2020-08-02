

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using reddit_scraper.Http;
using reddit_scraper.Src;
using System;
using System.IO;

namespace reddit_scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .ConfigureServices()
                .AddSingleton<IPostArchiver, PostArchiver>()
                .AddSingleton<IHttpClientThrottler, HttpClientThrottler>()
                .BuildServiceProvider();
            try {
                serviceProvider.GetService<IPostArchiver>().Run();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
    }
    public static class ConfigRegister
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            serviceCollection.AddSingleton(configuration);
            return serviceCollection;
        }
    }
}
