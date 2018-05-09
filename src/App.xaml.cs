using Exceptionless;
using MahApps.Metro;
using MaterialDesignThemes.Wpf;
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
            ExceptionlessClient.Default.Register();
            if (string.IsNullOrWhiteSpace(Phony.Properties.Settings.Default.Theme))
            {
                Phony.Properties.Settings.Default.Theme = "BaseLight";
                Phony.Properties.Settings.Default.Save();
            }
            if (string.IsNullOrWhiteSpace(Phony.Properties.Settings.Default.Color))
            {
                Phony.Properties.Settings.Default.Color = "Teal";
                Phony.Properties.Settings.Default.Save();
            }
            try
            {
                new PaletteHelper().ReplacePrimaryColor(Phony.Properties.Settings.Default.Color);
                new PaletteHelper().ReplaceAccentColor("Yellow");
                if (Phony.Properties.Settings.Default.Theme == "BaseLight")
                {
                    new PaletteHelper().SetLightDark(false);
                }
                else
                {
                    new PaletteHelper().SetLightDark(true);
                }
                Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(appStyle.Item2.Name), ThemeManager.GetAppTheme(Phony.Properties.Settings.Default.Theme));
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
            }
            Core.StartUp_Engine();
            base.OnStartup(e);
        }
    }
}
