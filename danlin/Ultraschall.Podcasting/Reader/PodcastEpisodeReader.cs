using Ultraschall.Podcasting.Models;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Ultraschall.Podcasting.Reader
{
    internal class PodcastEpisodeReader
    {
        private readonly Podcast _podcast;
        public readonly List<Link> Links;
        public PodcastEpisodeReader(Podcast podcastToWrite, XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            _podcast = podcastToWrite ?? throw new ArgumentNullException(nameof(podcastToWrite));
            var feed = XElement.Load(reader);
            var channel = feed.Element("channel");
            if (channel == null) return;

            Links = new List<Link>();

            ParseFeed(channel);
        }

        private void ParseLinks(XElement item)
        {
            var links = item.Elements(Atom.Namespace + "link");
            foreach (var link in links)
            {
                var linkModel = new Link();
                new LinkReader(linkModel, link);
                Links.Add(linkModel);
            }
        }

        private void ParseFeed(XElement channel)
        {
            ParseLinks(channel);
            foreach (var item in channel.Elements("item"))
            {
                var episode = new Episode();
                new EpisodeReader(episode, item.CreateReader());
                if (episode.Image == null)
                    episode.Image = _podcast.Image;
                _podcast.Episodes.Add(episode);
            }
        }
    }
}
