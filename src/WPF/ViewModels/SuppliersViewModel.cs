using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.WPF.Data;
using Phony.WPF.Models;
using Phony.WPF.Views;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Phony.WPF.ViewModels
{
    public class SuppliersViewModel : BaseViewModelWithAnnotationValidation
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
                _supplierId = value;
                NotifyOfPropertyChange(() => SupplierId);
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

        public string SuppliersCount
        {
            get => _suppliersCount;
            set
            {
                _suppliersCount = value;
                NotifyOfPropertyChange(() => SuppliersCount);
            }
        }

        public string SuppliersDebits
        {
            get => _suppliersDebits;
            set
            {
                _suppliersDebits = value;
                NotifyOfPropertyChange(() => SuppliersDebits);
            }
        }

        public string SuppliersCredits
        {
            get => _suppliersCredits;
            set
            {
                _suppliersCredits = value;
                NotifyOfPropertyChange(() => SuppliersCredits);
            }
        }

        public string SuppliersProfit
        {
            get => _suppliersProfit;
            set
            {
                _suppliersProfit = value;
                NotifyOfPropertyChange(() => SuppliersProfit);
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

        public bool IsAddSupplierFlyoutOpen
        {
            get => _isSupplierFlyoutOpen;
            set
            {
                _isSupplierFlyoutOpen = value;
                NotifyOfPropertyChange(() => IsAddSupplierFlyoutOpen);
            }
        }

        public Supplier DataGridSelectedSupplier
        {
            get => _dataGridSelectedSupplier;
            set
            {
                _dataGridSelectedSupplier = value;
                NotifyOfPropertyChange(() => DataGridSelectedSupplier);
            }
        }

        public long SelectedSalesMan
        {
            get => _selectedSalesMan;
            set
            {
                _selectedSalesMan = value;
                NotifyOfPropertyChange(() => SelectedSalesMan);
            }
        }

        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
                NotifyOfPropertyChange(() => Suppliers);
            }
        }

        public ObservableCollection<SalesMan> SalesMen
        {
            get => _salesMen;
            set
            {
                _salesMen = value;
                NotifyOfPropertyChange(() => SalesMen);
            }
        }

        public ObservableCollection<User> Users { get; set; }

        public SuppliersViewModel()
        {
            Title = "الموردين";
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(Data.DBCollections.Suppliers).FindAll());
                SalesMen = new ObservableCollection<SalesMan>(db.GetCollection<SalesMan>(Data.DBCollections.SalesMen).FindAll());
                Users = new ObservableCollection<User>(db.GetCollection<User>(Data.DBCollections.Users).FindAll());
            }
            DebitCredit();
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

        private void DoSupplierPayAsync()
        {
            var result = MessageBox.MaterialInputBox.Show($"ادخل المبلغ الذى استلمته من المورد {DataGridSelectedSupplier.Name}", "تدفيع", true);
            if (string.IsNullOrWhiteSpace(result))
            {
                MessageBox.MaterialMessageBox.ShowWarning("لم تقم بادخال اى مبلغ لاضافته للرصيد", "ادخل مبلغ", true);
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal supplierpaymentamount);
                if (isvalidmoney)
                {
                    using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
                    var s = db.GetCollection<Supplier>(DBCollections.Suppliers).FindById(DataGridSelectedSupplier.Id);
                    s.Balance += supplierpaymentamount;
                    db.GetCollection<SupplierMove>(DBCollections.SuppliersMoves).Insert(new SupplierMove
                    {
                        Supplier = db.GetCollection<Supplier>(DBCollections.Suppliers).FindById(DataGridSelectedSupplier.Id),
                        Credit = supplierpaymentamount,
                        CreateDate = DateTime.Now,
                        //Creator = Core.ReadUserSession(),
                        EditDate = null,
                        Editor = null
                    });
                    MessageBox.MaterialMessageBox.Show($"تم استلام للمورد {DataGridSelectedSupplier.Name} مبلغ {supplierpaymentamount} جنية بنجاح", "تمت العملية", true);
                    Suppliers[Suppliers.IndexOf(DataGridSelectedSupplier)] = s;
                    DebitCredit();
                    DataGridSelectedSupplier = null;
                    SupplierId = 0;
                }
                else
                {
                    MessageBox.MaterialMessageBox.Show("ادخل مبلغ صحيح بعلامه عشرية واحدة", "خطاء فى المبلغ", true);
                }
            }
        }

        private bool CanPaySupplier()
        {
            return DataGridSelectedSupplier == null ? false : true;
        }

        private void DoPaySupplierAsync()
        {
            var result = MessageBox.MaterialInputBox.Show($"ادخل المبلغ الذى تريد تدفيعه للمورد {DataGridSelectedSupplier.Name}", "تدفيع", true);
            if (string.IsNullOrWhiteSpace(result))
            {
                MessageBox.MaterialMessageBox.ShowWarning("لم تقم بادخال اى مبلغ لاضافته للرصيد", "ادخل مبلغ", true);
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal supplierpaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                    {
                        var s = db.GetCollection<Supplier>(DBCollections.Suppliers).FindById(DataGridSelectedSupplier.Id);
                        s.Balance -= supplierpaymentamount;
                        db.GetCollection<SupplierMove>(DBCollections.SuppliersMoves).Insert(new SupplierMove
                        {
                            Supplier = db.GetCollection<Supplier>(DBCollections.Suppliers).FindById(DataGridSelectedSupplier.Id),
                            Debit = supplierpaymentamount,
                            CreateDate = DateTime.Now,
                            //Creator = Core.ReadUserSession(),
                            EditDate = null,
                            Editor = null
                        });
                        db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Insert(new TreasuryMove
                        {
                            Treasury = db.GetCollection<Treasury>(DBCollections.Treasuries).FindById(1),
                            Credit = supplierpaymentamount,
                            Notes = $"دفع المورد بكود {DataGridSelectedSupplier.Id} باسم {DataGridSelectedSupplier.Name}",
                            CreateDate = DateTime.Now,
                            //Creator = Core.ReadUserSession(),
                            EditDate = null,
                            Editor = null
                        });
                        MessageBox.MaterialMessageBox.Show($"تم دفع للمورد {DataGridSelectedSupplier.Name} مبلغ {supplierpaymentamount} جنية بنجاح", "تمت العملية", true);
                        Suppliers[Suppliers.IndexOf(DataGridSelectedSupplier)] = s;
                        DebitCredit();
                        DataGridSelectedSupplier = null;
                        SupplierId = 0;
                    }
                }
                else
                {
                    MessageBox.MaterialMessageBox.ShowWarning("ادخل مبلغ صحيح بعلامه عشرية واحدة", "خطاء فى المبلغ", true);
                }
            }
        }

        private bool CanAddSupplier()
        {
            return string.IsNullOrWhiteSpace(Name) || SelectedSalesMan < 1 ? false : true;
        }

        private void DoAddSupplier()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                var exist = db.GetCollection<Supplier>(DBCollections.Suppliers).Find(x => x.Name == Name).FirstOrDefault();
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
                        SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(SelectedSalesMan),
                        Notes = Notes,
                        CreateDate = DateTime.Now,
                        //Creator = Core.ReadUserSession(),
                        EditDate = null,
                        Editor = null
                    };
                    db.GetCollection<Supplier>(DBCollections.Suppliers).Insert(s);
                    Suppliers.Add(s);
                    MessageBox.MaterialMessageBox.Show("تم اضافة المورد بنجاح", "تمت العملية", true);
                    DebitCredit();
                }
                else
                {
                    MessageBox.MaterialMessageBox.ShowWarning("المورد موجود من قبل بالفعل", "موجود", true);
                }
            }
        }

        private bool CanEditSupplier()
        {
            return string.IsNullOrWhiteSpace(Name) || SupplierId < 1 || SelectedSalesMan < 1 || DataGridSelectedSupplier == null ? false : true;
        }

        private void DoEditSupplier()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            var s = db.GetCollection<Supplier>(DBCollections.Suppliers).FindById(DataGridSelectedSupplier.Id);
            s.Name = Name;
            s.Balance = Balance;
            s.Site = Site;
            s.Email = Email;
            s.Phone = Phone;
            s.Image = Image;
            s.SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(SelectedSalesMan);
            s.Notes = Notes;
            //s.Editor = Core.ReadUserSession();
            s.EditDate = DateTime.Now;
            db.GetCollection<Supplier>(DBCollections.Suppliers).Update(s);
            MessageBox.MaterialMessageBox.Show("تم تعديل المورد بنجاح", "تمت العملية", true);
            Suppliers[Suppliers.IndexOf(DataGridSelectedSupplier)] = s;
            DebitCredit();
            DataGridSelectedSupplier = null;
            SupplierId = 0;
        }

        private bool CanDeleteSupplier()
        {
            return DataGridSelectedSupplier == null || DataGridSelectedSupplier.Id == 1 ? false : true;
        }

        private void DoDeleteSupplier()
        {
            var result = MessageBox.MaterialMessageBox.ShowWithCancel($"هل انت متاكد من حذف المورد {DataGridSelectedSupplier.Name}", "حذف المورد", true);
            if (result == MessageBoxResult.OK)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    db.GetCollection<Supplier>(DBCollections.Suppliers).Delete(DataGridSelectedSupplier.Id);
                    Suppliers.Remove(DataGridSelectedSupplier);
                }
                MessageBox.MaterialMessageBox.Show("تم حذف المورد بنجاح", "تمت العملية", true);
                DebitCredit();
                DataGridSelectedSupplier = null;
            }
        }

        private bool CanSearch()
        {
            return string.IsNullOrWhiteSpace(SearchText) ? false : true;
        }

        private void DoSearch()
        {
            try
            {
                using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
                Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(DBCollections.Suppliers).Find(x => x.Name.Contains(SearchText)));
                if (Suppliers.Count < 1)
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

        private bool CanReloadAllSuppliers()
        {
            return true;
        }

        private void DoReloadAllSuppliers()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(DBCollections.Suppliers).FindAll());
            }
            DebitCredit();
        }

        private bool CanFillUI()
        {
            return DataGridSelectedSupplier == null ? false : true;
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
            IsAddSupplierFlyoutOpen = IsAddSupplierFlyoutOpen ? false : true;
        }
    }
}