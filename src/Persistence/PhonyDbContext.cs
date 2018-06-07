using Phony.Model;
using Phony.Persistence.EntityConfigurations;
using System.Data.Entity;

namespace Phony.Persistence
{
    public class PhonyDbContext : DbContext
    {
        public PhonyDbContext()
            : base($"name=Phony.Properties.Settings.ConnectionString")
        {
        }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillItemMove> BillsItemsMoves { get; set; }
        public DbSet<BillServiceMove> BillsServicesMoves { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientMove> ClientsMoves { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyMove> CompaniesMoves { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<SalesMan> SalesMen { get; set; }
        public DbSet<SalesManMove> SalesMenMoves { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceMove> ServicesMoves { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierMove> SuppliersMoves { get; set; }
        public DbSet<Treasury> Treasuries { get; set; }
        public DbSet<TreasuryMove> TreasuriesMoves { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BillConfig());
            modelBuilder.Configurations.Add(new BillItemMoveConfig());
            modelBuilder.Configurations.Add(new BillServiceMoveConfig());
            modelBuilder.Configurations.Add(new ClientConfig());
            modelBuilder.Configurations.Add(new ClientMoveConfig());
            modelBuilder.Configurations.Add(new CompanyConfig());
            modelBuilder.Configurations.Add(new CompanyMoveConfig());
            modelBuilder.Configurations.Add(new ItemConfig());
            modelBuilder.Configurations.Add(new NoteConfig());
            modelBuilder.Configurations.Add(new SalesManConfig());
            modelBuilder.Configurations.Add(new SalesManMoveConfig());
            modelBuilder.Configurations.Add(new ServiceConfig());
            modelBuilder.Configurations.Add(new ServiceMoveConfig());
            modelBuilder.Configurations.Add(new StoreConfig());
            modelBuilder.Configurations.Add(new SupplierConfig());
            modelBuilder.Configurations.Add(new SupplierMoveConfig());
            modelBuilder.Configurations.Add(new TreasuryConfig());
            modelBuilder.Configurations.Add(new TreasuryMoveConfig());
            modelBuilder.Configurations.Add(new UserConfig());
        }
    }
}