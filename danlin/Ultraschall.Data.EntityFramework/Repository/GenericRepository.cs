using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ultraschall.Data.Abstractions;

namespace Ultraschall.Data.EntityFramework.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class, IEntity
    {
        internal readonly UltraschallContext Context;
 
        public GenericRepository(UltraschallContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }
        
        public async Task<TEntity> GetById(Guid id)
        {
            return await Context.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        
        public async Task Create(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }
 
        public async Task Update(Guid id, TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            await Context.SaveChangesAsync();
        }
 
        public async Task Delete(Guid id)
        {
            var entity = await GetById(id);
            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
        }

    }
}