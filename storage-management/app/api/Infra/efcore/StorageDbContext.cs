using Microsoft.EntityFrameworkCore;
using StorageManagementAPI.Entities;

namespace storageapi.Infra.efcore
{
    public class StorageDbContext : DbContext
    {
        public StorageDbContext(DbContextOptions<StorageDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<StorageDocument> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StorageDocument>(document =>
            {
                document.Property(d => d.CreationDate)
                    .HasDefaultValueSql("getutcdate()")
                    .ValueGeneratedOnAdd();

                document.Property(d => d.ModificationDate)
                    .HasDefaultValueSql("getutcdate()")
                    .ValueGeneratedOnAddOrUpdate();

                document.Property("IsActivated").HasDefaultValue(true);
            });
        }
    }
}
