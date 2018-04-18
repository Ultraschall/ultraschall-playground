using Ultraschall.Podcasting.Models;
using Ultraschall.Podcasting.Reader;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Ultraschall.Podcasting
{
    public class PodcastFeedService
    {
        private readonly ILogger _logger;
        private readonly IDistributedCache _cache;

        public PodcastFeedService(ILogger logger, IDistributedCache cache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));

            _parsedUris = new List<Uri>();
        }

        private static string CalculateHash(string input)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private static string CalculateErrorHash(string input)
        {
            return "Error_" + CalculateHash(input);
        }

        private async Task<HttpResponseMessage> GetHeadAsync(HttpClient client, Uri requestUri)
        {
                var message = new HttpRequestMessage(HttpMethod.Head, requestUri);
                message.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                var response = await client.SendAsync(message);
                return response;
        }

        public async Task<Podcast> GetAsync(Uri requestUri)
        {
            _logger.LogInformation("GetAsync({Uri})", requestUri);

            Podcast podcast = new Podcast();
            try
            {
                using (var client = new HttpClient(new HttpClientHandler() {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip, 
                }))
                {
                    var cached = await _cache.GetAsync(CalculateHash(requestUri.ToString()));
                    if (cached != null && cached.Length > 0)
                    {
                        _logger.LogInformation("Cached");
                        return JsonConvert.DeserializeObject<Podcast>(Encoding.UTF8.GetString(cached));
                    }
                    cached = await _cache.GetAsync(CalculateErrorHash(requestUri.ToString()));
                    if (cached != null && cached.Length > 0)
                    {
                        _logger.LogWarning("Cached Error");
                        return null;
                    }

                    var headResponse = await GetHeadAsync(client, requestUri);
                    if (!headResponse.IsSuccessStatusCode) {
                        _logger.LogWarning("Not Success StatusCode {Uri}, {Head}", requestUri, headResponse);
                        return null;
                    }

                    var stream = await client.GetStreamAsync(requestUri);
                    var reader = XmlReader.Create(stream);

                    new PodcastReader(podcast, reader);

                    var self = podcast.Links.FirstOrDefault(p => p.Relationship == "self");
                    if (self != null)
                    {
                        _parsedUris.Add(self.Uri);
                        var next = podcast.Links.FirstOrDefault(p => p.Relationship == "next");
                        if (next != null)
                        {
                            await GetNextAsync(client, podcast, next.Uri);
                        }
                    }
                }
            } catch(HttpRequestException ex) {
                _logger.LogWarning("{Uri}, {Message}", requestUri, ex);
                throw new PodcastFeedServiceException($"{requestUri}, {ex}");
            }
            catch (XmlException ex)
            {
                _logger.LogWarning("{Uri}, {Message}", requestUri, ex);
                throw new PodcastFeedServiceException($"{requestUri}, {ex}");
            }

            await _cache.SetAsync(CalculateHash(requestUri.ToString()), Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(podcast)), new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(7)
            });
            _logger.LogInformation("Podcast: {Title} {Uri}", podcast.Title, requestUri);
            return podcast;
        }

        private List<Uri> _parsedUris;
        private async Task GetNextAsync(HttpClient client, Podcast podcast, Uri uri)
        {
            _logger.LogInformation("GetNextAsync({Uri})", uri);

            var stream = await client.GetStreamAsync(uri);
            var reader = XmlReader.Create(stream);

            var pod = new PodcastEpisodeReader(podcast, reader);
            var next = pod.Links.FirstOrDefault(p => p.Relationship == "next");
            var self = pod.Links.FirstOrDefault(p => p.Relationship == "self");
            if (next != null)
            {
                if (_parsedUris.Any(i => i == self.Uri))
                {
                     _logger.LogWarning("Loop: {Uri}", uri);
                    return;
                }
                _parsedUris.Add(self.Uri);
                await GetNextAsync(client, podcast, next.Uri);
            }

        }

        public static void UpdateCache(Uri[] feedlist, ILogger logger, IDistributedCache cache)
        {
            var mutex = new SemaphoreSlim(0, Environment.ProcessorCount * 2);
            var statMutex = new Mutex();
            var tasks = new List<Task>();

            int done = 0;
            int open = feedlist.Length;
            int broken = 0;

            foreach (var feed in feedlist)
            {
                tasks.Add(Task.Run(async () =>
                {
                    await mutex.WaitAsync();
                    try
                    {
                        var parser = new PodcastFeedService(logger, cache);
                        try
                        {
                            await parser.GetAsync(feed);
                            statMutex.WaitOne();
                            done++;
                            statMutex.ReleaseMutex();
                        } catch (PodcastFeedServiceException ex)
                        {
                            statMutex.WaitOne();
                            broken++;
                            statMutex.ReleaseMutex();
                            await cache.SetAsync(CalculateErrorHash(feed.ToString()), Encoding.UTF8.GetBytes(ex.Message), new DistributedCacheEntryOptions
                            {
                                SlidingExpiration = TimeSpan.FromDays(7)
                            });
                        }
                        statMutex.WaitOne();
                        open--;
                        statMutex.ReleaseMutex();

                        logger.LogInformation("Stats Done: {Done} Broken: {Broken} Open: {Open}", done, broken, open);
                    } catch (CachedPodcastFeedServiceException ex) {
                        logger.LogError("{Error}", ex);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("Fatal Error: {Uri}, {Message}", feed, ex);
                        await cache.SetAsync(CalculateErrorHash(feed.ToString()), Encoding.UTF8.GetBytes($"Fatal Error: {feed}, {ex}"), new DistributedCacheEntryOptions
                        {
                            SlidingExpiration = TimeSpan.FromDays(7)
                        });

                    }

                    mutex.Release();
                }));
            }
            mutex.Release(Environment.ProcessorCount * 2);
            Task.WaitAll(tasks.ToArray());
        }
    }
}
