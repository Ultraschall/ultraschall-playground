using System;
using System.Linq;
using System.Threading.Tasks;
using Ultraschall.Data.Entities;

namespace Ultraschall.Data.Abstractions
{
    public interface ICategoriesRepository : IGenericRepository<Category>
    {
        IQueryable<Category> GetSubCategories(Guid id);

        Task SetParent(Guid id, Guid parentId);
    }
}