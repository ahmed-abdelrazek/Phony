using Caliburn.Micro;
using Phony.WPF.Models;
using Phony.WPF.Views;
using System.Data.Common;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Phony.WPF.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<User>
    {
        SimpleContainer _container;
        IEventAggregator _events;
        IWindowManager _windowManager;

        DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();

        public ShellViewModel(IWindowManager windowManager, IEventAggregator events, SimpleContainer container)
        {
            _container = container;
            _windowManager = windowManager;
            _events = events;
            _events.SubscribeOnUIThread(this);
            ActivateItem(_container.GetInstance<LoginViewModel>());
        }

        public void Loaded()
        {
            connectionStringBuilder.ConnectionString = Properties.Settings.Default.LiteDbConnectionString;
            if (string.IsNullOrWhiteSpace(connectionStringBuilder.ConnectionString) || !File.Exists(connectionStringBuilder["Filename"].ToString()) || !Properties.Settings.Default.IsConfigured)
            {
                _windowManager.ShowDialog(_container.GetInstance<SettingsViewModel>());
            }
        }

        public async Task HandleAsync(User message, CancellationToken cancellationToken)
        {
            ActivateItem(_container.GetInstance<MainViewModel>());
            await Task.Delay(20);
        }
    }
}
