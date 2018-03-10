namespace Phony
{
    using Phony.EntityConfigurations;
    using Phony.Model;
    using System.Data.Entity;

    public class PhonyDbContext : DbContext
    {
        public PhonyDbContext()
            : base("name=PhonyDbContext")
        {
        }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillMove> BillsMoves { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BillConfig());
            modelBuilder.Configurations.Add(new BillMoveConfig());
            modelBuilder.Configurations.Add(new ClientConfig());
            modelBuilder.Configurations.Add(new CompanyConfig());
            modelBuilder.Configurations.Add(new ItemConfig());
            modelBuilder.Configurations.Add(new ServiceConfig());
            modelBuilder.Configurations.Add(new SupplierConfig());
            modelBuilder.Configurations.Add(new UserConfig());
        }
    }
}