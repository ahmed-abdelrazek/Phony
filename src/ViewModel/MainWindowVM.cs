using MahApps.Metro.Controls.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs;
using Phony.Kernel;
using Phony.Persistence;
using Phony.Utility;
using Phony.View;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
        static int _servicesCount;
        static int _suppliersCount;
        static int _cardsCount;
        static int _companiesCount;
        static int _salesMenCount;
        static int _numbersCount;
        static int _usersCount;

        public int ItemsCount
        {
            get => _itemsCount;
            set
            {
                if (value != _itemsCount)
                {
                    _itemsCount = value;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        public int ServicesCount
        {
            get => _servicesCount;
            set
            {
                if (value != _servicesCount)
                {
                    _servicesCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int SuppliersCount
        {
            get => _suppliersCount;
            set
            {
                if (value != _suppliersCount)
                {
                    _suppliersCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int CardsCount
        {
            get => _cardsCount;
            set
            {
                if (value != _cardsCount)
                {
                    _cardsCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int CompaniesCount
        {
            get => _companiesCount;
            set
            {
                if (value != _companiesCount)
                {
                    _companiesCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int SalesMenCount
        {
            get => _salesMenCount;
            set
            {
                if (value != _salesMenCount)
                {
                    _salesMenCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int NumbersCount
        {
            get => _numbersCount;
            set
            {
                if (value != _numbersCount)
                {
                    _numbersCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int UsersCount
        {
            get => _usersCount;
            set
            {
                if (value != _usersCount)
                {
                    _usersCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand ChangeSource { get; set; }
        public ICommand OpenItemsWindow { get; set; }
        public ICommand OpenClientsWindow { get; set; }
        public ICommand OpenShortagesWindow { get; set; }
        public ICommand OpenServicesWindow { get; set; }
        public ICommand OpenSuppliersWindow { get; set; }
        public ICommand OpenCardsWindow { get; set; }
        public ICommand OpenCompaniesWindow { get; set; }
        public ICommand OpenSalesMenWindow { get; set; }
        public ICommand TakeBackup { get; set; }
        public ICommand RestoreBackup { get; set; }
        public ICommand OpenStoreInfoWindow { get; set; }
        public ICommand OpenNumbersWindow { get; set; }
        public ICommand OpenUsersWindow { get; set; }

        MainWindow Massage = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

        public MainWindowVM()
        {
            LoadCommands();
            if (string.IsNullOrWhiteSpace(PageName))
            {
                PageName = "Users/Login";
            }
            NavigateToPage(PageName);
            Task.WhenAll(CountEveryThing());
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
                await Task.Run(() =>
                {
                    ServicesCount = db.Services.Count();
                });
                await Task.Run(() =>
                {
                    SuppliersCount = db.Suppliers.Count();
                });
                await Task.Run(() =>
                {
                    CardsCount = db.Items.Where(i => i.Group == ItemGroup.Card).Count();
                });
                await Task.Run(() =>
                {
                    CompaniesCount = db.Companies.Count();
                });
                await Task.Run(() =>
                {
                    SalesMenCount = db.SalesMen.Count();
                });
                await Task.Run(() =>
                {
                    NumbersCount = db.Notes.Count();
                });
                await Task.Run(() =>
                {
                    UsersCount = db.Users.Count();
                });
                await Task.Delay(500);
            }
        }

        public void LoadCommands()
        {
            ChangeSource = new CustomCommand(ChangeCurrentSource, CanChangeCurrentSource);
            OpenItemsWindow = new CustomCommand(DoOpenItemsWindow, CanOpenItemsWindow);
            OpenClientsWindow = new CustomCommand(DoOpenClientsWindow, CanOpenClientsWindow);
            OpenShortagesWindow = new CustomCommand(DoOpenShortagesWindow, CanOpenShortagesWindow);
            OpenServicesWindow = new CustomCommand(DoOpenServicesWindow, CanOpenServicesWindow);
            OpenSuppliersWindow = new CustomCommand(DoOpenSuppliersWindow, CanOpenSuppliersWindow);
            OpenCardsWindow = new CustomCommand(DoOpenCardsWindow, CanOpenCardsWindow);
            OpenCompaniesWindow = new CustomCommand(DoOpenCompaniesWindow, CanOpenCompaniesWindow);
            OpenSalesMenWindow = new CustomCommand(DoOpenSalesMenWindow, CanOpenSalesMenWindow);
            TakeBackup = new CustomCommand(DoTakeBackup, CanTakeBackup);
            RestoreBackup = new CustomCommand(DoRestoreBackup, CanRestoreBackup);
            OpenStoreInfoWindow = new CustomCommand(DoOpenStoreInfoWindow, CanOpenStoreInfoWindow);
            OpenNumbersWindow = new CustomCommand(DoOpenNumbersWindow, CanOpenNumbersWindow);
            OpenUsersWindow = new CustomCommand(DoOpenUsersWindow, CanOpenUsersWindow);
        }

        private bool CanOpenUsersWindow(object obj)
        {
            return true;
        }

        private void DoOpenUsersWindow(object obj)
        {
            new View.Users().Show();
        }

        private bool CanOpenNumbersWindow(object obj)
        {
            return true;
        }

        private void DoOpenNumbersWindow(object obj)
        {
            new Notes().Show();
        }

        private bool CanOpenStoreInfoWindow(object obj)
        {
            return true;
        }

        private void DoOpenStoreInfoWindow(object obj)
        {
            new Stores().Show();
        }

        private bool CanOpenSalesMenWindow(object obj)
        {
            return true;
        }

        private void DoOpenSalesMenWindow(object obj)
        {
            new SalesMen().Show();
        }

        private bool CanRestoreBackup(object obj)
        {
            return true;
        }

        private void DoRestoreBackup(object obj)
        {
            var dlg = new CommonOpenFileDialog();
            dlg.Title = "اختار نسخه احتياطية لاسترجعها";
            dlg.IsFolderPicker = false;
            dlg.InitialDirectory = Properties.Settings.Default.BackUpsFolder;
            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.Filters.Add(new CommonFileDialogFilter("Backup file", "*.bak"));
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["PhonyDbContext"].ConnectionString;
                var backupFolder = ConfigurationManager.AppSettings["BackupFolder"];
                var sqlConStrBuilder = new SqlConnectionStringBuilder(connectionString);
                var database = sqlConStrBuilder.InitialCatalog;
                string query = null;
                using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
                {
                    query = $"ALTER DATABASE [{database}] SET Single_User WITH Rollback Immediate";
                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    query = $"USE master RESTORE DATABASE [{database}] FROM DISK='{dlg.FileName}' WITH  FILE = 1,  NOUNLOAD,  STATS = 10";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    query = $"USE master ALTER DATABASE [{database}] SET Multi_User";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    Massage.ShowMessageAsync("تمت العملية", "تم استرجاع النسخه الاحتياطية بنجاح");
                }
            }
        }

        private bool CanTakeBackup(object obj)
        {
            return true;
        }

        private async void DoTakeBackup(object obj)
        {
            try
            {
                var dlg = new CommonOpenFileDialog();
                dlg.Title = "اختار مكان لحفظ النسخه الاحتياطية";
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = Properties.Settings.Default.BackUpsFolder;
                dlg.AddToMostRecentlyUsedList = false;
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;
                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    Properties.Settings.Default.BackUpsFolder = dlg.FileName;
                    if (!Properties.Settings.Default.BackUpsFolder.EndsWith("\\"))
                    {
                        Properties.Settings.Default.BackUpsFolder += "\\";
                    }
                    Properties.Settings.Default.Save();
                    var connectionString = ConfigurationManager.ConnectionStrings["PhonyDbContext"].ConnectionString;
                    var sqlConStrBuilder = new SqlConnectionStringBuilder(connectionString);
                    var backupFileName = $"{Properties.Settings.Default.BackUpsFolder}{sqlConStrBuilder.InitialCatalog} {DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.bak";
                    using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
                    {
                        var query = $"BACKUP DATABASE [{sqlConStrBuilder.InitialCatalog}] TO DISK='{backupFileName}'";
                        using (var command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            await Massage.ShowMessageAsync("تمت العملية", "تم اخذ نسخه احتياطية بنجاح");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Core.SaveExceptionAsync(ex);
                await Massage.ShowMessageAsync("مشكله", "هناك مشكله فى حفظ النسخه الاحتياطية جرب مكان اخر");
            }
        }

        private bool CanOpenCompaniesWindow(object obj)
        {
            return true;
        }

        private void DoOpenCompaniesWindow(object obj)
        {
            new Companies().Show();
        }

        private bool CanOpenCardsWindow(object obj)
        {
            return true;
        }

        private void DoOpenCardsWindow(object obj)
        {
            new Cards().Show();
        }

        private bool CanOpenSuppliersWindow(object obj)
        {
            return true;
        }

        private void DoOpenSuppliersWindow(object obj)
        {
            new Suppliers().Show();
        }

        private bool CanOpenServicesWindow(object obj)
        {
            return true;
        }

        private void DoOpenServicesWindow(object obj)
        {
            new Services().Show();
        }

        private bool CanOpenShortagesWindow(object obj)
        {
            return true;
        }

        private void DoOpenShortagesWindow(object obj)
        {
            new Shortages().Show();
        }

        private bool CanOpenClientsWindow(object obj)
        {
            return true;
        }

        private void DoOpenClientsWindow(object obj)
        {
            new Clients().Show();
        }

        private void DoOpenItemsWindow(object obj)
        {
            new Items().Show();
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