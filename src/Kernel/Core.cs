using Exceptionless;
using Phony.Model;
using Phony.Persistence;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;

namespace Phony.Kernel
{
    public class Core
    {
        /// <summary> 
        /// The first thing that program is going to do while showing up
        /// like checking for database connection etc
        /// Upgrade the database and add stuff for the first use
        /// </summary> 
        public static async void StartUp_Engine()
        {
            try
            {
                await Task.Run(() =>
                {
                    Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhonyDbContext, Migrations.Configuration>());
                });
            }
            catch (Exception e)
            {
                await SaveExceptionAsync(e);
            }
            if (!Properties.Settings.Default.IsConfigured)
            {
                try
                {
                    using (var db = new UnitOfWork(new PhonyDbContext()))
                    {
                        var u = db.Users.Get(1);
                        if (u == null)
                        {
                            u = new User
                            {
                                Name = "admin",
                                Pass = SecurePasswordHasher.Hash("admin"),
                                Group = ViewModel.UserGroup.Manager,
                                IsActive = true
                            };
                            db.Users.Add(u);
                            await db.CompleteAsync();
                        }
                        var cl = db.Clients.Get(1);
                        if (cl == null)
                        {
                            cl = new Client
                            {
                                Name = "كاش",
                                Balance = 0,
                                CreatedById = 1,
                                CreateDate = DateTime.Now,
                                EditById = null,
                                EditDate = null
                            };
                            db.Clients.Add(cl);
                            await db.CompleteAsync();
                        }
                        var co = db.Companies.Get(1);
                        if (co == null)
                        {
                            co = new Company
                            {
                                Name = "لا يوجد",
                                Balance = 0,
                                CreatedById = 1,
                                CreateDate = DateTime.Now,
                                EditById = null,
                                EditDate = null
                            };
                            db.Companies.Add(co);
                            await db.CompleteAsync();
                        }
                        var sa = db.SalesMen.Get(1);
                        if (sa == null)
                        {
                            sa = new SalesMan
                            {
                                Name = "لا يوجد",
                                Balance = 0,
                                CreatedById = 1,
                                CreateDate = DateTime.Now,
                                EditById = null,
                                EditDate = null
                            };
                            db.SalesMen.Add(sa);
                            await db.CompleteAsync();
                        }
                        var su = db.Suppliers.Get(1);
                        if (su == null)
                        {
                            su = new Supplier
                            {
                                Name = "لا يوجد",
                                Balance = 0,
                                SalesManId = 1,
                                CreatedById = 1,
                                CreateDate = DateTime.Now,
                                EditById = null,
                                EditDate = null
                            };
                            db.Suppliers.Add(su);
                            await db.CompleteAsync();
                        }
                        var st = db.Stores.Get(1);
                        if (st == null)
                        {
                            st = new Store
                            {
                                Name = "التوكل",
                                CreatedById = 1,
                                CreateDate = DateTime.Now,
                                EditById = null,
                                EditDate = null
                            };
                            db.Stores.Add(st);
                            await db.CompleteAsync();
                        }
                        var t = db.Treasuries.Get(1);
                        if (t == null)
                        {
                            t = new Treasury
                            {
                                Name = "الرئيسية",
                                StoreId = 1,
                                Balance = 0,
                                CreatedById = 1,
                                CreateDate = DateTime.Now,
                                EditById = null,
                                EditDate = null
                            };
                            db.Treasuries.Add(t);
                            await db.CompleteAsync();
                        }
                    }
                    Properties.Settings.Default.IsConfigured = true;
                    Properties.Settings.Default.Save();
                }
                catch (Exception e)
                {
                    Properties.Settings.Default.IsConfigured = false;
                    Properties.Settings.Default.Save();
                    SaveException(e);
                }
            }
        }

        /// <summary> 
        /// Save Exceptions to the program folder in LocalApplicationData folder
        /// </summary> 
        /// <param name="e">exception string</param>
        public static void SaveException(Exception e)
        {
            lock (e)
            {
                Console.WriteLine(e.ToString());
                string appPath = UserLocalAppFolderPath();
                DateTime now = DateTime.Now;
                string str = string.Concat(new object[] { now.Year, "-", now.Month, "-", now.Day, "//" });
                if (!Directory.Exists($"{appPath}..\\Logs"))
                {
                    Directory.CreateDirectory($"{appPath}..\\Logs");
                }
                if (!Directory.Exists($"{appPath}..\\Logs\\{str}"))
                {
                    Directory.CreateDirectory($"{appPath}..\\Logs\\{str}");
                }
                File.WriteAllLines(($"{appPath}..\\Logs\\{str}\\") + string.Concat(new object[] { now.Hour, "-", now.Minute, "-", now.Second, "-", now.Ticks & 10L }) + ".txt", new List<string>
                {
                    "----Exception message----",
                    e.Message,
                    "----End of exception message----\r\n",
                    "----Stack trace----",
                    e.StackTrace,
                    "----End of stack trace----\r\n"
                }.ToArray());
                e.ToExceptionless().Submit();
            }
        }

        /// <summary> 
        /// Save Exceptions to the program folder in LocalApplicationData folder
        /// </summary> 
        /// <param name="e">exception string</param>
        public async static Task SaveExceptionAsync(Exception e)
        {
            Console.WriteLine(e.ToString());
            string appPath = UserLocalAppFolderPath();
            DateTime now = DateTime.Now;
            string str = string.Concat(new object[] { now.Year, "-", now.Month, "-", now.Day, "//" });
            if (!Directory.Exists($"{appPath}..\\Logs"))
            {
                Directory.CreateDirectory($"{appPath}..\\Logs");
            }
            if (!Directory.Exists($"{appPath}..\\Logs\\{str}"))
            {
                Directory.CreateDirectory($"{appPath}..\\Logs\\{str}");
            }
            await Task.Run(() =>
            {
                File.WriteAllLines(($"{appPath}..\\Logs\\{str}\\") + string.Concat(new object[] { now.Hour, "-", now.Minute, "-", now.Second, "-", now.Ticks & 10L }) + ".txt", new List<string>
                {
                    "----Exception message----",
                    e.Message,
                    "----End of exception message----\r\n",
                    "----Stack trace----",
                    e.StackTrace,
                    "----End of stack trace----\r\n"
                }.ToArray());
                e.ToExceptionless().Submit();
            });
        }

        /// <summary>
        /// Gets the full path to the app temp folder for settings and stuff in user AppData\Local
        /// </summary>
        /// <returns>Return a full path with \ at the end</returns>
        public static string UserLocalAppFolderPath()
        {
            var level = ConfigurationUserLevel.PerUserRoamingAndLocal;
            var configuration = ConfigurationManager.OpenExeConfiguration(level);
            var configurationFilePath = configuration.FilePath.Replace("user.config", "");
            return configurationFilePath;
        }
    }
}