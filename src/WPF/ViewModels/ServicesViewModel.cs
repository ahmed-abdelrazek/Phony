using LiteDB;
using Phony.Data.Models.Lite;
using Phony.WPF.Data;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TinyLittleMvvm;

namespace Phony.WPF.ViewModels
{
    public class ServicesViewModel : BaseViewModelWithAnnotationValidation, IOnLoadedHandler
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
                _serviceId = value;
                NotifyOfPropertyChange(() => ServiceId);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string Site
        {
            get => _site;
            set
            {
                _site = value;
                NotifyOfPropertyChange(() => Site);
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                NotifyOfPropertyChange(() => Email);
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

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                NotifyOfPropertyChange(() => SearchText);
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                NotifyOfPropertyChange(() => Notes);
            }
        }

        public string ServicesCount
        {
            get => _servicesCount;
            set
            {
                _servicesCount = value;
                NotifyOfPropertyChange(() => ServicesCount);
            }
        }

        public string ServicesPurchasePrice
        {
            get => _servicesPurchasePrice;
            set
            {
                _servicesPurchasePrice = value;
                NotifyOfPropertyChange(() => ServicesPurchasePrice);
            }
        }

        public string ServicesSalePrice
        {
            get => _servicesSalePrice;
            set
            {
                _servicesSalePrice = value;
                NotifyOfPropertyChange(() => ServicesSalePrice);
            }
        }

        public string ServicesProfit
        {
            get => _servicesProfit;
            set
            {
                _servicesProfit = value;
                NotifyOfPropertyChange(() => ServicesProfit);
            }
        }

        public byte[] Image
        {
            get => _image;
            set
            {
                _image = value;
                NotifyOfPropertyChange(() => Image);
            }
        }

        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                NotifyOfPropertyChange(() => Balance);
            }
        }

        public bool IsAddServiceFlyoutOpen
        {
            get => _isServiceFlyoutOpen;
            set
            {
                _isServiceFlyoutOpen = value;
                NotifyOfPropertyChange(() => IsAddServiceFlyoutOpen);
            }
        }

        public Service DataGridSelectedService
        {
            get => _dataGridSelectedService;
            set
            {
                _dataGridSelectedService = value;
                NotifyOfPropertyChange(() => DataGridSelectedService);
            }
        }

        public ObservableCollection<Service> Services
        {
            get => _services;
            set
            {
                _services = value;
                NotifyOfPropertyChange(() => Services);
            }
        }

        public ServicesViewModel()
        {
            Title = "خدمات شركات";
        }

        public async Task OnLoadedAsync()
        {
            await Task.Run(() =>
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    Services = new ObservableCollection<Service>(db.GetCollection<Service>(DBCollections.Services).FindAll());
                }
            });
            await DebitCredit();
        }

        async Task DebitCredit()
        {
            decimal Debit = decimal.Round(Services.Where(c => c.Balance > 0).Sum(i => i.Balance), 2);
            await Task.Run(() =>
            {
                ServicesCount = $"مجموع الخدمات: {Services.Count()}";
                ServicesPurchasePrice = $"اجمالى لينا: {Math.Abs(Debit)}";
            });
        }

        private bool CanAddBalance()
        {
            return DataGridSelectedService != null;
        }

        private async Task DoAddBalance()
        {
            var result = MessageBox.MaterialInputBox.Show($"ادخل المبلغ الذى تريد شحن الخدمه به {DataGridSelectedService.Name}", "شحن", true);
            if (string.IsNullOrWhiteSpace(result))
            {
                MessageBox.MaterialMessageBox.ShowWarning("لم تقم بادخال اى مبلغ لاضافته للرصيد", "ادخل مبلغ", true);
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal servicepaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                    {
                        var s = db.GetCollection<Service>(DBCollections.Services).FindById(DataGridSelectedService.Id);
                        s.Balance += servicepaymentamount;
                        db.GetCollection<Service>(DBCollections.Services).Update(s);
                        db.GetCollection<ServiceMove>(DBCollections.ServicesMoves).Insert(new ServiceMove
                        {
                            Service = db.GetCollection<Service>(DBCollections.Services).FindById(DataGridSelectedService.Id),
                            Debit = servicepaymentamount,
                            CreatedAt = DateTime.Now,
                            Creator = CurrentUser,
                            Editor = CurrentUser
                        });
                        MessageBox.MaterialMessageBox.Show($"تم شحن خدمة {DataGridSelectedService.Name} بمبلغ {servicepaymentamount} جنية بنجاح", "تمت العملية", true);
                        Services[Services.IndexOf(DataGridSelectedService)] = s;
                        await DebitCredit();
                        DataGridSelectedService = null;
                        ServiceId = 0;
                    }
                }
                else
                {
                    MessageBox.MaterialMessageBox.ShowWarning("ادخل مبلغ صحيح بعلامه عشرية واحدة", "خطأ فى المبلغ", true);
                }
            }
        }

        private bool CanAddService()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private async Task DoAddService()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            var exist = db.GetCollection<Service>(DBCollections.Services).Find(x => x.Name == Name).FirstOrDefault();
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
                    CreatedAt = DateTime.Now,
                    Creator = CurrentUser,
                    Editor = CurrentUser
                };
                db.GetCollection<Service>(DBCollections.Services).Insert(s);
                Services.Add(s);
                MessageBox.MaterialMessageBox.Show("تم اضافة الخدمة بنجاح", "تمت العملية", true);
                await DebitCredit();
            }
            else
            {
                MessageBox.MaterialMessageBox.ShowWarning("الخدمة موجودة من قبل بالفعل", "موجود", true);
            }
        }

        private bool CanEditService()
        {
            return !string.IsNullOrWhiteSpace(Name) && ServiceId != 0 && DataGridSelectedService != null;
        }

        private async Task DoEditService()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            var s = db.GetCollection<Service>(DBCollections.Services).FindById(DataGridSelectedService.Id);
            s.Name = Name;
            s.Balance = Balance;
            s.Site = Site;
            s.Email = Email;
            s.Phone = Phone;
            s.Image = Image;
            s.Notes = Notes;
            s.Editor = CurrentUser;
            s.EditedAt = DateTime.Now;
            db.GetCollection<Service>(DBCollections.Services).Update(s);
            MessageBox.MaterialMessageBox.Show("تمت العملية", "تم تعديل الخدمة بنجاح");
            Services[Services.IndexOf(DataGridSelectedService)] = s;
            await DebitCredit();
            DataGridSelectedService = null;
            ServiceId = 0;
        }

        private bool CanDeleteService()
        {
            return DataGridSelectedService != null && DataGridSelectedService.Id != 1;
        }

        private async Task DoDeleteService()
        {
            var result = MessageBox.MaterialMessageBox.ShowWithCancel($"هل انت متاكد من حذف الخدمة {DataGridSelectedService.Name}", "حذف الخدمة", true);
            if (result == MessageBoxResult.OK)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    db.GetCollection<Service>(DBCollections.Services).Delete(DataGridSelectedService.Id);
                    Services.Remove(DataGridSelectedService);
                }
                MessageBox.MaterialMessageBox.Show("تمت العملية", "تم حذف الخدمة بنجاح");
                await DebitCredit();
                DataGridSelectedService = null;
            }
        }

        private bool CanSearch()
        {
            return !string.IsNullOrWhiteSpace(SearchText);
        }

        private void DoSearch()
        {
            try
            {
                using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
                Services = new ObservableCollection<Service>(db.GetCollection<Service>(DBCollections.Services).Find(x => x.Name.Contains(SearchText)));
                if (Services.Count < 1)
                {
                    MessageBox.MaterialMessageBox.ShowWarning("لم يتم العثور على شئ", "غير موجود", true);
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                MessageBox.MaterialMessageBox.ShowError("لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله", "خطأ", true);
            }
        }

        private bool CanReloadAllServices()
        {
            return true;
        }

        private async Task DoReloadAllServices()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                Services = new ObservableCollection<Service>(db.GetCollection<Service>(DBCollections.Services).FindAll());
            }
            await DebitCredit();
        }

        private bool CanFillUI()
        {
            return DataGridSelectedService != null;
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
            IsAddServiceFlyoutOpen = !IsAddServiceFlyoutOpen;
        }
    }
}