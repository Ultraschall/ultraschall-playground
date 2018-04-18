using System;

namespace Ultraschall.Podcasting
{
    public class PodcastFeedServiceException : Exception
    {
        public PodcastFeedServiceException(string message) : base(message)
        {

        }
    }
    public class CachedPodcastFeedServiceException : Exception
    {
        public CachedPodcastFeedServiceException(string message) : base(message)
        {

        }
    }
}
