using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework.Repository
{
    public class PodcastsRepository : GenericRepository<Podcast>
    {
        public PodcastsRepository(UltraschallContext context) : base(context)
        {
        }
    }
}