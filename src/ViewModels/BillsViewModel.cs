using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
using Phony.Models;
using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class BillsViewModel : BindableBase
    {
        long _searchSelectedValue;
        decimal _itemChildItemPrice;
        decimal _itemChildItemQTYExist;
        decimal _itemChildItemQTYSell;
        decimal _serviceChildServiceBalance;
        decimal _serviceChildServiceCost;
        decimal _childDiscount;
        decimal _billTotal;
        decimal _billTotalAfterEachDiscount;
        decimal _billDiscount;
        decimal _billTotalAfterDiscount;
        decimal _billClientPayment;
        decimal _billClientPaymentChange;
        long _currentBillNo;
        string? _searchText;
        string? _itemChildItemName;
        string? _serviceChildServiceName;
        string? _serviceChildNotes;
        string? _itemChildNotes;
        bool _byItem;
        bool _byCard;
        bool _byService;
        bool _byName;
        bool _byShopCode;
        bool _byBarCode;
        bool _isAddItemChildOpen;
        bool _isAddServiceChildOpen;
        bool _isAddBillNote;
        bool _isSearchDropDownOpen;
        Client? _selectedClient;
        Item? _selectedItem;
        Service? _selectedService;
        BillItemMove? _dataGridSelectedBillItemMove;
        BillServiceMove? _dataGridSelectedBillServiceMove;

        Visibility _billClientPaymentChangeVisible;

        List<Client> _clients = [];
        List<Item> _items = [];
        List<Service> _services = [];
        List<User> _users = [];

        ObservableCollection<object> _searchItems = [];
        ObservableCollection<BillItemMove> _billItemsMoves = [];
        ObservableCollection<BillServiceMove> _billServicesMoves = [];

        public long SearchSelectedValue
        {
            get => _searchSelectedValue;
            set => SetProperty(ref _searchSelectedValue, value);
        }

        public decimal ItemChildItemPrice
        {
            get => _itemChildItemPrice;
            set => SetProperty(ref _itemChildItemPrice, value);
        }

        public decimal ItemChildItemQTYSell
        {
            get => _itemChildItemQTYSell;
            set => SetProperty(ref _itemChildItemQTYSell, value);
        }

        public decimal ItemChildItemQTYExist
        {
            get => _itemChildItemQTYExist;
            set => SetProperty(ref _itemChildItemQTYExist, value);
        }

        public decimal ServiceChildServiceBalance
        {
            get => _serviceChildServiceBalance;
            set => SetProperty(ref _serviceChildServiceBalance, value);
        }

        public decimal ServiceChildServiceCost
        {
            get => _serviceChildServiceCost;
            set => SetProperty(ref _serviceChildServiceCost, value);
        }

        public decimal ChildDiscount
        {
            get => _childDiscount;
            set => SetProperty(ref _childDiscount, value);
        }

        public decimal BillTotal
        {
            get => _billTotal;
            set => SetProperty(ref _billTotal, Math.Round(value, 2));
        }

        public decimal BillTotalAfterEachDiscount
        {
            get => _billTotalAfterEachDiscount;
            set => SetProperty(ref _billTotalAfterEachDiscount, Math.Round(value, 2));
        }

        public decimal BillDiscount
        {
            get => _billDiscount;
            set
            {
                if (value != _billDiscount)
                {
                    _billDiscount = value;
                    BillTotalAfterDiscount = _billDiscount > 0
                        ? Math.Round(BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (_billDiscount / 100)), 2)
                        : Math.Round(BillTotalAfterEachDiscount, 2);
                    RaisePropertyChanged();
                }
            }
        }

        public decimal BillTotalAfterDiscount
        {
            get => _billTotalAfterDiscount;
            set
            {
                if (SetProperty(ref _billTotalAfterDiscount, Math.Round(value, 2)))
                {
                    RecalculateChange();
                }
            }
        }

        public decimal BillClientPayment
        {
            get => _billClientPayment;
            set
            {
                if (value != _billClientPayment)
                {
                    _billClientPayment = value;
                    RecalculateChange();
                    RaisePropertyChanged();
                }
            }
        }

        private void RecalculateChange()
        {
            BillClientPaymentChange = InvoiceService.Change(BillClientPayment, BillTotalAfterDiscount);
            BillClientPaymentChangeVisible = BillClientPaymentChange > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool _isSaving;
        public bool IsSaving
        {
            get => _isSaving;
            private set => SetProperty(ref _isSaving, value);
        }
        public decimal BillClientPaymentChange
        {
            get => _billClientPaymentChange;
            set => SetProperty(ref _billClientPaymentChange, Math.Round(value, 2));
        }

        public long CurrentBillNo
        {
            get => _currentBillNo;
            set => SetProperty(ref _currentBillNo, value);
        }

        public string? SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public string? ItemChildItemName
        {
            get => _itemChildItemName;
            set => SetProperty(ref _itemChildItemName, value);
        }

        public string? ServiceChildServiceName
        {
            get => _serviceChildServiceName;
            set => SetProperty(ref _serviceChildServiceName, value);
        }

        public string? ServiceChildNotes
        {
            get => _serviceChildNotes;
            set => SetProperty(ref _serviceChildNotes, value);
        }

        public string? ItemChildNotes
        {
            get => _itemChildNotes;
            set => SetProperty(ref _itemChildNotes, value);
        }

        public bool ByItem
        {
            get => _byItem;
            set => SetProperty(ref _byItem, value);
        }

        public bool ByCard
        {
            get => _byCard;
            set => SetProperty(ref _byCard, value);
        }

        public bool ByService
        {
            get => _byService;
            set => SetProperty(ref _byService, value);
        }

        public bool ByName
        {
            get => _byName;
            set => SetProperty(ref _byName, value);
        }

        public bool ByShopCode
        {
            get => _byShopCode;
            set => SetProperty(ref _byShopCode, value);
        }

        public bool ByBarCode
        {
            get => _byBarCode;
            set => SetProperty(ref _byBarCode, value);
        }

        public bool IsAddItemChildOpen
        {
            get => _isAddItemChildOpen;
            set => SetProperty(ref _isAddItemChildOpen, value);
        }

        public bool IsAddServiceChildOpen
        {
            get => _isAddServiceChildOpen;
            set => SetProperty(ref _isAddServiceChildOpen, value);
        }

        public bool IsAddBillNote
        {
            get => _isAddBillNote;
            set => SetProperty(ref _isAddBillNote, value);
        }

        public bool IsSearchDropDownOpen
        {
            get => _isSearchDropDownOpen;
            set => SetProperty(ref _isSearchDropDownOpen, value);
        }

        public Client? SelectedClient
        {
            get => _selectedClient;
            set => SetProperty(ref _selectedClient, value);
        }

        public Item? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public Service? SelectedService
        {
            get => _selectedService;
            set => SetProperty(ref _selectedService, value);
        }

        public BillItemMove? DataGridSelectedBillItemMove
        {
            get => _dataGridSelectedBillItemMove;
            set => SetProperty(ref _dataGridSelectedBillItemMove, value);
        }

        public BillServiceMove? DataGridSelectedBillServiceMove
        {
            get => _dataGridSelectedBillServiceMove;
            set => SetProperty(ref _dataGridSelectedBillServiceMove, value);
        }

        public Visibility BillClientPaymentChangeVisible
        {
            get => _billClientPaymentChangeVisible;
            set => SetProperty(ref _billClientPaymentChangeVisible, value);
        }

        public ObservableCollection<object> SearchItems
        {
            get => _searchItems;
            set => SetProperty(ref _searchItems, value);
        }

        public ObservableCollection<BillItemMove> BillItemsMoves
        {
            get => _billItemsMoves;
            set => SetProperty(ref _billItemsMoves, value);
        }

        public ObservableCollection<BillServiceMove> BillServicesMoves
        {
            get => _billServicesMoves;
            set => SetProperty(ref _billServicesMoves, value);
        }

        public List<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        public List<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public List<Service> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        public List<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public ICommand? Search { get; set; }
        public ICommand? AddBillMove { get; set; }
        public ICommand? AddItemToBill { get; set; }
        public ICommand? AddServiceToBill { get; set; }
        public ICommand? DeleteBillMove { get; set; }
        public ICommand? RedoBill { get; set; }
        public ICommand? SaveBill { get; set; }
        public ICommand? SaveAndShow { get; set; }

        private Bills Message => Application.Current.Windows.OfType<Bills>().FirstOrDefault() ?? throw new InvalidOperationException("An open view is required for this UI action.");

        public BillsViewModel()
        {
            LoadCommands();
            ByName = true;
            ByItem = true;
            BillClientPaymentChangeVisible = Visibility.Collapsed;
            using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
            {
                Clients = new List<Client>(db.GetCollection<Client>(DBCollections.Clients).FindAll());
                Items = new List<Item>(db.GetCollection<Item>(DBCollections.Items).FindAll());
                Services = new List<Service>(db.GetCollection<Service>(DBCollections.Services).FindAll());
                Users = new List<User>(db.GetCollection<User>(DBCollections.Users).FindAll());
            }
            BillItemsMoves = new ObservableCollection<BillItemMove>();
            BillServicesMoves = new ObservableCollection<BillServiceMove>();
            NewBillNo();
        }

        public void LoadCommands()
        {
            Search = new DelegateCommand(DoSearch, CanSearch).ObservesProperty(() => SearchText);
            AddBillMove = new DelegateCommand(DoAddBillMove, CanAddBillMove).ObservesProperty(() => SearchSelectedValue);
            AddItemToBill = new DelegateCommand(DoAddItemToBill, CanAddItemToBill).ObservesProperty(() => ItemChildItemQTYSell).ObservesProperty(() => SelectedItem).ObservesProperty(() => ChildDiscount).ObservesProperty(() => IsSaving);
            AddServiceToBill = new DelegateCommand(DoAddServiceToBill, CanAddServiceToBill).ObservesProperty(() => ServiceChildServiceCost).ObservesProperty(() => ServiceChildServiceBalance).ObservesProperty(() => SelectedService).ObservesProperty(() => ChildDiscount).ObservesProperty(() => IsSaving);
            DeleteBillMove = new DelegateCommand(DoDeleteBillMove, CanDeleteBillMove).ObservesProperty(() => DataGridSelectedBillItemMove).ObservesProperty(() => ByItem).ObservesProperty(() => DataGridSelectedBillServiceMove).ObservesProperty(() => ByService);
            RedoBill = new DelegateCommand(DoRedoBill, CanRedoBill);
            SaveBill = new DelegateCommand(DoSaveBill, CanSaveBill).ObservesProperty(() => SelectedClient).ObservesProperty(() => BillItemsMoves.Count).ObservesProperty(() => BillServicesMoves.Count).ObservesProperty(() => IsSaving).ObservesProperty(() => BillClientPayment).ObservesProperty(() => BillDiscount);
            SaveAndShow = new DelegateCommand(DoSaveAndShow, CanSaveAndShow).ObservesProperty(() => SelectedClient).ObservesProperty(() => BillItemsMoves.Count).ObservesProperty(() => BillServicesMoves.Count).ObservesProperty(() => IsSaving).ObservesProperty(() => BillClientPayment).ObservesProperty(() => BillDiscount);
        }

        async Task<long> SaveBillNoAsync()
        {
            if (!CanSaveBill())
            {
                return 0;
            }

            IsSaving = true;
            try { return await SaveBillCoreAsync(); }
            finally { IsSaving = false; }
        }

        async Task<long> SaveBillCoreAsync()
        {
            var selectedClient = SelectedClient;
            if (selectedClient is null)
            {
                return 0;
            }
            if (BillClientPayment < BillTotalAfterDiscount)
            {
                if (selectedClient.Id == 1)
                {
                    await Message.ShowMessageAsync("خطأ", "لا يمكن عمل فاتورة اجل لهذا العميل اختار عميل اخر او اضف عميل جديد");
                    return 0;
                }
                var result = await Message.ShowMessageAsync("اجل", $"هل انت متاكد من تسجيل الفاتورة كاجل؟", MessageDialogStyle.AffirmativeAndNegative);
                if (result != MessageDialogResult.Affirmative)
                {
                    return 0;
                }
            }
            string? billNote = null;
            if (IsAddBillNote)
            {
                billNote = await Message.ShowInputAsync("ملاحظة", $"اكتب اى شئ ليتم طباعته مع الفاتورة");
            }
            long id;
            try
            {
                id = InvoiceService.Save(LiteDbContext.ConnectionString, selectedClient.Id,
                    BillDiscount, BillClientPayment, billNote, Core.ReadUserSession(), BillItemsMoves, BillServicesMoves);
            }
            catch (Exception ex)
            {
                await Core.SaveExceptionAsync(ex);
                return -1;
            }
            // A refresh failure after commit must not report the sale as failed.
            try { Clear(); }
            catch (Exception ex) { await Core.SaveExceptionAsync(ex); }
            CurrentBillNo = id + 1;
            return id;
        }

        private bool CanSaveAndShow()
        {
            return CanSaveBill();
        }

        private async void DoSaveAndShow()
        {
            var i = await SaveBillNoAsync();
            if (i > 0)
            {
                await Message.ShowMessageAsync("تم الحفظ", $"تم حفظ الفاتورة بالرقم {i} بنجاح و سيتم عرضها للطباعه الان");
                new SalesBillsViewer(i).Show();
            }
            else if (i < 0)
            {
                await Message.ShowMessageAsync("خطا", "حدث خطا اثناء حفظ الفاتورة");
            }
        }

        private bool CanSaveBill()
        {
            return !IsSaving && SelectedClient != null && SelectedClient.Id > 0 && BillClientPayment >= 0 && BillDiscount >= 0 && BillDiscount <= 100 && (BillItemsMoves.Count > 0 || BillServicesMoves.Count > 0);
        }

        private async void DoSaveBill()
        {
            var i = await SaveBillNoAsync();
            if (i > 0)
            {
                await Message.ShowMessageAsync("تم الحفظ", $"تم حفظ الفاتورة بالرقم {i}");
            }
            else if (i < 0)
            {
                await Message.ShowMessageAsync("خطا", "حدث خطا اثناء حفظ الفاتورة");
            }
        }

        private bool CanRedoBill()
        {
            return true;
        }

        private void DoRedoBill()
        {
            Clear();
        }

        private bool CanDeleteBillMove()
        {
            return (DataGridSelectedBillItemMove is not null || !ByItem) && (DataGridSelectedBillServiceMove is not null || !ByService);
        }

        private void DoDeleteBillMove()
        {
            if (ByItem && DataGridSelectedBillItemMove is not null)
            {
                var ItemToQtyPrice = DataGridSelectedBillItemMove.ItemPrice * DataGridSelectedBillItemMove.QTY;
                BillTotal -= ItemToQtyPrice;
                BillTotalAfterEachDiscount -= ItemToQtyPrice - (ItemToQtyPrice * (DataGridSelectedBillItemMove.Discount / 100));
                BillTotalAfterDiscount = BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (BillDiscount / 100));
                BillItemsMoves.Remove(DataGridSelectedBillItemMove);
            }
            if (ByService && DataGridSelectedBillServiceMove is not null)
            {
                BillTotal -= DataGridSelectedBillServiceMove.Cost;
                BillTotalAfterEachDiscount -= DataGridSelectedBillServiceMove.Cost - (DataGridSelectedBillServiceMove.Cost * (DataGridSelectedBillServiceMove.Discount / 100));
                BillTotalAfterDiscount = BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (BillDiscount / 100));
                BillServicesMoves.Remove(DataGridSelectedBillServiceMove);
            }
            DataGridSelectedBillItemMove = null;
            DataGridSelectedBillServiceMove = null;
        }

        void Clear()
        {
            SelectedClient = null;
            BillItemsMoves = new ObservableCollection<BillItemMove>();
            BillServicesMoves = new ObservableCollection<BillServiceMove>();
            BillTotal = 0;
            BillTotalAfterEachDiscount = 0;
            BillDiscount = 0;
            BillTotalAfterDiscount = 0;
            BillClientPayment = 0;
            SearchSelectedValue = 0;
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
            Clients = new List<Client>(db.GetCollection<Client>(DBCollections.Clients).FindAll());
            Items = new List<Item>(db.GetCollection<Item>(DBCollections.Items).FindAll());
            Services = new List<Service>(db.GetCollection<Service>(DBCollections.Services).FindAll());
            SearchSelectedValue = 0;
        }

        void ClearChild()
        {
            if (IsAddItemChildOpen)
            {
                IsAddItemChildOpen = false;
                ItemChildItemName = null;
                ItemChildItemPrice = 0;
                ItemChildItemQTYExist = 0;
                ItemChildItemQTYSell = 0;
                SelectedItem = null;
                ItemChildNotes = null;
            }
            if (IsAddServiceChildOpen)
            {
                IsAddServiceChildOpen = false;
                ServiceChildServiceName = null;
                ServiceChildServiceBalance = 0;
                ServiceChildServiceCost = 0;
                SelectedService = null;
                ServiceChildNotes = null;
            }
            ChildDiscount = 0;
        }

        private bool CanAddServiceToBill()
        {
            return !IsSaving && SelectedService != null && ServiceChildServiceCost > 0 && ServiceChildServiceBalance >= 0 && ChildDiscount >= 0 && ChildDiscount <= 100;
        }

        private void DoAddServiceToBill()
        {
            if (SelectedService is null)
            {
                return;
            }
            decimal balanceNeeded = 0;
            foreach (var item in BillServicesMoves)
            {
                if (item.Service?.Id == SearchSelectedValue)
                {
                    balanceNeeded += item.Balance;
                }
            }
            balanceNeeded += ServiceChildServiceBalance;
            if (SelectedService.Balance >= balanceNeeded)
            {
                using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
                {
                    BillServicesMoves.Add(new BillServiceMove
                    {
                        Bill = db.GetCollection<Bill>(DBCollections.Bills).FindById(CurrentBillNo),
                        Service = db.GetCollection<Service>(DBCollections.Services).FindById(SearchSelectedValue),
                        Balance = ServiceChildServiceBalance,
                        Cost = ServiceChildServiceCost,
                        Discount = ChildDiscount,
                        Notes = ServiceChildNotes,
                        Creator = Core.ReadUserSession(),
                        CreateDate = DateTime.Now,
                        Editor = null,
                        EditDate = null
                    });
                }
                BillTotal += ServiceChildServiceCost;
                BillTotalAfterEachDiscount += ServiceChildServiceCost - (ServiceChildServiceCost * (ChildDiscount / 100));
                BillTotalAfterDiscount = BillDiscount > 0
                    ? BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (BillDiscount / 100))
                    : BillTotalAfterEachDiscount;
                ClearChild();
            }
            else
            {
                Message.ShowMessageAsync("رصيد غير كافى", "الرصيد فى الخدمه لا يكفى لتسجيل العمليه");
            }
        }

        private bool CanAddItemToBill()
        {
            return !IsSaving && SelectedItem != null && ItemChildItemQTYSell > 0 && ChildDiscount >= 0 && ChildDiscount <= 100;
        }

        private void DoAddItemToBill()
        {
            if (SelectedItem is null)
            {
                return;
            }
            decimal QTYNeeded = 0;
            foreach (var item in BillItemsMoves)
            {
                if (item.Item?.Id == SearchSelectedValue)
                {
                    QTYNeeded += item.QTY;
                }
            }
            QTYNeeded += ItemChildItemQTYSell;
            if (SelectedItem.QTY >= QTYNeeded)
            {
                var ItemToQtyPrice = SelectedItem.RetailPrice * ItemChildItemQTYSell;
                using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
                {
                    BillItemsMoves.Add(new BillItemMove
                    {
                        Bill = db.GetCollection<Bill>(DBCollections.Bills).FindById(CurrentBillNo),
                        Item = db.GetCollection<Item>(DBCollections.Items).FindById(SearchSelectedValue),
                        QTY = ItemChildItemQTYSell,
                        ItemPrice = SelectedItem.RetailPrice,
                        Discount = ChildDiscount,
                        Notes = ItemChildNotes,
                        Creator = Core.ReadUserSession(),
                        CreateDate = DateTime.Now,
                        Editor = null,
                        EditDate = null
                    });
                }
                BillTotal += ItemToQtyPrice;
                BillTotalAfterEachDiscount += ItemToQtyPrice - (ItemToQtyPrice * (ChildDiscount / 100));
                BillTotalAfterDiscount = BillDiscount > 0
                    ? BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (BillDiscount / 100))
                    : BillTotalAfterEachDiscount;
                ClearChild();
            }
            else
            {
                Message.ShowMessageAsync("الكمية لا تكفى", "الكمية الخاصه بالصنف اقل من المراد بيعه");
            }
        }

        private bool CanAddBillMove()
        {
            return SearchSelectedValue > 0;
        }

        private void DoAddBillMove()
        {
            if (ByService)
            {
                SelectedService = Services.FirstOrDefault(f => f.Id == SearchSelectedValue);
                if (SelectedService is null)
                {
                    return;
                }
                IsAddServiceChildOpen = true;
                ServiceChildServiceName = SelectedService.Name;
            }
            else
            {
                SelectedItem = Items.FirstOrDefault(f => f.Id == SearchSelectedValue);
                if (SelectedItem is null)
                {
                    return;
                }
                IsAddItemChildOpen = true;
                ItemChildItemName = SelectedItem.Name;
                ItemChildItemPrice = SelectedItem.RetailPrice;
                ItemChildItemQTYExist = SelectedItem.QTY;
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
                if (ByItem)
                {
                    if (ByName)
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Other && i.Name != null && i.Name.Contains(SearchText ?? string.Empty)));
                        if (SearchItems.Count > 0)
                        {
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            await Message.ShowMessageAsync("غير موجود", "لم يستطع ايجاد صنف بهذا الاسم");
                        }
                    }
                    else if (ByBarCode)
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Other && i.Barcode == SearchText));
                        if (SearchItems.Count > 0)
                        {
                            SearchSelectedValue = Items.SingleOrDefault(i => i.Group == ItemGroup.Other && i.Barcode == SearchText)?.Id ?? 0;
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            await Message.ShowMessageAsync("غير موجود", "لم يستطع ايجاد صنف بهذا الباركود");
                        }
                    }
                    else
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Other && i.Shopcode == SearchText));
                        if (SearchItems.Count > 0)
                        {
                            SearchSelectedValue = Items.SingleOrDefault(i => i.Group == ItemGroup.Other && i.Shopcode == SearchText)?.Id ?? 0;
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            await Message.ShowMessageAsync("غير موجود", "لم يستطع ايجاد صنف بكود المحل هذا");
                        }
                    }
                }
                else if (ByCard)
                {
                    if (ByName)
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Card && i.Name != null && i.Name.Contains(SearchText ?? string.Empty)));
                        if (SearchItems.Count > 0)
                        {
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            await Message.ShowMessageAsync("غير موجود", "لم يستطع ايجاد كارت شحن بهذا الاسم");
                        }
                    }
                    else if (ByBarCode)
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Card && i.Barcode == SearchText));
                        if (SearchItems.Count > 0)
                        {
                            SearchSelectedValue = Items.SingleOrDefault(i => i.Group == ItemGroup.Card && i.Barcode == SearchText)?.Id ?? 0;
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            await Message.ShowMessageAsync("غير موجود", "لم يستطع ايجاد كارت شحن بهذا الباركود");
                        }
                    }
                    else
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Card && i.Shopcode == SearchText));
                        if (SearchItems.Count > 0)
                        {
                            SearchSelectedValue = Items.SingleOrDefault(i => i.Group == ItemGroup.Card && i.Shopcode == SearchText)?.Id ?? 0;
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            await Message.ShowMessageAsync("غير موجود", "لم يستطع ايجاد كارت شحن بكود المحل هذا");
                        }
                    }
                }
                else
                {
                    SearchItems = new ObservableCollection<object>(Services.Where(s => s.Name != null && s.Name.Contains(SearchText ?? string.Empty)));
                    if (SearchItems.Count > 0)
                    {
                        IsSearchDropDownOpen = true;
                    }
                    else
                    {
                        SearchSelectedValue = 0;
                        await Message.ShowMessageAsync("غير موجود", "لم يستطع ايجاد خدمه بهذا الاسم");
                    }
                }
            }
            catch (Exception ex)
            {
                SearchSelectedValue = 0;
                Core.SaveException(ex);
            }
        }

        void NewBillNo()
        {
            try
            {
                using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                var x = db.GetCollection<Bill>(DBCollections.Bills).FindAll().LastOrDefault()?.Id ?? 0;
                if (x < 1)
                {
                    x = 0;
                }
                CurrentBillNo = ++x;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                CurrentBillNo = 1;
            }
        }
    }
}
