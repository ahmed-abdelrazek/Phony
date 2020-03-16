using Phony.WPF.Data;

namespace Phony.WPF.ViewModels
{
    public class AboutAppViewModel : BaseViewModelWithAnnotationValidation
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
            AppVersion = "رقم الاصدار: " + new AssemblyInfo(System.Reflection.Assembly.GetEntryAssembly()).Version;
        }
    }
}
