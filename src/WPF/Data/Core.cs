using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Phony.WPF.Data
{
    public class Core
    {
        public static string UserLocalAppFolderPath { get; set; }

        /// <summary> 
        /// Save Exceptions to the program folder in LocalApplicationData folder
        /// </summary> 
        /// <param name="e">exception string</param>
        public static void SaveException(Exception e)
        {
            lock (e)
            {
                Console.WriteLine(e);
                DateTime now = DateTime.Now;
                string str = string.Concat(new object[] { now.Year, "-", now.Month, "-", now.Day, "//" });
                if (!Directory.Exists($"{UserLocalAppFolderPath}..\\Logs"))
                {
                    Directory.CreateDirectory($"{UserLocalAppFolderPath}..\\Logs");
                }
                if (!Directory.Exists($"{UserLocalAppFolderPath}..\\Logs\\{str}"))
                {
                    Directory.CreateDirectory($"{UserLocalAppFolderPath}..\\Logs\\{str}");
                }
                File.WriteAllLines(($"{UserLocalAppFolderPath}..\\Logs\\{str}\\") + string.Concat(new object[] { now.Hour, "-", now.Minute, "-", now.Second, "-", now.Ticks & 10L }) + ".txt", new List<string>
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
            Console.WriteLine(e);
            DateTime now = DateTime.Now;
            string str = string.Concat(new object[] { now.Year, "-", now.Month, "-", now.Day, "//" });
            if (!Directory.Exists($"{UserLocalAppFolderPath}..\\Logs"))
            {
                Directory.CreateDirectory($"{UserLocalAppFolderPath}..\\Logs");
            }
            if (!Directory.Exists($"{UserLocalAppFolderPath}..\\Logs\\{str}"))
            {
                Directory.CreateDirectory($"{UserLocalAppFolderPath}..\\Logs\\{str}");
            }
            await Task.Run(() =>
            {
                File.WriteAllLines(($"{UserLocalAppFolderPath}..\\Logs\\{str}\\") + string.Concat(new object[] { now.Hour, "-", now.Minute, "-", now.Second, "-", now.Ticks & 10L }) + ".txt", new List<string>
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

        /// <summary>
        /// Gets the full path to the app temp folder for settings and stuff in user AppData\Local
        /// </summary>
        /// <returns>Return a full path with \ at the end</returns>
        public static void GetUserLocalAppFolderPath()
        {
            var level = ConfigurationUserLevel.PerUserRoamingAndLocal;
            var configuration = ConfigurationManager.OpenExeConfiguration(level);
            var configurationFilePath = configuration.FilePath.Replace("user.config", "");
            UserLocalAppFolderPath = configurationFilePath;
        }
    }
}