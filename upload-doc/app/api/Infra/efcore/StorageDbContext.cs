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
            modelBuilder.Entity<StorageDocument>(topic =>
            {
                topic.Property("CreationDate").HasDefaultValueSql("getutcdate()");
                topic.Property("ModificationDate").HasDefaultValueSql("getutcdate()");
                topic.Property("IsActivated").HasDefaultValue(true);

            });
        }
    }
}
