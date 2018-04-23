using Phony.Kernel;
using Phony.Model;
using Phony.Persistence;
using Phony.Utility;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Phony.ViewModel
{
    public class MainWindowVM : CommonBase
    {
        static Uri _currentSource;
        static string _pageName;
        static int _itemsCount;
        static int _clientsCount;
        static int _shortagesCount;

        public int ItemsCount
        {
            get => _itemsCount;
            set
            {
                if (value != _itemsCount)
                {
                    _itemsCount = value;
                    RaisePropertyChanged(nameof(ItemsCount));
                }
            }
        }

        public int ClientsCount
        {
            get => _clientsCount;
            set
            {
                if (value != _clientsCount)
                {
                    _clientsCount = value;
                    RaisePropertyChanged(nameof(ClientsCount));
                }
            }
        }

        public int ShortagesCount
        {
            get => _shortagesCount;
            set
            {
                if (value != _shortagesCount)
                {
                    _shortagesCount = value;
                    RaisePropertyChanged(nameof(ShortagesCount));
                }
            }
        }

        public ICommand ChangeSource { get; set; }
        public ICommand OpenItemsWindow { get; set; }
        public ICommand OpenClientsWindow { get; set; }
        public ICommand OpenShortagesWindow { get; set; }

        public MainWindowVM()
        {
            LoadCommands();
            if (string.IsNullOrWhiteSpace(PageName))
            {
                PageName = "Users/Login";
            }
            NavigateToPage(PageName);
            Task.Run(() => this.CountEveryThing()).Wait();
        }

        public void LoadCommands()
        {
            ChangeSource = new CustomCommand(ChangeCurrentSource, CanChangeCurrentSource);
            OpenItemsWindow = new CustomCommand(DoOpenItemsWindow, CanOpenItemsWindow);
            OpenClientsWindow = new CustomCommand(DoOpenClientsWindow, CanOpenClientsWindow);
            OpenShortagesWindow = new CustomCommand(DoOpenShortagesWindow, CanOpenShortagesWindow);
        }

        async Task CountEveryThing()
        {
            using (var db = new PhonyDbContext())
            {
                await Task.Run(() =>
                {
                    ItemsCount = db.Items.Where(i => i.Group == ItemGroup.Other).Count();
                });
                await Task.Run(() =>
                {
                    ClientsCount = db.Clients.Count();
                });
                await Task.Run(() =>
                {
                    ShortagesCount = db.Items.Where(i => i.Group == ItemGroup.Other && i.QTY == 0).Count();
                });
            }
        }

        private bool CanOpenShortagesWindow(object obj)
        {
            return true;
        }

        private void DoOpenShortagesWindow(object obj)
        {
            new View.Shortages().Show();
        }

        private bool CanOpenClientsWindow(object obj)
        {
            return true;
        }

        private void DoOpenClientsWindow(object obj)
        {
            new View.Clients().Show();
        }

        private void DoOpenItemsWindow(object obj)
        {
            new View.Items().Show();
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