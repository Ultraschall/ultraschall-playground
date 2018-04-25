using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework.Repository
{
    public class EpisodesRepository : GenericRepository<Episode>
    {
        public EpisodesRepository(UltraschallContext context) : base(context)
        {
        }
    }
}