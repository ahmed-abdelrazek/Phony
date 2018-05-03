using Exceptionless;
using MahApps.Metro;
using MaterialDesignThemes.Wpf;
using Phony.Kernel;
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
            ExceptionlessClient.Default.Register();
            if (!string.IsNullOrWhiteSpace(Phony.Properties.Settings.Default.Color) || !string.IsNullOrWhiteSpace(Phony.Properties.Settings.Default.Theme))
            {
                new PaletteHelper().ReplacePrimaryColor(Phony.Properties.Settings.Default.Color);
                new PaletteHelper().ReplaceAccentColor(Phony.Properties.Settings.Default.Color);
                ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(Phony.Properties.Settings.Default.Color), ThemeManager.GetAppTheme(Phony.Properties.Settings.Default.Theme)); // or appStyle.Item1
            }
            else
            {
                ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Emerald"), ThemeManager.GetAppTheme("BaseDark")); // or appStyle.Item1
            }
            Core.StartUp_Engine();
            base.OnStartup(e);
        }
    }
}
