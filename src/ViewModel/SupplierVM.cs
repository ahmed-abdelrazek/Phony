using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Model;
using Phony.Utility;
using Phony.View;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModel
{
    public class SupplierVM : CommonBase
    {
        long _supplierId;
        string _name;
        string _site;
        string _email;
        string _searchText;
        string _phone;
        string _notes;
        static string _suppliersCount;
        static string _suppliersDebits;
        static string _suppliersCredits;
        static string _suppliersProfit;
        byte[] _image;
        decimal _balance;
        bool _isSupplierFlyoutOpen;
        Supplier _dataGridSelectedSupplier;
        long _selectedSalesMan;
        ObservableCollection<Supplier> _suppliers;
        ObservableCollection<SalesMan> _salesMen;

        public long SupplierId
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

        public string SuppliersDebits
        {
            get => _suppliersDebits;
            set
            {
                if (value != _suppliersDebits)
                {
                    _suppliersDebits = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SuppliersCredits
        {
            get => _suppliersCredits;
            set
            {
                if (value != _suppliersCredits)
                {
                    _suppliersCredits = value;
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

        public long SelectedSalesMan
        {
            get => _selectedSalesMan;
            set
            {
                if (value != _selectedSalesMan)
                {
                    _selectedSalesMan = value;
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

        public ObservableCollection<SalesMan> SalesMen
        {
            get => _salesMen;
            set
            {
                if (value != _salesMen)
                {
                    _salesMen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<User> Users { get; set; }

        public ICommand SupplierPay { get; set; }
        public ICommand PaySupplier { get; set; }
        public ICommand AddSupplier { get; set; }
        public ICommand EditSupplier { get; set; }
        public ICommand DeleteSupplier { get; set; }
        public ICommand OpenAddSupplierFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand ReloadAllSuppliers { get; set; }
        public ICommand Search { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        Suppliers SuppliersMessage = Application.Current.Windows.OfType<Suppliers>().FirstOrDefault();

        public SupplierVM()
        {
            LoadCommands();
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).FindAll());
                SalesMen = new ObservableCollection<SalesMan>(db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).FindAll());
                Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users.ToString()).FindAll());
            }
            DebitCredit();
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
            SupplierPay = new CustomCommand(DoSupplierPayAsync, CanSupplierPay);
            PaySupplier = new CustomCommand(DoPaySupplierAsync, CanPaySupplier);
        }

        async void DebitCredit()
        {
            decimal Debit = decimal.Round(Suppliers.Where(c => c.Balance < 0).Sum(i => i.Balance), 2);
            decimal Credit = decimal.Round(Suppliers.Where(c => c.Balance > 0).Sum(i => i.Balance), 2);
            await Task.Run(() =>
            {
                SuppliersCount = $"مجموع العملاء: {Suppliers.Count().ToString()}";
            });
            await Task.Run(() =>
            {
                SuppliersDebits = $"اجمالى لينا: {Math.Abs(Debit).ToString()}";
            });
            await Task.Run(() =>
            {
                SuppliersCredits = $"اجمالى علينا: {Math.Abs(Credit).ToString()}";
            });
            await Task.Run(() =>
            {
                SuppliersProfit = $"تقدير لصافى لينا: {(Math.Abs(Debit) - Math.Abs(Credit)).ToString()}";
            });
        }

        private bool CanSupplierPay(object obj)
        {
            if (DataGridSelectedSupplier == null)
            {
                return false;
            }
            return true;
        }

        private async void DoSupplierPayAsync(object obj)
        {
            var result = await SuppliersMessage.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى استلمته من المورد {DataGridSelectedSupplier.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await SuppliersMessage.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لاضافته للرصيد");
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal supplierpaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                    {
                        var s = db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).FindById(DataGridSelectedSupplier.Id);
                        s.Balance += supplierpaymentamount;
                        db.GetCollection<SupplierMove>(DBCollections.SuppliersMoves.ToString()).Insert(new SupplierMove
                        {
                            Supplier = db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).FindById(DataGridSelectedSupplier.Id),
                            Credit = supplierpaymentamount,
                            CreateDate = DateTime.Now,
                            Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                            EditDate = null,
                            Editor = null
                        });
                        await SuppliersMessage.ShowMessageAsync("تمت العملية", $"تم استلام للمورد {DataGridSelectedSupplier.Name} مبلغ {supplierpaymentamount} جنية بنجاح");
                        Suppliers[Suppliers.IndexOf(DataGridSelectedSupplier)] = s;
                        DebitCredit();
                        DataGridSelectedSupplier = null;
                        SupplierId = 0;
                    }
                }
                else
                {
                    await SuppliersMessage.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanPaySupplier(object obj)
        {
            if (DataGridSelectedSupplier == null)
            {
                return false;
            }
            return true;
        }

        private async void DoPaySupplierAsync(object obj)
        {
            var result = await SuppliersMessage.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى تريد تدفيعه للمورد {DataGridSelectedSupplier.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await SuppliersMessage.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لاضافته للرصيد");
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal supplierpaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                    {
                        var s = db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).FindById(DataGridSelectedSupplier.Id);
                        s.Balance -= supplierpaymentamount;
                        db.GetCollection<SupplierMove>(DBCollections.SuppliersMoves.ToString()).Insert(new SupplierMove
                        {
                            Supplier = db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).FindById(DataGridSelectedSupplier.Id),
                            Debit = supplierpaymentamount,
                            CreateDate = DateTime.Now,
                            Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                            EditDate = null,
                            Editor = null
                        });
                        db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves.ToString()).Insert(new TreasuryMove
                        {
                            Treasury = db.GetCollection<Treasury>(DBCollections.Treasuries.ToString()).FindById(1),
                            Credit = supplierpaymentamount,
                            Notes = $"دفع المورد بكود {DataGridSelectedSupplier.Id} باسم {DataGridSelectedSupplier.Name}",
                            CreateDate = DateTime.Now,
                            Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                            EditDate = null,
                            Editor = null
                        });
                        await SuppliersMessage.ShowMessageAsync("تمت العملية", $"تم دفع للمورد {DataGridSelectedSupplier.Name} مبلغ {supplierpaymentamount} جنية بنجاح");
                        Suppliers[Suppliers.IndexOf(DataGridSelectedSupplier)] = s;
                        DebitCredit();
                        DataGridSelectedSupplier = null;
                        SupplierId = 0;
                    }
                }
                else
                {
                    await SuppliersMessage.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanAddSupplier(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || SelectedSalesMan < 1)
            {
                return false;
            }
            return true;
        }

        private void DoAddSupplier(object obj)
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var exist = db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).Find(x => x.Name == Name).FirstOrDefault();
                if (exist == null)
                {
                    var s = new Supplier
                    {
                        Name = Name,
                        Balance = Balance,
                        Site = Site,
                        Email = Email,
                        Phone = Phone,
                        Image = Image,
                        SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).FindById(SelectedSalesMan),
                        Notes = Notes,
                        CreateDate = DateTime.Now,
                        Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                        EditDate = null,
                        Editor = null
                    };
                    db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).Insert(s);
                    Suppliers.Add(s);
                    SuppliersMessage.ShowMessageAsync("تمت العملية", "تم اضافة المورد بنجاح");
                    DebitCredit();
                }
                else
                {
                    SuppliersMessage.ShowMessageAsync("موجود", "المورد موجود من قبل بالفعل");
                }
            }
        }

        private bool CanEditSupplier(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || SupplierId < 1 || SelectedSalesMan < 1 || DataGridSelectedSupplier == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditSupplier(object obj)
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var s = db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).FindById(DataGridSelectedSupplier.Id);
                s.Name = Name;
                s.Balance = Balance;
                s.Site = Site;
                s.Email = Email;
                s.Phone = Phone;
                s.Image = Image;
                s.SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).FindById(SelectedSalesMan);
                s.Notes = Notes;
                s.EditDate = DateTime.Now;
                s.Editor = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id);
                db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).Update(s);
                SuppliersMessage.ShowMessageAsync("تمت العملية", "تم تعديل المورد بنجاح");
                Suppliers[Suppliers.IndexOf(DataGridSelectedSupplier)] = s;
                DebitCredit();
                DataGridSelectedSupplier = null;
                SupplierId = 0;
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
            var result = await SuppliersMessage.ShowMessageAsync("حذف المورد", $"هل انت متاكد من حذف المورد {DataGridSelectedSupplier.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).Delete(DataGridSelectedSupplier.Id);
                    Suppliers.Remove(DataGridSelectedSupplier);
                }
                await SuppliersMessage.ShowMessageAsync("تمت العملية", "تم حذف المورد بنجاح");
                DebitCredit();
                DataGridSelectedSupplier = null;
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
            try
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).Find(x => x.Name.Contains(SearchText)));
                    if (Suppliers.Count < 1)
                    {
                        SuppliersMessage.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                    }
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله");
            }
        }

        private bool CanReloadAllSuppliers(object obj)
        {
            return true;
        }

        private void DoReloadAllSuppliers(object obj)
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).FindAll());
            }
            DebitCredit();
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
            SelectedSalesMan = DataGridSelectedSupplier.SalesMan.Id;
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