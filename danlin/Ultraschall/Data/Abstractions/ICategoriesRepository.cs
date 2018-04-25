using System;
using System.Linq;
using Ultraschall.Data.Entities;

namespace Ultraschall.Data.Abstractions
{
    public interface ICategoriesRepository : IGenericRepository<Category>
    {
        IQueryable<Category> GetSubCategories(Guid id);
    }
}