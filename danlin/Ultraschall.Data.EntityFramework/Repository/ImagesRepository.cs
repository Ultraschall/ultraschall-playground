using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework.Repository
{
    public class ImagesRepository : GenericRepository<Image>
    {
        public ImagesRepository(UltraschallContext context) : base(context)
        {
        }
    }
}