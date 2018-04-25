using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ultraschall.Data.Abstractions;
using Ultraschall.Data.Entities;
using Ultraschall.Data.EntityFramework.Repository;

namespace Ultraschall.Data.EntityFramework
{
    public static class DependencyInjectionExtensions
    {
        public static void AddSqliteDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UltraschallContext>(options =>
                options.UseSqlite(connectionString));
            services.AddScoped<ICategoriesRepository>(serviceProvider =>
                new CategoriesRepository(serviceProvider.GetRequiredService<UltraschallContext>()));
            services.AddScoped<IGenericRepository<Chapter>>(serviceProvider =>
                new ChaptersRepository(serviceProvider.GetRequiredService<UltraschallContext>()));
            services.AddScoped<IGenericRepository<Contribution>>(serviceProvider =>
                new ContributionsRepository(serviceProvider.GetRequiredService<UltraschallContext>()));
        }
        
        public static void AddSqlDatabase(this IServiceCollection services)
        {
            
        }
    }
}