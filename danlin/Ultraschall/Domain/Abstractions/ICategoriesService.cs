using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ultraschall.Domain.Models;

namespace Ultraschall.Domain.Abstractions
{
    public interface ICategoriesService
    {
        IEnumerable<CategoryModel> GetAll();
        Task<CategoryModel> GetById(Guid id);
        Task Create(CategoryModel model);
        Task Update(Guid id, CategoryModel model);
        Task Delete(Guid id);

        IEnumerable<CategoryModel> GetSubCategories(Guid id);
        Task Patch(Guid id, CategoryPatchModel patchModel);
    }
}