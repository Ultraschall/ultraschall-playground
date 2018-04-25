using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework.Repository
{
    public class ChaptersRepository : GenericRepository<Chapter>
    {
        public ChaptersRepository(UltraschallContext context) : base(context)
        {
        }
    }
}