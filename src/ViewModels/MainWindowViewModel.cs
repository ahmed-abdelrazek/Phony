using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        static Uri _currentSource;
        static string _pageName;

        public ICommand ChangeSource { get; set; }
        public ICommand OpenSettingsWindow { get; set; }

        public MainWindowViewModel()
        {
            LoadCommands();
            if (string.IsNullOrWhiteSpace(PageName))
            {
                PageName = "Login";
            }
            NavigateToPage(PageName);
        }

        public void LoadCommands()
        {
            ChangeSource = new DelegateCommand(ChangeCurrentSource, CanChangeCurrentSource);
            OpenSettingsWindow = new DelegateCommand(DoOpenSettingsWindow, CanOpenSettingsWindow);
        }

        private bool CanOpenSettingsWindow()
        {
            return true;
        }

        private void DoOpenSettingsWindow()
        {
            new Settings(0).ShowDialog();
        }

        private void ChangeCurrentSource()
        {
            NavigateToPage(PageName);
        }

        private bool CanChangeCurrentSource()
        {
            if (CurrentSource != null)
            {
                return true;
            }
            return false;
        }

        public static Uri CurrentSource
        {
            get { return _currentSource; }
            set
            {
                if (_currentSource != value && value != null)
                {
                    _currentSource = value;
                }
            }
        }

        public string PageName
        {
            get => _pageName;
            set
            {
                if (_pageName != value)
                {
                    _pageName = value;
                    NavigateToPage(value);
                }
            }
        }

        public void NavigateToPage(string Page)
        {
            CurrentSource = new Uri("/PhonyLite;component/Views/Pages/" + Page + ".xaml", UriKind.Relative);
        }
    }
}