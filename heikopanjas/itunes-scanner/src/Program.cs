using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace ultraschall_scanner
{
   class Program
   {
      private const String DATA_FOLDER = "../data/";
      private const String JSON_FILE_EXTENSION = ".json";
      private const String JSON_FILE_FILTER = "*.json";

      private static HashSet<String> Entries
      {
         get; set;
      } = new HashSet<String>();

      static String[] Letters
      {
         get;
      } =
        new String[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                        "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                         "U", "V", "W", "X", "Y", "Z", "*"};

      static MSSQLDataAccess da_ = null;

      static MSSQLDataAccess DataAccess
      {
         get
         {
            if (da_ == null)
            {
               da_ = new MSSQLDataAccess();
            }

            return da_;
         }
      }

      static bool UseDatabase
      {
         get; set;
      } = false;

      static void Main(string[] args)
      {
         ParseCommandLine(args);
         Configure();

         if (UseDatabase == true)
         {
            List<Podcast> podcasts = LoadPodcastsFromDisk();
            podcasts.ForEach(podcast =>
            {
               if (Entries.Contains(podcast.CollectionId) == false)
               {
                  DataAccess.InsertPodcast(podcast);
               }
            });
         }
         else
         {
            //String url = "https://itunes.apple.com/de/genre/podcasts/id26?mt=2";
            String url = "https://itunes.apple.com/us/genre/podcasts/id26?mt=2";
            Console.WriteLine("Scanning iTunes Podcast Directory ({0})...", url);
            ScanDirectory(url);
         }
      }

      static void Configure()
      {
         Console.WriteLine("Scanning for locally stored known podcast ids...");
         if (UseDatabase == true)
         {
            LoadPodcastIdsFromDatabase();
         }
         else
         {
            LoadPodcastIdsFromDisk();
         }

         if (Entries.Count > 0)
         {
            Console.WriteLine("Added {0} known podcast ids to repository.", Entries.Count);
         }
         else
         {
            Console.WriteLine("Starting from scratch...");
         }
      }

      static void ParseCommandLine(String[] args)
      {
         args.ToList().ForEach(arg =>
         {
            switch (arg.ToLower())
            {
               case "--mssql":
                  UseDatabase = true;
                  break;

               default:
                  Console.WriteLine("Unknown command '{0}'", arg);
                  break;
            }
         });
      }

      static List<Podcast> LoadPodcastsFromDisk()
      {
         List<Podcast> podcasts = new List<Podcast>();

         List<String> files = Directory.EnumerateFiles(DATA_FOLDER, "*.json", SearchOption.AllDirectories).ToList();
         files.ForEach(file =>
         {
            try
            {
               using (StreamReader reader = new StreamReader(file))
               {
                  String json = reader.ReadToEnd();
                  if (String.IsNullOrEmpty(json) == false)
                  {
                     JObject root = JObject.Parse(json);
                     IList<JToken> results = root["results"].Children().ToList();
                     results.ToList().ForEach(result =>
                     {
                        Podcast podcast = result.ToObject<Podcast>();
                        if (podcast != null)
                        {
                           podcasts.Add(podcast);
                        }
                     });
                  }
               }
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
            }
         });

         return podcasts;
      }

      static Podcast ParseJson(String json)
      {
         Podcast podcast = null;

         if (String.IsNullOrEmpty(json) == false)
         {
            if (String.IsNullOrEmpty(json) == false)
            {
               JObject root = JObject.Parse(json);
               IList<JToken> results = root["results"].Children().ToList();
               results.ToList().ForEach(result =>
               {
                  podcast = result.ToObject<Podcast>();
               });
            }
         }

         return podcast;
      }
      static Podcast ParseJsonFile(String file)
      {
         Podcast podcast = null;

         if (String.IsNullOrEmpty(file) == false)
         {
            using (StreamReader reader = new StreamReader(file))
            {
               podcast = ParseJson(reader.ReadToEnd());
            }
         }

         return podcast;
      }

      static void LoadPodcastIdsFromDatabase()
      {
         Entries.Clear();
         List<Podcast> podcasts = DataAccess.SelectAllPodcasts();
         podcasts.ForEach(podcast =>
         {
            Entries.Add(podcast.CollectionId);
         });
      }

      static void LoadPodcastIdsFromDisk()
      {
         try
         {
            if (Directory.Exists(DATA_FOLDER) == false)
            {
               Directory.CreateDirectory(DATA_FOLDER);
            }

            List<String> results = Directory.EnumerateFiles(DATA_FOLDER, "*.json", SearchOption.AllDirectories).ToList();
            results.ForEach(result =>
            {
               String id = result.Substring(DATA_FOLDER.Length, result.Length - JSON_FILE_EXTENSION.Length - DATA_FOLDER.Length);
               if (String.IsNullOrEmpty(id) == false)
               {
                  Entries.Add(id);
               }
            });
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
         }
      }

      static void ScanDirectory(String url)
      {
         String content = Download(url);
         content?.FindUrls()?.ForEach(u =>
         {
            if (u.ToLower().Contains("/genre/podcasts-"))
            {
               ScanCategory(u);
            }
         });
      }

      static void ScanCategorySection(String url)
      {
         if (String.IsNullOrEmpty(url) == false)
         {
            String timestamp = String.Format("{0:u}", DateTime.Now);
            Console.WriteLine("[{0}] Scanning {1} [{2}]", timestamp, url, Entries.Count);
            String content = Download(url);
            content?.FindUrls()?.ForEach(e =>
            {
               if (e.ToLower().Contains("/podcast/"))
               {
                  String id = e.FindPodcastId();
                  if (String.IsNullOrEmpty(id) == false)
                  {
                     bool result = SavePodcastId(id);
                     if (result == false)
                     {
                        Console.WriteLine("Failed to lookup podcast with id {0}, url = {1}", id, e);
                     }
                  }
               }
            });

            List<String> pages = content?.FindPageUrls();
            pages.ForEach(page =>
            {
               timestamp = String.Format("{0:u}", DateTime.Now);
               ;
               Console.WriteLine("[{0}] Scanning {1} [{2}]", timestamp, (url + "&page=" + page), Entries.Count);
               content = Download(url + "&page=" + page);
               content?.FindUrls()?.ForEach(e =>
               {
                  if (e.ToLower().Contains("/podcast/"))
                  {
                     String id = e.FindPodcastId();
                     if (String.IsNullOrEmpty(id) == false)
                     {
                        bool result = SavePodcastId(id);
                        if (result == false)
                        {
                           Console.WriteLine("Failed to lookup podcast with id {0}, url = {1}", id, e);
                        }
                     }
                  }
               });
            });
         }
      }

      static async Task ScanCategorySectionAsync(String url)
      {
         if (String.IsNullOrEmpty(url) == false)
         {
            String content = await DownloadAsync(url);
            content?.FindUrls()?.ForEach(async e =>
            {
               if (e.ToLower().Contains("/podcast/"))
               {
                  String id = e.FindPodcastId();
                  if (String.IsNullOrEmpty(id) == false)
                  {
                     bool result = await SavePodcastIdAsync(id);
                     if (result == false)
                     {
                        Console.WriteLine("Failed to lookup podcast with id {0}, url = {1}", id, e);
                     }
                  }
               }
            });

            List<String> pages = content?.FindPageUrls();
            pages.ForEach(async page =>
            {
               content = await DownloadAsync(url + "&page=" + page);
               content?.FindUrls()?.ForEach(async e =>
               {
                  if (e.ToLower().Contains("/podcast/"))
                  {
                     String id = e.FindPodcastId();
                     if (String.IsNullOrEmpty(id) == false)
                     {
                        bool result = await SavePodcastIdAsync(id);
                        if (result == false)
                        {
                           Console.WriteLine("Failed to lookup podcast with id {0}, url = {1}", id, e);
                        }
                     }
                  }
               });
            });
         }
      }

      static void ScanCategory(String url)
      {
         if (String.IsNullOrEmpty(url) == false)
         {
            //Task[] tasks = new Task[Letters.Length];

            for (int i = 0; i < Letters.Length; ++i)
            {
               String letter = Letters[i];
               ScanCategorySection(url + "&letter=" + letter);
               //tasks[i] = new Task(() => ScanCategorySection(url + "&letter=" + letter), TaskCreationOptions.LongRunning);
               //tasks[i].Start();
            }
            //Task.WaitAll(tasks);
         }
      }

      static String Download(String url)
      {
         String content = null;

         try
         {
            if (String.IsNullOrEmpty(url) == false)
            {
               HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
               using (WebResponse response = request.GetResponse())
               {
                  using (Stream responseStream = response.GetResponseStream())
                  {
                     StreamReader reader = new StreamReader(responseStream);
                     content = reader.ReadToEnd();
                  }
               }
            }
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
         }

         return content;
      }

      static async Task<String> DownloadAsync(String url)
      {
         String content = null;
         if (String.IsNullOrEmpty(url) == false)
         {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (WebResponse response = await request.GetResponseAsync())
            {
               using (Stream responseStream = response.GetResponseStream())
               {
                  StreamReader reader = new StreamReader(responseStream);
                  content = reader.ReadToEnd();
               }
            }
         }

         return content;
      }

      static String LookupPodcast(String id)
      {
         String content = null;

         try
         {
            if (String.IsNullOrEmpty(id) == false)
            {
               String url = String.Format("https://itunes.apple.com/lookup?id={0}", id);
               HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
               using (WebResponse response = request.GetResponse())
               {
                  using (Stream responseStream = response.GetResponseStream())
                  {
                     StreamReader reader = new StreamReader(responseStream);
                     content = reader.ReadToEnd();
                  }
               }
            }
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
         }

         return content;
      }

      static async Task<String> LookupPodcastAsync(String id)
      {
         String content = null;
         if (String.IsNullOrEmpty(id) == false)
         {
            String url = String.Format("https://itunes.apple.com/lookup?id={0}", id);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (WebResponse response = await request.GetResponseAsync())
            {
               using (Stream responseStream = response.GetResponseStream())
               {
                  StreamReader reader = new StreamReader(responseStream);
                  content = reader.ReadToEnd();
               }
            }
         }

         return content;
      }

      static bool SavePodcastId(String id)
      {
         bool result = false;
         try
         {
            if (String.IsNullOrEmpty(id) == false)
            {
               if (Entries.Contains(id) == false)
               {
                  String json = LookupPodcast(id);
                  if (String.IsNullOrEmpty(json) == false)
                  {
                     using (StreamWriter os = new StreamWriter(String.Format(DATA_FOLDER + "{0}.json", id)))
                     {
                        os.WriteLine(json);
                     }

                     Entries.Add(id);
                     result = true;
                  }
               }
               else
               {
                  result = true;
               }
            }
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
            result = false;
         }

         return result;
      }

      static async Task<bool> SavePodcastIdAsync(String id)
      {
         bool result = false;
         try
         {
            if (String.IsNullOrEmpty(id) == false)
            {
               if (Entries.Contains(id) == false)
               {
                  String json = await LookupPodcastAsync(id);
                  if (String.IsNullOrEmpty(json) == false)
                  {
                     using (StreamWriter os = new StreamWriter(String.Format(DATA_FOLDER + "{0}.json", id)))
                     {
                        os.WriteLine(json);
                     }

                     Entries.Add(id);
                     result = true;
                  }
               }
               else
               {
                  result = true;
               }
            }
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
            result = false;
         }

         return result;
      }
   }
}
