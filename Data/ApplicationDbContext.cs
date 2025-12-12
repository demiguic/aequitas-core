using Microsoft.EntityFrameworkCore;
using AequitasTracker.Models;

namespace AequitasTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PriceUpdate> PriceUpdates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>()
                .HasIndex(a => a.Ticker)
                .IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}