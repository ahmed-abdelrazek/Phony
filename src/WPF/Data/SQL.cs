using LiteDB;
using Phony.Data.Core;
using Phony.Data.Models.Lite;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Phony.WPF.Data
{
    public class SQL
    {
        readonly string _connectionString;

        public SQL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ImportFromMSSQL()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand("select * from [Users]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var id = Convert.ToInt32(reader["Id"]);
                                    var userCol = db.GetCollection<User>(DBCollections.Users);
                                    if (id == 1)
                                    {
                                        var u = userCol.Find(x => x.Id == id).FirstOrDefault();
                                        u.Name = reader["Name"].ToString();
                                        u.Pass = reader["Pass"].ToString();
                                        u.Group = (UserGroup)Convert.ToByte(reader["Group"]);
                                        u.Phone = reader["Phone"].ToString();
                                        u.Notes = reader["Notes"].ToString();
                                        u.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                        userCol.Update(u);
                                    }
                                    else
                                    {
                                        userCol.Insert(new User
                                        {
                                            Id = id,
                                            Name = reader["Name"].ToString(),
                                            Pass = reader["Pass"].ToString(),
                                            Group = (UserGroup)Convert.ToByte(reader["Group"]),
                                            Phone = reader["Phone"].ToString(),
                                            Notes = reader["Notes"].ToString(),
                                            IsActive = Convert.ToBoolean(reader["IsActive"])
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [Clients]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var id = Convert.ToInt64(reader["Id"]);
                                    var clientCol = db.GetCollection<Client>(DBCollections.Clients);
                                    if (id == 1)
                                    {
                                        var c = clientCol.Find(x => x.Id == id).FirstOrDefault();
                                        c.Name = reader["Name"].ToString();
                                        c.Balance = Convert.ToDecimal(reader["Balance"]);
                                        c.Site = reader["Site"].ToString();
                                        c.Email = reader["Email"].ToString();
                                        c.Phone = reader["Phone"].ToString();
                                        c.Notes = reader["Notes"].ToString();
                                        c.Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"]));
                                        c.CreatedAt = Convert.ToDateTime(reader["CreateDate"]);
                                        c.Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"]));
                                        c.EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"]);
                                        clientCol.Update(c);
                                    }
                                    else
                                    {
                                        clientCol.Insert(new Client
                                        {
                                            Id = id,
                                            Name = reader["Name"].ToString(),
                                            Balance = Convert.ToDecimal(reader["Balance"]),
                                            Site = reader["Site"].ToString(),
                                            Email = reader["Email"].ToString(),
                                            Phone = reader["Phone"].ToString(),
                                            Notes = reader["Notes"].ToString(),
                                            Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                            CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                            Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                            EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [Companies]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var id = Convert.ToInt64(reader["Id"]);
                                    var companyCol = db.GetCollection<Company>(DBCollections.Companies);
                                    if (id == 1)
                                    {
                                        var c = companyCol.Find(x => x.Id == id).FirstOrDefault();
                                        c.Name = reader["Name"].ToString();
                                        c.Balance = Convert.ToDecimal(reader["Balance"]);
                                        c.Image = Encoding.ASCII.GetBytes(reader["Image"].ToString());
                                        c.Site = reader["Site"].ToString();
                                        c.Email = reader["Email"].ToString();
                                        c.Phone = reader["Phone"].ToString();
                                        c.Notes = reader["Notes"].ToString();
                                        c.Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"]));
                                        c.CreatedAt = Convert.ToDateTime(reader["CreateDate"]);
                                        c.Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"]));
                                        c.EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"]);
                                        companyCol.Update(c);
                                    }
                                    else
                                    {
                                        companyCol.Insert(new Company
                                        {
                                            Id = id,
                                            Name = reader["Name"].ToString(),
                                            Balance = Convert.ToDecimal(reader["Balance"]),
                                            Image = Encoding.ASCII.GetBytes(reader["Image"].ToString()),
                                            Site = reader["Site"].ToString(),
                                            Email = reader["Email"].ToString(),
                                            Phone = reader["Phone"].ToString(),
                                            Notes = reader["Notes"].ToString(),
                                            Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                            CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                            Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                            EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [SalesMen]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var id = Convert.ToInt64(reader["Id"]);
                                    var salesmanCol = db.GetCollection<SalesMan>(DBCollections.SalesMen);
                                    if (id == 1)
                                    {
                                        var s = salesmanCol.Find(x => x.Id == id).FirstOrDefault();
                                        s.Name = reader["Name"].ToString();
                                        s.Balance = Convert.ToDecimal(reader["Balance"]);
                                        s.Site = reader["Site"].ToString();
                                        s.Email = reader["Email"].ToString();
                                        s.Phone = reader["Phone"].ToString();
                                        s.Notes = reader["Notes"].ToString();
                                        s.Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"]));
                                        s.CreatedAt = Convert.ToDateTime(reader["CreateDate"]);
                                        s.Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"]));
                                        s.EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"]);
                                        salesmanCol.Update(s);
                                    }
                                    else
                                    {
                                        salesmanCol.Insert(new SalesMan
                                        {
                                            Id = id,
                                            Name = reader["Name"].ToString(),
                                            Balance = Convert.ToDecimal(reader["Balance"]),
                                            Site = reader["Site"].ToString(),
                                            Email = reader["Email"].ToString(),
                                            Phone = reader["Phone"].ToString(),
                                            Notes = reader["Notes"].ToString(),
                                            Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                            CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                            Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                            EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [Suppliers]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var id = Convert.ToInt64(reader["Id"]);
                                    var supplierCol = db.GetCollection<Supplier>(DBCollections.Suppliers);
                                    if (id == 1)
                                    {
                                        var s = supplierCol.Find(x => x.Id == id).FirstOrDefault();
                                        s.Name = reader["Name"].ToString();
                                        s.Balance = Convert.ToDecimal(reader["Balance"]);
                                        s.Image = Encoding.ASCII.GetBytes(reader["Image"].ToString());
                                        s.Site = reader["Site"].ToString();
                                        s.Email = reader["Email"].ToString();
                                        s.Phone = reader["Phone"].ToString();
                                        s.SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(Convert.ToInt64(reader["SalesManId"]));
                                        s.Notes = reader["Notes"].ToString();
                                        s.Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"]));
                                        s.CreatedAt = Convert.ToDateTime(reader["CreateDate"]);
                                        s.Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"]));
                                        s.EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"]);
                                        supplierCol.Update(s);
                                    }
                                    else
                                    {
                                        supplierCol.Insert(new Supplier
                                        {
                                            Id = id,
                                            Name = reader["Name"].ToString(),
                                            Balance = Convert.ToDecimal(reader["Balance"]),
                                            Image = Encoding.ASCII.GetBytes(reader["Image"].ToString()),
                                            Site = reader["Site"].ToString(),
                                            Email = reader["Email"].ToString(),
                                            Phone = reader["Phone"].ToString(),
                                            SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(Convert.ToInt64(reader["SalesManId"])),
                                            Notes = reader["Notes"].ToString(),
                                            Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                            CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                            Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                            EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [Stores]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var id = Convert.ToInt64(reader["Id"]);
                                    var storeCol = db.GetCollection<Store>(DBCollections.Stores);
                                    if (id == 1)
                                    {
                                        var s = storeCol.Find(x => x.Id == id).FirstOrDefault();
                                        s.Name = reader["Name"].ToString();
                                        s.Motto = reader["Motto"].ToString();
                                        s.Image = Encoding.ASCII.GetBytes(reader["Image"].ToString());
                                        s.Address1 = reader["Address1"].ToString();
                                        s.Address2 = reader["Address2"].ToString();
                                        s.Tel1 = reader["Tel1"].ToString();
                                        s.Tel2 = reader["Tel2"].ToString();
                                        s.Phone1 = reader["Phone1"].ToString();
                                        s.Phone2 = reader["Phone2"].ToString();
                                        s.Email1 = reader["Email1"].ToString();
                                        s.Email2 = reader["Email2"].ToString();
                                        s.Site = reader["Site"].ToString();
                                        s.Notes = reader["Notes"].ToString();
                                        s.Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"]));
                                        s.CreatedAt = Convert.ToDateTime(reader["CreateDate"]);
                                        s.Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"]));
                                        s.EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"]);
                                        storeCol.Update(s);
                                    }
                                    else
                                    {
                                        storeCol.Insert(new Store
                                        {
                                            Id = id,
                                            Name = reader["Name"].ToString(),
                                            Motto = reader["Motto"].ToString(),
                                            Image = Encoding.ASCII.GetBytes(reader["Image"].ToString()),
                                            Address1 = reader["Address1"].ToString(),
                                            Address2 = reader["Address2"].ToString(),
                                            Tel1 = reader["Tel1"].ToString(),
                                            Tel2 = reader["Tel2"].ToString(),
                                            Phone1 = reader["Phone1"].ToString(),
                                            Phone2 = reader["Phone2"].ToString(),
                                            Email1 = reader["Email1"].ToString(),
                                            Email2 = reader["Email2"].ToString(),
                                            Site = reader["Site"].ToString(),
                                            Notes = reader["Notes"].ToString(),
                                            Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                            CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                            Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                            EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [Treasuries]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var id = Convert.ToInt64(reader["Id"]);
                                    var treasuryCol = db.GetCollection<Treasury>(DBCollections.Treasuries);
                                    if (id == 1)
                                    {
                                        var t = treasuryCol.Find(x => x.Id == id).FirstOrDefault();
                                        t.Name = reader["Name"].ToString();
                                        t.Balance = Convert.ToDecimal(reader["Balance"]);
                                        t.Store = db.GetCollection<Store>(DBCollections.Stores).FindById(Convert.ToInt64(reader["StoreId"]));
                                        t.Notes = reader["Notes"].ToString();
                                        t.Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"]));
                                        t.CreatedAt = Convert.ToDateTime(reader["CreateDate"]);
                                        t.Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"]));
                                        t.EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"]);
                                        treasuryCol.Update(t);
                                    }
                                    else
                                    {
                                        treasuryCol.Insert(new Treasury
                                        {
                                            Id = id,
                                            Name = reader["Name"].ToString(),
                                            Balance = Convert.ToDecimal(reader["Balance"]),
                                            Store = db.GetCollection<Store>(DBCollections.Stores).FindById(Convert.ToInt64(reader["StoreId"])),
                                            Notes = reader["Notes"].ToString(),
                                            Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                            CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                            Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                            EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [ClientMoves]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var clientMoveCol = db.GetCollection<ClientMove>(DBCollections.ClientsMoves);
                                    clientMoveCol.Insert(new ClientMove
                                    {
                                        Id = Convert.ToInt64(reader["Id"]),
                                        Client = db.GetCollection<Client>(DBCollections.Clients).FindById(Convert.ToInt64(reader["ClientId"])),
                                        Debit = Convert.ToDecimal(reader["Debit"]),
                                        Credit = Convert.ToDecimal(reader["Credit"]),
                                        Notes = reader["Notes"].ToString(),
                                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                        CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                        Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                        EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [CompanyMoves]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var companyMoveCol = db.GetCollection<CompanyMove>(DBCollections.CompaniesMoves);
                                    companyMoveCol.Insert(new CompanyMove
                                    {
                                        Id = Convert.ToInt64(reader["Id"]),
                                        Company = db.GetCollection<Company>(DBCollections.Companies).FindById(Convert.ToInt64(reader["CompanyId"])),
                                        Debit = Convert.ToDecimal(reader["Debit"]),
                                        Credit = Convert.ToDecimal(reader["Credit"]),
                                        Notes = reader["Notes"].ToString(),
                                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                        CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                        Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                        EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [SalesManMoves]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var salesmanMoveCol = db.GetCollection<SalesManMove>(DBCollections.SalesMenMoves);
                                    salesmanMoveCol.Insert(new SalesManMove
                                    {
                                        Id = Convert.ToInt64(reader["Id"]),
                                        SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(Convert.ToInt64(reader["SalesManId"])),
                                        Debit = Convert.ToDecimal(reader["Debit"]),
                                        Credit = Convert.ToDecimal(reader["Credit"]),
                                        Notes = reader["Notes"].ToString(),
                                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                        CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                        Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                        EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [SupplierMoves]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var supplierMoveCol = db.GetCollection<SupplierMove>(DBCollections.SuppliersMoves);
                                    supplierMoveCol.Insert(new SupplierMove
                                    {
                                        Id = Convert.ToInt64(reader["Id"]),
                                        Supplier = db.GetCollection<Supplier>(DBCollections.Suppliers).FindById(Convert.ToInt64(reader["SupplierId"])),
                                        Debit = Convert.ToDecimal(reader["Debit"]),
                                        Credit = Convert.ToDecimal(reader["Credit"]),
                                        Notes = reader["Notes"].ToString(),
                                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                        CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                        Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                        EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [TreasuryMoves]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var treasuryMoveCol = db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves);
                                    treasuryMoveCol.Insert(new TreasuryMove
                                    {
                                        Id = Convert.ToInt64(reader["Id"]),
                                        Treasury = db.GetCollection<Treasury>(DBCollections.Treasuries).FindById(Convert.ToInt64(reader["TreasuryId"])),
                                        Debit = Convert.ToDecimal(reader["Debit"]),
                                        Credit = Convert.ToDecimal(reader["Credit"]),
                                        Notes = reader["Notes"].ToString(),
                                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                        CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                        Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                        EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [Services]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var serviceCol = db.GetCollection<Service>(DBCollections.Services);
                                    serviceCol.Insert(new Service
                                    {
                                        Id = Convert.ToInt64(reader["Id"]),
                                        Name = reader["Name"].ToString(),
                                        Balance = Convert.ToDecimal(reader["Balance"]),
                                        Image = Encoding.ASCII.GetBytes(reader["Image"].ToString()),
                                        Site = reader["Site"].ToString(),
                                        Email = reader["Email"].ToString(),
                                        Phone = reader["Phone"].ToString(),
                                        Notes = reader["Notes"].ToString(),
                                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                        CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                        Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                        EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [ServiceMoves]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var serviceMoveCol = db.GetCollection<ServiceMove>(DBCollections.ServicesMoves);
                                    serviceMoveCol.Insert(new ServiceMove
                                    {
                                        Id = Convert.ToInt64(reader["Id"]),
                                        Service = db.GetCollection<Service>(DBCollections.Services).FindById(Convert.ToInt64(reader["ServiceId"])),
                                        Debit = Convert.ToDecimal(reader["Debit"]),
                                        Credit = Convert.ToDecimal(reader["Credit"]),
                                        Notes = reader["Notes"].ToString(),
                                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                        CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                        Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                        EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [Items]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var itemCol = db.GetCollection<Item>(DBCollections.Items);
                                    itemCol.Insert(new Item
                                    {
                                        Id = Convert.ToInt64(reader["Id"]),
                                        Name = reader["Name"].ToString(),
                                        Barcode = reader["Barcode"].ToString(),
                                        Shopcode = reader["Shopcode"].ToString(),
                                        Image = Encoding.ASCII.GetBytes(reader["Image"].ToString()),
                                        Group = (ItemGroup)Convert.ToByte(reader["Group"]),
                                        PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]),
                                        WholeSalePrice = Convert.ToDecimal(reader["WholeSalePrice"]),
                                        HalfWholeSalePrice = Convert.ToDecimal(reader["HalfWholeSalePrice"]),
                                        RetailPrice = Convert.ToDecimal(reader["RetailPrice"]),
                                        QTY = Convert.ToDecimal(reader["QTY"]),
                                        Company = db.GetCollection<Company>(DBCollections.Companies).FindById(Convert.ToInt64(reader["CompanyId"])),
                                        Supplier = db.GetCollection<Supplier>(DBCollections.Suppliers).FindById(Convert.ToInt64(reader["SupplierId"])),
                                        Notes = reader["Notes"].ToString(),
                                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                        CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                        Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                        EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [Notes]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    var itemCol = db.GetCollection<Note>(DBCollections.Notes);
                                    itemCol.Insert(new Note
                                    {
                                        Id = Convert.ToInt64(reader["Id"]),
                                        Name = reader["Name"].ToString(),
                                        Group = (NoteGroup)Convert.ToByte(reader["Group"]),
                                        Phone = reader["Phone"].ToString(),
                                        Notes = reader["Notes"].ToString(),
                                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                        CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                        Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                        EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [Bills]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    db.GetCollection<Bill>(DBCollections.Bills).Insert(new Bill
                                    {
                                        Id = Convert.ToInt64(reader["Id"]),
                                        Client = db.GetCollection<Client>(DBCollections.Clients).FindById(Convert.ToInt64(reader["ClientId"])),
                                        Store = db.GetCollection<Store>(DBCollections.Stores).FindById(Convert.ToInt64(reader["StoreId"])),
                                        Discount = Convert.ToDecimal(reader["Discount"]),
                                        TotalAfterDiscounts = Convert.ToDecimal(reader["TotalAfterDiscounts"]),
                                        TotalPayed = Convert.ToDecimal(reader["TotalPayed"]),
                                        IsReturned = Convert.ToBoolean(reader["IsReturned"]),
                                        Notes = reader["Notes"].ToString(),
                                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                        CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                        Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                        EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [BillItemMoves]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    db.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves).Insert(new BillItemMove
                                    {
                                        Id = Convert.ToInt64(reader["Id"]),
                                        Bill = db.GetCollection<Bill>(DBCollections.Bills).FindById(Convert.ToInt64(reader["BillId"])),
                                        Item = db.GetCollection<Item>(DBCollections.Items).FindById(Convert.ToInt64(reader["ItemId"])),
                                        ItemPrice = Convert.ToDecimal(reader["ItemPrice"]),
                                        QTY = Convert.ToDecimal(reader["QTY"]),
                                        Discount = Convert.ToDecimal(reader["Discount"]),
                                        Notes = reader["Notes"].ToString(),
                                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                        CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                        Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                        EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("select * from [BillServiceMoves]", conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                                {
                                    db.GetCollection<BillServiceMove>(DBCollections.BillsServicesMoves).Insert(new BillServiceMove
                                    {
                                        Id = Convert.ToInt64(reader["Id"]),
                                        Bill = db.GetCollection<Bill>(DBCollections.Bills).FindById(Convert.ToInt64(reader["BillId"])),
                                        Service = db.GetCollection<Service>(DBCollections.Services).FindById(Convert.ToInt64(reader["ServiceId"])),
                                        Cost = Convert.ToDecimal(reader["ServicePayment"]),
                                        Discount = Convert.ToDecimal(reader["Discount"]),
                                        Notes = reader["Notes"].ToString(),
                                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["CreatedById"])),
                                        CreatedAt = Convert.ToDateTime(reader["CreateDate"]),
                                        Editor = reader["EditById"] == DBNull.Value ? null : db.GetCollection<User>(DBCollections.Users).FindById(Convert.ToInt32(reader["EditById"])),
                                        EditedAt = reader["EditDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["EditDate"])
                                    });
                                }
                            }
                        }
                    }
                }
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}