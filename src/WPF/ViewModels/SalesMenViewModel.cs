using Caliburn.Micro;
using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.WPF.Data;
using Phony.WPF.Models;
using Phony.WPF.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.WPF.ViewModels
{
    public class SalesMenViewModel : Screen
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

        SalesMen Message = Application.Current.Windows.OfType<SalesMen>().FirstOrDefault();

        public SalesMenViewModel()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                SalesMen = new ObservableCollection<SalesMan>(db.GetCollection<SalesMan>(DBCollections.SalesMen).FindAll());
                Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users).FindAll());
            }
            DebitCredit();
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
            return DataGridSelectedSalesMan == null ? false : true;
        }

        private async void DoSalesManPayAsync()
        {
            var result = await Message.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى استلامته من المندوب {DataGridSelectedSalesMan.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await Message.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لتدفيعه");
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal SalesManpaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                    {
                        var s = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id);
                        s.Balance += SalesManpaymentamount;
                        db.GetCollection<SalesMan>(DBCollections.SalesMen).Update(s);
                        db.GetCollection<SalesManMove>(DBCollections.SalesMenMoves).Insert(new SalesManMove
                        {
                            SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id),
                            Credit = SalesManpaymentamount,
                            CreateDate = DateTime.Now,
                            //Creator = Core.ReadUserSession(),
                            EditDate = null,
                            Editor = null
                        });
                        await Message.ShowMessageAsync("تمت العملية", $"تم استلام مبلغ من {DataGridSelectedSalesMan.Name} و قدره {SalesManpaymentamount} جنية بنجاح");
                        SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = s;
                        DebitCredit();
                        DataGridSelectedSalesMan = null;
                        SalesManId = 0;
                    }
                }
                else
                {
                    await Message.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanPaySalesMan()
        {
            return DataGridSelectedSalesMan == null ? false : true;
        }

        private async void DoPaySalesManAsync()
        {
            var result = await Message.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى تريد تدفيعه للمندوب {DataGridSelectedSalesMan.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await Message.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لتدفيعه");
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal SalesManpaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                    {
                        var s = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id);
                        s.Balance -= SalesManpaymentamount;
                        db.GetCollection<SalesMan>(DBCollections.SalesMen).Update(s);
                        db.GetCollection<SalesManMove>(DBCollections.SalesMenMoves).Insert(new SalesManMove
                        {
                            SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id),
                            Debit = SalesManpaymentamount,
                            CreateDate = DateTime.Now,
                            //Creator = Core.ReadUserSession(),
                            EditDate = null,
                            Editor = null
                        });
                        db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Insert(new TreasuryMove
                        {
                            Treasury = db.GetCollection<Treasury>(DBCollections.Treasuries).FindById(1),
                            Credit = SalesManpaymentamount,
                            Notes = $"تدفيع المندوب بكود {DataGridSelectedSalesMan.Id} باسم {DataGridSelectedSalesMan.Name}",
                            CreateDate = DateTime.Now,
                            //Creator = Core.ReadUserSession(),
                            EditDate = null,
                            Editor = null
                        });
                        await Message.ShowMessageAsync("تمت العملية", $"تم الدفع لـ {DataGridSelectedSalesMan.Name} مبلغ {SalesManpaymentamount} جنية بنجاح");
                        SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = s;
                        DebitCredit();
                        DataGridSelectedSalesMan = null;
                        SalesManId = 0;
                    }
                }
                else
                {
                    await Message.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanAddSalesMan()
        {
            return string.IsNullOrWhiteSpace(Name) ? false : true;
        }

        private async void DoAddSalesMan()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
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
                        CreateDate = DateTime.Now,
                        //Creator = Core.ReadUserSession(),
                        EditDate = null,
                        Editor = null
                    };
                    db.GetCollection<SalesMan>(DBCollections.SalesMen).Insert(c);
                    SalesMen.Add(c);
                    await Message.ShowMessageAsync("تمت العملية", "تم اضافة المندوب بنجاح");
                    DebitCredit();
                }
                else
                {
                    await Message.ShowMessageAsync("موجود", "المندوب موجود من قبل بالفعل");
                }
            }
        }

        private bool CanEditSalesMan()
        {
            return string.IsNullOrWhiteSpace(Name) || SalesManId == 0 || DataGridSelectedSalesMan == null ? false : true;
        }

        private async void DoEditSalesMan()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                var s = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id);
                s.Name = Name;
                s.Balance = Balance;
                s.Site = Site;
                s.Email = Email;
                s.Phone = Phone;
                s.Notes = Notes;
                //s.Editor = Core.ReadUserSession();
                s.EditDate = DateTime.Now;
                db.GetCollection<SalesMan>(DBCollections.SalesMen).Update(s);
                await Message.ShowMessageAsync("تمت العملية", "تم تعديل المندوب بنجاح");
                SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = s;
                DebitCredit();
                DataGridSelectedSalesMan = null;
                SalesManId = 0;
            }
        }

        private bool CanDeleteSalesMan()
        {
            return DataGridSelectedSalesMan == null || DataGridSelectedSalesMan.Id == 1 ? false : true;
        }

        private async void DoDeleteSalesMan()
        {
            var result = await Message.ShowMessageAsync("حذف الصنف", $"هل انت متاكد من حذف المندوب {DataGridSelectedSalesMan.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    db.GetCollection<SalesMan>(DBCollections.SalesMen).Delete(DataGridSelectedSalesMan.Id);
                    SalesMen.Remove(DataGridSelectedSalesMan);
                }
                await Message.ShowMessageAsync("تمت العملية", "تم حذف المندوب بنجاح");
                DebitCredit();
                DataGridSelectedSalesMan = null;
            }
        }

        private bool CanSearch()
        {
            return string.IsNullOrWhiteSpace(SearchText) ? false : true;
        }

        private async void DoSearch()
        {
            try
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
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
                        await Message.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                    }
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                await Message.ShowMessageAsync("خطأ", "لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله");
            }
        }

        private bool CanReloadAllSalesMen()
        {
            return true;
        }

        private void DoReloadAllSalesMen()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                SalesMen = new ObservableCollection<SalesMan>(db.GetCollection<SalesMan>(DBCollections.SalesMen).FindAll());
            }
            DebitCredit();
        }

        private bool CanFillUI()
        {
            return DataGridSelectedSalesMan == null ? false : true;
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
            IsAddSalesManFlyoutOpen = IsAddSalesManFlyoutOpen ? false : true;
        }
    }
}