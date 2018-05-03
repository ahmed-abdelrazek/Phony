using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Model;
using Phony.Persistence;
using Phony.Utility;
using Phony.View;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModel
{
    public class ServiceVM : CommonBase
    {
        int _serviceId;
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

        public int ServiceId
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

        public ICommand OpenAddServiceFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand DeleteService { get; set; }
        public ICommand ReloadAllServices { get; set; }
        public ICommand Search { get; set; }
        public ICommand AddService { get; set; }
        public ICommand EditService { get; set; }
        public ICommand AddBalance { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        Services ServicesMassage = Application.Current.Windows.OfType<Services>().FirstOrDefault();

        public ServiceVM()
        {
            LoadCommands();
            using (var db = new PhonyDbContext())
            {
                Services = new ObservableCollection<Service>(db.Services);
                Users = new ObservableCollection<User>(db.Users);
            }
            new Thread(() =>
            {
                ServicesCount = $"إجمالى الخدمات: {Services.Count().ToString()}";
                ServicesPurchasePrice = $"اجمالى لينا: {decimal.Round(Services.Where(c => c.Balance > 0).Sum(i => i.Balance), 2).ToString()}";
                ServicesSalePrice = $"اجمالى علينا: {decimal.Round(Services.Where(c => c.Balance < 0).Sum(i => i.Balance), 2).ToString()}";
                ServicesProfit = $"تقدير لصافى لينا: {decimal.Round((Services.Where(c => c.Balance > 0).Sum(i => i.Balance) + Services.Where(c => c.Balance < 0).Sum(i => i.Balance)), 2).ToString()}";
                Thread.CurrentThread.Abort();
            }).Start();
        }

        public void LoadCommands()
        {
            OpenAddServiceFlyout = new CustomCommand(DoOpenAddServiceFlyout, CanOpenAddServiceFlyout);
            SelectImage = new CustomCommand(DoSelectImage, CanSelectImage);
            FillUI = new CustomCommand(DoFillUI, CanFillUI);
            DeleteService = new CustomCommand(DoDeleteService, CanDeleteService);
            ReloadAllServices = new CustomCommand(DoReloadAllServices, CanReloadAllServices);
            Search = new CustomCommand(DoSearch, CanSearch);
            AddService = new CustomCommand(DoAddService, CanAddService);
            EditService = new CustomCommand(DoEditService, CanEditService);
            AddBalance = new CustomCommand(DoAddBalance, CanAddBalance);
        }

        private bool CanAddBalance(object obj)
        {
            if (DataGridSelectedService == null)
            {
                return false;
            }
            return true;
        }

        private async void DoAddBalance(object obj)
        {
            var result = await ServicesMassage.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى تريد تدفيعه للخدمة {DataGridSelectedService.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await ServicesMassage.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لاضافته للرصيد");
            }
            else
            {
                decimal servicepaymentamount;
                bool isvalidmoney = decimal.TryParse(result, out servicepaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new UnitOfWork(new PhonyDbContext()))
                    {
                        var s = db.Services.Get(DataGridSelectedService.Id);
                        s.Balance -= servicepaymentamount;
                        s.EditDate = DateTime.Now;
                        s.EditById = CurrentUser.Id;
                        var sm = new ServiceMove
                        {
                            ServiceId = DataGridSelectedService.Id,
                            Amount = servicepaymentamount,
                            CreateDate = DateTime.Now,
                            CreatedById = CurrentUser.Id,
                            EditDate = null,
                            EditById = null
                        };
                        db.ServicesMoves.Add(sm);
                        db.Complete();
                        await ServicesMassage.ShowMessageAsync("تمت العملية", $"تم شحن خدمة {DataGridSelectedService.Name} مبلغ {servicepaymentamount} جنية بنجاح");
                        DataGridSelectedService = null;
                        ServiceId = 0;
                        Services.Remove(DataGridSelectedService);
                        Services.Add(s);
                    }
                }
                else
                {
                    await ServicesMassage.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanEditService(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || ServiceId == 0 || DataGridSelectedService == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditService(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var s = db.Services.Get(DataGridSelectedService.Id);
                s.Name = Name;
                s.Balance = Balance;
                s.Site = Site;
                s.Email = Email;
                s.Phone = Phone;
                s.Image = Image;
                s.Notes = Notes;
                s.EditDate = DateTime.Now;
                s.EditById = CurrentUser.Id;
                db.Complete();
                ServiceId = 0;
                Services.Remove(DataGridSelectedService);
                Services.Add(s);
                DataGridSelectedService = null;
                ServicesMassage.ShowMessageAsync("تمت العملية", "تم تعديل الخدمة بنجاح");
            }
        }

        private bool CanAddService(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }

        private void DoAddService(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
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
                    CreatedById = CurrentUser.Id,
                    EditDate = null,
                    EditById = null
                };
                db.Services.Add(s);
                db.Complete();
                Services.Add(s);
                ServicesMassage.ShowMessageAsync("تمت العملية", "تم اضافة الخدمة بنجاح");
            }
        }

        private bool CanSearch(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return false;
            }
            return true;
        }

        private void DoSearch(object obj)
        {
            using (var db = new PhonyDbContext())
            {
                Services = new ObservableCollection<Service>(db.Services.Where(i => i.Name.Contains(SearchText)));
                if (Services.Count < 1)
                {
                    ServicesMassage.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                }
            }
        }

        private bool CanReloadAllServices(object obj)
        {
            return true;
        }

        private void DoReloadAllServices(object obj)
        {
            using (var db = new PhonyDbContext())
            {
                Services = new ObservableCollection<Service>(db.Services);
            }
        }

        private bool CanDeleteService(object obj)
        {
            if (DataGridSelectedService == null || DataGridSelectedService.Id == 1)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteService(object obj)
        {
            var result = await ServicesMassage.ShowMessageAsync("حذف الخدمة", $"هل انت متاكد من حذف الخدمة {DataGridSelectedService.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new UnitOfWork(new PhonyDbContext()))
                {
                    db.Services.Remove(db.Services.Get(DataGridSelectedService.Id));
                    db.Complete();
                    Services.Remove(DataGridSelectedService);
                }
                DataGridSelectedService = null;
                await ServicesMassage.ShowMessageAsync("تمت العملية", "تم حذف الخدمة بنجاح");
            }
        }

        private bool CanFillUI(object obj)
        {
            if (DataGridSelectedService == null)
            {
                return false;
            }
            return true;
        }

        private void DoFillUI(object obj)
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

        private bool CanSelectImage(object obj)
        {
            return true;
        }

        private void DoSelectImage(object obj)
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

        private bool CanOpenAddServiceFlyout(object obj)
        {
            return true;
        }

        private void DoOpenAddServiceFlyout(object obj)
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