using Microsoft.EntityFrameworkCore;
using OrdersService.Infrastructure.Data.Entities;

namespace OrdersService.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderProductLink> OrderProductLinks { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<OrderUser> OrderUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
