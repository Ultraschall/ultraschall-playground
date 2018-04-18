using Ultraschall.Podcasting.Models;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Ultraschall.Podcasting.Reader
{
    class PodcastReader
    {
        private Podcast _podcast;
        public PodcastReader(Podcast podcastToWrite, XmlReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            _podcast = podcastToWrite ?? throw new ArgumentNullException(nameof(podcastToWrite));
            var feed = XElement.Load(reader);
            var channel = feed.Element("channel");
            if (channel == null) return;

            ParseFeed(channel);
        }

        #region Feed Parser
        //private string createId(string data)
        //{
        //    var alg = HashAlgorithmProvider.OpenAlgorithm("MD5");
        //    IBuffer buff = CryptographicBuffer.ConvertStringToBinary(data, BinaryStringEncoding.Utf8);
        //    var hashed = alg.HashData(buff);
        //    var res = CryptographicBuffer.EncodeToHexString(hashed);
        //    return res;
        //}
        private void ParseId(XElement item)
        {
            //Id = createId(syndicationFeed.Title.Text);
        }

        private void ParseTitle(XElement item)
        {
            var title = item.Element("title");
            if (title == null) return;

            _podcast.Title = title.Value;
        }

        private void ParseLinks(XElement item)
        {
            var links = item.Elements(Atom.Namespace + "link");
            foreach (var link in links)
            {
                var linkModel = new Link();
                new LinkReader(linkModel, link);
                _podcast.Links.Add(linkModel);
            }
        }

        private void ParseLanguage(XElement item)
        {
            var language = item.Element("language");
            if (language == null) return;

            _podcast.Language = language.Value;
        }

        private void ParseLastUpdatedTime(XElement item)
        {

            //LastUpdatedTime = syndicationFeed.LastUpdatedTime;
        }

        private void ParseAuthor(XElement item)
        {
            var author = item.Element(iTunes.Namespace + "author");
            if (author == null) return;

            _podcast.Author = author.Value;
            // TODO: add fallback support 
        }

        private void ParseBlocked(XElement item)
        {
            var block = item.Element(iTunes.Namespace + "block");
            if (block != null)
            {
                _podcast.Block = (String.Compare(block.Value, "YES", StringComparison.CurrentCultureIgnoreCase) == 0);
            }
            else
            {
                _podcast.Block = false;
            }
        }

        private void ParseImage(XElement item)
        {
            var image = item.Element(iTunes.Namespace + "image");

            if (image == null) return;

            var imageHrefAttribute = image.Attribute("href");
            if (imageHrefAttribute == null) return;

            _podcast.Image = new Uri(imageHrefAttribute.Value);
        }

        private void ParseExplicit(XElement item)
        {
            var explicitElement = item.Element(iTunes.Namespace + "explicit");

            if (explicitElement == null) return;

            Explicit value = Explicit.No;
            if (String.Compare(explicitElement.Value, "YES", StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                value = Explicit.Yes;
            }
            else if (string.Compare(explicitElement.Value, "CLEAN", StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                value = Explicit.Clean;
            }
            _podcast.Explicit = value;

        }

        private void ParseComplete(XElement item)
        {
            var complete = item.Element(iTunes.Namespace + "complete");

            if (complete != null)
            {
                _podcast.Complete = (string.Compare(complete.Value, "YES", StringComparison.CurrentCultureIgnoreCase) == 0);
            }
            else
            {
                _podcast.Complete = false;
            }
        }

        private void ParseKeywords(XElement item)
        {
            var keywords = item.Element(iTunes.Namespace + "keywords");

            if (keywords == null) return;

            _podcast.Keywords = keywords.Value.Split(',').ToList();


        }

        private void ParseNewFeedUrl(XElement item)
        {
            var newFeedUrl = item.Element(iTunes.Namespace + "new-feed-url");
            if (newFeedUrl == null) return;

            _podcast.NewFeedUrl = new Uri(newFeedUrl.Value);

        }

        private void ParseOwner(XElement item)
        {
            var owner = item.Element(iTunes.Namespace + "owner");
            if (owner == null) return;

               var name = owner.Attribute("name");

               var email = owner.Attribute("email");

               _podcast.Owner = name?.Value;

        }

        private void ParseSubtitle(XElement item)
        {
            var subtitle = item.Element(iTunes.Namespace + "subtitle");
            if (subtitle == null) return;

            _podcast.Subtitle = subtitle.Value;

        }

        private void ParseSummary(XElement item)
        {
            var summary = item.Element(iTunes.Namespace + "summary");
            if (summary == null) return;

            _podcast.Summary = summary.Value;
        }

        private void ParseCopyright(XElement item)
        {
            //if (syndicationFeed.Rights != null && syndicationFeed.Rights.Text != null)
            //{
            //    Copyright = syndicationFeed.Rights.Text;
            //}
        }

        private void ParseCategories(XElement item)
        {
            foreach (var feedCategory in item.Elements(iTunes.Namespace + "category"))
            {
               _podcast.Categories.Add(new Category(feedCategory));
            }
        }


        private void ParseFeed(XElement channel)
        {
            ParseId(channel);
            ParseTitle(channel);
            ParseLinks(channel);
            ParseLastUpdatedTime(channel);
            ParseLanguage(channel);
            ParseAuthor(channel);
            ParseBlocked(channel);
            ParseCategories(channel);
            ParseImage(channel);
            ParseExplicit(channel);
            ParseComplete(channel);
            ParseKeywords(channel);
            ParseNewFeedUrl(channel);
            ParseOwner(channel);
            ParseSubtitle(channel);
            ParseSummary(channel);
            ParseCopyright(channel);

            foreach (var item in channel.Elements("item"))
            {
                var episode = new Episode();
                new EpisodeReader(episode, item.CreateReader());
                if (episode.Image == null)
                    episode.Image = _podcast.Image;
                _podcast.Episodes.Add(episode);
            }
        }

        #endregion
    }
}
