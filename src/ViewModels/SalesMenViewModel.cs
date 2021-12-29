﻿using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
using Phony.DTOs;
using Phony.Helpers;
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
    public class SalesMenViewModel : BindableBase
    {
        long _salesManId;
        string _name;
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
        SalesManDto _dataGridSelectedSalesMan;

        ObservableCollection<SalesManDto> _salesMen;

        public long SalesManId
        {
            get => _salesManId;
            set => SetProperty(ref _salesManId, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
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

        public string ChildName
        {
            get => _childName;
            set => SetProperty(ref _childName, value);
        }

        public string ChildPrice
        {
            get => _childPrice;
            set => SetProperty(ref _childPrice, value);
        }

        public string SalesMenCount
        {
            get => _salesMenCount;
            set => SetProperty(ref _salesMenCount, value);
        }

        public string SalesMenCredits
        {
            get => _salesMenPurchasePrice;
            set => SetProperty(ref _salesMenPurchasePrice, value);
        }

        public string SalesMenDebits
        {
            get => _salesMenSalePrice;
            set => SetProperty(ref _salesMenSalePrice, value);
        }

        public string SalesMenProfit
        {
            get => _salesMenProfit;
            set => SetProperty(ref _salesMenProfit, value);
        }

        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        public bool FastResult
        {
            get => _fastResult;
            set => SetProperty(ref _fastResult, value);
        }

        public bool OpenFastResult
        {
            get => _openFastResult;
            set => SetProperty(ref _openFastResult, value);
        }

        public bool IsAddSalesManFlyoutOpen
        {
            get => _isAddSalesManFlyoutOpen;
            set => SetProperty(ref _isAddSalesManFlyoutOpen, value);
        }

        public SalesManDto DataGridSelectedSalesMan
        {
            get => _dataGridSelectedSalesMan;
            set => SetProperty(ref _dataGridSelectedSalesMan, value);
        }

        public ObservableCollection<SalesManDto> SalesMen
        {
            get => _salesMen;
            set => SetProperty(ref _salesMen, value);
        }

        public ICommand SalesManPay { get; set; }
        public ICommand PaySalesMan { get; set; }
        public ICommand AddSalesMan { get; set; }
        public ICommand EditSalesMan { get; set; }
        public ICommand DeleteSalesMan { get; set; }
        public ICommand Search { get; set; }
        public ICommand OpenAddSalesManFlyout { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand ReloadAllSalesMen { get; set; }

        private readonly SalesMen Message = Application.Current.Windows.OfType<SalesMen>().FirstOrDefault();

        public SalesMenViewModel()
        {
            LoadCommands();
            using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
            {
                SalesMen = ObjectMapper.Mapper.Map<ObservableCollection<SalesManDto>>(db.GetCollection<SalesMan>(DBCollections.SalesMen).Include(x => x.Creator).Include(x => x.Editor).FindAll());
            }
            DebitCredit();
        }

        public void LoadCommands()
        {
            SalesManPay = new DelegateCommand(DoSalesManPayAsync, CanSalesManPay).ObservesProperty(() => DataGridSelectedSalesMan);
            PaySalesMan = new DelegateCommand(DoPaySalesManAsync, CanPaySalesMan).ObservesProperty(() => DataGridSelectedSalesMan);
            AddSalesMan = new DelegateCommand(DoAddSalesMan, CanAddSalesMan).ObservesProperty(() => Name);
            EditSalesMan = new DelegateCommand(DoEditSalesMan, CanEditSalesMan).ObservesProperty(() => Name).ObservesProperty(() => SalesManId).ObservesProperty(() => DataGridSelectedSalesMan);
            DeleteSalesMan = new DelegateCommand(DoDeleteSalesMan, CanDeleteSalesMan).ObservesProperty(() => DataGridSelectedSalesMan);
            Search = new DelegateCommand(DoSearch, CanSearch).ObservesProperty(() => SearchText);
            OpenAddSalesManFlyout = new DelegateCommand(DoOpenAddSalesManFlyout, CanOpenAddSalesManFlyout);
            FillUI = new DelegateCommand(DoFillUI, CanFillUI).ObservesProperty(() => DataGridSelectedSalesMan);
            ReloadAllSalesMen = new DelegateCommand(DoReloadAllSalesMen, CanReloadAllSalesMen);
        }

        async void DebitCredit()
        {
            decimal Debit = decimal.Round(SalesMen.Where(c => c.Balance < 0).Sum(i => i.Balance), 2);
            decimal Credit = decimal.Round(SalesMen.Where(c => c.Balance > 0).Sum(i => i.Balance), 2);
            await Task.Run(() =>
            {
                SalesMenCount = $"مجموع العملاء: {SalesMen.Count}";
            });
            await Task.Run(() =>
            {
                SalesMenDebits = $"اجمالى لينا: {Math.Abs(Debit)}";
            });
            await Task.Run(() =>
            {
                SalesMenCredits = $"اجمالى علينا: {Math.Abs(Credit)}";
            });
            await Task.Run(() =>
            {
                SalesMenProfit = $"تقدير لصافى لينا: {(Math.Abs(Debit) - Math.Abs(Credit))}";
            });
        }

        private bool CanSalesManPay()
        {
            return DataGridSelectedSalesMan is not null;
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
                    using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                    var s = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id);
                    s.Balance += SalesManpaymentamount;
                    db.GetCollection<SalesMan>(DBCollections.SalesMen).Update(s);
                    db.GetCollection<SalesManMove>(DBCollections.SalesMenMoves).Insert(new SalesManMove
                    {
                        SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id),
                        Credit = SalesManpaymentamount,
                        CreateDate = DateTime.Now,
                        Creator = Core.ReadUserSession(),
                        EditDate = null,
                        Editor = null
                    });
                    await Message.ShowMessageAsync("تمت العملية", $"تم استلام مبلغ من {DataGridSelectedSalesMan.Name} و قدره {SalesManpaymentamount} جنية بنجاح");
                    SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = ObjectMapper.Mapper.Map<SalesManDto>(s);
                    DebitCredit();
                    DataGridSelectedSalesMan = null;
                    SalesManId = 0;
                }
                else
                {
                    await Message.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanPaySalesMan()
        {
            return DataGridSelectedSalesMan is not null;
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
                    using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                    var salesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id);
                    salesMan.Balance -= SalesManpaymentamount;
                    db.GetCollection<SalesMan>(DBCollections.SalesMen).Update(salesMan);
                    db.GetCollection<SalesManMove>(DBCollections.SalesMenMoves).Insert(new SalesManMove
                    {
                        SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id),
                        Debit = SalesManpaymentamount,
                        CreateDate = DateTime.Now,
                        Creator = Core.ReadUserSession(),
                        EditDate = null,
                        Editor = null
                    });
                    db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Insert(new TreasuryMove
                    {
                        Treasury = db.GetCollection<Treasury>(DBCollections.Treasuries).FindById(1),
                        Credit = SalesManpaymentamount,
                        Notes = $"تدفيع المندوب بكود {DataGridSelectedSalesMan.Id} باسم {DataGridSelectedSalesMan.Name}",
                        CreateDate = DateTime.Now,
                        Creator = Core.ReadUserSession(),
                        EditDate = null,
                        Editor = null
                    });
                    await Message.ShowMessageAsync("تمت العملية", $"تم الدفع لـ {DataGridSelectedSalesMan.Name} مبلغ {SalesManpaymentamount} جنية بنجاح");
                    SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = ObjectMapper.Mapper.Map<SalesManDto>(salesMan);
                    DebitCredit();
                    DataGridSelectedSalesMan = null;
                    SalesManId = 0;
                }
                else
                {
                    await Message.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanAddSalesMan()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private async void DoAddSalesMan()
        {
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
            var exist = db.GetCollection<SalesMan>(DBCollections.SalesMen).Find(x => x.Name == Name).FirstOrDefault();
            if (exist is null)
            {
                var salesMan = new SalesMan
                {
                    Name = Name,
                    Balance = Balance,
                    Email = Email,
                    Phone = Phone,
                    Notes = Notes,
                    CreateDate = DateTime.Now,
                    Creator = Core.ReadUserSession(),
                    EditDate = null,
                    Editor = null
                };
                db.GetCollection<SalesMan>(DBCollections.SalesMen).Insert(salesMan);
                SalesMen.Add(ObjectMapper.Mapper.Map<SalesManDto>(salesMan));
                await Message.ShowMessageAsync("تمت العملية", "تم اضافة المندوب بنجاح");
                DebitCredit();
            }
            else
            {
                await Message.ShowMessageAsync("موجود", "المندوب موجود من قبل بالفعل");
            }
        }

        private bool CanEditSalesMan()
        {
            return !string.IsNullOrWhiteSpace(Name) && SalesManId != 0 && DataGridSelectedSalesMan is not null;
        }

        private async void DoEditSalesMan()
        {
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
            var salesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(DataGridSelectedSalesMan.Id);
            salesMan.Name = Name;
            salesMan.Balance = Balance;
            salesMan.Email = Email;
            salesMan.Phone = Phone;
            salesMan.Notes = Notes;
            salesMan.Editor = Core.ReadUserSession();
            salesMan.EditDate = DateTime.Now;
            db.GetCollection<SalesMan>(DBCollections.SalesMen).Update(salesMan);
            await Message.ShowMessageAsync("تمت العملية", "تم تعديل المندوب بنجاح");
            SalesMen[SalesMen.IndexOf(DataGridSelectedSalesMan)] = ObjectMapper.Mapper.Map<SalesManDto>(salesMan);
            DebitCredit();
            DataGridSelectedSalesMan = null;
            SalesManId = 0;
        }

        private bool CanDeleteSalesMan()
        {
            return DataGridSelectedSalesMan is not null && DataGridSelectedSalesMan.Id != 1;
        }

        private async void DoDeleteSalesMan()
        {
            var result = await Message.ShowMessageAsync("حذف الصنف", $"هل انت متاكد من حذف المندوب {DataGridSelectedSalesMan.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
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
            return !string.IsNullOrWhiteSpace(SearchText);
        }

        private async void DoSearch()
        {
            try
            {
                using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                SalesMen = ObjectMapper.Mapper.Map<ObservableCollection<SalesManDto>>(db.GetCollection<SalesMan>(DBCollections.SalesMen).Include(x => x.Creator).Include(x => x.Editor).Find(x => x.Name.Contains(SearchText)));
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
            using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
            {
                SalesMen = ObjectMapper.Mapper.Map<ObservableCollection<SalesManDto>>(db.GetCollection<SalesMan>(DBCollections.SalesMen).Include(x => x.Creator).Include(x => x.Editor).FindAll());
            }
            DebitCredit();
        }

        private bool CanFillUI()
        {
            return DataGridSelectedSalesMan is not null;
        }

        private void DoFillUI()
        {
            SalesManId = DataGridSelectedSalesMan.Id;
            Name = DataGridSelectedSalesMan.Name;
            Balance = DataGridSelectedSalesMan.Balance;
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
            IsAddSalesManFlyoutOpen = !IsAddSalesManFlyoutOpen;
        }
    }
}