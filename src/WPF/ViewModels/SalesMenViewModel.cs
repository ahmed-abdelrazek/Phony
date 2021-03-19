using LiteDB;
using Phony.Data.Models.Lite;
using Phony.WPF.Data;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TinyLittleMvvm;

namespace Phony.WPF.ViewModels
{
    public class SalesMenViewModel : BaseViewModelWithAnnotationValidation, IOnLoadedHandler
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
                _salesManId = value;
                NotifyOfPropertyChange(() => SalesManId);
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

        public string ChildName
        {
            get => _childName;
            set
            {
                _childName = value;
                NotifyOfPropertyChange(() => ChildName);
            }
        }

        public string ChildPrice
        {
            get => _childPrice;
            set
            {
                _childPrice = value;
                NotifyOfPropertyChange(() => ChildPrice);
            }
        }

        public string SalesMenCount
        {
            get => _salesMenCount;
            set
            {
                _salesMenCount = value;
                NotifyOfPropertyChange(() => SalesMenCount);
            }
        }

        public string SalesMenCredits
        {
            get => _salesMenPurchasePrice;
            set
            {
                _salesMenPurchasePrice = value;
                NotifyOfPropertyChange(() => SalesMenCredits);
            }
        }

        public string SalesMenDebits
        {
            get => _salesMenSalePrice;
            set
            {
                _salesMenSalePrice = value;
                NotifyOfPropertyChange(() => SalesMenDebits);
            }
        }

        public string SalesMenProfit
        {
            get => _salesMenProfit;
            set
            {
                _salesMenProfit = value;
                NotifyOfPropertyChange(() => SalesMenProfit);
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

        public bool FastResult
        {
            get => _fastResult;
            set
            {
                _fastResult = value;
                NotifyOfPropertyChange(() => FastResult);
            }
        }

        public bool OpenFastResult
        {
            get => _openFastResult;
            set
            {
                _openFastResult = value;
                NotifyOfPropertyChange(() => OpenFastResult);
            }
        }

        public bool IsAddSalesManFlyoutOpen
        {
            get => _isAddSalesManFlyoutOpen;
            set
            {
                _isAddSalesManFlyoutOpen = value;
                NotifyOfPropertyChange(() => IsAddSalesManFlyoutOpen);
            }
        }

        public SalesMan DataGridSelectedSalesMan
        {
            get => _dataGridSelectedSalesMan;
            set
            {
                _dataGridSelectedSalesMan = value;
                NotifyOfPropertyChange(() => DataGridSelectedSalesMan);

                ((RelayCommand)OpenEditSalesManFlyoutCommand).RaiseCanExecuteChanged();
                ((AsyncRelayCommand)DeleteSalesManCommand).RaiseCanExecuteChanged();
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

        public ICommand AddSalesManCommand { get; }
        public ICommand EditSalesManCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand OpenAddSalesManFlyoutCommand { get; }
        public ICommand OpenEditSalesManFlyoutCommand { get; }
        public ICommand DeleteSalesManCommand { get; }
        public ICommand ReloadAllSalesMenCommand { get; }
        public ICommand SalesManPayCommand { get; }
        public ICommand PaySalesManCommand { get; }
        public ICommand CloseFlyoutCommand { get; }

        public SalesMenViewModel()
        {
            Title = "المندوبين";

            AddSalesManCommand = new AsyncRelayCommand(DoAddSalesMan, CanAddSalesMan);
            EditSalesManCommand = new AsyncRelayCommand(DoEditSalesMan, CanEditSalesMan);
            SearchCommand = new RelayCommand(DoSearch, CanSearch);
            OpenAddSalesManFlyoutCommand = new RelayCommand(DoOpenAddSalesManFlyout, () => true);
            OpenEditSalesManFlyoutCommand = new RelayCommand(DoFillUI, CanFillUI);
            DeleteSalesManCommand = new AsyncRelayCommand(DoDeleteSalesMan, CanDeleteSalesMan);
            ReloadAllSalesMenCommand = new AsyncRelayCommand(DoReloadAllSalesMen, () => true);
            SalesManPayCommand = new AsyncRelayCommand(DoSalesManPayAsync, CanSalesManPay);
            PaySalesManCommand = new AsyncRelayCommand(DoPaySalesManAsync, CanPaySalesMan);
            CloseFlyoutCommand = new RelayCommand(DoCloseFlyout, () => true);
        }

        public async Task OnLoadedAsync()
        {
            await Task.Run(() =>
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    SalesMen = new ObservableCollection<SalesMan>(db.GetCollection<SalesMan>(DBCollections.SalesMen).FindAll());
                }
            });
            await DebitCredit();
        }

        async Task DebitCredit()
        {
            await Task.Run(() =>
            {
                decimal Debit = decimal.Round(SalesMen.Where(c => c.Balance < 0).Sum(i => i.Balance), 2);
                decimal Credit = decimal.Round(SalesMen.Where(c => c.Balance > 0).Sum(i => i.Balance), 2);

                SalesMenCount = $"مجموع العملاء: {SalesMen.Count}";
                SalesMenDebits = $"اجمالى لينا: {Math.Abs(Debit)}";
                SalesMenCredits = $"اجمالى علينا: {Math.Abs(Credit)}";
                SalesMenProfit = $"تقدير لصافى لينا: {Math.Abs(Debit) - Math.Abs(Credit)}";
            });
        }

        private bool CanSalesManPay()
        {
            return DataGridSelectedSalesMan != null;
        }

        private async Task DoSalesManPayAsync()
        {
            var result = MessageBox.MaterialInputBox.Show($"ادخل المبلغ الذى استلامته من المندوب {DataGridSelectedSalesMan.Name}", "تدفيع", true);
            if (string.IsNullOrWhiteSpace(result))
            {
                MessageBox.MaterialMessageBox.ShowWarning("لم تقم بادخال اى مبلغ لتدفيعه", "ادخل مبلغ", true);
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal SalesManpaymentamount);
                if (isvalidmoney)
                {
                    using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
                    var s = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id);
                    s.Balance += SalesManpaymentamount;
                    db.GetCollection<SalesMan>(DBCollections.SalesMen).Update(s);
                    db.GetCollection<SalesManMove>(DBCollections.SalesMenMoves).Insert(new SalesManMove
                    {
                        SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id),
                        Credit = SalesManpaymentamount,
                        CreatedAt = DateTime.Now,
                        Creator = CurrentUser,
                        Editor = CurrentUser
                    });
                    MessageBox.MaterialMessageBox.Show($"تم استلام مبلغ من {DataGridSelectedSalesMan.Name} و قدره {SalesManpaymentamount} جنية بنجاح", "تمت العملية", true);
                    SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = s;
                    await DebitCredit();
                    DataGridSelectedSalesMan = null;
                    SalesManId = 0;
                }
                else
                {
                    MessageBox.MaterialMessageBox.ShowError("ادخل مبلغ صحيح بعلامه عشرية واحدة", "خطأ فى المبلغ", true);
                }
            }
        }

        private bool CanPaySalesMan()
        {
            return DataGridSelectedSalesMan != null;
        }

        private async Task DoPaySalesManAsync()
        {
            var result = MessageBox.MaterialInputBox.Show($"ادخل المبلغ الذى تريد تدفيعه للمندوب {DataGridSelectedSalesMan.Name}", "تدفيع", true);
            if (string.IsNullOrWhiteSpace(result))
            {
                MessageBox.MaterialMessageBox.ShowWarning("لم تقم بادخال اى مبلغ لتدفيعه", "ادخل مبلغ", true);
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal SalesManpaymentamount);
                if (isvalidmoney)
                {
                    using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
                    var s = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id);
                    s.Balance -= SalesManpaymentamount;
                    db.GetCollection<SalesMan>(DBCollections.SalesMen).Update(s);
                    db.GetCollection<SalesManMove>(DBCollections.SalesMenMoves).Insert(new SalesManMove
                    {
                        SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id),
                        Debit = SalesManpaymentamount,
                        CreatedAt = DateTime.Now,
                        Creator = CurrentUser,
                        Editor = CurrentUser
                    });
                    db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Insert(new TreasuryMove
                    {
                        Treasury = db.GetCollection<Treasury>(DBCollections.Treasuries).FindById(1),
                        Credit = SalesManpaymentamount,
                        Notes = $"تدفيع المندوب بكود {DataGridSelectedSalesMan.Id} باسم {DataGridSelectedSalesMan.Name}",
                        CreatedAt = DateTime.Now,
                        Creator = CurrentUser,
                        Editor = CurrentUser
                    });
                    MessageBox.MaterialMessageBox.Show($"تم الدفع لـ {DataGridSelectedSalesMan.Name} مبلغ {SalesManpaymentamount} جنية بنجاح", "تمت العملية", true);
                    SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = s;
                    await DebitCredit();
                    DataGridSelectedSalesMan = null;
                    SalesManId = 0;
                }
                else
                {
                    MessageBox.MaterialMessageBox.ShowWarning("ادخل مبلغ صحيح بعلامه عشرية واحدة", "خطأ فى المبلغ", true);
                }
            }
        }

        private bool CanAddSalesMan()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private async Task DoAddSalesMan()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            var exist = db.GetCollection<SalesMan>(DBCollections.SalesMen).Find(x => x.Name == Name).FirstOrDefault();
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
                    CreatedAt = DateTime.Now,
                    Creator = CurrentUser,
                    Editor = CurrentUser
                };
                db.GetCollection<SalesMan>(DBCollections.SalesMen).Insert(c);
                SalesMen.Add(c);

                await DebitCredit();
                MessageBox.MaterialMessageBox.Show("تم اضافة المندوب بنجاح", "تمت العملية", true);

                DoClearUI();
            }
            else
            {
                MessageBox.MaterialMessageBox.ShowWarning("المندوب موجود من قبل بالفعل", "موجود", true);
            }
        }

        private bool CanEditSalesMan()
        {
            return !string.IsNullOrWhiteSpace(Name) && SalesManId != 0 && DataGridSelectedSalesMan != null;
        }

        private async Task DoEditSalesMan()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            var s = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id);
            s.Name = Name;
            s.Balance = Balance;
            s.Site = Site;
            s.Email = Email;
            s.Phone = Phone;
            s.Notes = Notes;
            s.Editor = CurrentUser;
            s.EditedAt = DateTime.Now;
            db.GetCollection<SalesMan>(DBCollections.SalesMen).Update(s);

            await DebitCredit();
            MessageBox.MaterialMessageBox.Show("تم تعديل المندوب بنجاح", "تمت العملية", true);
            SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = s;
            
            DataGridSelectedSalesMan = null;
            SalesManId = 0;
            DoClearUI();
        }

        private bool CanDeleteSalesMan()
        {
            return DataGridSelectedSalesMan != null && DataGridSelectedSalesMan.Id != 1;
        }

        private async Task DoDeleteSalesMan()
        {
            var result = MessageBox.MaterialMessageBox.ShowWithCancel($"هل انت متاكد من حذف المندوب {DataGridSelectedSalesMan.Name}", "حذف الصنف", true);
            if (result == MessageBoxResult.OK)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    db.GetCollection<SalesMan>(DBCollections.SalesMen).Delete(DataGridSelectedSalesMan.Id);
                    SalesMen.Remove(DataGridSelectedSalesMan);
                }

                await DebitCredit();
                MessageBox.MaterialMessageBox.Show("تم حذف المندوب بنجاح", "تمت العملية", true);
                
                DataGridSelectedSalesMan = null;
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
                SalesMen = new ObservableCollection<SalesMan>(db.GetCollection<SalesMan>(DBCollections.SalesMen).Find(x => x.Name.Contains(SearchText)));
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
                    MessageBox.MaterialMessageBox.ShowWarning("لم يتم العثور على شئ", "غير موجود", true);
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                MessageBox.MaterialMessageBox.ShowError("لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله", "خطأ", true);
            }
        }

        private async Task DoReloadAllSalesMen()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                SalesMen = new ObservableCollection<SalesMan>(db.GetCollection<SalesMan>(DBCollections.SalesMen).FindAll());
            }
            await DebitCredit();
        }

        private bool CanFillUI()
        {
            return DataGridSelectedSalesMan != null;
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
            DoOpenAddSalesManFlyout();
        }

        private void DoClearUI()
        {
            SalesManId = 0;
            Name = "";
            Balance = 0;
            Site = "";
            Email = "";
            Phone = "";
            Notes = "";
        }

        private void DoOpenAddSalesManFlyout()
        {
            IsAddSalesManFlyoutOpen = !IsAddSalesManFlyoutOpen;
        }

        private void DoCloseFlyout()
        {
            IsAddSalesManFlyoutOpen = false;
            DataGridSelectedSalesMan = null;
            DoClearUI();
        }
    }
}