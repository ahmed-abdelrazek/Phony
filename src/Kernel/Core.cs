using Phony.Model;
using Phony.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;

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
                using (var db = new UnitOfWork(new PhonyDbContext()))
                {
                    var c = db.Users.Get(1);
                    if (c == null)
                    {
                        User u = new User
                        {
                            Name = "admin",
                            Pass = Encryption.EncryptText("admin"),
                            Group = ViewModel.UserGroup.Manager,
                            IsActive = true
                        };
                        db.Users.Add(u);
                        db.Complete();
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