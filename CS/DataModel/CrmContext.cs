using Microsoft.EntityFrameworkCore;

namespace DataModel {
    public class CrmContext : DbContext {
        public CrmContext() { }
        public CrmContext(DbContextOptions<CrmContext> options) : base(options) {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=crm.db");

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
    }
}
