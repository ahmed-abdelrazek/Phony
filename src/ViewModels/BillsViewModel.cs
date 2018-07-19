using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
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
            set => SetProperty(ref _billTotal, value);
        }

        public decimal BillTotalAfterEachDiscount
        {
            get => _billTotalAfterEachDiscount;
            set => SetProperty(ref _billTotalAfterEachDiscount, value);
        }

        public decimal BillDiscount
        {
            get => _billDiscount;
            set => SetProperty(ref _billDiscount, value);
        }

        public decimal BillTotalAfterDiscount
        {
            get => _billTotalAfterDiscount;
            set => SetProperty(ref _billTotalAfterDiscount, value);
        }

        public decimal BillClientPayment
        {
            get => _billClientPayment;
            set => SetProperty(ref _billClientPayment, value);
        }

        public decimal BillClientPaymentChange
        {
            get => _billClientPaymentChange;
            set => SetProperty(ref _billClientPaymentChange, value);
        }

        public long CurrentBillNo
        {
            get => _currentBillNo;
            set => SetProperty(ref _currentBillNo, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public string ItemChildItemName
        {
            get => _itemChildItemName;
            set => SetProperty(ref _itemChildItemName, value);
        }

        public string ServiceChildServiceName
        {
            get => _serviceChildServiceName;
            set => SetProperty(ref _serviceChildServiceName, value);
        }

        public string ServiceChildNotes
        {
            get => _serviceChildNotes;
            set => SetProperty(ref _serviceChildNotes, value);
        }

        public string ItemChildNotes
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

        public Client SelectedClient
        {
            get => _selectedClient;
            set => SetProperty(ref _selectedClient, value);
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public Service SelectedService
        {
            get => _selectedService;
            set => SetProperty(ref _selectedService, value);
        }

        public BillItemMove DataGridSelectedBillItemMove
        {
            get => _dataGridSelectedBillItemMove;
            set => SetProperty(ref _dataGridSelectedBillItemMove, value);
        }

        public BillServiceMove DataGridSelectedBillServiceMove
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

        public ICommand Search { get; set; }
        public ICommand AddBillMove { get; set; }
        public ICommand AddItemToBill { get; set; }
        public ICommand AddServiceToBill { get; set; }
        public ICommand DeleteBillMove { get; set; }
        public ICommand RedoBill { get; set; }
        public ICommand SaveBill { get; set; }
        public ICommand SaveAndShow { get; set; }

        Bills Message = Application.Current.Windows.OfType<Bills>().FirstOrDefault();

        public BillsViewModel()
        {
            LoadCommands();
            ByName = true;
            ByItem = true;
            BillClientPaymentChangeVisible = Visibility.Collapsed;
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Clients = new List<Client>(db.GetCollection<Client>(Data.DBCollections.Clients).FindAll());
                Items = new List<Item>(db.GetCollection<Item>(Data.DBCollections.Items).FindAll());
                Services = new List<Service>(db.GetCollection<Service>(Data.DBCollections.Services).FindAll());
                Users = new List<User>(db.GetCollection<User>(Data.DBCollections.Users).FindAll());
            }
            BillItemsMoves = new ObservableCollection<BillItemMove>();
            BillServicesMoves = new ObservableCollection<BillServiceMove>();
            NewBillNo();
        }

        public void LoadCommands()
        {
            Search = new DelegateCommand(DoSearch, CanSearch).ObservesProperty(() => SearchText);
            AddBillMove = new DelegateCommand(DoAddBillMove, CanAddBillMove).ObservesProperty(() => SearchSelectedValue);
            AddItemToBill = new DelegateCommand(DoAddItemToBill, CanAddItemToBill).ObservesProperty(() => ItemChildItemQTYSell);
            AddServiceToBill = new DelegateCommand(DoAddServiceToBill, CanAddServiceToBill).ObservesProperty(() => ServiceChildServiceCost);
            DeleteBillMove = new DelegateCommand(DoDeleteBillMove, CanDeleteBillMove).ObservesProperty(() => DataGridSelectedBillItemMove).ObservesProperty(() => ByItem).ObservesProperty(() => DataGridSelectedBillServiceMove).ObservesProperty(() => ByService);
            RedoBill = new DelegateCommand(DoRedoBill, CanRedoBill);
            SaveBill = new DelegateCommand(DoSaveBill, CanSaveBill).ObservesProperty(() => SelectedClient);
            SaveAndShow = new DelegateCommand(DoSaveAndShow, CanSaveAndShow).ObservesProperty(() => SelectedClient);
        }

        async Task<long> SaveBillNoAsync()
        {
            if (BillClientPayment < BillTotalAfterDiscount)
            {
                if (SelectedClient.Id == 1)
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
            string billNote = null;
            if (IsAddBillNote)
            {
                billNote = await Message.ShowInputAsync("ملاحظة", $"اكتب اى شئ ليتم طباعته مع الفاتورة");
            }
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                try
                {
                    var bi = new Bill
                    {
                        Client = db.GetCollection<Client>(Data.DBCollections.Clients.ToString()).FindById(SelectedClient.Id),
                        Store = db.GetCollection<Store>(Data.DBCollections.Stores.ToString()).FindById(1),
                        Discount = BillDiscount,
                        TotalAfterDiscounts = BillTotalAfterDiscount,
                        TotalPayed = BillClientPayment,
                        Notes = billNote,
                        CreateDate = DateTime.Now,
                        Creator = Core.ReadUserSession(),
                        Editor = null,
                        EditDate = null
                    };
                    db.GetCollection<Bill>(Data.DBCollections.Bills.ToString()).Insert(bi);
                    foreach (var item in BillItemsMoves)
                    {
                        item.Bill = bi;
                        db.GetCollection<BillItemMove>(Data.DBCollections.BillsItemsMoves.ToString()).Insert(item);
                        var i = db.GetCollection<Item>(Data.DBCollections.Items.ToString()).FindById(item.Item.Id);
                        i.QTY -= item.QTY;
                        db.GetCollection<Item>(Data.DBCollections.Items.ToString()).Update(i);
                    }
                    foreach (var service in BillServicesMoves)
                    {
                        service.Bill = bi;
                        db.GetCollection<BillServiceMove>(Data.DBCollections.BillsServicesMoves.ToString()).Insert(service);
                        var s = db.GetCollection<Service>(Data.DBCollections.Services.ToString()).FindById(service.Service.Id);
                        s.Balance -= service.ServicePayment;
                        db.GetCollection<Service>(Data.DBCollections.Services.ToString()).Update(s);
                    }
                    if (BillClientPayment < BillTotalAfterDiscount)
                    {
                        var c = db.GetCollection<Client>(Data.DBCollections.Clients.ToString()).FindById(SelectedClient.Id);
                        c.Balance += BillTotalAfterDiscount - BillClientPayment;
                        db.GetCollection<Client>(Data.DBCollections.Clients.ToString()).Update(c);
                    }
                    db.GetCollection<TreasuryMove>(Data.DBCollections.TreasuriesMoves.ToString()).Insert(new TreasuryMove
                    {
                        Treasury = db.GetCollection<Treasury>(Data.DBCollections.Treasuries.ToString()).FindById(1),
                        Debit = BillClientPayment,
                        Credit = BillClientPaymentChange,
                        Notes = $"فاتورة رقم {bi.Id}",
                        CreateDate = DateTime.Now,
                        Creator = Core.ReadUserSession(),
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
            if (SelectedClient == null)
            {
                return false;
            }
            if ((BillItemsMoves.Count > 0 || BillServicesMoves.Count > 0) && SelectedClient.Id > 0)
            {
                return true;
            }
            return false;
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
            if ((DataGridSelectedBillItemMove == null && ByItem) || (DataGridSelectedBillServiceMove == null && ByService))
            {
                return false;
            }
            return true;
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
                BillTotal -= DataGridSelectedBillServiceMove.ServicePayment;
                BillTotalAfterEachDiscount -= DataGridSelectedBillServiceMove.ServicePayment - (DataGridSelectedBillServiceMove.ServicePayment * (DataGridSelectedBillServiceMove.Discount / 100));
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
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Clients = new List<Client>(db.GetCollection<Client>(Data.DBCollections.Clients).FindAll());
                Items = new List<Item>(db.GetCollection<Item>(Data.DBCollections.Items).FindAll());
                Services = new List<Service>(db.GetCollection<Service>(Data.DBCollections.Services).FindAll());
                SearchSelectedValue = 0;
            }
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
            if (ServiceChildServiceCost > 0)
            {
                return true;
            }
            return false;
        }

        private void DoAddServiceToBill()
        {
            decimal balanceNeeded = 0;
            foreach (var item in BillServicesMoves)
            {
                if (item.Service.Id == SearchSelectedValue)
                {
                    balanceNeeded += item.ServicePayment;
                }
            }
            balanceNeeded += ServiceChildServiceCost;
            if (SelectedService.Balance >= balanceNeeded)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    BillServicesMoves.Add(new BillServiceMove
                    {
                        Bill = db.GetCollection<Bill>(Data.DBCollections.Bills.ToString()).FindById(CurrentBillNo),
                        Service = db.GetCollection<Service>(Data.DBCollections.Services.ToString()).FindById(SearchSelectedValue),
                        Balance = ServiceChildServiceBalance,
                        ServicePayment = ServiceChildServiceCost,
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
                if (BillDiscount > 0)
                {
                    BillTotalAfterDiscount = BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (BillDiscount / 100));
                }
                else
                {
                    BillTotalAfterDiscount = BillTotalAfterEachDiscount;
                }
                ClearChild();
            }
            else
            {
                Message.ShowMessageAsync("رصيد غير كافى", "الرصيد فى الخدمه لا يكفى لتسجيل العمليه");
            }
        }

        private bool CanAddItemToBill()
        {
            if (ItemChildItemQTYSell > 0)
            {
                return true;
            }
            return false;
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
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    BillItemsMoves.Add(new BillItemMove
                    {
                        Bill = db.GetCollection<Bill>(Data.DBCollections.Bills.ToString()).FindById(CurrentBillNo),
                        Item = db.GetCollection<Item>(Data.DBCollections.Items.ToString()).FindById(SearchSelectedValue),
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
                if (BillDiscount > 0)
                {
                    BillTotalAfterDiscount = BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (BillDiscount / 100));
                }
                else
                {
                    BillTotalAfterDiscount = BillTotalAfterEachDiscount;
                }
                ClearChild();
            }
            else
            {
                Message.ShowMessageAsync("الكمية لا تكفى", "الكمية الخاصه بالصنف اقل من المراد بيعه");
            }
        }

        private bool CanAddBillMove()
        {
            if (SearchSelectedValue > 0)
            {
                return true;
            }
            return false;
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
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                return true;
            }
            return false;
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
                            BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد صنف بهذا الاسم");
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
                            BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد صنف بهذا الباركود");
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
                            BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد صنف بكود المحل هذا");
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
                            BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد كارت شحن بهذا الاسم");
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
                            BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد كارت شحن بهذا الباركود");
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
                            BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد كارت شحن بكود المحل هذا");
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
                        BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد خدمه بهذا الاسم");
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
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    CurrentBillNo = ++db.GetCollection<Client>(Data.DBCollections.Clients).FindAll().LastOrDefault().Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                CurrentBillNo = 1;
            }
        }
    }
}