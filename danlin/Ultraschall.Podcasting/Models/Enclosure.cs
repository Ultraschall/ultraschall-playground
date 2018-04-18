using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultraschall.Podcasting.Models
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
