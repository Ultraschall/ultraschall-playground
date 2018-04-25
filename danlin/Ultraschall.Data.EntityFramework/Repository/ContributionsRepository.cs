using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework.Repository
{
    public class ContributionsRepository : GenericRepository<Contribution>
    {
        public ContributionsRepository(UltraschallContext context) : base(context)
        {
        }
    }
}