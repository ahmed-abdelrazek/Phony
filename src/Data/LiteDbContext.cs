using LiteDB;
using Phony.Models;
using System;
using System.Diagnostics;

namespace Phony.Data
{
    public class LiteDbContext
    {
        public static ConnectionString ConnectionString { get; set; }

        public static LiteDatabase LiteDb { get; set; }

        /// <summary>
        /// Set the Database Connection string and initialize the database
        /// </summary>
        public static void InitializeDatabase()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.LiteDbConnectionString))
            {
                throw new ArgumentNullException("Db Path is Null or Empty");
            }

            ConnectionString = GetConnectionString(Properties.Settings.Default.LiteDbConnectionString, Properties.Settings.Default.LiteDbConnectionPassword);

            LiteDb = new LiteDatabase(ConnectionString);
            try
            {
                LiteDb.Mapper.Entity<Bill>()
                    .DbRef(x => x.Client, DBCollections.Clients)
                    .DbRef(x => x.Store, DBCollections.Stores)
                    .DbRef(x => x.Creator, DBCollections.Users)
                    .DbRef(x => x.Editor, DBCollections.Users)
                    .DbRef(x => x.ItemsMoves, DBCollections.BillsItemsMoves)
                    .DbRef(x => x.ServicesMoves, DBCollections.BillsServicesMoves);

                LiteDb.Mapper.Entity<BillItemMove>()
                    .DbRef(x => x.Bill, DBCollections.Bills)
                    .DbRef(x => x.Item, DBCollections.Items)
                    .DbRef(x => x.Creator, DBCollections.Users)
                    .DbRef(x => x.Editor, DBCollections.Users);

                LiteDb.Mapper.Entity<BillServiceMove>()
                    .DbRef(x => x.Bill, DBCollections.Bills)
                    .DbRef(x => x.Service, DBCollections.Services)
                    .DbRef(x => x.Creator, DBCollections.Users)
                    .DbRef(x => x.Editor, DBCollections.Users);

                var UsersCollection = LiteDb.GetCollection<User>(DBCollections.Users).Query();
                UsersCollection.ToEnumerable();
                var BillsCollection = LiteDb.GetCollection<Bill>(DBCollections.Bills).Query();
                BillsCollection.ToEnumerable();
                var BillsItemsMovesCollection = LiteDb.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves).Query();
                BillsItemsMovesCollection.ToEnumerable();
                var BillsServicesMovesCollection = LiteDb.GetCollection<BillServiceMove>(DBCollections.BillsServicesMoves).Query();
                BillsServicesMovesCollection.ToEnumerable();

                var userCol = LiteDb.GetCollection<User>(DBCollections.Users);
                var user = userCol.FindById(1);
                if (user is null)
                {
                    userCol.Insert(new User
                    {
                        Id = 1,
                        Name = "admin",
                        Pass = SecurePasswordHasher.Hash("admin"),
                        Group = UserGroup.Manager,
                        IsActive = true
                    });
                }
                var clientCol = LiteDb.GetCollection<Client>(DBCollections.Clients);
                var client = clientCol.FindById(1);
                if (client is null)
                {
                    clientCol.Insert(new Client
                    {
                        Id = 1,
                        Name = "كاش",
                        Balance = 0,
                        Creator = LiteDb.GetCollection<User>(DBCollections.Users).FindById(1),
                        CreateDate = DateTime.Now,
                        Editor = null,
                        EditDate = null
                    });
                }
                var companyCol = LiteDb.GetCollection<Company>(DBCollections.Companies);
                var company = companyCol.FindById(1);
                if (company is null)
                {
                    companyCol.Insert(new Company
                    {
                        Id = 1,
                        Name = "لا يوجد",
                        Balance = 0,
                        Creator = LiteDb.GetCollection<User>(DBCollections.Users).FindById(1),
                        CreateDate = DateTime.Now,
                        Editor = null,
                        EditDate = null
                    });
                }
                var salesMenCol = LiteDb.GetCollection<SalesMan>(DBCollections.SalesMen);
                var salesMen = salesMenCol.FindById(1);
                if (salesMen is null)
                {
                    salesMenCol.Insert(new SalesMan
                    {
                        Id = 1,
                        Name = "لا يوجد",
                        Balance = 0,
                        Creator = LiteDb.GetCollection<User>(DBCollections.Users).FindById(1),
                        CreateDate = DateTime.Now,
                        Editor = null,
                        EditDate = null
                    });
                }
                var suppliersCol = LiteDb.GetCollection<Supplier>(DBCollections.Suppliers);
                var supplier = suppliersCol.FindById(1);
                if (supplier is null)
                {
                    suppliersCol.Insert(new Supplier
                    {
                        Id = 1,
                        Name = "لا يوجد",
                        Balance = 0,
                        SalesMan = LiteDb.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(1),
                        Creator = LiteDb.GetCollection<User>(DBCollections.Users).FindById(1),
                        CreateDate = DateTime.Now,
                        Editor = null,
                        EditDate = null
                    });
                }
                var storesCol = LiteDb.GetCollection<Store>(DBCollections.Stores);
                var store = storesCol.FindById(1);
                if (store is null)
                {
                    storesCol.Insert(new Store
                    {
                        Id = 1,
                        Name = "التوكل",
                        Motto = "لخدمات المحمول",
                        Creator = LiteDb.GetCollection<User>(DBCollections.Users).FindById(1),
                        CreateDate = DateTime.Now,
                        Editor = null,
                        EditDate = null
                    });
                }
                var treasuriesCol = LiteDb.GetCollection<Treasury>(DBCollections.Treasuries);
                var treasury = treasuriesCol.FindById(1);
                if (treasury is null)
                {
                    treasuriesCol.Insert(new Treasury
                    {
                        Id = 1,
                        Name = "الرئيسية",
                        Store = LiteDb.GetCollection<Store>(DBCollections.Stores).FindById(1),
                        Balance = 0,
                        Creator = LiteDb.GetCollection<User>(DBCollections.Users).FindById(1),
                        CreateDate = DateTime.Now,
                        Editor = null,
                        EditDate = null
                    });
                }

                LiteDb.Rebuild();
                LiteDb.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public static ConnectionString GetConnectionString(string path, string pass = null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }

            if (string.IsNullOrWhiteSpace(pass))
            {
                Console.WriteLine($"'{nameof(pass)}' cannot be null or whitespace.", nameof(pass));
                return new ConnectionString { Filename = Properties.Settings.Default.LiteDbConnectionString, Connection = ConnectionType.Shared };
            }

            return LiteDbContext.ConnectionString;
        }
    }
}
