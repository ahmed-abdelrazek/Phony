using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
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
    public class ServicesViewModel : BindableBase
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
            set => SetProperty(ref _serviceId, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Site
        {
            get => _site;
            set => SetProperty(ref _site, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public string ServicesCount
        {
            get => _servicesCount;
            set => SetProperty(ref _servicesCount, value);
        }

        public string ServicesPurchasePrice
        {
            get => _servicesPurchasePrice;
            set => SetProperty(ref _servicesPurchasePrice, value);
        }

        public string ServicesSalePrice
        {
            get => _servicesSalePrice;
            set => SetProperty(ref _servicesSalePrice, value);
        }

        public string ServicesProfit
        {
            get => _servicesProfit;
            set => SetProperty(ref _servicesProfit, value);
        }

        public byte[] Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        public bool IsAddServiceFlyoutOpen
        {
            get => _isServiceFlyoutOpen;
            set => SetProperty(ref _isServiceFlyoutOpen, value);
        }

        public Service DataGridSelectedService
        {
            get => _dataGridSelectedService;
            set => SetProperty(ref _dataGridSelectedService, value);
        }

        public ObservableCollection<Service> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
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

        Services Message = Application.Current.Windows.OfType<Services>().FirstOrDefault();

        public ServicesViewModel()
        {
            LoadCommands();
            using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
            {
                Services = new ObservableCollection<Service>(db.GetCollection<Service>(DBCollections.Services).FindAll());
                Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users).FindAll());
            }
            DebitCredit();
        }

        public void LoadCommands()
        {
            AddBalance = new DelegateCommand(DoAddBalance, CanAddBalance).ObservesProperty(() => DataGridSelectedService);
            AddService = new DelegateCommand(DoAddService, CanAddService).ObservesProperty(() => Name);
            EditService = new DelegateCommand(DoEditService, CanEditService).ObservesProperty(() => Name).ObservesProperty(() => ServiceId).ObservesProperty(() => DataGridSelectedService);
            DeleteService = new DelegateCommand(DoDeleteService, CanDeleteService).ObservesProperty(() => DataGridSelectedService);
            OpenAddServiceFlyout = new DelegateCommand(DoOpenAddServiceFlyout, CanOpenAddServiceFlyout);
            SelectImage = new DelegateCommand(DoSelectImage, CanSelectImage);
            FillUI = new DelegateCommand(DoFillUI, CanFillUI).ObservesProperty(() => DataGridSelectedService);
            ReloadAllServices = new DelegateCommand(DoReloadAllServices, CanReloadAllServices);
            Search = new DelegateCommand(DoSearch, CanSearch).ObservesProperty(() => SearchText);
        }

        async void DebitCredit()
        {
            decimal Debit = decimal.Round(Services.Where(c => c.Balance > 0).Sum(i => i.Balance), 2);
            await Task.Run(() =>
            {
                ServicesCount = $"مجموع الخدمات: {Services.Count()}";
            });
            await Task.Run(() =>
            {
                ServicesPurchasePrice = $"اجمالى لينا: {Math.Abs(Debit)}";
            });
        }

        private bool CanAddBalance()
        {
            return DataGridSelectedService is not null;
        }

        private async void DoAddBalance()
        {
            var result = await Message.ShowInputAsync("شحن", $"ادخل المبلغ الذى تريد شحن الخدمه به {DataGridSelectedService.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await Message.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لاضافته للرصيد");
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal servicepaymentamount);
                if (isvalidmoney)
                {
                    using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                    var s = db.GetCollection<Service>(DBCollections.Services).FindById(DataGridSelectedService.Id);
                    s.Balance += servicepaymentamount;
                    db.GetCollection<Service>(DBCollections.Services).Update(s);
                    db.GetCollection<ServiceMove>(DBCollections.ServicesMoves).Insert(new ServiceMove
                    {
                        Service = db.GetCollection<Service>(DBCollections.Services).FindById(DataGridSelectedService.Id),
                        Debit = servicepaymentamount,
                        CreateDate = DateTime.Now,
                        Creator = Core.ReadUserSession(),
                        EditDate = null,
                        Editor = null
                    });
                    await Message.ShowMessageAsync("تمت العملية", $"تم شحن خدمة {DataGridSelectedService.Name} بمبلغ {servicepaymentamount} جنية بنجاح");
                    Services[Services.IndexOf(DataGridSelectedService)] = s;
                    DebitCredit();
                    DataGridSelectedService = null;
                    ServiceId = 0;
                }
                else
                {
                    await Message.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanAddService()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private async void DoAddService()
        {
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
            var exist = db.GetCollection<Service>(DBCollections.Services).Find(x => x.Name == Name).FirstOrDefault();
            if (exist is null)
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
                    Creator = Core.ReadUserSession(),
                    EditDate = null,
                    Editor = null
                };
                db.GetCollection<Service>(DBCollections.Services).Insert(s);
                Services.Add(s);
                await Message.ShowMessageAsync("تمت العملية", "تم اضافة الخدمة بنجاح");
                DebitCredit();
            }
            else
            {
                await Message.ShowMessageAsync("موجود", "الخدمة موجودة من قبل بالفعل");
            }
        }

        private bool CanEditService()
        {
            return !string.IsNullOrWhiteSpace(Name) && ServiceId != 0 && DataGridSelectedService is not null;
        }

        private async void DoEditService()
        {
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
            var s = db.GetCollection<Service>(DBCollections.Services).FindById(DataGridSelectedService.Id);
            s.Name = Name;
            s.Balance = Balance;
            s.Site = Site;
            s.Email = Email;
            s.Phone = Phone;
            s.Image = Image;
            s.Notes = Notes;
            s.Editor = Core.ReadUserSession();
            s.EditDate = DateTime.Now;
            db.GetCollection<Service>(DBCollections.Services).Update(s);
            await Message.ShowMessageAsync("تمت العملية", "تم تعديل الخدمة بنجاح");
            Services[Services.IndexOf(DataGridSelectedService)] = s;
            DebitCredit();
            DataGridSelectedService = null;
            ServiceId = 0;
        }

        private bool CanDeleteService()
        {
            return DataGridSelectedService is not null && DataGridSelectedService.Id != 1;
        }

        private async void DoDeleteService()
        {
            var result = await Message.ShowMessageAsync("حذف الخدمة", $"هل انت متاكد من حذف الخدمة {DataGridSelectedService.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
                {
                    db.GetCollection<Service>(DBCollections.Services).Delete(DataGridSelectedService.Id);
                    Services.Remove(DataGridSelectedService);
                }
                await Message.ShowMessageAsync("تمت العملية", "تم حذف الخدمة بنجاح");
                DebitCredit();
                DataGridSelectedService = null;
            }
        }

        private bool CanSearch()
        {
            return !string.IsNullOrWhiteSpace(SearchText);
        }

        private async void DoSearch()
        {
            try
            {
                using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                Services = new ObservableCollection<Service>(db.GetCollection<Service>(DBCollections.Services).Find(x => x.Name.Contains(SearchText)));
                if (Services.Count < 1)
                {
                    await Message.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                await Message.ShowMessageAsync("خطأ", "لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله");
            }
        }

        private bool CanReloadAllServices()
        {
            return true;
        }

        private void DoReloadAllServices()
        {
            using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
            {
                Services = new ObservableCollection<Service>(db.GetCollection<Service>(DBCollections.Services).FindAll());
            }
            DebitCredit();
        }

        private bool CanFillUI()
        {
            return DataGridSelectedService is not null;
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
            Microsoft.Win32.OpenFileDialog dlg = new();
            var codecs = ImageCodecInfo.GetImageEncoders();
            dlg.Filter = $"All image files ({string.Join(";", codecs.Select(codec => codec.FilenameExtension).ToArray())})|{string.Join(";", codecs.Select(codec => codec.FilenameExtension).ToArray())}|All files|*";
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
            IsAddServiceFlyoutOpen =!IsAddServiceFlyoutOpen;
        }
    }
}