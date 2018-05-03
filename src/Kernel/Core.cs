using Exceptionless;
using Phony.Model;
using Phony.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Phony.Kernel
{
    public class Core
    {
        /// <summary>
        /// Program Theme settings
        /// </summary>
        public static string Theme = Properties.Settings.Default.Theme, Color = Properties.Settings.Default.Color;

        /// <summary> 
        /// The first thing that program is going to do after showing up
        /// like checking for database connection etc
        /// </summary> 
        public static void StartUp_Engine()
        {
            try
            {
                new Thread(() =>
                {
                    try
                    {
                        Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhonyDbContext, Migrations.Configuration>());
                    }
                    catch (Exception e)
                    {
                        SaveException(e);
                    }
                    finally
                    {
                        Thread.CurrentThread.Abort();
                    }
                }).Start();
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
                                db.Complete();
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
                                db.Complete();
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
                                db.Complete();
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
                                db.Complete();
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
                                db.Complete();
                            }
                            var st = db.Stores.Get(1);
                            if (st == null)
                            {
                                st = new Store
                                {
                                    Name = "التوكل على الله",
                                    CreatedById = 1,
                                    CreateDate = DateTime.Now,
                                    EditById = null,
                                    EditDate = null
                                };
                                db.Stores.Add(st);
                                db.Complete();
                            }
                        }
                        Properties.Settings.Default.IsConfigured = true;
                        Properties.Settings.Default.Save();
                    }
                    catch (Exception e)
                    {
                        SaveException(e);
                    }
                }
            }
            catch (Exception ex)
            {
                SaveException(ex);
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
                e.ToExceptionless().Submit();
                if (e.TargetSite.Name == "ThrowInvalidOperationException") return;
                DateTime now = DateTime.Now;
                string str = string.Concat(new object[] { now.Month, "-", now.Day, "//" });
                if (!Directory.Exists(Paths.UnhandledExceptionsPath))
                {
                    Directory.CreateDirectory(Paths.UnhandledExceptionsPath);
                }
                if (!Directory.Exists(Paths.UnhandledExceptionsPath + str))
                {
                    Directory.CreateDirectory(Paths.UnhandledExceptionsPath + str);
                }
                if (!Directory.Exists(Paths.UnhandledExceptionsPath + str + e.TargetSite.Name))
                {
                    Directory.CreateDirectory(Paths.UnhandledExceptionsPath + str + e.TargetSite.Name);
                }
                File.WriteAllLines((Paths.UnhandledExceptionsPath + str + e.TargetSite.Name + "\\") + string.Concat(new object[] { now.Hour, "-", now.Minute, "-", now.Ticks & 10L }) + ".txt", new List<string>
                {
                    "----Exception message----",
                    e.Message,
                    "----End of exception message----\r\n",
                    "----Stack trace----",
                    e.StackTrace,
                    "----End of stack trace----\r\n"
                }.ToArray());
            }
        }

        /// <summary> 
        /// Save Exceptions to the program folder in LocalApplicationData folder
        /// </summary> 
        /// <param name="e">exception string</param>
        public async static Task SaveExceptionAsync(Exception e)
        {
            await Task.Delay(100);
            Console.WriteLine(e.ToString());
            e.ToExceptionless().Submit();
            if (e.TargetSite.Name == "ThrowInvalidOperationException") return;
            DateTime now = DateTime.Now;
            string str = string.Concat(new object[] { now.Month, "-", now.Day, "//" });
            if (!Directory.Exists(Paths.UnhandledExceptionsPath))
            {
                Directory.CreateDirectory(Paths.UnhandledExceptionsPath);
            }
            if (!Directory.Exists(Paths.UnhandledExceptionsPath + str))
            {
                Directory.CreateDirectory(Paths.UnhandledExceptionsPath + str);
            }
            if (!Directory.Exists(Paths.UnhandledExceptionsPath + str + e.TargetSite.Name))
            {
                Directory.CreateDirectory(Paths.UnhandledExceptionsPath + str + e.TargetSite.Name);
            }
            await Task.Run(() =>
            {
                File.WriteAllLines((Paths.UnhandledExceptionsPath + str + e.TargetSite.Name + "\\") + string.Concat(new object[] { now.Hour, "-", now.Minute, "-", now.Ticks & 10L }) + ".txt", new List<string>
                {
                    "----Exception message----",
                    e.Message,
                    "----End of exception message----\r\n",
                    "----Stack trace----",
                    e.StackTrace,
                    "----End of stack trace----\r\n"
                }.ToArray());
            });
        }
    }
}