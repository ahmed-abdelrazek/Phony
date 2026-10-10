using LiteDB;
using Phony.Data;
using Phony.Models;
using System;
using System.IO;

namespace Phony.Tests
{
    public abstract class DatabaseTestBase : IDisposable
    {
        private readonly ConnectionString _previousConnection;
        private readonly string _previousPaperSize;
        protected readonly string _folder = Path.Combine(Path.GetTempPath(), "PhonyTests-" + Guid.NewGuid().ToString("N"));
        protected readonly ConnectionString _connection;
        protected readonly User _user = new() { Id = 1, Name = "test", IsActive = true };

        protected DatabaseTestBase()
        {
            Directory.CreateDirectory(_folder);
            _previousConnection = LiteDbContext.ConfiguredConnectionString;
            _previousPaperSize = Phony.Properties.Settings.Default.SalesBillsPaperSize;
            _connection = LiteDbContext.GetConnectionString(Path.Combine(_folder, "Phony.db"), "test password");
            using var db = new LiteDatabase(_connection);
            LiteDbContext.ConnectionString = _connection;
            _user.Pass = SecurePasswordHasher.Hash("password");
            db.GetCollection<User>(DBCollections.Users).Insert(_user);
            db.GetCollection<Client>(DBCollections.Clients).Insert(new Client { Id = 2 });
            db.GetCollection<Store>(DBCollections.Stores).Insert(new Store { Id = 1 });
            db.GetCollection<Treasury>(DBCollections.Treasuries).Insert(new Treasury { Id = 1 });
            db.GetCollection<Item>(DBCollections.Items).Insert(new Item { Id = 1, QTY = 10 });
            db.GetCollection<Service>(DBCollections.Services).Insert(new Service { Id = 1, Balance = 100 });
        }

        protected BillItemMove Item(decimal quantity = 2) => new BillItemMove { Item = new Item { Id = 1 }, QTY = quantity, ItemPrice = 50, Discount = 10 };
        protected BillServiceMove Service(decimal balance = 20) => new BillServiceMove { Service = new Service { Id = 1 }, Balance = balance, Cost = 10 };
        protected long Save(decimal paid = 120, BillItemMove[] items = null, BillServiceMove[] services = null) =>
            InvoiceService.Save(_connection, 2, 0, paid, null, _user, items ?? new[] { Item() }, services ?? new[] { Service() });

        public void Dispose()
        {
            LiteDbContext.ConfiguredConnectionString = _previousConnection;
            Phony.Properties.Settings.Default.SalesBillsPaperSize = _previousPaperSize;
            Directory.Delete(_folder, true);
        }
    }
}
