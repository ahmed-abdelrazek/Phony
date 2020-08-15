using MaterialDesignColors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Phony.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TinyLittleMvvm;

namespace Phony.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = new HostBuilder()
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                    Data.Core.GetUserLocalAppFolderPath();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddTinyLittleMvvm();

                    ConfigureServices(services);
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddFilter("Phony", LogLevel.Debug);
                    logging.AddDebug();
                })
                .Build();
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();

            // if the app lost this data for some reason
            // or the app runs for the first time
            // it will load the default theme in App.xaml file
            if (!string.IsNullOrEmpty(WPF.Properties.Settings.Default.PrimaryColor) && !string.IsNullOrEmpty(WPF.Properties.Settings.Default.AccentColor))
            {
                try
                {
                    IEnumerable<Swatch> Swatches = new SwatchesProvider().Swatches;

                    PaletteSelectorViewModel.ApplyPrimary(Swatches.FirstOrDefault(x => x.Name == WPF.Properties.Settings.Default.PrimaryColor.ToLower()));
                    PaletteSelectorViewModel.ApplyAccent(Swatches.FirstOrDefault(x => x.Name == WPF.Properties.Settings.Default.AccentColor.ToLower()));

                    PaletteSelectorViewModel.ApplyBase(WPF.Properties.Settings.Default.IsDarkTheme);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            _host.Services
                .GetRequiredService<IWindowManager>()
                .ShowWindow<LoginViewModel>();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
            _host.Dispose();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass && type.IsPublic)
                .Where(type => (type.Name.EndsWith("ViewModel") || type.Name.EndsWith("View")))
                .ToList()
                .ForEach(viewModelType => services.AddTransient(viewModelType));
        }
    }
}