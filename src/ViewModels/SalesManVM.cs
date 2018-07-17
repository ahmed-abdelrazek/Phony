using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Models;
using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class SalesManVM : BindableBase
    {
        long _salesManId;
        string _name;
        string _site;
        string _email;
        string _phone;
        string _notes;
        string _searchText;
        string _childName;
        string _childPrice;
        static string _salesMenCount;
        static string _salesMenPurchasePrice;
        static string _salesMenSalePrice;
        static string _salesMenProfit;
        decimal _balance;
        bool _fastResult;
        bool _openFastResult;
        bool _isAddSalesManFlyoutOpen;
        SalesMan _dataGridSelectedSalesMan;

        ObservableCollection<SalesMan> _salesMen;

        public long SalesManId
        {
            get => _salesManId;
            set
            {
                if (value != _salesManId)
                {
                    _salesManId = value;
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

        public string ChildName
        {
            get => _childName;
            set
            {
                if (value != _childName)
                {
                    _childName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ChildPrice
        {
            get => _childPrice;
            set
            {
                if (value != _childPrice)
                {
                    _childPrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SalesMenCount
        {
            get => _salesMenCount;
            set
            {
                if (value != _salesMenCount)
                {
                    _salesMenCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SalesMenCredits
        {
            get => _salesMenPurchasePrice;
            set
            {
                if (value != _salesMenPurchasePrice)
                {
                    _salesMenPurchasePrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SalesMenDebits
        {
            get => _salesMenSalePrice;
            set
            {
                if (value != _salesMenSalePrice)
                {
                    _salesMenSalePrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SalesMenProfit
        {
            get => _salesMenProfit;
            set
            {
                if (value != _salesMenProfit)
                {
                    _salesMenProfit = value;
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

        public bool FastResult
        {
            get => _fastResult;
            set
            {
                if (value != _fastResult)
                {
                    _fastResult = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool OpenFastResult
        {
            get => _openFastResult;
            set
            {
                if (value != _openFastResult)
                {
                    _openFastResult = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsAddSalesManFlyoutOpen
        {
            get => _isAddSalesManFlyoutOpen;
            set
            {
                if (value != _isAddSalesManFlyoutOpen)
                {
                    _isAddSalesManFlyoutOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public SalesMan DataGridSelectedSalesMan
        {
            get => _dataGridSelectedSalesMan;
            set
            {
                if (value != _dataGridSelectedSalesMan)
                {
                    _dataGridSelectedSalesMan = value;
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

        public ICommand SalesManPay { get; set; }
        public ICommand PaySalesMan { get; set; }
        public ICommand AddSalesMan { get; set; }
        public ICommand EditSalesMan { get; set; }
        public ICommand DeleteSalesMan { get; set; }
        public ICommand Search { get; set; }
        public ICommand OpenAddSalesManFlyout { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand ReloadAllSalesMen { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        SalesMen SalesMenMessage = Application.Current.Windows.OfType<SalesMen>().FirstOrDefault();

        public SalesManVM()
        {
            LoadCommands();
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                SalesMen = new ObservableCollection<SalesMan>(db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).FindAll());
                Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users.ToString()).FindAll());
            }
            DebitCredit();
        }

        public void LoadCommands()
        {
            Search = new DelegateCommand(DoSearch, CanSearch);
            OpenAddSalesManFlyout = new DelegateCommand(DoOpenAddSalesManFlyout, CanOpenAddSalesManFlyout);
            FillUI = new DelegateCommand(DoFillUI, CanFillUI);
            SalesManPay = new DelegateCommand(DoSalesManPayAsync, CanSalesManPay);
            PaySalesMan = new DelegateCommand(DoPaySalesManAsync, CanPaySalesMan);
            ReloadAllSalesMen = new DelegateCommand(DoReloadAllSalesMen, CanReloadAllSalesMen);
            AddSalesMan = new DelegateCommand(DoAddSalesMan, CanAddSalesMan);
            EditSalesMan = new DelegateCommand(DoEditSalesMan, CanEditSalesMan);
            DeleteSalesMan = new DelegateCommand(DoDeleteSalesMan, CanDeleteSalesMan);
        }

        async void DebitCredit()
        {
            decimal Debit = decimal.Round(SalesMen.Where(c => c.Balance < 0).Sum(i => i.Balance), 2);
            decimal Credit = decimal.Round(SalesMen.Where(c => c.Balance > 0).Sum(i => i.Balance), 2);
            await Task.Run(() =>
            {
                SalesMenCount = $"مجموع العملاء: {SalesMen.Count().ToString()}";
            });
            await Task.Run(() =>
            {
                SalesMenDebits = $"اجمالى لينا: {Math.Abs(Debit).ToString()}";
            });
            await Task.Run(() =>
            {
                SalesMenCredits = $"اجمالى علينا: {Math.Abs(Credit).ToString()}";
            });
            await Task.Run(() =>
            {
                SalesMenProfit = $"تقدير لصافى لينا: {(Math.Abs(Debit) - Math.Abs(Credit)).ToString()}";
            });
        }

        private bool CanSalesManPay()
        {
            if (DataGridSelectedSalesMan == null)
            {
                return false;
            }
            return true;
        }

        private async void DoSalesManPayAsync()
        {
            var result = await SalesMenMessage.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى استلامته من المندوب {DataGridSelectedSalesMan.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await SalesMenMessage.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لتدفيعه");
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal SalesManpaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                    {
                        var s = db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).FindById(DataGridSelectedSalesMan.Id);
                        s.Balance += SalesManpaymentamount;
                        db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).Update(s);
                        db.GetCollection<SalesManMove>(DBCollections.SalesMenMoves.ToString()).Insert(new SalesManMove
                        {
                            SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).FindById(DataGridSelectedSalesMan.Id),
                            Credit = SalesManpaymentamount,
                            CreateDate = DateTime.Now,
                            Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                            EditDate = null,
                            Editor = null
                        });
                        await SalesMenMessage.ShowMessageAsync("تمت العملية", $"تم استلام مبلغ من {DataGridSelectedSalesMan.Name} و قدره {SalesManpaymentamount} جنية بنجاح");
                        SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = s;
                        DebitCredit();
                        DataGridSelectedSalesMan = null;
                        SalesManId = 0;
                    }
                }
                else
                {
                    await SalesMenMessage.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanPaySalesMan()
        {
            if (DataGridSelectedSalesMan == null)
            {
                return false;
            }
            return true;
        }

        private async void DoPaySalesManAsync()
        {
            var result = await SalesMenMessage.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى تريد تدفيعه للمندوب {DataGridSelectedSalesMan.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await SalesMenMessage.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لتدفيعه");
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal SalesManpaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                    {
                        var s = db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).FindById(DataGridSelectedSalesMan.Id);
                        s.Balance -= SalesManpaymentamount;
                        db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).Update(s);
                        db.GetCollection<SalesManMove>(DBCollections.SalesMenMoves.ToString()).Insert(new SalesManMove
                        {
                            SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).FindById(DataGridSelectedSalesMan.Id),
                            Debit = SalesManpaymentamount,
                            CreateDate = DateTime.Now,
                            Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                            EditDate = null,
                            Editor = null
                        });
                        db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves.ToString()).Insert(new TreasuryMove
                        {
                            Treasury = db.GetCollection<Treasury>(DBCollections.Treasuries.ToString()).FindById(1),
                            Credit = SalesManpaymentamount,
                            Notes = $"تدفيع المندوب بكود {DataGridSelectedSalesMan.Id} باسم {DataGridSelectedSalesMan.Name}",
                            CreateDate = DateTime.Now,
                            Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                            EditDate = null,
                            Editor = null
                        });
                        await SalesMenMessage.ShowMessageAsync("تمت العملية", $"تم الدفع لـ {DataGridSelectedSalesMan.Name} مبلغ {SalesManpaymentamount} جنية بنجاح");
                        SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = s;
                        DebitCredit();
                        DataGridSelectedSalesMan = null;
                        SalesManId = 0;
                    }
                }
                else
                {
                    await SalesMenMessage.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanAddSalesMan()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }

        private void DoAddSalesMan()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var exist = db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).Find(x => x.Name == Name).FirstOrDefault();
                if (exist == null)
                {
                    var c = new SalesMan
                    {
                        Name = Name,
                        Balance = Balance,
                        Site = Site,
                        Email = Email,
                        Phone = Phone,
                        Notes = Notes,
                        CreateDate = DateTime.Now,
                        Creator = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id),
                        EditDate = null,
                        Editor = null
                    };
                    db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).Insert(c);
                    SalesMen.Add(c);
                    SalesMenMessage.ShowMessageAsync("تمت العملية", "تم اضافة المندوب بنجاح");
                    DebitCredit();
                }
                else
                {
                    SalesMenMessage.ShowMessageAsync("موجود", "المندوب موجود من قبل بالفعل");
                }
            }
        }

        private bool CanEditSalesMan()
        {
            if (string.IsNullOrWhiteSpace(Name) || SalesManId == 0 || DataGridSelectedSalesMan == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditSalesMan()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var s = db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).FindById(DataGridSelectedSalesMan.Id);
                s.Name = Name;
                s.Balance = Balance;
                s.Site = Site;
                s.Email = Email;
                s.Phone = Phone;
                s.Notes = Notes;
                s.EditDate = DateTime.Now;
                s.Editor = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id);
                db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).Update(s);
                SalesMenMessage.ShowMessageAsync("تمت العملية", "تم تعديل المندوب بنجاح");
                SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = s;
                DebitCredit();
                DataGridSelectedSalesMan = null;
                SalesManId = 0;
            }
        }

        private bool CanDeleteSalesMan()
        {
            if (DataGridSelectedSalesMan == null || DataGridSelectedSalesMan.Id == 1)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteSalesMan()
        {
            var result = await SalesMenMessage.ShowMessageAsync("حذف الصنف", $"هل انت متاكد من حذف المندوب {DataGridSelectedSalesMan.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).Delete(DataGridSelectedSalesMan.Id);
                    SalesMen.Remove(DataGridSelectedSalesMan);
                }
                await SalesMenMessage.ShowMessageAsync("تمت العملية", "تم حذف المندوب بنجاح");
                DebitCredit();
                DataGridSelectedSalesMan = null;
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
                    SalesMen = new ObservableCollection<SalesMan>(db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).Find(x => x.Name.Contains(SearchText)));
                    if (SalesMen.Count > 0)
                    {
                        if (FastResult)
                        {
                            ChildName = SalesMen.FirstOrDefault().Name;
                            ChildPrice = SalesMen.FirstOrDefault().Balance.ToString();
                            OpenFastResult = true;
                        }
                    }
                    else
                    {
                        SalesMenMessage.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                    }
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله");
            }
        }

        private bool CanReloadAllSalesMen()
        {
            return true;
        }

        private void DoReloadAllSalesMen()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                SalesMen = new ObservableCollection<SalesMan>(db.GetCollection<SalesMan>(DBCollections.SalesMen.ToString()).FindAll());
            }
            DebitCredit();
        }

        private bool CanFillUI()
        {
            if (DataGridSelectedSalesMan == null)
            {
                return false;
            }
            return true;
        }

        private void DoFillUI()
        {
            SalesManId = DataGridSelectedSalesMan.Id;
            Name = DataGridSelectedSalesMan.Name;
            Balance = DataGridSelectedSalesMan.Balance;
            Site = DataGridSelectedSalesMan.Site;
            Email = DataGridSelectedSalesMan.Email;
            Phone = DataGridSelectedSalesMan.Phone;
            Notes = DataGridSelectedSalesMan.Notes;
            IsAddSalesManFlyoutOpen = true;
        }

        private bool CanOpenAddSalesManFlyout()
        {
            return true;
        }

        private void DoOpenAddSalesManFlyout()
        {
            if (IsAddSalesManFlyoutOpen)
            {
                IsAddSalesManFlyoutOpen = false;
            }
            else
            {
                IsAddSalesManFlyoutOpen = true;
            }
        }
    }
}