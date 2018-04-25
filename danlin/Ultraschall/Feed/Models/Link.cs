using System;

namespace Ultraschall.Feed.Models
{
    public class Link
    {
        public uint Length { get; set; }
        public string MediaType { get; set; }
        public string Relationship { get; set; }
        public string Title { get; set; }
        public Uri Uri { get; set; }
    }
}
