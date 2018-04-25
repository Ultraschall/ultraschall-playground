using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework.Repository
{
    public class ContributorPresencesRepository : GenericRepository<ContributorPresence>
    {
        public ContributorPresencesRepository(UltraschallContext context) : base(context)
        {
        }
    }
}