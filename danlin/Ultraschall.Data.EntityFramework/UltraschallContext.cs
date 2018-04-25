using Microsoft.EntityFrameworkCore;
using Ultraschall.Data.Entities;

namespace Ultraschall.Data.EntityFramework
{
    public class UltraschallContext : DbContext
    {
        public UltraschallContext(DbContextOptions<UltraschallContext> options)
            :base(options)
        { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
        public DbSet<ContributorPresence> ContributorPresences { get; set; }
        public DbSet<ContributorRole> ContributorRoles { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Shownote> Shownotes { get; set; }
    }
}