using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.WPF.Data;
using Phony.WPF.Models;
using Phony.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Phony.WPF.ViewModels
{

    public class BillsViewModel : BaseViewModelWithAnnotationValidation
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
        string _searchText;
        string _itemChildItemName;
        string _serviceChildServiceName;
        string _serviceChildNotes;
        string _itemChildNotes;
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
        Client _selectedClient;
        Item _selectedItem;
        Service _selectedService;
        BillItemMove _dataGridSelectedBillItemMove;
        BillServiceMove _dataGridSelectedBillServiceMove;

        Visibility _billClientPaymentChangeVisible;

        List<Client> _clients;
        List<Item> _items;
        List<Service> _services;
        List<User> _users;

        ObservableCollection<object> _searchItems;
        ObservableCollection<BillItemMove> _billItemsMoves;
        ObservableCollection<BillServiceMove> _billServicesMoves;

        public long SearchSelectedValue
        {
            get => _searchSelectedValue;
            set
            {
                _searchSelectedValue = value;
                NotifyOfPropertyChange(() => SearchSelectedValue);
            }
        }

        public decimal ItemChildItemPrice
        {
            get => _itemChildItemPrice;
            set
            {
                _itemChildItemPrice = value;
                NotifyOfPropertyChange(() => ItemChildItemPrice);
            }
        }

        public decimal ItemChildItemQTYSell
        {
            get => _itemChildItemQTYSell;
            set
            {
                _itemChildItemQTYSell = value;
                NotifyOfPropertyChange(() => ItemChildItemQTYSell);
            }
        }

        public decimal ItemChildItemQTYExist
        {
            get => _itemChildItemQTYExist;
            set
            {
                _itemChildItemQTYExist = value;
                NotifyOfPropertyChange(() => ItemChildItemQTYExist);
            }
        }

        public decimal ServiceChildServiceBalance
        {
            get => _serviceChildServiceBalance;
            set
            {
                _serviceChildServiceBalance = value;
                NotifyOfPropertyChange(() => ServiceChildServiceBalance);
            }
        }

        public decimal ServiceChildServiceCost
        {
            get => _serviceChildServiceCost;
            set
            {
                _serviceChildServiceCost = value;
                NotifyOfPropertyChange(() => ServiceChildServiceCost);
            }
        }

        public decimal ChildDiscount
        {
            get => _childDiscount;
            set
            {
                _childDiscount = value;
                NotifyOfPropertyChange(() => ChildDiscount);
            }
        }

        public decimal BillTotal
        {
            get => _billTotal;
            set
            {
                _billTotal = Math.Round(value, 2);
                NotifyOfPropertyChange(() => BillTotal);
            }
        }

        public decimal BillTotalAfterEachDiscount
        {
            get => _billTotalAfterEachDiscount;
            set
            {
                _billTotalAfterEachDiscount = Math.Round(value, 2);
                NotifyOfPropertyChange(() => BillTotalAfterEachDiscount);
            }
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
                    NotifyOfPropertyChange(() => BillDiscount);
                }
            }
        }

        public decimal BillTotalAfterDiscount
        {
            get => _billTotalAfterDiscount;
            set
            {
                _billTotalAfterDiscount = Math.Round(value, 2);
                NotifyOfPropertyChange(() => BillTotalAfterDiscount);
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
                    if (_billClientPayment > BillTotalAfterDiscount)
                    {
                        BillClientPaymentChange = Math.Round(_billClientPayment - BillTotalAfterDiscount, 2);
                        BillClientPaymentChangeVisible = Visibility.Visible;
                    }
                    else
                    {
                        BillClientPaymentChange = 0;
                        BillClientPaymentChangeVisible = Visibility.Collapsed;
                    }
                    NotifyOfPropertyChange(() => BillClientPayment);
                }
            }
        }

        public decimal BillClientPaymentChange
        {
            get => _billClientPaymentChange;
            set
            {
                _billClientPaymentChange = Math.Round(value, 2);
                NotifyOfPropertyChange(() => BillClientPaymentChange);
            }
        }

        public long CurrentBillNo
        {
            get => _currentBillNo;
            set
            {
                _currentBillNo = value;
                NotifyOfPropertyChange(() => CurrentBillNo);
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

        public string ItemChildItemName
        {
            get => _itemChildItemName;
            set
            {
                _itemChildItemName = value;
                NotifyOfPropertyChange(() => ItemChildItemName);
            }
        }

        public string ServiceChildServiceName
        {
            get => _serviceChildServiceName;
            set
            {
                _serviceChildServiceName = value;
                NotifyOfPropertyChange(() => ServiceChildServiceName);
            }
        }

        public string ServiceChildNotes
        {
            get => _serviceChildNotes;
            set
            {
                _serviceChildNotes = value;
                NotifyOfPropertyChange(() => ServiceChildNotes);
            }
        }

        public string ItemChildNotes
        {
            get => _itemChildNotes;
            set
            {
                _itemChildNotes = value;
                NotifyOfPropertyChange(() => ItemChildNotes);
            }
        }

        public bool ByItem
        {
            get => _byItem;
            set
            {
                _byItem = value;
                NotifyOfPropertyChange(() => ByItem);
            }
        }

        public bool ByCard
        {
            get => _byCard;
            set
            {
                _byCard = value;
                NotifyOfPropertyChange(() => ByCard);
            }
        }

        public bool ByService
        {
            get => _byService;
            set
            {
                _byService = value;
                NotifyOfPropertyChange(() => ByService);
            }
        }

        public bool ByName
        {
            get => _byName;
            set
            {
                _byName = value;
                NotifyOfPropertyChange(() => ByName);
            }
        }

        public bool ByShopCode
        {
            get => _byShopCode;
            set
            {
                _byShopCode = value;
                NotifyOfPropertyChange(() => ByShopCode);
            }
        }

        public bool ByBarCode
        {
            get => _byBarCode;
            set
            {
                _byBarCode = value;
                NotifyOfPropertyChange(() => ByBarCode);
            }
        }

        public bool IsAddItemChildOpen
        {
            get => _isAddItemChildOpen;
            set
            {
                _isAddItemChildOpen = value;
                NotifyOfPropertyChange(() => IsAddItemChildOpen);
            }
        }

        public bool IsAddServiceChildOpen
        {
            get => _isAddServiceChildOpen;
            set
            {
                _isAddServiceChildOpen = value;
                NotifyOfPropertyChange(() => IsAddServiceChildOpen);
            }
        }

        public bool IsAddBillNote
        {
            get => _isAddBillNote;
            set
            {
                _isAddBillNote = value;
                NotifyOfPropertyChange(() => IsAddBillNote);
            }
        }

        public bool IsSearchDropDownOpen
        {
            get => _isSearchDropDownOpen;
            set
            {
                _isSearchDropDownOpen = value;
                NotifyOfPropertyChange(() => IsSearchDropDownOpen);
            }
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                NotifyOfPropertyChange(() => SelectedClient);
            }
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange(() => SelectedItem);
            }
        }

        public Service SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                NotifyOfPropertyChange(() => SelectedService);
            }
        }

        public BillItemMove DataGridSelectedBillItemMove
        {
            get => _dataGridSelectedBillItemMove;
            set
            {
                _dataGridSelectedBillItemMove = value;
                NotifyOfPropertyChange(() => DataGridSelectedBillItemMove);
            }
        }

        public BillServiceMove DataGridSelectedBillServiceMove
        {
            get => _dataGridSelectedBillServiceMove;
            set
            {
                _dataGridSelectedBillServiceMove = value;
                NotifyOfPropertyChange(() => DataGridSelectedBillServiceMove);
            }
        }

        public Visibility BillClientPaymentChangeVisible
        {
            get => _billClientPaymentChangeVisible;
            set
            {
                _billClientPaymentChangeVisible = value;
                NotifyOfPropertyChange(() => BillClientPaymentChangeVisible);
            }
        }

        public ObservableCollection<object> SearchItems
        {
            get => _searchItems;
            set
            {
                _searchItems = value;
                NotifyOfPropertyChange(() => SearchItems);
            }
        }

        public ObservableCollection<BillItemMove> BillItemsMoves
        {
            get => _billItemsMoves;
            set
            {
                _billItemsMoves = value;
                NotifyOfPropertyChange(() => BillItemsMoves);
            }
        }

        public ObservableCollection<BillServiceMove> BillServicesMoves
        {
            get => _billServicesMoves;
            set
            {
                _billServicesMoves = value;
                NotifyOfPropertyChange(() => BillServicesMoves);
            }
        }

        public List<Client> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                NotifyOfPropertyChange(() => Clients);
            }
        }

        public List<Item> Items
        {
            get => _items;
            set
            {
                _items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        public List<Service> Services
        {
            get => _services;
            set
            {
                _services = value;
                NotifyOfPropertyChange(() => Services);
            }
        }

        public List<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public BillsViewModel()
        {
            Title = "فواتير";
            ByName = true;
            ByItem = true;
            BillClientPaymentChangeVisible = Visibility.Collapsed;
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
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

        async Task<long> SaveBillNoAsync()
        {
            if (BillClientPayment < BillTotalAfterDiscount)
            {
                if (SelectedClient.Id == 1)
                {
                    MessageBox.MaterialMessageBox.ShowError("لا يمكن عمل فاتورة اجل لهذا العميل اختار عميل اخر او اضف عميل جديد", "خطأ", true);
                    return 0;
                }
                var result = MessageBox.MaterialMessageBox.ShowWithCancel($"هل انت متاكد من تسجيل الفاتورة كاجل؟", "اجل", true);
                if (result != MessageBoxResult.OK)
                {
                    return 0;
                }
            }
            string billNote = null;
            if (IsAddBillNote)
            {
                billNote = MessageBox.MaterialInputBox.Show($"اكتب اى شئ ليتم طباعته مع الفاتورة", "ملاحظة", true);
            }
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            try
            {
                var bi = new Bill
                {
                    Client = db.GetCollection<Client>(DBCollections.Clients).FindById(SelectedClient.Id),
                    Store = db.GetCollection<Store>(DBCollections.Stores).FindById(1),
                    Discount = BillDiscount,
                    TotalAfterDiscounts = BillTotalAfterDiscount,
                    TotalPayed = BillClientPayment,
                    Notes = billNote,
                    CreateDate = DateTime.Now,
                    //Creator = Core.ReadUserSession(),
                    Editor = null,
                    EditDate = null
                };
                db.GetCollection<Bill>(DBCollections.Bills).Insert(bi);
                foreach (var item in BillItemsMoves)
                {
                    item.Bill = bi;
                    db.GetCollection<BillItemMove>(DBCollections.BillsItemsMoves).Insert(item);
                    var i = db.GetCollection<Item>(DBCollections.Items).FindById(item.Item.Id);
                    i.QTY -= item.QTY;
                    db.GetCollection<Item>(DBCollections.Items).Update(i);
                }
                foreach (var service in BillServicesMoves)
                {
                    service.Bill = bi;
                    db.GetCollection<BillServiceMove>(DBCollections.BillsServicesMoves).Insert(service);
                    var s = db.GetCollection<Service>(DBCollections.Services).FindById(service.Service.Id);
                    s.Balance -= service.Balance;
                    db.GetCollection<Service>(DBCollections.Services).Update(s);
                }
                if (BillClientPayment < BillTotalAfterDiscount)
                {
                    var c = db.GetCollection<Client>(DBCollections.Clients).FindById(SelectedClient.Id);
                    c.Balance += BillTotalAfterDiscount - BillClientPayment;
                    db.GetCollection<Client>(DBCollections.Clients).Update(c);
                }
                db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Insert(new TreasuryMove
                {
                    Treasury = db.GetCollection<Treasury>(DBCollections.Treasuries).FindById(1),
                    Debit = BillClientPayment,
                    Credit = BillClientPaymentChange,
                    Notes = $"فاتورة رقم {bi.Id}",
                    CreateDate = DateTime.Now,
                    //Creator = Core.ReadUserSession(),
                    EditDate = null,
                    Editor = null
                });
                Clear();
                CurrentBillNo = bi.Id + 1;
                return bi.Id;
            }
            catch (Exception ex)
            {
                await Core.SaveExceptionAsync(ex);
                return -1;
            }
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
                MessageBox.MaterialMessageBox.Show($"تم حفظ الفاتورة بالرقم {i} بنجاح و سيتم عرضها للطباعه الان", "تم الحفظ", true);
                new SalesBillsViewerView(i).Show();
            }
            else if (i < 0)
            {
                MessageBox.MaterialMessageBox.ShowError("حدث خطا اثناء حفظ الفاتورة", "خطأ", true);
            }
        }

        private bool CanSaveBill()
        {
            return SelectedClient == null ? false : (BillItemsMoves.Count > 0 || BillServicesMoves.Count > 0) && SelectedClient.Id > 0;
        }

        private async void DoSaveBill()
        {
            var i = await SaveBillNoAsync();
            if (i > 0)
            {
                MessageBox.MaterialMessageBox.Show($"تم حفظ الفاتورة بالرقم {i}", "تم الحفظ", true);
            }
            else if (i < 0)
            {
                MessageBox.MaterialMessageBox.ShowError("حدث خطا اثناء حفظ الفاتورة", "خطأ", true);
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
            return (DataGridSelectedBillItemMove != null || !ByItem) && (DataGridSelectedBillServiceMove != null || !ByService);
        }

        private void DoDeleteBillMove()
        {
            if (ByItem)
            {
                var ItemToQtyPrice = DataGridSelectedBillItemMove.ItemPrice * DataGridSelectedBillItemMove.QTY;
                BillTotal -= ItemToQtyPrice;
                BillTotalAfterEachDiscount -= ItemToQtyPrice - (ItemToQtyPrice * (DataGridSelectedBillItemMove.Discount / 100));
                BillTotalAfterDiscount = BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (BillDiscount / 100));
                BillItemsMoves.Remove(DataGridSelectedBillItemMove);
            }
            if (ByService)
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
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
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
            return ServiceChildServiceCost > 0;
        }

        private void DoAddServiceToBill()
        {
            decimal balanceNeeded = 0;
            foreach (var item in BillServicesMoves)
            {
                if (item.Service.Id == SearchSelectedValue)
                {
                    balanceNeeded += item.Balance;
                }
            }
            balanceNeeded += ServiceChildServiceCost;
            if (SelectedService.Balance >= balanceNeeded)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    BillServicesMoves.Add(new BillServiceMove
                    {
                        Bill = db.GetCollection<Bill>(DBCollections.Bills).FindById(CurrentBillNo),
                        Service = db.GetCollection<Service>(DBCollections.Services).FindById(SearchSelectedValue),
                        Balance = ServiceChildServiceBalance,
                        Cost = ServiceChildServiceCost,
                        Discount = ChildDiscount,
                        Notes = ServiceChildNotes,
                        //Creator = Core.ReadUserSession(),
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
                MessageBox.MaterialMessageBox.ShowWarning("الرصيد فى الخدمه لا يكفى لتسجيل العمليه", "رصيد غير كافى", true);
            }
        }

        private bool CanAddItemToBill()
        {
            return ItemChildItemQTYSell > 0;
        }

        private void DoAddItemToBill()
        {
            decimal QTYNeeded = 0;
            foreach (var item in BillItemsMoves)
            {
                if (item.Item.Id == SearchSelectedValue)
                {
                    QTYNeeded += item.QTY;
                }
            }
            QTYNeeded += ItemChildItemQTYSell;
            if (SelectedItem.QTY >= QTYNeeded)
            {
                var ItemToQtyPrice = SelectedItem.RetailPrice * ItemChildItemQTYSell;
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    BillItemsMoves.Add(new BillItemMove
                    {
                        Bill = db.GetCollection<Bill>(DBCollections.Bills).FindById(CurrentBillNo),
                        Item = db.GetCollection<Item>(DBCollections.Items).FindById(SearchSelectedValue),
                        QTY = ItemChildItemQTYSell,
                        ItemPrice = SelectedItem.RetailPrice,
                        Discount = ChildDiscount,
                        Notes = ItemChildNotes,
                        //Creator = Core.ReadUserSession(),
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
                MessageBox.MaterialMessageBox.ShowWarning("الكمية الخاصه بالصنف اقل من المراد بيعه", "الكمية لا تكفى", true);
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
                IsAddServiceChildOpen = true;
                ServiceChildServiceName = SelectedService.Name;
            }
            else
            {
                SelectedItem = Items.FirstOrDefault(f => f.Id == SearchSelectedValue);
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

        private void DoSearch()
        {
            try
            {
                if (ByItem)
                {
                    if (ByName)
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Other && i.Name.Contains(SearchText)));
                        if (SearchItems.Count > 0)
                        {
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            MessageBox.MaterialMessageBox.ShowWarning("لم يستطع ايجاد صنف بهذا الاسم", "غير موجود", true);
                        }
                    }
                    else if (ByBarCode)
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Other && i.Barcode == SearchText));
                        if (SearchItems.Count > 0)
                        {
                            SearchSelectedValue = Items.SingleOrDefault(i => i.Group == ItemGroup.Other && i.Barcode == SearchText).Id;
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            MessageBox.MaterialMessageBox.ShowWarning("لم يستطع ايجاد صنف بهذا الباركود", "غير موجود", true);
                        }
                    }
                    else
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Other && i.Shopcode == SearchText));
                        if (SearchItems.Count > 0)
                        {
                            SearchSelectedValue = Items.SingleOrDefault(i => i.Group == ItemGroup.Other && i.Shopcode == SearchText).Id;
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            MessageBox.MaterialMessageBox.ShowWarning("لم يستطع ايجاد صنف بكود المحل هذا", "غير موجود", true);
                        }
                    }
                }
                else if (ByCard)
                {
                    if (ByName)
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Card && i.Name.Contains(SearchText)));
                        if (SearchItems.Count > 0)
                        {
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            MessageBox.MaterialMessageBox.ShowWarning("لم يستطع ايجاد كارت شحن بهذا الاسم", "غير موجود", true);
                        }
                    }
                    else if (ByBarCode)
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Card && i.Barcode == SearchText));
                        if (SearchItems.Count > 0)
                        {
                            SearchSelectedValue = Items.SingleOrDefault(i => i.Group == ItemGroup.Card && i.Barcode == SearchText).Id;
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            MessageBox.MaterialMessageBox.ShowWarning("لم يستطع ايجاد كارت شحن بهذا الباركود", "غير موجود", true);
                        }
                    }
                    else
                    {
                        SearchItems = new ObservableCollection<object>(Items.Where(i => i.Group == ItemGroup.Card && i.Shopcode == SearchText));
                        if (SearchItems.Count > 0)
                        {
                            SearchSelectedValue = Items.SingleOrDefault(i => i.Group == ItemGroup.Card && i.Shopcode == SearchText).Id;
                            IsSearchDropDownOpen = true;
                        }
                        else
                        {
                            SearchSelectedValue = 0;
                            MessageBox.MaterialMessageBox.ShowWarning("لم يستطع ايجاد كارت شحن بكود المحل هذا", "غير موجود", true);
                        }
                    }
                }
                else
                {
                    SearchItems = new ObservableCollection<object>(Services.Where(s => s.Name.Contains(SearchText)));
                    if (SearchItems.Count > 0)
                    {
                        IsSearchDropDownOpen = true;
                    }
                    else
                    {
                        SearchSelectedValue = 0;
                        MessageBox.MaterialMessageBox.ShowWarning("لم يستطع ايجاد خدمه بهذا الاسم", "غير موجود", true);
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
                using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
                var x = db.GetCollection<Bill>(DBCollections.Bills).FindAll().LastOrDefault().Id;
                if (x == 1)
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