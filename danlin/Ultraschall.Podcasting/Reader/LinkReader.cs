using Ultraschall.Podcasting.Models;
using System;
using System.Xml.Linq;

namespace Ultraschall.Podcasting.Reader
{
    internal class LinkReader
    {
        public LinkReader(Link linkToWrite, XElement element)
        {
            var hrefAttribute = element.Attribute("href");
            if (hrefAttribute == null) return;
            linkToWrite.Uri = new Uri(hrefAttribute.Value);

            var lengthAttribute = element.Attribute("length");
            if (lengthAttribute != null)
            {
                try
                {
                    linkToWrite.Length = Convert.ToUInt32(lengthAttribute.Value);
                } catch
                {
                    linkToWrite.Length = 0;
                }
            }
            linkToWrite.MediaType = element.Attribute("type")?.Value;
            linkToWrite.Relationship = element.Attribute("rel")?.Value;
            linkToWrite.Title = element.Attribute("title")?.Value;
        }
    }
}
