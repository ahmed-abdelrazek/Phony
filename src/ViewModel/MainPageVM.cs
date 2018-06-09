using MahApps.Metro.Controls.Dialogs;
using Microsoft.WindowsAPICodePack.Dialogs;
using Phony.Extensions;
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
using System.Windows.Threading;

namespace Phony.ViewModel
{
    class MainPageVM : CommonBase
    {
        int _itemsCount;
        int _clientsCount;
        int _shortagesCount;
        int _servicesCount;
        int _suppliersCount;
        int _cardsCount;
        int _companiesCount;
        int _salesMenCount;
        int _numbersCount;
        int _usersCount;
        string _userName;
        string _password;
        string _newPassword;
        string _phone;
        string _group;

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

        public string UserName
        {
            get => _userName;
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (value != _password)
                {
                    _password = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if (value != _newPassword)
                {
                    _newPassword = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (value != _phone)
                {
                    _phone = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Group
        {
            get => _group;
            set
            {
                if (value != _group)
                {
                    _group = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand ChangeSource { get; set; }
        public ICommand OpenItemsWindow { get; set; }
        public ICommand OpenClientsWindow { get; set; }
        public ICommand OpenBillsWindow { get; set; }
        public ICommand OpenSalesBillsWindow { get; set; }
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
        public ICommand OpenBarcodesWindow { get; set; }
        public ICommand SignOut { get; set; }
        public ICommand SaveUser { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        MainWindowVM v = new MainWindowVM();

        MainWindow Message = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

        DispatcherTimer Timer = new DispatcherTimer();

        public MainPageVM()
        {
            LoadCommands();
            Timer.Tick += Timer_Tick;
            Timer.Interval = TimeSpan.FromMilliseconds(500);
            Timer.Start();
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var u = db.Users.Get(CurrentUser.Id);
                UserName = u.Name;
                Phone = u.Phone;
                Group = Enumerations.GetEnumDescription((UserGroup)u.Group);
            }
        }

        public void LoadCommands()
        {
            OpenItemsWindow = new CustomCommand(DoOpenItemsWindow, CanOpenItemsWindow);
            OpenClientsWindow = new CustomCommand(DoOpenClientsWindow, CanOpenClientsWindow);
            OpenBillsWindow = new CustomCommand(DoOpenBillsWindow, CanOpenBillsWindow);
            OpenSalesBillsWindow = new CustomCommand(DoOpenSalesBillsWindow, CanOpenSalesBillsWindow);
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
            OpenBarcodesWindow = new CustomCommand(DoOpenBarcodesWindow, CanOpenBarcodesWindow);
            SignOut = new CustomCommand(DoSignOut, CanSignOut);
            SaveUser = new CustomCommand(DoSaveUser, CanSaveUser);
        }

        async Task CountEveryThing()
        {
            using (var db = new PhonyDbContext())
            {
                try
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
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Contains("A network-related or instance-specific error occurred while establishing a connection to SQL Server"))
                    {
                        BespokeFusion.MaterialMessageBox.Show("البرنامج لا يستطيع الاتصال بقاعده البيانات لسبب ما تاكد من اتصالك");
                    }
                }
            }
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            await CountEveryThing();
        }

        private bool CanOpenBarcodesWindow(object obj)
        {
            if (CurrentUser.Group == UserGroup.Manager)
            {
                return true;
            }
            return false;
        }

        private void DoOpenBarcodesWindow(object obj)
        {
            new BarCodes().Show();
        }

        private bool CanSaveUser(object obj)
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                return false;
            }
            return true;
        }

        private async void DoSaveUser(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                Model.User u = null;
                await Task.Run(() =>
                {
                    u = db.Users.GetLoginCredentials(UserName, Password);
                });
                if (u == null)
                {
                    await Message.ShowMessageAsync("خطا", "تاكد ان كلمه المرور الحاليه صحيحة");
                }
                else if (string.IsNullOrWhiteSpace(NewPassword))
                {
                    u.Name = UserName;
                    u.Phone = Phone;
                }
                else
                {
                    u.Name = UserName;
                    u.Pass = SecurePasswordHasher.Hash(NewPassword);
                    u.Phone = Phone;
                }
                await db.CompleteAsync();
                Password = null;
                NewPassword = null;
                await Message.ShowMessageAsync("تمت", "تم تعديل بيانات المستخدم بنجاح");
            }
        }

        private bool CanSignOut(object obj)
        {
            return true;
        }

        private void DoSignOut(object obj)
        {
            CurrentUser.Id = 0;
            CurrentUser.Name = null;
            CurrentUser.SecurePassword = null;
            CurrentUser.Group = UserGroup.None;
            v.PageName = "Users/Login";
        }

        private bool CanOpenSalesBillsWindow(object obj)
        {
            return true;
        }

        private void DoOpenSalesBillsWindow(object obj)
        {
            new SalesBillsViewer().Show();
        }

        private bool CanOpenBillsWindow(object obj)
        {
            return true;
        }

        private void DoOpenBillsWindow(object obj)
        {
            new Bills().Show();
        }

        private bool CanOpenUsersWindow(object obj)
        {
            if (CurrentUser.Group == UserGroup.Manager)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            if (CurrentUser.Group == UserGroup.Manager)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DoOpenStoreInfoWindow(object obj)
        {
            new Stores().Show();
        }

        private bool CanOpenSalesMenWindow(object obj)
        {
            if (CurrentUser.Group == UserGroup.Manager)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DoOpenSalesMenWindow(object obj)
        {
            new SalesMen().Show();
        }

        private bool CanRestoreBackup(object obj)
        {
            if (CurrentUser.Group == UserGroup.Manager)
            {
                return true;
            }
            else
            {
                return false;
            }
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
                    Message.ShowMessageAsync("تمت العملية", "تم استرجاع النسخه الاحتياطية بنجاح");
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
                            await Message.ShowMessageAsync("تمت العملية", "تم اخذ نسخه احتياطية بنجاح");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Core.SaveExceptionAsync(ex);
                await Message.ShowMessageAsync("مشكله", "هناك مشكله فى حفظ النسخه الاحتياطية جرب مكان اخر");
            }
        }

        private bool CanOpenCompaniesWindow(object obj)
        {
            if (CurrentUser.Group == UserGroup.Manager)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DoOpenCompaniesWindow(object obj)
        {
            new Companies().Show();
        }

        private bool CanOpenCardsWindow(object obj)
        {
            if (CurrentUser.Group == UserGroup.Manager)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DoOpenCardsWindow(object obj)
        {
            new Cards().Show();
        }

        private bool CanOpenSuppliersWindow(object obj)
        {
            if (CurrentUser.Group == UserGroup.Manager)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DoOpenSuppliersWindow(object obj)
        {
            new Suppliers().Show();
        }

        private bool CanOpenServicesWindow(object obj)
        {
            if (CurrentUser.Group == UserGroup.Manager)
            {
                return true;
            }
            else
            {
                return false;
            }
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
    }
}