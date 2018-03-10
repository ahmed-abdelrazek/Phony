using System;
using System.IO;
using System.Windows;

namespace Phony.Kernel
{
    public class Paths
    {
        public static string UnhandledExceptionsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + Path.GetFileNameWithoutExtension(Application.ResourceAssembly.Location) + "\\Logs\\";
    }
}