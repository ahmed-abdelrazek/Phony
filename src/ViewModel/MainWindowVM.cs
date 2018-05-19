using Phony.Kernel;
using Phony.Utility;
using Phony.View;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModel
{
    public class MainWindowVM : CommonBase
    {
        static Uri _currentSource;
        static string _pageName;

        public ICommand ChangeSource { get; set; }
        public ICommand OpenSettingsWindow { get; set; }

        MainWindow Message = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

        public MainWindowVM()
        {
            LoadCommands();
            if (string.IsNullOrWhiteSpace(PageName))
            {
                PageName = "Users/Login";
            }
            NavigateToPage(PageName);
        }

        public void LoadCommands()
        {
            ChangeSource = new CustomCommand(ChangeCurrentSource, CanChangeCurrentSource);
            OpenSettingsWindow = new CustomCommand(DoOpenSettingsWindow, CanOpenSettingsWindow);
        }

        private bool CanOpenSettingsWindow(object obj)
        {
            return true;
        }

        private void DoOpenSettingsWindow(object obj)
        {
            new Settings().ShowDialog();
        }

        private void ChangeCurrentSource(object obj)
        {
            NavigateToPage(PageName);
        }

        private bool CanChangeCurrentSource(object obj)
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
            CurrentSource = new Uri("/Phony;component/Pages/" + Page + ".xaml", UriKind.Relative);
        }
    }
}