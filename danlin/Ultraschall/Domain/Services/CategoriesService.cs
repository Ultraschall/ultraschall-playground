using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultraschall.Data.Abstractions;
using Ultraschall.Data.Entities;
using Ultraschall.Domain.Abstractions;
using Ultraschall.Domain.Models;

namespace Ultraschall.Domain.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _repository;
        public CategoriesService(ICategoriesRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        public IEnumerable<CategoryModel> GetSubCategories(Guid id)
        {
            return _repository.GetSubCategories(id).Select(entity => MapEntityToModel(entity)).AsEnumerable();
        }

        private CategoryModel MapEntityToModel(Category entity)
        {
            if (entity == null) return null;

            return new CategoryModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Parent = entity.Parent == null ? null : new CategoryReferene { Id = entity.Parent.Id }
            };
        }

        private Category MapModelToEntity(CategoryModel model)
        {
            return new Category
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public IEnumerable<CategoryModel> GetAll()
        {
            return _repository.GetAll()
                .Select(e => MapEntityToModel(e))
                .AsEnumerable();
        }

        public async Task<CategoryModel> GetById(Guid id)
        {
            return MapEntityToModel(await _repository.GetById(id));
        }

        public async Task Create(CategoryModel model)
        {
            await _repository.Create(MapModelToEntity(model));
        }

        public async Task Update(Guid id, CategoryModel model)
        {
            await _repository.Update(id, MapModelToEntity(model));
        }

        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }

        public async Task Patch(Guid id, CategoryPatchModel patchModel)
        {
            if (patchModel.Parent != Guid.Empty)
            {
                await _repository.SetParent(id, patchModel.Parent);
            }
            if (patchModel.Child != Guid.Empty)
            {
                await _repository.SetParent(id, patchModel.Child);
            }
        }
    }
}