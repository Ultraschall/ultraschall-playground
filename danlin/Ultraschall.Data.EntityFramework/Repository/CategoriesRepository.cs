using System;
using System.Linq;
using System.Threading.Tasks;
using Ultraschall.Data.Abstractions;
using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework.Repository
{
    internal class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
    {
        public CategoriesRepository(UltraschallContext context)
            : base(context)
        {
        }

        public IQueryable<Category> GetSubCategories(Guid id)
        {
            return Context.Categories.Where(e => e.Parent.Id == id);
        }

        public async Task SetParent(Guid id, Guid parentId)
        {
            var entity = await Context.Categories.FindAsync(id);
            entity.Parent = await Context.Categories.FindAsync(parentId);
        }
    }
}