using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Model;
using Phony.Persistence;
using Phony.Utility;
using Phony.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModel
{
    public class SalesManVM : CommonBase
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

        public ICommand Search { get; set; }
        public ICommand OpenAddSalesManFlyout { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand SalesManPay { get; set; }
        public ICommand ReloadAllSalesMen { get; set; }
        public ICommand AddSalesMan { get; set; }
        public ICommand EditSalesMan { get; set; }
        public ICommand DeleteSalesMan { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        SalesMen SalesMenMessage = Application.Current.Windows.OfType<SalesMen>().FirstOrDefault();

        public SalesManVM()
        {
            LoadCommands();
            using (var db = new PhonyDbContext())
            {
                SalesMen = new ObservableCollection<SalesMan>(db.SalesMen);
                Users = new ObservableCollection<User>(db.Users);
            }
            new Thread(() =>
            {
                SalesMenCount = $"مجموع المندوبين: {SalesMen.Count().ToString()}";
                SalesMenDebits = $"اجمالى لينا: {decimal.Round(SalesMen.Where(c => c.Balance > 0).Sum(i => i.Balance), 2).ToString()}";
                SalesMenCredits = $"اجمالى علينا: {decimal.Round(SalesMen.Where(c => c.Balance < 0).Sum(i => i.Balance), 2).ToString()}";
                SalesMenProfit = $"تقدير لصافى لينا: {decimal.Round((SalesMen.Where(c => c.Balance > 0).Sum(i => i.Balance) + SalesMen.Where(c => c.Balance < 0).Sum(i => i.Balance)), 2).ToString()}";
            }).Start();
        }

        public void LoadCommands()
        {
            Search = new CustomCommand(DoSearch, CanSearch);
            OpenAddSalesManFlyout = new CustomCommand(DoOpenAddSalesManFlyout, CanOpenAddSalesManFlyout);
            FillUI = new CustomCommand(DoFillUI, CanFillUI);
            SalesManPay = new CustomCommand(DoSalesManPayAsync, CanSalesManPay);
            ReloadAllSalesMen = new CustomCommand(DoReloadAllSalesMen, CanReloadAllSalesMen);
            AddSalesMan = new CustomCommand(DoAddSalesMan, CanAddSalesMan);
            EditSalesMan = new CustomCommand(DoEditSalesMan, CanEditSalesMan);
            DeleteSalesMan = new CustomCommand(DoDeleteSalesMan, CanDeleteSalesMan);
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
                using (var db = new PhonyDbContext())
                {
                    SalesMen = new ObservableCollection<SalesMan>(db.SalesMen.Where(i => i.Name.Contains(SearchText)));
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

        private bool CanReloadAllSalesMen(object obj)
        {
            return true;
        }

        private void DoReloadAllSalesMen(object obj)
        {
            using (var db = new PhonyDbContext())
            {
                SalesMen = new ObservableCollection<SalesMan>(db.SalesMen);
            }
        }

        private bool CanFillUI(object obj)
        {
            if (DataGridSelectedSalesMan == null)
            {
                return false;
            }
            return true;
        }

        private void DoFillUI(object obj)
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

        private bool CanSalesManPay(object obj)
        {
            if (DataGridSelectedSalesMan == null)
            {
                return false;
            }
            return true;
        }

        private async void DoSalesManPayAsync(object obj)
        {
            var result = await SalesMenMessage.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى تريد تدفيعه للمندوب {DataGridSelectedSalesMan.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await SalesMenMessage.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لتدفيعه");
            }
            else
            {
                decimal SalesManpaymentamount;
                bool isvalidmoney = decimal.TryParse(result, out SalesManpaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new UnitOfWork(new PhonyDbContext()))
                    {
                        var s = db.SalesMen.Get(DataGridSelectedSalesMan.Id);
                        s.Balance -= SalesManpaymentamount;
                        s.EditDate = DateTime.Now;
                        s.EditById = CurrentUser.Id;
                        var sm = new SalesManMove
                        {
                            SalesManId = DataGridSelectedSalesMan.Id,
                            Amount = SalesManpaymentamount,
                            CreateDate = DateTime.Now,
                            CreatedById = CurrentUser.Id,
                            EditDate = null,
                            EditById = null
                        };
                        db.SalesMenMoves.Add(sm);
                        if (SalesManpaymentamount > 0)
                        {
                            db.TreasuriesMoves.Add(new TreasuryMove
                            {
                                TreasuryId = 1,
                                In = SalesManpaymentamount,
                                Out = 0,
                                Notes = $"تدفيع المندوب بكود {DataGridSelectedSalesMan.Id} باسم {DataGridSelectedSalesMan.Name}",
                                CreateDate = DateTime.Now,
                                CreatedById = CurrentUser.Id
                            });
                        }
                        else
                        {
                            db.TreasuriesMoves.Add(new TreasuryMove
                            {
                                TreasuryId = 1,
                                In = 0,
                                Out = SalesManpaymentamount,
                                Notes = $"استلام من المندوب بكود {DataGridSelectedSalesMan.Id} باسم {DataGridSelectedSalesMan.Name}",
                                CreateDate = DateTime.Now,
                                CreatedById = CurrentUser.Id
                            });
                        }
                        db.Complete();
                        await SalesMenMessage.ShowMessageAsync("تمت العملية", $"تم تدفيع {DataGridSelectedSalesMan.Name} مبلغ {SalesManpaymentamount} جنية بنجاح");
                        SalesManId = 0;
                        SalesMen.Remove(DataGridSelectedSalesMan);
                        SalesMen.Add(s);
                        DataGridSelectedSalesMan = null;
                    }
                }
                else
                {
                    await SalesMenMessage.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanOpenAddSalesManFlyout(object obj)
        {
            return true;
        }

        private void DoOpenAddSalesManFlyout(object obj)
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

        private bool CanAddSalesMan(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }

        private void DoAddSalesMan(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
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
                    CreatedById = CurrentUser.Id,
                    EditDate = null,
                    EditById = null
                };
                db.SalesMen.Add(c);
                db.Complete();
                SalesMen.Add(c);
                SalesMenMessage.ShowMessageAsync("تمت العملية", "تم اضافة المندوب بنجاح");
            }
        }

        private bool CanEditSalesMan(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || SalesManId == 0 || DataGridSelectedSalesMan == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditSalesMan(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var s = db.SalesMen.Get(DataGridSelectedSalesMan.Id);
                s.Name = Name;
                s.Balance = Balance;
                s.Site = Site;
                s.Email = Email;
                s.Phone = Phone;
                s.Notes = Notes;
                s.EditDate = DateTime.Now;
                s.EditById = CurrentUser.Id;
                db.Complete();
                SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = s;
                SalesManId = 0;
                DataGridSelectedSalesMan = null;
                SalesMenMessage.ShowMessageAsync("تمت العملية", "تم تعديل المندوب بنجاح");
            }
        }

        private bool CanDeleteSalesMan(object obj)
        {
            if (DataGridSelectedSalesMan == null || DataGridSelectedSalesMan.Id == 1)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteSalesMan(object obj)
        {
            var result = await SalesMenMessage.ShowMessageAsync("حذف الصنف", $"هل انت متاكد من حذف المندوب {DataGridSelectedSalesMan.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new UnitOfWork(new PhonyDbContext()))
                {
                    db.SalesMen.Remove(db.SalesMen.Get(DataGridSelectedSalesMan.Id));
                    db.Complete();
                    SalesMen.Remove(DataGridSelectedSalesMan);
                }
                DataGridSelectedSalesMan = null;
                await SalesMenMessage.ShowMessageAsync("تمت العملية", "تم حذف المندوب بنجاح");
            }
        }
    }
}