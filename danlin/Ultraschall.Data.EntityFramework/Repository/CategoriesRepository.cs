using System;
using System.Linq;
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
    }
}