using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
using Phony.Extensions;
using Phony.Models;
using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace Phony.ViewModels
{
    class MainPageVM : BindableBase
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

        bool isBacking;

        public int ItemsCount
        {
            get => _itemsCount;
            set => SetProperty(ref _itemsCount, value);
        }

        public int ClientsCount
        {
            get => _clientsCount;
            set => SetProperty(ref _clientsCount, value);
        }

        public int ShortagesCount
        {
            get => _shortagesCount;
            set => SetProperty(ref _shortagesCount, value);
        }

        public int ServicesCount
        {
            get => _servicesCount;
            set => SetProperty(ref _servicesCount, value);
        }

        public int SuppliersCount
        {
            get => _suppliersCount;
            set => SetProperty(ref _suppliersCount, value);
        }

        public int CardsCount
        {
            get => _cardsCount;
            set => SetProperty(ref _cardsCount, value);
        }

        public int CompaniesCount
        {
            get => _companiesCount;
            set => SetProperty(ref _companiesCount, value);
        }

        public int SalesMenCount
        {
            get => _salesMenCount;
            set => SetProperty(ref _salesMenCount, value);
        }

        public int NumbersCount
        {
            get => _numbersCount;
            set => SetProperty(ref _numbersCount, value);
        }

        public int UsersCount
        {
            get => _usersCount;
            set => SetProperty(ref _usersCount, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public string Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
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

        private readonly MainWindow Message = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        private DbConnectionStringBuilder ConnectionStringBuilder = new();
        private readonly MainWindowViewModel v = new();
        private DispatcherTimer Timer = new();

        public MainPageVM()
        {
            LoadCommands();
            Timer.Tick += Timer_Tick;
            Timer.Interval = TimeSpan.FromMilliseconds(500);
            Timer.Start();
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
            var u = db.GetCollection<User>(DBCollections.Users).Find(x => x.Id == Core.ReadUserSession().Id).FirstOrDefault();
            UserName = u.Name;
            Phone = u.Phone;
            Group = Enumerations.GetEnumDescription(u.Group);
        }

        public void LoadCommands()
        {
            OpenItemsWindow = new DelegateCommand(DoOpenItemsWindow, CanOpenItemsWindow);
            OpenClientsWindow = new DelegateCommand(DoOpenClientsWindow, CanOpenClientsWindow);
            OpenBillsWindow = new DelegateCommand(DoOpenBillsWindow, CanOpenBillsWindow);
            OpenSalesBillsWindow = new DelegateCommand(DoOpenSalesBillsWindow, CanOpenSalesBillsWindow);
            OpenShortagesWindow = new DelegateCommand(DoOpenShortagesWindow, CanOpenShortagesWindow);
            OpenServicesWindow = new DelegateCommand(DoOpenServicesWindow, CanOpenServicesWindow);
            OpenSuppliersWindow = new DelegateCommand(DoOpenSuppliersWindow, CanOpenSuppliersWindow);
            OpenCardsWindow = new DelegateCommand(DoOpenCardsWindow, CanOpenCardsWindow);
            OpenCompaniesWindow = new DelegateCommand(DoOpenCompaniesWindow, CanOpenCompaniesWindow);
            OpenSalesMenWindow = new DelegateCommand(DoOpenSalesMenWindow, CanOpenSalesMenWindow);
            TakeBackup = new DelegateCommand(DoTakeBackup, CanTakeBackup);
            RestoreBackup = new DelegateCommand(DoRestoreBackup, CanRestoreBackup);
            OpenStoreInfoWindow = new DelegateCommand(DoOpenStoreInfoWindow, CanOpenStoreInfoWindow);
            OpenNumbersWindow = new DelegateCommand(DoOpenNumbersWindow, CanOpenNumbersWindow);
            OpenUsersWindow = new DelegateCommand(DoOpenUsersWindow, CanOpenUsersWindow);
            OpenBarcodesWindow = new DelegateCommand(DoOpenBarcodesWindow, CanOpenBarcodesWindow);
            SignOut = new DelegateCommand(DoSignOut, CanSignOut);
            SaveUser = new DelegateCommand(DoSaveUser, CanSaveUser);
        }

        async Task CountEveryThing()
        {
            if (isBacking)
            {
                return;
            }
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
            try
            {
                await Task.Run(() =>
                {
                    ItemsCount = db.GetCollection<Item>(DBCollections.Items).Count(x => x.Group == ItemGroup.Other);
                });
                await Task.Run(() =>
                {
                    ClientsCount = db.GetCollection<Client>(DBCollections.Clients.ToString()).Count();
                });
                await Task.Run(() =>
                {
                    ShortagesCount = db.GetCollection<Item>(DBCollections.Items).Count(x => x.QTY == 0);
                });
                await Task.Run(() =>
                {
                    ServicesCount = db.GetCollection<Service>(DBCollections.Services).Count();
                });
                await Task.Run(() =>
                {
                    SuppliersCount = db.GetCollection<Supplier>(DBCollections.Suppliers).Count();
                });
                await Task.Run(() =>
                {
                    CardsCount = db.GetCollection<Item>(DBCollections.Items).Count(x => x.Group == ItemGroup.Card);
                });
                await Task.Run(() =>
                {
                    CompaniesCount = db.GetCollection<Company>(DBCollections.Companies).Count();
                });
                await Task.Run(() =>
                {
                    SalesMenCount = db.GetCollection<SalesMan>(DBCollections.SalesMen).Count();
                });
                await Task.Run(() =>
                {
                    NumbersCount = db.GetCollection<Note>(DBCollections.Notes).Count();
                });
                await Task.Run(() =>
                {
                    UsersCount = db.GetCollection<User>(DBCollections.Users).Count();
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            await CountEveryThing();
        }

        private bool CanOpenBarcodesWindow()
        {
            return Core.ReadUserSession().Group == UserGroup.Manager;
        }

        private void DoOpenBarcodesWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<BarCodes>().Count();
            if (opened == 0)
            {
                new BarCodes().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<BarCodes>().FirstOrDefault().Activate();
            }
        }

        private bool CanSaveUser()
        {
            return !string.IsNullOrWhiteSpace(Password);
        }

        private async void DoSaveUser()
        {
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
            var userCol = db.GetCollection<User>(DBCollections.Users);
            User u = null;
            await Task.Run(() =>
            {
                u = userCol.Find(x => x.Name == UserName).FirstOrDefault();
            });
            if (u is null)
            {
                await Message.ShowMessageAsync("خطا", "تاكد من اسم المستخدم و ان كلمه المرور الحاليه صحيحة");
            }
            else if (string.IsNullOrWhiteSpace(NewPassword))
            {
                u.Name = UserName;
                u.Phone = Phone;
            }
            else
            {
                if (SecurePasswordHasher.Verify(Password, u.Pass))
                {
                    u.Name = UserName;
                    u.Pass = SecurePasswordHasher.Hash(NewPassword);
                    u.Phone = Phone;
                }
            }
            userCol.Update(u);
            Password = null;
            NewPassword = null;
            await Message.ShowMessageAsync("تمت", "تم تعديل بيانات المستخدم بنجاح");
        }

        private bool CanSignOut()
        {
            return true;
        }

        private void DoSignOut()
        {
            try
            {
                File.Delete(Core.UserLocalAppFolderPath() + "..\\..\\session");
                v.PageName = "Login";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Environment.Exit(0);
            }
        }

        private bool CanOpenSalesBillsWindow()
        {
            return true;
        }

        private void DoOpenSalesBillsWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<SalesBillsViewer>().Count();
            if (opened == 0)
            {
                new SalesBillsViewer().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<SalesBillsViewer>().FirstOrDefault().Activate();
            }
        }

        private bool CanOpenBillsWindow()
        {
            return true;
        }

        private void DoOpenBillsWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<Bills>().Count();
            if (opened == 0)
            {
                new Bills().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<Bills>().FirstOrDefault().Activate();
            }
        }

        private bool CanOpenUsersWindow()
        {
            return Core.ReadUserSession().Group == UserGroup.Manager;
        }

        private void DoOpenUsersWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<Users>().Count();
            if (opened == 0)
            {
                new Users().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<Users>().FirstOrDefault().Activate();
            }
        }

        private bool CanOpenNumbersWindow()
        {
            return true;
        }

        private void DoOpenNumbersWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<Notes>().Count();
            if (opened == 0)
            {
                new Notes().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<Notes>().FirstOrDefault().Activate();
            }
        }

        private bool CanOpenStoreInfoWindow()
        {
            return Core.ReadUserSession().Group == UserGroup.Manager;
        }

        private void DoOpenStoreInfoWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<Stores>().Count();
            if (opened == 0)
            {
                new Stores().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<Stores>().FirstOrDefault().Activate();
            }
        }

        private bool CanOpenSalesMenWindow()
        {
            return Core.ReadUserSession().Group == UserGroup.Manager;
        }

        private void DoOpenSalesMenWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<SalesMen>().Count();
            if (opened == 0)
            {
                new SalesMen().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<SalesMen>().FirstOrDefault().Activate();
            }
        }

        private bool CanRestoreBackup()
        {
            return Core.ReadUserSession().Group == UserGroup.Manager;
        }

        private async void DoRestoreBackup()
        {
            isBacking = true;
            var progressbar = await Message.ShowProgressAsync("استرجع نسخه احتياطية", "جارى استعادة نسخه احتياطية الان");
            progressbar.SetIndeterminate();
            try
            {
                var dlg = new Microsoft.Win32.OpenFileDialog
                {
                    Title = "اختار نسخه احتياطية لاسترجعها",
                    InitialDirectory = Properties.Settings.Default.BackUpsFolder,
                    Filter = "Backup file|*.bak",
                    CheckFileExists = true
                };
                if (dlg.ShowDialog() == true)
                {
                    ConnectionStringBuilder.ConnectionString = Properties.Settings.Default.LiteDbConnectionString;
                    File.Copy(dlg.FileName, ConnectionStringBuilder["Filename"].ToString(), true);
                    await Message.ShowMessageAsync("تمت العملية", "تم استرجاع النسخه الاحتياطية بنجاح");
                }
            }
            catch (Exception ex)
            {
                await progressbar.CloseAsync();
                Core.SaveException(ex);
            }
            finally
            {
                if (progressbar.IsOpen)
                {
                    await progressbar.CloseAsync();
                }
                isBacking = false;
            }
        }

        private bool CanTakeBackup()
        {
            return true;
        }

        private async void DoTakeBackup()
        {
            isBacking = true;
            var progressbar = await Message.ShowProgressAsync("اخذ نسخه احتياطية", "جارى اخذ نسخه احتياطية الان");
            progressbar.SetIndeterminate();
            try
            {
                var dlg = new FolderBrowserDialog
                {
                    Description = "اختار مكان لحفظ النسخه الاحتياطية",
                    InitialDirectory = Properties.Settings.Default.BackUpsFolder,
                };
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default.BackUpsFolder = dlg.SelectedPath;
                    if (!Properties.Settings.Default.BackUpsFolder.EndsWith("\\"))
                    {
                        Properties.Settings.Default.BackUpsFolder += "\\";
                    }
                    Properties.Settings.Default.Save();
                    ConnectionStringBuilder.ConnectionString = Properties.Settings.Default.LiteDbConnectionString;
                    File.Copy(ConnectionStringBuilder["Filename"].ToString(), $"{Properties.Settings.Default.BackUpsFolder}PhonyDbBackup {DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.bak");
                    await Message.ShowMessageAsync("تمت العملية", "تم اخذ نسخه احتياطية بنجاح");
                }
            }
            catch (Exception ex)
            {
                await progressbar.CloseAsync();
                await Core.SaveExceptionAsync(ex);
                await Message.ShowMessageAsync("مشكله", "هناك مشكله فى حفظ النسخه الاحتياطية جرب مكان اخر");
            }
            finally
            {
                if (progressbar.IsOpen)
                {
                    await progressbar.CloseAsync();
                }
                isBacking = false;
            }
        }

        private bool CanOpenCompaniesWindow()
        {
            return Core.ReadUserSession().Group == UserGroup.Manager;
        }

        private void DoOpenCompaniesWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<Companies>().Count();
            if (opened == 0)
            {
                new Companies().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<Companies>().FirstOrDefault().Activate();
            }
        }

        private bool CanOpenCardsWindow()
        {
            return Core.ReadUserSession().Group == UserGroup.Manager;
        }

        private void DoOpenCardsWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<Cards>().Count();
            if (opened == 0)
            {
                new Cards().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<Cards>().FirstOrDefault().Activate();
            }
        }

        private bool CanOpenSuppliersWindow()
        {
            return Core.ReadUserSession().Group == UserGroup.Manager;
        }

        private void DoOpenSuppliersWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<Suppliers>().Count();
            if (opened == 0)
            {
                new Suppliers().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<Suppliers>().FirstOrDefault().Activate();
            }
        }

        private bool CanOpenServicesWindow()
        {
            return Core.ReadUserSession().Group == UserGroup.Manager;
        }

        private void DoOpenServicesWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<Services>().Count();
            if (opened == 0)
            {
                new Services().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<Services>().FirstOrDefault().Activate();
            }
        }

        private bool CanOpenShortagesWindow()
        {
            return true;
        }

        private void DoOpenShortagesWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<Shortages>().Count();
            if (opened == 0)
            {
                new Shortages().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<Shortages>().FirstOrDefault().Activate();
            }
        }

        private bool CanOpenClientsWindow()
        {
            return true;
        }

        private void DoOpenClientsWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<Clients>().Count();
            if (opened == 0)
            {
                new Clients().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<Clients>().FirstOrDefault().Activate();
            }
        }

        private bool CanOpenItemsWindow()
        {
            return true;
        }

        private void DoOpenItemsWindow()
        {
            var opened = System.Windows.Application.Current.Windows.OfType<Items>().Count();
            if (opened == 0)
            {
                new Items().Show();
            }
            else
            {
                System.Windows.Application.Current.Windows.OfType<Items>().FirstOrDefault().Activate();
            }
        }
    }
}