using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ultraschall.Podcasting
{
    struct iTunes
    {
        public static XNamespace Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd";
    }

    struct Podlove
    {
        public static XNamespace Namespace = "http://podlove.org/simple-chapters";
    }

    struct Atom
    {
        public static XNamespace Namespace = "http://www.w3.org/2005/Atom";
    }

    public enum Explicit
    {
        No,
        Yes,
        Clean
    }
}
