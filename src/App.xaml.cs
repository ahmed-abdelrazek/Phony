using MahApps.Metro;
using Phony.Kernel;
using System;
using System.Windows;

namespace Phony
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            OnlyYou.Make();
            Core.StartUp_Engine();
            if (!string.IsNullOrWhiteSpace(Core.Color) || !string.IsNullOrWhiteSpace(Core.Theme))
            {
                ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(Core.Color), ThemeManager.GetAppTheme(Core.Theme)); // or appStyle.Item1
            }
            else
            {
                ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Emerald"), ThemeManager.GetAppTheme("BaseDark")); // or appStyle.Item1
            }
            base.OnStartup(e);
        }
    }
}
