using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Ultraschall.Caching;
using Ultraschall.Feed;
using Ultraschall.Feed.Models;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Ultraschall.Podcasting.Cli
{
    internal class Program
    {
        private struct CommandLineOption
        {
            public CommandLineOption(
                string configurationKey,                                     
                string longUsage, 
                string description,                    
                string shortUsage = null,
                string defaultConfigurationValue = null)
            {
                ConfigurationKey = configurationKey;
                LongUsage = longUsage;
                Description = description;
                ShortUsage = shortUsage;
                DefaultConfigurationValue = defaultConfigurationValue;
            }
            public string ConfigurationKey { get; }
            public string DefaultConfigurationValue { get; }
            public string LongUsage { get; }
            public string ShortUsage { get; }
            public string Description { get; }
        }
        
        private static Dictionary<string, string> GetSwitchMappings(
            IReadOnlyList<CommandLineOption> options)
        {
            var result = new Dictionary<string, string>();
            foreach (var commandLineOption in options)
            {
                result.Add($"--{commandLineOption.LongUsage}", commandLineOption.ConfigurationKey);
                if (!string.IsNullOrEmpty(commandLineOption.ShortUsage))
                {
                    result.Add($"-{commandLineOption.ShortUsage}", commandLineOption.ConfigurationKey);
                }
            }

            return result;
        }

        private static Dictionary<string, string> GetDefaults(
            IReadOnlyList<CommandLineOption> options)
        {
            var result = new Dictionary<string, string>();
            foreach (var commandLineOption in options)
            {
                if (!string.IsNullOrEmpty(commandLineOption.DefaultConfigurationValue))
                {
                    result.Add(commandLineOption.ConfigurationKey, commandLineOption.DefaultConfigurationValue);
                }
            }

            return result;
        }
        
        private static void PrintUsage(IEnumerable<CommandLineOption> options)
        {
            Console.Write(Environment.NewLine +
                          $"Usage: {AppDomain.CurrentDomain.FriendlyName} [options]" + Environment.NewLine +
                          Environment.NewLine +
                          $"Options:" + Environment.NewLine);
            foreach (var commandLineOption in options)
            {
                Console.Write($"\t--{commandLineOption.LongUsage}");
                if (commandLineOption.ShortUsage != null)
                {
                    Console.Write($"|{commandLineOption.ShortUsage}");
                }
                Console.Write($"\t{commandLineOption.Description}");
                if (commandLineOption.DefaultConfigurationValue != null)
                {
                    Console.Write($"\t(default={commandLineOption.DefaultConfigurationValue})");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void DumpPodcast(ILogger logger, Podcast podcast)
        {
            logger.LogInformation("{Title}", podcast.Title);

            foreach (var episode in podcast.Episodes)
                logger.LogInformation("{Title}: {Durication}", episode.Title, episode.Durication);
        }

        private static int Main(string[] args)
        {
            return MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task<int> MainAsync(string[] args)
        {
            var options = new List<CommandLineOption>
            {
                new CommandLineOption("Feed:Url", "url", "Parse podcast url."),
                new CommandLineOption("Feed:File", "file", "Read podcast feeds from a file."),
                new CommandLineOption("Daemon:Enabed", "daemon", "Run in daemon mode", "-d", "false"),
                new CommandLineOption("Daemon:Time", "delay", "Delay between refresh in seconds.", null, "10")
            };
            
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .AddInMemoryCollection(GetDefaults(options))
                .AddCommandLine(args, GetSwitchMappings(options));
            var configuration = configurationBuilder.Build();

            if (configuration["Feed:Url"] == null
                && configuration["Feed:File"] == null)
            {
                PrintUsage(options);
            }
            
            var serilog = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .WriteTo.File("log.txt", LogEventLevel.Error)
                .CreateLogger();

            var loggerFactory = new LoggerFactory()
                .AddSerilog(serilog);
            var logger = loggerFactory.CreateLogger<Program>();
            //var cache = new RedisCache(new RedisCacheOptions
            //{
            //    Configuration = "localhost:6379"
            //});
            //var cache = new FileCache("../cache/");
            // var cache = new SqlServerCache(new SqlServerCacheOptions
            // {
            //     ConnectionString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = FeedCache; Integrated Security = True;",
            //     SchemaName = "dbo",
            //     TableName = "TestCache"
            // });
            var cache = new FileCache(new FileCacheOptions
            {
                Path = "./cache"
            });

            var parser = new PodcastReaderService(logger, cache);

            if (configuration["Feed:Url"] != null)
            {
                var uri = new Uri(configuration["Feed:Url"]);
                logger.LogInformation("Read Feed");
                var podcast = await parser.GetAsync(uri);
                logger.LogInformation("Done");
                DumpPodcast(logger, podcast);
            }

            if (configuration["Feed:File"] != null)
            {
                var file = File.OpenText(configuration["Feed:File"]);
                string line;
                var feeds = new List<Uri>();
                logger.LogInformation("Read File");
                while ((line = file.ReadLine()) != null)
                    try
                    {
                        feeds.Add(new Uri(line));
                    }
                    catch (UriFormatException ex)
                    {
                        logger.LogError("Uri malformed {Uri}, {Message}", line, ex.Message);
                    }

                logger.LogInformation("Parse Podcasts");
                PodcastReaderService.UpdateCache(feeds.ToArray(), logger, cache);
            }


            return 0;
        }
    }
}