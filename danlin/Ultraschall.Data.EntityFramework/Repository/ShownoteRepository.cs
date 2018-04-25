using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework.Repository
{
    public class ShownoteRepository : GenericRepository<Shownote>
    {
        public ShownoteRepository(UltraschallContext context) : base(context)
        {
        }
    }
}