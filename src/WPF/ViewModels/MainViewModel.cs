using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Phony.Data.Core;
using Phony.Data.Models.Lite;
using Phony.WPF.Data;
using Phony.WPF.Extensions;
using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TinyLittleMvvm;

namespace Phony.WPF.ViewModels
{
    public class MainViewModel : BaseViewModelWithAnnotationValidation, IOnLoadedHandler
    {
        string _userName;
        string _password;
        string _newPassword;
        string _phone;
        string _group;

        bool isBacking;

        private readonly IServiceProvider _serviceProvider;
        private readonly IWindowManager _windowManager;

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

        public ICommand SignOut { get; }
        public ICommand SaveUser { get; }
        public ICommand OpenItemsWindow { get; }
        public ICommand OpenClientsWindow { get; }
        public ICommand OpenBillsWindow { get; }
        public ICommand OpenSalesBillsWindow { get; }
        public ICommand OpenShortagesWindow { get; }
        public ICommand OpenServicesWindow { get; }
        public ICommand OpenSuppliersWindow { get; }
        public ICommand OpenCardsWindow { get; }
        public ICommand OpenCompaniesWindow { get; }
        public ICommand OpenSalesMenWindow { get; }
        public ICommand OpenNumbersWindow { get; }
        public ICommand OpenUsersWindow { get; }
        public ICommand TakeBackup { get; }
        public ICommand RestoreBackup { get; }
        public ICommand OpenStoreInfoWindow { get; }
        public ICommand OpenBarcodesWindow { get; }

        private readonly DbConnectionStringBuilder ConnectionStringBuilder = new();

        public MainViewModel(IServiceProvider serviceProvider, IWindowManager windowManager)
        {
            this._serviceProvider = serviceProvider;
            this._windowManager = windowManager;

            SignOut = new RelayCommand(DoSignOut);
            SaveUser = new AsyncRelayCommand(DoSaveUser, CanSaveUser);
            OpenItemsWindow = new AsyncRelayCommand(DoOpenItemsWindow);
            OpenClientsWindow = new AsyncRelayCommand(DoOpenClientsWindow);
            OpenBillsWindow = new AsyncRelayCommand(DoOpenBillsWindow);
            OpenSalesBillsWindow = new AsyncRelayCommand(DoOpenSalesBillsWindow);
            OpenShortagesWindow = new AsyncRelayCommand(DoOpenShortagesWindow);
            OpenServicesWindow = new AsyncRelayCommand(DoOpenServicesWindow, CanOpenServicesWindow);
            OpenSuppliersWindow = new AsyncRelayCommand(DoOpenSuppliersWindow, CanOpenSuppliersWindow);
            OpenCardsWindow = new AsyncRelayCommand(DoOpenCardsWindow, CanOpenCardsWindow);
            OpenCompaniesWindow = new AsyncRelayCommand(DoOpenCompaniesWindow, CanOpenCompaniesWindow);
            OpenSalesMenWindow = new AsyncRelayCommand(DoOpenSalesMenWindow);
            OpenNumbersWindow = new AsyncRelayCommand(DoOpenNumbersWindow);
            OpenUsersWindow = new AsyncRelayCommand(DoOpenUsersWindow);
            TakeBackup = new AsyncRelayCommand(DoTakeBackup);
            RestoreBackup = new RelayCommand(DoRestoreBackup, CanRestoreBackup);
            OpenStoreInfoWindow = new AsyncRelayCommand(DoOpenStoreInfoWindow, CanOpenStoreInfoWindow);
            OpenBarcodesWindow = new AsyncRelayCommand(DoOpenBarcodesWindow, CanOpenBarcodesWindow);
        }

        public Task OnLoadedAsync()
        {
            UserId = CurrentUser.Id;
            UserName = CurrentUser.Name;
            UserGroup = CurrentUser.Group;
            Group = Enumerations.GetEnumDescription(UserGroup);
            Phone = CurrentUser.Phone;

            return Task.CompletedTask;
        }

        public async Task DoOpenItemsWindow()
        {
            await OpenWindowAsync<ItemsViewModel>("الاصناف");
        }

        public async Task DoOpenClientsWindow()
        {
            await OpenWindowAsync<ClientsViewModel>("العملاء");
        }

        public async Task DoOpenBillsWindow()
        {
            await OpenWindowAsync<BillsViewModel>("فواتير");
        }

        public async Task DoOpenSalesBillsWindow()
        {
            await OpenWindowAsync<SalesBillsViewerViewModel>("طباعة الفواتير");
        }

        public async Task DoOpenShortagesWindow()
        {
            await OpenWindowAsync<ShortagesViewModel>("النواقص");
        }

        public bool CanOpenServicesWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public async Task DoOpenServicesWindow()
        {
            await OpenWindowAsync<ServicesViewModel>("خدمات شركات");
        }

        public bool CanOpenSuppliersWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public async Task DoOpenSuppliersWindow()
        {
            await OpenWindowAsync<SuppliersViewModel>("الموردين");
        }

        public bool CanOpenCardsWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public async Task DoOpenCardsWindow()
        {
            await OpenWindowAsync<CardsViewModel>("كروت الشحن");
        }

        public bool CanOpenCompaniesWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public async Task DoOpenCompaniesWindow()
        {
            await OpenWindowAsync<CompaniesViewModel>("شركات");
        }

        public bool CanOpenSalesMenWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public async Task DoOpenSalesMenWindow()
        {
            await OpenWindowAsync<SalesMenViewModel>("المندوبين");
        }

        public async Task DoOpenNumbersWindow()
        {
            await OpenWindowAsync<NotesViewModel>("ارقام");
        }

        public bool CanOpenUsersWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public async Task DoOpenUsersWindow()
        {
            await OpenWindowAsync<UsersViewModel>("المستخدمين");
        }

        public async Task DoTakeBackup()
        {
            isBacking = true;
            try
            {
                //todo
                //var dlg = new System.Windows.Forms.FolderBrowserDialog
                //{
                //    SelectedPath = Properties.Settings.Default.BackUpsFolder
                //};
                //dlg.ShowDialog();
                //if (string.IsNullOrWhiteSpace(dlg.SelectedPath))
                //{
                //    Properties.Settings.Default.BackUpsFolder = dlg.SelectedPath;
                //    if (!Properties.Settings.Default.BackUpsFolder.EndsWith("\\"))
                //    {
                //        Properties.Settings.Default.BackUpsFolder += "\\";
                //    }
                //    Properties.Settings.Default.Save();
                //    ConnectionStringBuilder.ConnectionString = Properties.Settings.Default.LiteDbConnectionString;
                //    File.Copy(ConnectionStringBuilder["Filename"].ToString(), $"{Properties.Settings.Default.BackUpsFolder}PhonyDbBackup {DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.bak");
                //    await Message.ShowMessageAsync("تمت العملية", "تم اخذ نسخه احتياطية بنجاح");
                //}
            }
            catch (Exception ex)
            {
                await Core.SaveExceptionAsync(ex);
                MessageBox.MaterialMessageBox.ShowError("هناك مشكله فى حفظ النسخه الاحتياطية جرب مكان اخر", "مشكله", true);
            }
            finally
            {
                isBacking = false;
            }
        }

        public bool CanRestoreBackup()
        {
            return UserGroup == UserGroup.Manager;
        }

        public void DoRestoreBackup()
        {
            isBacking = true;
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
                    MessageBox.MaterialMessageBox.Show("تم استرجاع النسخه الاحتياطية بنجاح", "تمت العملية", true);
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
            }
            finally
            {
                isBacking = false;
            }
        }

        public bool CanOpenStoreInfoWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public async Task DoOpenStoreInfoWindow()
        {
            await OpenWindowAsync<StoresViewModel>("بيانات المحل");
        }

        public bool CanOpenBarcodesWindow()
        {
            return UserGroup == UserGroup.Manager;
        }

        public async Task DoOpenBarcodesWindow()
        {
            await OpenWindowAsync<BarcodesViewModel>("باركود");
        }

        public bool CanSaveUser()
        {
            return !string.IsNullOrWhiteSpace(Password);
        }

        private async Task DoSaveUser()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            var userCol = db.GetCollection<User>(DBCollections.Users);
            User u = null;
            await Task.Run(() =>
            {
                u = userCol.Find(x => x.Name == UserName).FirstOrDefault();
            });
            if (u == null)
            {
                MessageBox.MaterialMessageBox.ShowWarning("تاكد من اسم المستخدم و ان كلمه المرور الحاليه صحيحة", "خطا", true);
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
            MessageBox.MaterialMessageBox.Show("تم تعديل بيانات المستخدم بنجاح", "تمت", true);
        }

        public void DoSignOut()
        {
            try
            {
                CurrentUser = new User();
                _windowManager.ShowDialog<LoginViewModel>();
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Checks if the window is already one to focus it or not to open it.
        /// </summary>
        /// <typeparam name="T">the ViewModel for the window.</typeparam>
        /// <param name="title">The window title in the ViewModel.</param>
        /// <returns>The viewmodel for the windowManager to show as window or dialog.</returns>
        private async Task OpenWindowAsync<T>(string title) where T : BaseViewModelWithAnnotationValidation
        {
            if (isBacking)
            {
                return;
            }

            await Task.Delay(5);

            bool windowAlreadyOpenned = false;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Title == title)
                {
                    windowAlreadyOpenned = true;
                    if (window.WindowState == WindowState.Minimized)
                    {
                        window.WindowState = WindowState.Normal;
                    }
                    window.Activate();
                    break;
                }
            }

            if (!windowAlreadyOpenned)
            {
                T w = _serviceProvider.GetRequiredService<T>();

                //send the current logged user to the opened window
                w.CurrentUser.Id = CurrentUser.Id;
                w.CurrentUser.Name = CurrentUser.Name;
                w.CurrentUser.Group = CurrentUser.Group;
                w.CurrentUser.Phone = CurrentUser.Phone;
                w.CurrentUser.Notes = CurrentUser.Notes;
                w.CurrentUser.IsActive = CurrentUser.IsActive;

                _windowManager.ShowWindow(w);
            }
        }
    }
}