using System;

namespace Ultraschall.Feed.Models
{
    public class Enclosure
    {
        public Uri Uri { get; }
        public int Lenght { get; set; }
        public Enclosure(string uriString)
        {
            Uri = new Uri(uriString);
        }

        public Enclosure()
        {

        }
    }
}
