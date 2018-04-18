using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultraschall.Podcasting.Models
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
