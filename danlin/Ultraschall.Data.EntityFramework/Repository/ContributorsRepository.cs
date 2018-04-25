using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework.Repository
{
    public class ContributorsRepository : GenericRepository<Contributor>
    {
        public ContributorsRepository(UltraschallContext context) : base(context)
        {
        }
    }
}