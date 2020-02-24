using Caliburn.Micro;
using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Phony.WPF.Data;
using Phony.WPF.Extensions;
using Phony.WPF.Models;
using Phony.WPF.Views;
using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Phony.WPF.ViewModels
{
    public class MainViewModel : Screen, IHandle<User>
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

        SimpleContainer _container;
        IEventAggregator _events;

        public int ItemsCount
        {
            get => _itemsCount;
            set
            {
                _itemsCount = value;
                NotifyOfPropertyChange(() => ItemsCount);
            }
        }

        public int ClientsCount
        {
            get => _clientsCount;
            set
            {
                _clientsCount = value;
                NotifyOfPropertyChange(() => ClientsCount);
            }
        }

        public int ShortagesCount
        {
            get => _shortagesCount;
            set
            {
                _shortagesCount = value;
                NotifyOfPropertyChange(() => ShortagesCount);
            }
        }

        public int ServicesCount
        {
            get => _servicesCount;
            set
            {
                _servicesCount = value;
                NotifyOfPropertyChange(() => ServicesCount);
            }
        }

        public int SuppliersCount
        {
            get => _suppliersCount;
            set
            {
                _suppliersCount = value;
                NotifyOfPropertyChange(() => SuppliersCount);
            }
        }

        public int CardsCount
        {
            get => _cardsCount;
            set
            {
                _cardsCount = value;
                NotifyOfPropertyChange(() => CardsCount);
            }
        }

        public int CompaniesCount
        {
            get => _companiesCount;
            set
            {
                _companiesCount = value;
                NotifyOfPropertyChange(() => CompaniesCount);
            }
        }

        public int SalesMenCount
        {
            get => _salesMenCount;
            set
            {
                _salesMenCount = value;
                NotifyOfPropertyChange(() => SalesMenCount);
            }
        }

        public int NumbersCount
        {
            get => _numbersCount;
            set
            {
                _numbersCount = value;
                NotifyOfPropertyChange(() => NumbersCount);
            }
        }

        public int UsersCount
        {
            get => _usersCount;
            set
            {
                _usersCount = value;
                NotifyOfPropertyChange(() => UsersCount);
            }
        }

        public int UserId { get; set; }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                NotifyOfPropertyChange(() => NewPassword);
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                NotifyOfPropertyChange(() => Phone);
            }
        }

        public UserGroup UserGroup { get; set; }

        public string Group
        {
            get => _group;
            set
            {
                _group = value;
                NotifyOfPropertyChange(() => Group);
            }
        }

        ShellView Message = Application.Current.Windows.OfType<ShellView>().FirstOrDefault();
        DbConnectionStringBuilder ConnectionStringBuilder = new DbConnectionStringBuilder();

        DispatcherTimer Timer = new DispatcherTimer();

        public MainViewModel(IEventAggregator events, SimpleContainer container)
        {
            _container = container;
            _events = events;
            _events.SubscribeOnPublishedThread(this);
            Timer.Tick += Timer_Tick;
            Timer.Interval = TimeSpan.FromMilliseconds(500);
            Timer.Start();
        }

        async Task CountEveryThing()
        {
            if (isBacking)
            {
                return;
            }
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
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
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            //await CountEveryThing();
        }

        public void OpenItemsWindow()
        {
            var opened = Application.Current.Windows.OfType<Items>().Count();
            if (opened == 0)
            {
                new Items().Show();
            }
            else
            {
                Application.Current.Windows.OfType<Items>().FirstOrDefault().Activate();
            }
        }

        public void OpenClientsWindow()
        {
            var opened = Application.Current.Windows.OfType<Clients>().Count();
            if (opened == 0)
            {
                new Clients().Show();
            }
            else
            {
                Application.Current.Windows.OfType<Clients>().FirstOrDefault().Activate();
            }
        }

        public void OpenBillsWindow()
        {
            var opened = Application.Current.Windows.OfType<Bills>().Count();
            if (opened == 0)
            {
                new Bills().Show();
            }
            else
            {
                Application.Current.Windows.OfType<Bills>().FirstOrDefault().Activate();
            }
        }

        public void OpenSalesBillsWindow()
        {
            var opened = Application.Current.Windows.OfType<SalesBillsViewer>().Count();
            if (opened == 0)
            {
                new SalesBillsViewer().Show();
            }
            else
            {
                Application.Current.Windows.OfType<SalesBillsViewer>().FirstOrDefault().Activate();
            }
        }

        public void OpenShortagesWindow()
        {
            var opened = Application.Current.Windows.OfType<Shortages>().Count();
            if (opened == 0)
            {
                new Shortages().Show();
            }
            else
            {
                Application.Current.Windows.OfType<Shortages>().FirstOrDefault().Activate();
            }
        }

        public bool CanOpenServicesWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public void OpenServicesWindow()
        {
            var opened = Application.Current.Windows.OfType<Services>().Count();
            if (opened == 0)
            {
                new Services().Show();
            }
            else
            {
                Application.Current.Windows.OfType<Services>().FirstOrDefault().Activate();
            }
        }

        public bool CanOpenSuppliersWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public void OpenSuppliersWindow()
        {
            var opened = Application.Current.Windows.OfType<Suppliers>().Count();
            if (opened == 0)
            {
                new Suppliers().Show();
            }
            else
            {
                Application.Current.Windows.OfType<Suppliers>().FirstOrDefault().Activate();
            }
        }

        public bool CanOpenCardsWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public void OpenCardsWindow()
        {
            var opened = Application.Current.Windows.OfType<Cards>().Count();
            if (opened == 0)
            {
                new Cards().Show();
            }
            else
            {
                Application.Current.Windows.OfType<Cards>().FirstOrDefault().Activate();
            }
        }

        public bool CanOpenCompaniesWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public void OpenCompaniesWindow()
        {
            var opened = Application.Current.Windows.OfType<Companies>().Count();
            if (opened == 0)
            {
                new Companies().Show();
            }
            else
            {
                Application.Current.Windows.OfType<Companies>().FirstOrDefault().Activate();
            }
        }

        public bool CanOpenSalesMenWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public void OpenSalesMenWindow()
        {
            var opened = Application.Current.Windows.OfType<SalesMen>().Count();
            if (opened == 0)
            {
                new SalesMen().Show();
            }
            else
            {
                Application.Current.Windows.OfType<SalesMen>().FirstOrDefault().Activate();
            }
        }

        public void OpenNumbersWindow()
        {
            var opened = Application.Current.Windows.OfType<Notes>().Count();
            if (opened == 0)
            {
                new Notes().Show();
            }
            else
            {
                Application.Current.Windows.OfType<Notes>().FirstOrDefault().Activate();
            }
        }

        public bool CanOpenUsersWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public void OpenUsersWindow()
        {
            var opened = Application.Current.Windows.OfType<Users>().Count();
            if (opened == 0)
            {
                new Users().Show();
                _container.GetInstance<Users>().Show();
            }
            else
            {
                Application.Current.Windows.OfType<Users>().FirstOrDefault().Activate();
            }
        }

        public async void TakeBackup()
        {
            isBacking = true;
            var progressbar = await Message.ShowProgressAsync("اخذ نسخه احتياطية", "جارى اخذ نسخه احتياطية الان");
            progressbar.SetIndeterminate();
            try
            {
                var dlg = new System.Windows.Forms.FolderBrowserDialog
                {
                    SelectedPath = Properties.Settings.Default.BackUpsFolder
                };
                dlg.ShowDialog();
                if (string.IsNullOrWhiteSpace(dlg.SelectedPath))
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

        public bool CanRestoreBackup()
        {
            return UserGroup == UserGroup.Manager;
        }

        public async void RestoreBackup()
        {
            isBacking = true;
            var progressbar = await Message.ShowProgressAsync("استرجع نسخه احتياطية", "جارى استعادة نسخه احتياطية الان");
            progressbar.SetIndeterminate();
            try
            {
                var dlg = new OpenFileDialog
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

        public bool CanOpenStoreInfoWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public void OpenStoreInfoWindow()
        {
            var opened = Application.Current.Windows.OfType<Stores>().Count();
            if (opened == 0)
            {
                new Stores().Show();
            }
            else
            {
                Application.Current.Windows.OfType<Stores>().FirstOrDefault().Activate();
            }
        }

        public bool CanOpenBarcodesWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public void DoOpenBarcodesWindow()
        {
            var opened = Application.Current.Windows.OfType<BarCodes>().Count();
            if (opened == 0)
            {
                new BarCodes().Show();
            }
            else
            {
                Application.Current.Windows.OfType<BarCodes>().FirstOrDefault().Activate();
            }
        }

        public bool CanSaveUser()
        {
            return !string.IsNullOrWhiteSpace(Password);
        }

        public void SignOut()
        {
            try
            {
                File.Delete(Core.UserLocalAppFolderPath() + "..\\..\\session");
                _container.GetInstance<LoginViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Environment.Exit(0);
            }
        }

        public async Task HandleAsync(User message, CancellationToken cancellationToken)
        {
            UserId = message.Id;
            UserName = message.Name;
            UserGroup = message.Group;
            Group = Enumerations.GetEnumDescription(UserGroup);
            Phone = message.Phone;
            await Task.Delay(20);
        }
    }
}