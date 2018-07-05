using Exceptionless;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                //Todo Do something at statup
            }
            catch (Exception e)
            {
                await SaveExceptionAsync(e);
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
            }
            e.ToExceptionless().Submit();
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