using CleanArchDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchDemo.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();

                // Global Query Filter to ignore soft-deleted users
                entity.HasQueryFilter(e => !e.IsDeleted);
            });
        }
    }
}
