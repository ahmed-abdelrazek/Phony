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
                    new Thread(() =>
                    {
                        try
                        {
                            using (var db = new UnitOfWork(new PhonyDbContext()))
                            {
                                var u = db.Users.Get(1);
                                if (u == null)
                                {
                                    var nu = new User
                                    {
                                        Name = "admin",
                                        Pass = Encryption.EncryptText("admin"),
                                        Group = ViewModel.UserGroup.Manager,
                                        IsActive = true
                                    };
                                    db.Users.Add(nu);
                                    db.Complete();
                                }
                                var cl = db.Clients.Get(1);
                                if (cl == null)
                                {
                                    var ncl = new Client
                                    {
                                        Name = "كاش",
                                        Balance = 0,
                                        CreatedById = 1,
                                        CreateDate = DateTime.Now,
                                        EditById = null,
                                        EditDate = null
                                    };
                                    db.Clients.Add(ncl);
                                    db.Complete();
                                }
                                var co = db.Companies.Get(1);
                                if (co == null)
                                {
                                    var nco = new Company
                                    {
                                        Name = "لا يوجد",
                                        Balance = 0,
                                        CreatedById = 1,
                                        CreateDate = DateTime.Now,
                                        EditById = null,
                                        EditDate = null
                                    };
                                    db.Companies.Add(nco);
                                    db.Complete();
                                }
                                var s = db.Suppliers.Get(1);
                                if (s == null)
                                {
                                    var ns = new Supplier
                                    {
                                        Name = "لا يوجد",
                                        Balance = 0,
                                        CreatedById = 1,
                                        CreateDate = DateTime.Now,
                                        EditById = null,
                                        EditDate = null
                                    };
                                    db.Suppliers.Add(ns);
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
                        finally
                        {
                            Thread.CurrentThread.Abort();
                        }
                    }).Start();
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
    }
}