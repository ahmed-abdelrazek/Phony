using RazorEngineCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
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
            var cul = new CultureInfo("ar-EG");
            Thread.CurrentThread.CurrentCulture = cul;
            Thread.CurrentThread.CurrentUICulture = cul;

            Data.Core.StartUp_Engine();

            if (string.IsNullOrWhiteSpace(Phony.Properties.Settings.Default.PrimaryColor))
            {
                Phony.Properties.Settings.Default.PrimaryColor = "Teal";
                Phony.Properties.Settings.Default.Save();
            }
            if (string.IsNullOrWhiteSpace(Phony.Properties.Settings.Default.AccentColor))
            {
                Phony.Properties.Settings.Default.AccentColor = "Yellow";
                Phony.Properties.Settings.Default.Save();
            }
            try
            {
                Helpers.ThemeHelper.ApplyBase(Phony.Properties.Settings.Default.IsDarkTheme);
                Helpers.ThemeHelper.ApplyPrimary(Helpers.ThemeHelper.Swatches.First(x => x.Name == Phony.Properties.Settings.Default.PrimaryColor));
                Helpers.ThemeHelper.ApplyAccent(Helpers.ThemeHelper.Swatches.First(x => x.Name == Phony.Properties.Settings.Default.AccentColor));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            // Init Razor Engine to be faster at first load
            new Thread(new ThreadStart(() =>
            {
                IRazorEngine razorEngine = new RazorEngine();
                IRazorEngineCompiledTemplate template = razorEngine.Compile("Hello @Model.Name");

                string result = template.Run(new
                {
                    Name = "RazorEngine"
                });

                System.Diagnostics.Debug.WriteLine(result);
            })).Start();

            base.OnStartup(e);
        }
    }
}