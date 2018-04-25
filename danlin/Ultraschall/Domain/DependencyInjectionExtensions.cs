using Microsoft.Extensions.DependencyInjection;
using Ultraschall.Data.Abstractions;
using Ultraschall.Domain.Abstractions;
using Ultraschall.Domain.Services;

namespace Ultraschall.Domain
{
    public static class DependencyInjectionExtensions
    {
        public static void AddUltraschallServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesService>(serviceProvider =>
                new CategoriesService(serviceProvider.GetRequiredService<ICategoriesRepository>()));
        }
    }
}