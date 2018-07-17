using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Models;
using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class ServiceVM : BindableBase
    {
        long _serviceId;
        string _name;
        string _site;
        string _email;
        string _searchText;
        string _phone;
        string _notes;
        static string _servicesCount;
        static string _servicesPurchasePrice;
        static string _servicesSalePrice;
        static string _servicesProfit;
        byte[] _image;
        decimal _balance;
        bool _isServiceFlyoutOpen;
        Service _dataGridSelectedService;
        ObservableCollection<Service> _services;

        public long ServiceId
        {
            get => _serviceId;
            set
            {
                if (value != _serviceId)
                {
                    _serviceId = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Site
        {
            get => _site;
            set
            {
                if (value != _site)
                {
                    _site = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (value != _email)
                {
                    _email = value;
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

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (value != _searchText)
                {
                    _searchText = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (value != _notes)
                {
                    _notes = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ServicesCount
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

        public string ServicesPurchasePrice
        {
            get => _servicesPurchasePrice;
            set
            {
                if (value != _servicesPurchasePrice)
                {
                    _servicesPurchasePrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ServicesSalePrice
        {
            get => _servicesSalePrice;
            set
            {
                if (value != _servicesSalePrice)
                {
                    _servicesSalePrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ServicesProfit
        {
            get => _servicesProfit;
            set
            {
                if (value != _servicesProfit)
                {
                    _servicesProfit = value;
                    RaisePropertyChanged();
                }
            }
        }

        public byte[] Image
        {
            get => _image;
            set
            {
                if (value != _image)
                {
                    _image = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal Balance
        {
            get => _balance;
            set
            {
                if (value != _balance)
                {
                    _balance = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsAddServiceFlyoutOpen
        {
            get => _isServiceFlyoutOpen;
            set
            {
                if (value != _isServiceFlyoutOpen)
                {
                    _isServiceFlyoutOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Service DataGridSelectedService
        {
            get => _dataGridSelectedService;
            set
            {
                if (value != _dataGridSelectedService)
                {
                    _dataGridSelectedService = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<Service> Services
        {
            get => _services;
            set
            {
                if (value != _services)
                {
                    _services = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<User> Users { get; set; }

        public ICommand AddBalance { get; set; }
        public ICommand AddService { get; set; }
        public ICommand EditService { get; set; }
        public ICommand DeleteService { get; set; }
        public ICommand OpenAddServiceFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand ReloadAllServices { get; set; }
        public ICommand Search { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        Services ServicesMessage = Application.Current.Windows.OfType<Services>().FirstOrDefault();

        public ServiceVM()
        {
            LoadCommands();
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Services = new ObservableCollection<Service>(db.GetCollection<Service>(DBCollections.Services.ToString()).FindAll());
                Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users.ToString()).FindAll());
            }
            DebitCredit();
        }

        public void LoadCommands()
        {
            OpenAddServiceFlyout = new DelegateCommand(DoOpenAddServiceFlyout, CanOpenAddServiceFlyout);
            SelectImage = new DelegateCommand(DoSelectImage, CanSelectImage);
            FillUI = new DelegateCommand(DoFillUI, CanFillUI);
            DeleteService = new DelegateCommand(DoDeleteService, CanDeleteService);
            ReloadAllServices = new DelegateCommand(DoReloadAllServices, CanReloadAllServices);
            Search = new DelegateCommand(DoSearch, CanSearch);
            AddService = new DelegateCommand(DoAddService, CanAddService);
            EditService = new DelegateCommand(DoEditService, CanEditService);
            AddBalance = new DelegateCommand(DoAddBalance, CanAddBalance);
        }

        async void DebitCredit()
        {
            decimal Debit = decimal.Round(Services.Where(c => c.Balance > 0).Sum(i => i.Balance), 2);
            await Task.Run(() =>
            {
                ServicesCount = $"مجموع الخدمات: {Services.Count().ToString()}";
            });
            await Task.Run(() =>
            {
                ServicesPurchasePrice = $"اجمالى لينا: {Math.Abs(Debit).ToString()}";
            });
        }

        private bool CanAddBalance()
        {
            if (DataGridSelectedService == null)
            {
                return false;
            }
            return true;
        }

        private async void DoAddBalance()
        {
            var result = await ServicesMessage.ShowInputAsync("شحن", $"ادخل المبلغ الذى تريد شحن الخدمه به {DataGridSelectedService.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await ServicesMessage.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لاضافته للرصيد");
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal servicepaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                    {
                        var s = db.GetCollection<Service>(DBCollections.Services.ToString()).FindById(DataGridSelectedService.Id);
                        s.Balance += servicepaymentamount;
                        db.GetCollection<Service>(DBCollections.Services.ToString()).Update(s);
                        db.GetCollection<ServiceMove>(DBCollections.ServicesMoves.ToString()).Insert(new ServiceMove
                        {
                            Service = db.GetCollection<Service>(DBCollections.Services.ToString()).FindById(DataGridSelectedService.Id),
                            Debit = servicepaymentamount,
                            CreateDate = DateTime.Now,
                            Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                            EditDate = null,
                            Editor = null
                        });
                        await ServicesMessage.ShowMessageAsync("تمت العملية", $"تم شحن خدمة {DataGridSelectedService.Name} بمبلغ {servicepaymentamount} جنية بنجاح");
                        Services[Services.IndexOf(DataGridSelectedService)] = s;
                        DebitCredit();
                        DataGridSelectedService = null;
                        ServiceId = 0;
                    }
                }
                else
                {
                    await ServicesMessage.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanAddService()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }

        private void DoAddService()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var exist = db.GetCollection<Service>(DBCollections.Services.ToString()).Find(x => x.Name == Name).FirstOrDefault();
                if (exist == null)
                {
                    var s = new Service
                    {
                        Name = Name,
                        Balance = Balance,
                        Site = Site,
                        Email = Email,
                        Phone = Phone,
                        Image = Image,
                        Notes = Notes,
                        CreateDate = DateTime.Now,
                        Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                        EditDate = null,
                        Editor = null
                    };
                    db.GetCollection<Service>(DBCollections.Services.ToString()).Insert(s);
                    Services.Add(s);
                    ServicesMessage.ShowMessageAsync("تمت العملية", "تم اضافة الخدمة بنجاح");
                    DebitCredit();
                }
                else
                {
                    ServicesMessage.ShowMessageAsync("موجود", "الخدمة موجودة من قبل بالفعل");
                }
            }
        }

        private bool CanEditService()
        {
            if (string.IsNullOrWhiteSpace(Name) || ServiceId == 0 || DataGridSelectedService == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditService()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var s = db.GetCollection<Service>(DBCollections.Services.ToString()).FindById(DataGridSelectedService.Id);
                s.Name = Name;
                s.Balance = Balance;
                s.Site = Site;
                s.Email = Email;
                s.Phone = Phone;
                s.Image = Image;
                s.Notes = Notes;
                s.EditDate = DateTime.Now;
                s.Editor = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id);
                db.GetCollection<Service>(DBCollections.Services.ToString()).Update(s);
                ServicesMessage.ShowMessageAsync("تمت العملية", "تم تعديل الخدمة بنجاح");
                Services[Services.IndexOf(DataGridSelectedService)] = s;
                DebitCredit();
                DataGridSelectedService = null;
                ServiceId = 0;
            }
        }

        private bool CanDeleteService()
        {
            if (DataGridSelectedService == null || DataGridSelectedService.Id == 1)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteService()
        {
            var result = await ServicesMessage.ShowMessageAsync("حذف الخدمة", $"هل انت متاكد من حذف الخدمة {DataGridSelectedService.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    db.GetCollection<Service>(DBCollections.Services.ToString()).Delete(DataGridSelectedService.Id);
                    Services.Remove(DataGridSelectedService);
                }
                await ServicesMessage.ShowMessageAsync("تمت العملية", "تم حذف الخدمة بنجاح");
                DebitCredit();
                DataGridSelectedService = null;
            }
        }

        private bool CanSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return false;
            }
            return true;
        }

        private void DoSearch()
        {
            try
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    Services = new ObservableCollection<Service>(db.GetCollection<Service>(DBCollections.Services.ToString()).Find(x => x.Name.Contains(SearchText)));
                    if (Services.Count < 1)
                    {
                        ServicesMessage.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                    }
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله");
            }
        }

        private bool CanReloadAllServices()
        {
            return true;
        }

        private void DoReloadAllServices()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Services = new ObservableCollection<Service>(db.GetCollection<Service>(DBCollections.Services.ToString()).FindAll());
            }
            DebitCredit();
        }

        private bool CanFillUI()
        {
            if (DataGridSelectedService == null)
            {
                return false;
            }
            return true;
        }

        private void DoFillUI()
        {
            ServiceId = DataGridSelectedService.Id;
            Name = DataGridSelectedService.Name;
            Balance = DataGridSelectedService.Balance;
            Site = DataGridSelectedService.Site;
            Image = DataGridSelectedService.Image;
            Email = DataGridSelectedService.Email;
            Phone = DataGridSelectedService.Phone;
            Notes = DataGridSelectedService.Notes;
            IsAddServiceFlyoutOpen = true;
        }

        private bool CanSelectImage()
        {
            return true;
        }

        private void DoSelectImage()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            var codecs = ImageCodecInfo.GetImageEncoders();
            dlg.Filter = string.Format("All image files ({1})|{1}|All files|*",
            string.Join("|", codecs.Select(codec =>
            string.Format("({1})|{1}", codec.CodecName, codec.FilenameExtension)).ToArray()),
            string.Join(";", codecs.Select(codec => codec.FilenameExtension).ToArray()));
            var result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                Image = File.ReadAllBytes(filename);
            }
        }

        private bool CanOpenAddServiceFlyout()
        {
            return true;
        }

        private void DoOpenAddServiceFlyout()
        {
            if (IsAddServiceFlyoutOpen)
            {
                IsAddServiceFlyoutOpen = false;
            }
            else
            {
                IsAddServiceFlyoutOpen = true;
            }
        }
    }
}