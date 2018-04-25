using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework.Repository
{
    public class ContributorRolesRepository : GenericRepository<ContributorRole>
    {
        public ContributorRolesRepository(UltraschallContext context) : base(context)
        {
        }
    }
}