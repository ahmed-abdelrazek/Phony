using Phony.WPF.Data;
using System.Threading.Tasks;
using TinyLittleMvvm;

namespace Phony.WPF.ViewModels
{
    public class AboutAppViewModel : BaseViewModelWithAnnotationValidation, IOnLoadedHandler
    {
        string _appVersion;

        public string AppVersion
        {
            get => _appVersion;
            set
            {
                _appVersion = value;
                NotifyOfPropertyChange(() => AppVersion);
            }
        }

        public AboutAppViewModel()
        {
            OnLoadedAsync().ConfigureAwait(false);
        }

        public async Task OnLoadedAsync()
        {
            await Task.Run(() =>
            {
                AppVersion = "رقم الاصدار: " + new AssemblyInfo(System.Reflection.Assembly.GetEntryAssembly()).Version;
            });
        }
    }
}
