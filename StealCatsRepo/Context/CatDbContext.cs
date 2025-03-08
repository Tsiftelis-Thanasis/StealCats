using Microsoft.EntityFrameworkCore;
using StealCatsModels;

namespace StealCatsRepo.Context
{
    public class CatDbContext : DbContext
    {
        public CatDbContext(DbContextOptions<CatDbContext> options) : base(options)
        {
        }

        public DbSet<CatEntity> Cats { get; set; }
        public DbSet<TagEntity> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatEntity>().HasMany(c => c.Tags).WithMany(t => t.Cats);
        }
    }
}