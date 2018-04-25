using System.Xml.Linq;

namespace Ultraschall.Feed
{
    // ReSharper disable once InconsistentNaming
    struct iTunes
    {
        public static readonly XNamespace Namespace = "http://www.itunes.com/dtds/podcast-1.0.dtd";
    }

    struct Podlove
    {
        public static readonly XNamespace Namespace = "http://podlove.org/simple-chapters";
    }

    struct Atom
    {
        public static readonly XNamespace Namespace = "http://www.w3.org/2005/Atom";
    }
    
    public enum Explicit
    {
        No,
        Yes,
        Clean
    }
}
