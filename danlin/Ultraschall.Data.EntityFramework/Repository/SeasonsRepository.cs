using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework.Repository
{
    public class SeasonsRepository : GenericRepository<Season>
    {
        public SeasonsRepository(UltraschallContext context) : base(context)
        {
        }
    }
}