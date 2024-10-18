using Microsoft.EntityFrameworkCore;

namespace SqlServerMongoApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define the relationship between User and Address
            modelBuilder.Entity<User>()
                .HasMany(u => u.Addresses);

        }
    }
}
