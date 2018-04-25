using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ultraschall.Data.EntityFramework.Design
{
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UltraschallContext>
    {
        public UltraschallContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UltraschallContext>();
            optionsBuilder.UseSqlite("Data Source=ultraschall.db");

            return new UltraschallContext(optionsBuilder.Options);
        }
    }
}