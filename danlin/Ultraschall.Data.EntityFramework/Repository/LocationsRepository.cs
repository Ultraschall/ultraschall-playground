using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework.Repository
{
    public class LocationsRepository : GenericRepository<Location>
    {
        public LocationsRepository(UltraschallContext context) : base(context)
        {
        }
    }
}