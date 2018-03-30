using Phony.Kernel;
using Phony.Utility;
using System;
using System.Windows.Input;

namespace Phony.ViewModel
{
    public class MainWindowVM : CommonBase
    {
        static Uri _currentSource;
        static string _pageName;
        
        public ICommand ChangeSource { get; set; }
        public ICommand OpenItemsWindow { get; set; }

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
            OpenItemsWindow = new CustomCommand(DoOpenItemsWindow, CanOpenItemsWindow);
        }

        private void DoOpenItemsWindow(object obj)
        {
            View.Items i = new View.Items();
            i.Show();
        }

        private bool CanOpenItemsWindow(object obj)
        {
            return true;
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

        public Uri CurrentSource
        {
            get { return _currentSource; }
            set
            {
                if (_currentSource != value)
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