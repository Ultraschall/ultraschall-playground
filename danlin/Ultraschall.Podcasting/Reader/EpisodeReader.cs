using Ultraschall.Podcasting.Models;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Ultraschall.Podcasting.Reader
{
    internal class EpisodeReader
    {
        private readonly Episode _episode;
        public EpisodeReader(Episode episodeToWrite, XmlReader reader)
        {
            _episode = episodeToWrite ?? throw new ArgumentNullException(nameof(episodeToWrite));

            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var item = XElement.Load(reader);
            ParseEpisode(item);
        }

        #region Episode Parser
        private void ParseTitle(XElement item)
        {
            var title = item.Element("title");
            if (title != null)
            {
                _episode.Title = title.Value;
            }
        }

        private void ParseGuid(XElement item)
        {
            var guid = item.Element("guid");
            if (guid != null)
            {
                _episode.Guid = guid.Value;
            }
        }

        private void ParseLink(XElement item)
        {
            var link = item.Element("link");
            if (link != null)
            {
                new LinkReader(_episode.Link, link);
            }
        }

        private void ParseLinks(XElement item)
        {
            var links = item.Elements(Atom.Namespace + "link");
            foreach (var link in links)
            {
                var linkModel = new Link();
                new LinkReader(linkModel, link);
                _episode.Links.Add(linkModel);
            }
        }

        private void ParsePubDate(XElement item)
        {
            var publishedDate = item.Element("pubData");
            if (publishedDate != null)
            {
                _episode.PublishedDate = DateTime.Parse(publishedDate.Value);
            }
        }

        private void ParseAuthor(XElement item)
        {
            var author = item.Element(iTunes.Namespace + "author");
            if (author != null)
            {
                _episode.Author = author.Value;
            }
            // TODO: add fallback support 
        }

        private void ParseBlock(XElement item)
        {
            var block = item.Element(iTunes.Namespace + "block");
            if (block != null)
            {
                _episode.Block = (String.Compare(block.Value, "YES", StringComparison.CurrentCultureIgnoreCase) == 0);
            }
            else
            {
                _episode.Block = false;
            }
        }

        private void ParseImage(XElement item)
        {
            var image = item.Element(iTunes.Namespace + "image");

            if (image != null)
            {
                var imageHrefAttribute = item.Attribute("href");
                if (imageHrefAttribute != null)
                {
                    _episode.Image = new Uri(imageHrefAttribute.Value);
                }
            }
        }

        private void ParseDuration(XElement item)
        {
            var duration = item.Element(iTunes.Namespace + "duration");

            if (duration != null)
            {
                int hours = 0;
                int minutes = 0;
                int seconds = 0;

                string[] time = duration.Value.Split(':');
                if (time.Count() >= 3)
                {
                    hours = Convert.ToInt32(time[0]);
                    minutes = Convert.ToInt32(time[1]);
                    seconds = Convert.ToInt32(time[2]);
                }
                else if (time.Count() == 2)
                {
                    minutes = Convert.ToInt32(time[0]);
                    seconds = Convert.ToInt32(time[1]);
                }
                else
                {
                    seconds = Convert.ToInt32(time[0]);
                }

                _episode.Durication = new TimeSpan(hours, minutes, seconds);
            }
        }

        private void ParseExplicit(XElement item)
        {
            var explicitValue = item.Element(iTunes.Namespace + "explicit");

            if (explicitValue != null)
            {
                Explicit value = Explicit.No;
                if (String.Compare(explicitValue.Value, "YES", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    value = Explicit.Yes;
                }
                else if (String.Compare(explicitValue.Value, "CLEAN", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    value = Explicit.Clean;
                }
                _episode.Explicit = value;
            }
            else
            {
                _episode.Explicit = Explicit.No;
            }
        }

        private void ParseIsClosedCaptioned(XElement item)
        {
            var isClosedCaptioned = item.Element(iTunes.Namespace + "isClosedCaptioned");
            if (isClosedCaptioned != null)
            {
                _episode.IsClosedCaptioned = (String.Compare(isClosedCaptioned.Value, "YES", StringComparison.CurrentCultureIgnoreCase) == 0);
            }
        }

        private void ParseOrder(XElement item)
        {
            var order = item.Element(iTunes.Namespace + "order");
            if (order != null)
            {
                _episode.Order = Convert.ToInt32(order.Value);
            }
        }

        private void ParseKeywords(XElement item)
        {
            var keywords = item.Element(iTunes.Namespace + "keywords");
            if (keywords != null)
            {
                _episode.Keywords = keywords.Value.Split(',').ToList();
            }
        }

        private void ParseSubtitle(XElement item)
        {
            var subtitle = item.Element(iTunes.Namespace + "subtitle");
            if (subtitle != null)
            {
                _episode.Subtitle = subtitle.Value;
            }
        }

        private void ParseSummary(XElement item)
        {
            var summary = item.Attribute(iTunes.Namespace + "summary");
            if (summary != null)
            {
                _episode.Summary = summary.Value;
            }
        }

        private void ParseEnclosure(XElement item)
        {
            var enclosure = item.Element("enclosure");
            if (enclosure != null)
            {
                var url = enclosure.Attribute("url")?.Value;
                var length = enclosure.Attribute("length")?.Value;
                var type = enclosure.Attribute("type")?.Value;

                // TODO: full support
                _episode.Enclosure = new Enclosure(url);
            }
        }

        private void ParseEpisode(XElement item)
        {
            ParseTitle(item);
            ParseGuid(item);
            ParseLink(item);
            ParseLinks(item);
            ParsePubDate(item);
            ParseAuthor(item);
            ParseBlock(item);
            ParseImage(item);
            ParseDuration(item);
            ParseExplicit(item);
            ParseIsClosedCaptioned(item);
            ParseOrder(item);
            ParseKeywords(item);
            ParseSubtitle(item);
            ParseSummary(item);
            ParseEnclosure(item);
        }

        #endregion
    }
}
