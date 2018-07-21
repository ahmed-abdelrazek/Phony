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
    public class SuppliersViewModel : BindableBase
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
            set => SetProperty(ref _supplierId, value);
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

        public string SuppliersCount
        {
            get => _suppliersCount;
            set => SetProperty(ref _suppliersCount, value);
        }

        public string SuppliersDebits
        {
            get => _suppliersDebits;
            set => SetProperty(ref _suppliersDebits, value);
        }

        public string SuppliersCredits
        {
            get => _suppliersCredits;
            set => SetProperty(ref _suppliersCredits, value);
        }

        public string SuppliersProfit
        {
            get => _suppliersProfit;
            set => SetProperty(ref _suppliersProfit, value);
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

        public bool IsAddSupplierFlyoutOpen
        {
            get => _isSupplierFlyoutOpen;
            set => SetProperty(ref _isSupplierFlyoutOpen, value);
        }

        public Supplier DataGridSelectedSupplier
        {
            get => _dataGridSelectedSupplier;
            set => SetProperty(ref _dataGridSelectedSupplier, value);
        }

        public long SelectedSalesMan
        {
            get => _selectedSalesMan;
            set => SetProperty(ref _selectedSalesMan, value);
        }

        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set => SetProperty(ref _suppliers, value);
        }

        public ObservableCollection<SalesMan> SalesMen
        {
            get => _salesMen;
            set => SetProperty(ref _salesMen, value);
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

        Suppliers SuppliersMessage = Application.Current.Windows.OfType<Suppliers>().FirstOrDefault();

        public SuppliersViewModel()
        {
            LoadCommands();
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(Data.DBCollections.Suppliers).FindAll());
                SalesMen = new ObservableCollection<SalesMan>(db.GetCollection<SalesMan>(Data.DBCollections.SalesMen).FindAll());
                Users = new ObservableCollection<User>(db.GetCollection<User>(Data.DBCollections.Users).FindAll());
            }
            DebitCredit();
        }

        public void LoadCommands()
        {
            SupplierPay = new DelegateCommand(DoSupplierPayAsync, CanSupplierPay).ObservesProperty(() => DataGridSelectedSupplier);
            PaySupplier = new DelegateCommand(DoPaySupplierAsync, CanPaySupplier).ObservesProperty(() => DataGridSelectedSupplier);
            AddSupplier = new DelegateCommand(DoAddSupplier, CanAddSupplier).ObservesProperty(() => Name).ObservesProperty(() => SelectedSalesMan);
            EditSupplier = new DelegateCommand(DoEditSupplier, CanEditSupplier).ObservesProperty(() => Name).ObservesProperty(() => SupplierId).ObservesProperty(() => SelectedSalesMan).ObservesProperty(() => DataGridSelectedSupplier);
            DeleteSupplier = new DelegateCommand(DoDeleteSupplier, CanDeleteSupplier).ObservesProperty(() => DataGridSelectedSupplier);
            OpenAddSupplierFlyout = new DelegateCommand(DoOpenAddSupplierFlyout, CanOpenAddSupplierFlyout);
            SelectImage = new DelegateCommand(DoSelectImage, CanSelectImage);
            FillUI = new DelegateCommand(DoFillUI, CanFillUI).ObservesProperty(() => DataGridSelectedSupplier);
            ReloadAllSuppliers = new DelegateCommand(DoReloadAllSuppliers, CanReloadAllSuppliers);
            Search = new DelegateCommand(DoSearch, CanSearch).ObservesProperty(() => SearchText);
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

        private bool CanSupplierPay()
        {
            if (DataGridSelectedSupplier == null)
            {
                return false;
            }
            return true;
        }

        private async void DoSupplierPayAsync()
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
                        var s = db.GetCollection<Supplier>(Data.DBCollections.Suppliers.ToString()).FindById(DataGridSelectedSupplier.Id);
                        s.Balance += supplierpaymentamount;
                        db.GetCollection<SupplierMove>(Data.DBCollections.SuppliersMoves.ToString()).Insert(new SupplierMove
                        {
                            Supplier = db.GetCollection<Supplier>(Data.DBCollections.Suppliers.ToString()).FindById(DataGridSelectedSupplier.Id),
                            Credit = supplierpaymentamount,
                            CreateDate = DateTime.Now,
                            Creator = Core.ReadUserSession(),
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

        private bool CanPaySupplier()
        {
            if (DataGridSelectedSupplier == null)
            {
                return false;
            }
            return true;
        }

        private async void DoPaySupplierAsync()
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
                        var s = db.GetCollection<Supplier>(Data.DBCollections.Suppliers.ToString()).FindById(DataGridSelectedSupplier.Id);
                        s.Balance -= supplierpaymentamount;
                        db.GetCollection<SupplierMove>(Data.DBCollections.SuppliersMoves.ToString()).Insert(new SupplierMove
                        {
                            Supplier = db.GetCollection<Supplier>(Data.DBCollections.Suppliers.ToString()).FindById(DataGridSelectedSupplier.Id),
                            Debit = supplierpaymentamount,
                            CreateDate = DateTime.Now,
                            Creator = Core.ReadUserSession(),
                            EditDate = null,
                            Editor = null
                        });
                        db.GetCollection<TreasuryMove>(Data.DBCollections.TreasuriesMoves.ToString()).Insert(new TreasuryMove
                        {
                            Treasury = db.GetCollection<Treasury>(Data.DBCollections.Treasuries.ToString()).FindById(1),
                            Credit = supplierpaymentamount,
                            Notes = $"دفع المورد بكود {DataGridSelectedSupplier.Id} باسم {DataGridSelectedSupplier.Name}",
                            CreateDate = DateTime.Now,
                            Creator = Core.ReadUserSession(),
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

        private bool CanAddSupplier()
        {
            if (string.IsNullOrWhiteSpace(Name) || SelectedSalesMan < 1)
            {
                return false;
            }
            return true;
        }

        private void DoAddSupplier()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var exist = db.GetCollection<Supplier>(Data.DBCollections.Suppliers.ToString()).Find(x => x.Name == Name).FirstOrDefault();
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
                        SalesMan = db.GetCollection<SalesMan>(Data.DBCollections.SalesMen.ToString()).FindById(SelectedSalesMan),
                        Notes = Notes,
                        CreateDate = DateTime.Now,
                        Creator = Core.ReadUserSession(),
                        EditDate = null,
                        Editor = null
                    };
                    db.GetCollection<Supplier>(Data.DBCollections.Suppliers.ToString()).Insert(s);
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

        private bool CanEditSupplier()
        {
            if (string.IsNullOrWhiteSpace(Name) || SupplierId < 1 || SelectedSalesMan < 1 || DataGridSelectedSupplier == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditSupplier()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var s = db.GetCollection<Supplier>(Data.DBCollections.Suppliers.ToString()).FindById(DataGridSelectedSupplier.Id);
                s.Name = Name;
                s.Balance = Balance;
                s.Site = Site;
                s.Email = Email;
                s.Phone = Phone;
                s.Image = Image;
                s.SalesMan = db.GetCollection<SalesMan>(Data.DBCollections.SalesMen.ToString()).FindById(SelectedSalesMan);
                s.Notes = Notes;
                s.Editor = Core.ReadUserSession();
                s.EditDate = DateTime.Now;
                db.GetCollection<Supplier>(Data.DBCollections.Suppliers.ToString()).Update(s);
                SuppliersMessage.ShowMessageAsync("تمت العملية", "تم تعديل المورد بنجاح");
                Suppliers[Suppliers.IndexOf(DataGridSelectedSupplier)] = s;
                DebitCredit();
                DataGridSelectedSupplier = null;
                SupplierId = 0;
            }
        }

        private bool CanDeleteSupplier()
        {
            if (DataGridSelectedSupplier == null || DataGridSelectedSupplier.Id == 1)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteSupplier()
        {
            var result = await SuppliersMessage.ShowMessageAsync("حذف المورد", $"هل انت متاكد من حذف المورد {DataGridSelectedSupplier.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    db.GetCollection<Supplier>(Data.DBCollections.Suppliers.ToString()).Delete(DataGridSelectedSupplier.Id);
                    Suppliers.Remove(DataGridSelectedSupplier);
                }
                await SuppliersMessage.ShowMessageAsync("تمت العملية", "تم حذف المورد بنجاح");
                DebitCredit();
                DataGridSelectedSupplier = null;
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
                    Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(Data.DBCollections.Suppliers.ToString()).Find(x => x.Name.Contains(SearchText)));
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

        private bool CanReloadAllSuppliers()
        {
            return true;
        }

        private void DoReloadAllSuppliers()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(Data.DBCollections.Suppliers).FindAll());
            }
            DebitCredit();
        }

        private bool CanFillUI()
        {
            if (DataGridSelectedSupplier == null)
            {
                return false;
            }
            return true;
        }

        private void DoFillUI()
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

        private bool CanOpenAddSupplierFlyout()
        {
            return true;
        }

        private void DoOpenAddSupplierFlyout()
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