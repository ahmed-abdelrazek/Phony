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
    public class SupplierVM : CommonBase
    {
        int _supplierId;
        string _name;
        string _site;
        string _email;
        string _searchText;
        string _phone;
        string _notes;
        static string _suppliersCount;
        static string _suppliersPurchasePrice;
        static string _suppliersSalePrice;
        static string _suppliersProfit;
        byte[] _image;
        decimal _balance;
        bool _isSupplierFlyoutOpen;
        Supplier _dataGridSelectedSupplier;
        ObservableCollection<Supplier> _suppliers;

        public int SupplierId
        {
            get => _supplierId;
            set
            {
                if (value != _supplierId)
                {
                    _supplierId = value;
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

        public string SuppliersCount
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

        public string SuppliersPurchasePrice
        {
            get => _suppliersPurchasePrice;
            set
            {
                if (value != _suppliersPurchasePrice)
                {
                    _suppliersPurchasePrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SuppliersSalePrice
        {
            get => _suppliersSalePrice;
            set
            {
                if (value != _suppliersSalePrice)
                {
                    _suppliersSalePrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SuppliersProfit
        {
            get => _suppliersProfit;
            set
            {
                if (value != _suppliersProfit)
                {
                    _suppliersProfit = value;
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

        public bool IsAddSupplierFlyoutOpen
        {
            get => _isSupplierFlyoutOpen;
            set
            {
                if (value != _isSupplierFlyoutOpen)
                {
                    _isSupplierFlyoutOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Supplier DataGridSelectedSupplier
        {
            get => _dataGridSelectedSupplier;
            set
            {
                if (value != _dataGridSelectedSupplier)
                {
                    _dataGridSelectedSupplier = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                if (value != _suppliers)
                {
                    _suppliers = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<User> Users { get; set; }

        public ICommand OpenAddSupplierFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand DeleteSupplier { get; set; }
        public ICommand ReloadAllSuppliers { get; set; }
        public ICommand Search { get; set; }
        public ICommand AddSupplier { get; set; }
        public ICommand EditSupplier { get; set; }
        public ICommand AddBalance { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        Suppliers SuppliersMassage = Application.Current.Windows.OfType<Suppliers>().FirstOrDefault();

        public SupplierVM()
        {
            LoadCommands();
            using (var db = new PhonyDbContext())
            {
                Suppliers = new ObservableCollection<Supplier>(db.Suppliers);
                Users = new ObservableCollection<User>(db.Users);
            }
            new Thread(() =>
            {
                SuppliersCount = $"إجمالى الموردين: {Suppliers.Count().ToString()}";
                SuppliersPurchasePrice = $"اجمالى لينا: {decimal.Round(Suppliers.Where(c => c.Balance > 0).Sum(i => i.Balance), 2).ToString()}";
                SuppliersSalePrice = $"اجمالى علينا: {decimal.Round(Suppliers.Where(c => c.Balance < 0).Sum(i => i.Balance), 2).ToString()}";
                SuppliersProfit = $"تقدير لصافى لينا: {decimal.Round((Suppliers.Where(c => c.Balance > 0).Sum(i => i.Balance) + Suppliers.Where(c => c.Balance < 0).Sum(i => i.Balance)), 2).ToString()}";
                Thread.CurrentThread.Abort();
            }).Start();
        }

        public void LoadCommands()
        {
            OpenAddSupplierFlyout = new CustomCommand(DoOpenAddSupplierFlyout, CanOpenAddSupplierFlyout);
            SelectImage = new CustomCommand(DoSelectImage, CanSelectImage);
            FillUI = new CustomCommand(DoFillUI, CanFillUI);
            DeleteSupplier = new CustomCommand(DoDeleteSupplier, CanDeleteSupplier);
            ReloadAllSuppliers = new CustomCommand(DoReloadAllSuppliers, CanReloadAllSuppliers);
            Search = new CustomCommand(DoSearch, CanSearch);
            AddSupplier = new CustomCommand(DoAddSupplier, CanAddSupplier);
            EditSupplier = new CustomCommand(DoEditSupplier, CanEditSupplier);
            AddBalance = new CustomCommand(DoAddBalance, CanAddBalance);
        }

        private bool CanAddBalance(object obj)
        {
            if (DataGridSelectedSupplier == null)
            {
                return false;
            }
            return true;
        }

        private async void DoAddBalance(object obj)
        {
            var result = await SuppliersMassage.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى تريد تدفيعه الموردين {DataGridSelectedSupplier.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await SuppliersMassage.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لاضافته للرصيد");
            }
            else
            {
                decimal supplierpaymentamount;
                bool isvalidmoney = decimal.TryParse(result, out supplierpaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new UnitOfWork(new PhonyDbContext()))
                    {
                        var s = db.Suppliers.Get(DataGridSelectedSupplier.Id);
                        s.Balance -= supplierpaymentamount;
                        s.EditDate = DateTime.Now;
                        s.EditById = CurrentUser.Id;
                        var sm = new SupplierMove
                        {
                            SupplierId = DataGridSelectedSupplier.Id,
                            Amount = supplierpaymentamount,
                            CreateDate = DateTime.Now,
                            CreatedById = CurrentUser.Id,
                            EditDate = null,
                            EditById = null
                        };
                        db.SuppliersMoves.Add(sm);
                        db.Complete();
                        await SuppliersMassage.ShowMessageAsync("تمت العملية", $"تم اضافة للمورد {DataGridSelectedSupplier.Name} مبلغ {supplierpaymentamount} جنية بنجاح");
                        DataGridSelectedSupplier = null;
                        SupplierId = 0;
                        Suppliers.Remove(DataGridSelectedSupplier);
                        Suppliers.Add(s);
                    }
                }
                else
                {
                    await SuppliersMassage.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanEditSupplier(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || SupplierId == 0 || DataGridSelectedSupplier == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditSupplier(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var s = db.Suppliers.Get(DataGridSelectedSupplier.Id);
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
                SupplierId = 0;
                Suppliers.Remove(DataGridSelectedSupplier);
                Suppliers.Add(s);
                DataGridSelectedSupplier = null;
                SuppliersMassage.ShowMessageAsync("تمت العملية", "تم تعديل المورد بنجاح");
            }
        }

        private bool CanAddSupplier(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }

        private void DoAddSupplier(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var s = new Supplier
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
                db.Suppliers.Add(s);
                db.Complete();
                Suppliers.Add(s);
                SuppliersMassage.ShowMessageAsync("تمت العملية", "تم اضافة المورد بنجاح");
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
                Suppliers = new ObservableCollection<Supplier>(db.Suppliers.Where(i => i.Name.Contains(SearchText)));
                if (Suppliers.Count < 1)
                {
                    SuppliersMassage.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                }
            }
        }

        private bool CanReloadAllSuppliers(object obj)
        {
            return true;
        }

        private void DoReloadAllSuppliers(object obj)
        {
            using (var db = new PhonyDbContext())
            {
                Suppliers = new ObservableCollection<Supplier>(db.Suppliers);
            }
        }

        private bool CanDeleteSupplier(object obj)
        {
            if (DataGridSelectedSupplier == null || DataGridSelectedSupplier.Id == 1)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteSupplier(object obj)
        {
            var result = await SuppliersMassage.ShowMessageAsync("حذف المورد", $"هل انت متاكد من حذف المورد {DataGridSelectedSupplier.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new UnitOfWork(new PhonyDbContext()))
                {
                    db.Suppliers.Remove(db.Suppliers.Get(DataGridSelectedSupplier.Id));
                    db.Complete();
                    Suppliers.Remove(DataGridSelectedSupplier);
                }
                DataGridSelectedSupplier = null;
                await SuppliersMassage.ShowMessageAsync("تمت العملية", "تم حذف المورد بنجاح");
            }
        }

        private bool CanFillUI(object obj)
        {
            if (DataGridSelectedSupplier == null)
            {
                return false;
            }
            return true;
        }

        private void DoFillUI(object obj)
        {
            SupplierId = DataGridSelectedSupplier.Id;
            Name = DataGridSelectedSupplier.Name;
            Balance = DataGridSelectedSupplier.Balance;
            Site = DataGridSelectedSupplier.Site;
            Image = DataGridSelectedSupplier.Image;
            Email = DataGridSelectedSupplier.Email;
            Phone = DataGridSelectedSupplier.Phone;
            Notes = DataGridSelectedSupplier.Notes;
            IsAddSupplierFlyoutOpen = true;
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

        private bool CanOpenAddSupplierFlyout(object obj)
        {
            return true;
        }

        private void DoOpenAddSupplierFlyout(object obj)
        {
            if (IsAddSupplierFlyoutOpen)
            {
                IsAddSupplierFlyoutOpen = false;
            }
            else
            {
                IsAddSupplierFlyoutOpen = true;
            }
        }
    }
}