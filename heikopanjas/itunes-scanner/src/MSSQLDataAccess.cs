using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace ultraschall_scanner
{
    public class MSSQLDataAccess : IDisposable
    {
        private SqlConnection connection_ = null;

        private SqlConnection Connection
        {
            get
            {
                if (connection_ == null)
                {
                    try
                    {
                        String connectionString = "Data Source=ultraschall.io,5001;Initial Catalog=ultraschall;User ID=ultraschall;Password=<Un!kn0wn>;";
                        Console.WriteLine("Connecting to Microsoft SQL Server at ultraschall.io:5001...");
                        connection_ = new SqlConnection(connectionString);
                        connection_.Open();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        connection_ = null;
                    }
                }

                return connection_;
            }
        }

        public MSSQLDataAccess()
        {
            SqlConnection connection = Connection;
            if (connection != null)
            {
            }
            else
            {
            }
        }

        public void Dispose()
        {
            connection_?.Close();
        }

        public HashSet<String> SelectPodcastIds()
        {
            HashSet<String> ids = new HashSet<String>();
            SqlCommand command = new SqlCommand("select collection_id from apple_podcasts", Connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ids.Add(reader.GetString(0));
            }

            return ids;
        }

        public List<Podcast> SelectAllPodcasts()
        {
            List<Podcast> podcasts = new List<Podcast>();
            String statement = "select id, collection_id, collection_name, artist_name, feed_url, release_date, track_count, country from apple_podcasts";
            SqlCommand command = new SqlCommand(statement, Connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Podcast podcast = new Podcast();
                podcast.Guid = reader.GetGuid(0);
                podcast.CollectionId = reader.GetString(1);
                podcast.CollectionName = reader.GetString(2);
                podcast.ArtistName = reader.GetString(3);
                podcast.FeedUrl = reader.GetString(4);
                podcast.ReleaseDate = reader.GetDateTime(5);
                podcast.TrackCount = reader.GetInt32(6);
                podcast.Country = reader.GetString(7);

                podcasts.Add(podcast);

                //Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
                //  podcast.Guid,
                //  podcast.CollectionId,
                //  podcast.CollectionName,
                //  podcast.ArtistName,
                //  podcast.FeedUrl,
                //  podcast.ReleaseDate,
                //  podcast.TrackCount,
                //  podcast.Country);
            }

            return podcasts;
        }

        public void InsertPodcast(Podcast podcast)
        {
            try
            {
                if (podcast != null)
                {
                    String statement = "insert into apple_podcasts (" +
                    "collection_id, collection_name, artist_name, feed_url, release_date, track_count, country) values (" +
                    "@collectionId, @collectionName, @artistName, @feedUrl, @releaseDate, @trackCount, @country)";
                    using (SqlCommand command = new SqlCommand(statement, Connection))
                    {
                        command.Parameters.AddWithValue("@collectionId", podcast.CollectionId);
                        command.Parameters.AddWithValue("@collectionName", podcast.CollectionName);
                        command.Parameters.AddWithValue("@artistName", podcast.ArtistName);
                        command.Parameters.AddWithValue("@feedUrl", podcast.FeedUrl);
                        command.Parameters.AddWithValue("@releaseDate", podcast.ReleaseDate);
                        command.Parameters.AddWithValue("@trackCount", podcast.TrackCount);
                        command.Parameters.AddWithValue("@country", podcast.Country);
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                        {
                            Console.WriteLine("Failed to insert {0}", podcast.CollectionId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

