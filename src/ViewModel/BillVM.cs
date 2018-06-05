﻿using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Model;
using Phony.Persistence;
using Phony.Utility;
using Phony.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModel
{

    public class BillVM : CommonBase
    {
        int _searchSelectedValue;
        decimal _itemChildItemPrice;
        decimal _itemChildItemQTYExist;
        decimal _itemChildItemQTYSell;
        decimal _serviceChildServiceCost;
        decimal _childDiscount;
        decimal _billTotal;
        decimal _billTotalAfterEachDiscount;
        decimal _billDiscount;
        decimal _billTotalAfterDiscount;
        decimal _billClientPayment;
        int _currentBillNo;
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
        Client _selectedClient;
        Item _selectedItem;
        Service _selectedService;
        BillItemMove _dataGridSelectedBillItemMove;
        BillServiceMove _dataGridSelectedBillServiceMove;

        ObservableCollection<object> _searchItems;

        ObservableCollection<BillItemMove> _billItemsMoves;

        ObservableCollection<BillServiceMove> _billServicesMoves;

        public int SearchSelectedValue
        {
            get => _searchSelectedValue;
            set
            {
                if (value != _searchSelectedValue)
                {
                    _searchSelectedValue = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal ItemChildItemPrice
        {
            get => _itemChildItemPrice;
            set
            {
                if (value != _itemChildItemPrice)
                {
                    _itemChildItemPrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal ItemChildItemQTYSell
        {
            get => _itemChildItemQTYSell;
            set
            {
                if (value != _itemChildItemQTYSell)
                {
                    _itemChildItemQTYSell = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal ItemChildItemQTYExist
        {
            get => _itemChildItemQTYExist;
            set
            {
                if (value != _itemChildItemQTYExist)
                {
                    _itemChildItemQTYExist = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal ServiceChildServiceCost
        {
            get => _serviceChildServiceCost;
            set
            {
                if (value != _serviceChildServiceCost)
                {
                    _serviceChildServiceCost = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal ChildDiscount
        {
            get => _childDiscount;
            set
            {
                if (value != _childDiscount)
                {
                    _childDiscount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal BillTotal
        {
            get => _billTotal;
            set
            {
                if (value != _billTotal)
                {
                    _billTotal = value;
                    RaisePropertyChanged();
                }
            }
        }

        public decimal BillTotalAfterEachDiscount
        {
            get => _billTotalAfterEachDiscount;
            set
            {
                if (value != _billTotalAfterEachDiscount)
                {
                    _billTotalAfterEachDiscount = value;
                    RaisePropertyChanged();
                }
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
                    if (_billDiscount > 0)
                    {
                        BillTotalAfterDiscount = BillTotalAfterEachDiscount - (BillTotalAfterEachDiscount * (_billDiscount / 100));
                    }
                    else
                    {
                        BillTotalAfterDiscount = BillTotalAfterEachDiscount;
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public decimal BillTotalAfterDiscount
        {
            get => _billTotalAfterDiscount;
            set
            {
                if (value != _billTotalAfterDiscount)
                {
                    _billTotalAfterDiscount = value;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        public int CurrentBillNo
        {
            get => _currentBillNo;
            set
            {
                if (value != _currentBillNo)
                {
                    _currentBillNo = value;
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

        public string ItemChildItemName
        {
            get => _itemChildItemName;
            set
            {
                if (value != _itemChildItemName)
                {
                    _itemChildItemName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ServiceChildServiceName
        {
            get => _serviceChildServiceName;
            set
            {
                if (value != _serviceChildServiceName)
                {
                    _serviceChildServiceName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ServiceChildNotes
        {
            get => _serviceChildNotes;
            set
            {
                if (value != _serviceChildNotes)
                {
                    _serviceChildNotes = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ItemChildNotes
        {
            get => _itemChildNotes;
            set
            {
                if (value != _itemChildNotes)
                {
                    _itemChildNotes = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByItem
        {
            get => _byItem;
            set
            {
                if (value != _byItem)
                {
                    _byItem = value;
                    if (_byItem)
                    {
                        using (var db = new PhonyDbContext())
                        {
                            SearchSelectedValue = 0;
                            SearchItems = new ObservableCollection<object>(db.Items.Where(i => i.Group == ItemGroup.Other && i.QTY > 0));
                        }
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByCard
        {
            get => _byCard;
            set
            {
                if (value != _byCard)
                {
                    _byCard = value;
                    if (_byCard)
                    {
                        using (var db = new PhonyDbContext())
                        {
                            SearchSelectedValue = 0;
                            SearchItems = new ObservableCollection<object>(db.Items.Where(c => c.Group == ItemGroup.Card && c.QTY > 0));
                        }
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByService
        {
            get => _byService;
            set
            {
                if (value != _byService)
                {
                    _byService = value;
                    if (_byService)
                    {
                        using (var db = new PhonyDbContext())
                        {
                            SearchSelectedValue = 0;
                            SearchItems = new ObservableCollection<object>(db.Services);
                        }
                    }
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByName
        {
            get => _byName;
            set
            {
                if (value != _byName)
                {
                    _byName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByShopCode
        {
            get => _byShopCode;
            set
            {
                if (value != _byShopCode)
                {
                    _byShopCode = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ByBarCode
        {
            get => _byBarCode;
            set
            {
                if (value != _byBarCode)
                {
                    _byBarCode = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsAddItemChildOpen
        {
            get => _isAddItemChildOpen;
            set
            {
                if (value != _isAddItemChildOpen)
                {
                    _isAddItemChildOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsAddServiceChildOpen
        {
            get => _isAddServiceChildOpen;
            set
            {
                if (value != _isAddServiceChildOpen)
                {
                    _isAddServiceChildOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsAddBillNote
        {
            get => _isAddBillNote;
            set
            {
                if (value != _isAddBillNote)
                {
                    _isAddBillNote = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                if (value != _selectedClient)
                {
                    _selectedClient = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value != _selectedItem)
                {
                    _selectedItem = value;
                }
            }
        }

        public Service SelectedService
        {
            get => _selectedService;
            set
            {
                if (value != _selectedService)
                {
                    _selectedService = value;
                    RaisePropertyChanged();
                }
            }
        }

        public BillItemMove DataGridSelectedBillItemMove
        {
            get => _dataGridSelectedBillItemMove;
            set
            {
                if (value != _dataGridSelectedBillItemMove)
                {
                    _dataGridSelectedBillItemMove = value;
                    RaisePropertyChanged();
                }
            }
        }

        public BillServiceMove DataGridSelectedBillServiceMove
        {
            get => _dataGridSelectedBillServiceMove;
            set
            {
                if (value != _dataGridSelectedBillServiceMove)
                {
                    _dataGridSelectedBillServiceMove = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<object> SearchItems
        {
            get => _searchItems;
            set
            {
                if (value != _searchItems)
                {
                    _searchItems = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<BillItemMove> BillItemsMoves
        {
            get => _billItemsMoves;
            set
            {
                if (value != _billItemsMoves)
                {
                    _billItemsMoves = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<BillServiceMove> BillServicesMoves
        {
            get => _billServicesMoves;
            set
            {
                if (value != _billServicesMoves)
                {
                    _billServicesMoves = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<Client> Clients { get; set; }

        public ObservableCollection<Item> Items { get; set; }

        public ObservableCollection<Service> Services { get; set; }

        public ObservableCollection<User> Users { get; set; }

        public ICommand Search { get; set; }
        public ICommand AddBillMove { get; set; }
        public ICommand AddItemToBill { get; set; }
        public ICommand AddServiceToBill { get; set; }
        public ICommand DeleteBillMove { get; set; }
        public ICommand RedoBill { get; set; }
        public ICommand SaveBill { get; set; }
        public ICommand SaveAndShow { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        Bills Message = Application.Current.Windows.OfType<Bills>().FirstOrDefault();

        public BillVM()
        {
            LoadCommands();
            ByName = true;
            ByItem = true;
            using (var db = new PhonyDbContext())
            {
                Clients = new ObservableCollection<Client>(db.Clients);
                Items = new ObservableCollection<Item>(db.Items);
                Services = new ObservableCollection<Service>(db.Services);
                Users = new ObservableCollection<User>(db.Users);
                SearchItems = new ObservableCollection<object>(db.Items.Where(i => i.Group == ItemGroup.Other && i.QTY > 0));
                BillItemsMoves = new ObservableCollection<BillItemMove>();
                BillServicesMoves = new ObservableCollection<BillServiceMove>();
            }
            NewBillNo();
        }

        public void LoadCommands()
        {
            Search = new CustomCommand(DoSearch, CanSearch);
            AddBillMove = new CustomCommand(DoAddBillMove, CanAddBillMove);
            AddItemToBill = new CustomCommand(DoAddItemToBill, CanAddItemToBill);
            AddServiceToBill = new CustomCommand(DoAddServiceToBill, CanAddServiceToBill);
            DeleteBillMove = new CustomCommand(DoDeleteBillMove, CanDeleteBillMove);
            RedoBill = new CustomCommand(DoRedoBill, CanRedoBill);
            SaveBill = new CustomCommand(DoSaveBill, CanSaveBill);
            SaveAndShow = new CustomCommand(DoSaveAndShow, CanSaveAndShow);
        }

        async Task<int> SaveBillNoAsync()
        {
            if (BillClientPayment > BillTotalAfterDiscount)
            {
                await Message.ShowMessageAsync("خطأ", "لا يمكن تدفيع العميل اكتر من قيمه الفاتورة");
                return 0;
            }
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
            using (var db = new PhonyDbContext())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var bi = new Bill
                        {
                            ClientId = SelectedClient.Id,
                            StoreId = db.Stores.FirstOrDefault().Id,
                            Discount = BillDiscount,
                            TotalAfterDiscounts = BillTotalAfterDiscount,
                            TotalPayed = BillClientPayment,
                            Notes = billNote,
                            CreateDate = DateTime.Now,
                            CreatedById = CurrentUser.Id,
                            EditById = null,
                            EditDate = null
                        };
                        db.Bills.Add(bi);
                        await db.SaveChangesAsync();
                        foreach (var item in BillItemsMoves)
                        {
                            item.BillId = bi.Id;
                            db.BillsItemsMoves.Add(item);
                            var i = db.Items.SingleOrDefault(n => n.Id == item.ItemId);
                            i.QTY -= item.QTY;
                        }
                        foreach (var service in BillServicesMoves)
                        {
                            service.BillId = bi.Id;
                            db.BillsServicesMoves.Add(service);
                            var s = db.Services.SingleOrDefault(n => n.Id == service.ServiceId);
                            s.Balance -= service.ServicePayment;
                        }
                        if (BillClientPayment < BillTotalAfterDiscount)
                        {
                            var c = db.Clients.SingleOrDefault(n => n.Id == SelectedClient.Id);
                            c.Balance += BillTotalAfterDiscount - BillClientPayment;
                        }
                        await db.SaveChangesAsync();
                        dbContextTransaction.Commit();
                        Clear();
                        CurrentBillNo = bi.Id + 1;
                        return bi.Id;
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        await Core.SaveExceptionAsync(ex);
                        return -1;
                    }
                }
            }
        }

        private bool CanSaveAndShow(object obj)
        {
            return CanSaveBill(obj);
        }

        private async void DoSaveAndShow(object obj)
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

        private bool CanSaveBill(object obj)
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

        private async void DoSaveBill(object obj)
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

        private bool CanRedoBill(object obj)
        {
            return true;
        }

        private void DoRedoBill(object obj)
        {
            Clear();
        }

        private bool CanDeleteBillMove(object obj)
        {
            if ((DataGridSelectedBillItemMove == null && ByItem) || (DataGridSelectedBillServiceMove == null && ByService))
            {
                return false;
            }
            return true;
        }

        private void DoDeleteBillMove(object obj)
        {
            if (ByItem)
            {
                BillItemsMoves.Remove(DataGridSelectedBillItemMove);
            }
            if (ByService)
            {
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
            using (var db = new PhonyDbContext())
            {
                if (ByItem)
                {
                    SearchItems = new ObservableCollection<object>(db.Items.Where(i => i.Group == ItemGroup.Other && i.QTY > 0));
                }
                else if (ByCard)
                {
                    SearchItems = new ObservableCollection<object>(db.Items.Where(c => c.Group == ItemGroup.Card && c.QTY > 0));
                }
                else if (ByService)
                {
                    SearchItems = new ObservableCollection<object>(db.Services);
                }
                Services = new ObservableCollection<Service>(db.Services);
                Items = new ObservableCollection<Item>(db.Items);
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
                ServiceChildServiceCost = 0;
                SelectedService = null;
                ServiceChildNotes = null;
            }
            ChildDiscount = 0;
        }

        private bool CanAddServiceToBill(object obj)
        {
            if (ServiceChildServiceCost > 0)
            {
                return true;
            }
            return false;
        }

        private void DoAddServiceToBill(object obj)
        {
            decimal balanceNeeded = 0;
            foreach (var item in BillServicesMoves)
            {
                if (item.ServiceId == SearchSelectedValue)
                {
                    balanceNeeded += item.ServicePayment;
                }
            }
            balanceNeeded += ServiceChildServiceCost;
            if (SelectedService.Balance >= balanceNeeded)
            {
                BillServicesMoves.Add(new BillServiceMove
                {
                    BillId = CurrentBillNo,
                    ServiceId = SearchSelectedValue,
                    ServicePayment = ServiceChildServiceCost,
                    Discount = ChildDiscount,
                    Notes = ServiceChildNotes,
                    CreatedById = CurrentUser.Id,
                    CreateDate = DateTime.Now,
                    EditById = null,
                    EditDate = null
                });
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

        private bool CanAddItemToBill(object obj)
        {
            if (ItemChildItemQTYSell > 0)
            {
                return true;
            }
            return false;
        }

        private void DoAddItemToBill(object obj)
        {
            decimal QTYNeeded = 0;
            foreach (var item in BillItemsMoves)
            {
                if (item.ItemId == SearchSelectedValue)
                {
                    QTYNeeded += item.QTY;
                }
            }
            QTYNeeded += ItemChildItemQTYSell;
            if (SelectedItem.QTY >= QTYNeeded)
            {
                var ItemToQtyPrice = SelectedItem.SalePrice * ItemChildItemQTYSell;
                BillItemsMoves.Add(new BillItemMove
                {
                    BillId = CurrentBillNo,
                    ItemId = SearchSelectedValue,
                    QTY = ItemChildItemQTYSell,
                    Discount = ChildDiscount,
                    Notes = ItemChildNotes,
                    CreatedById = CurrentUser.Id,
                    CreateDate = DateTime.Now,
                    EditById = null,
                    EditDate = null
                });
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

        private bool CanAddBillMove(object obj)
        {
            if (SearchSelectedValue > 0)
            {
                return true;
            }
            return false;
        }

        private void DoAddBillMove(object obj)
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
                ItemChildItemPrice = SelectedItem.SalePrice;
                ItemChildItemQTYExist = SelectedItem.QTY;
            }
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
            if (ByItem)
            {
                if (ByName)
                {
                    using (var db = new PhonyDbContext())
                    {
                        SearchSelectedValue = db.Items.SingleOrDefault(i => i.Group == ItemGroup.Other && i.QTY > 0 && i.Name.Contains(SearchText)).Id;
                    }
                }
                else if (ByBarCode)
                {
                    using (var db = new PhonyDbContext())
                    {
                        SearchSelectedValue = db.Items.SingleOrDefault(i => i.Group == ItemGroup.Other && i.QTY > 0 && i.Barcode == SearchText).Id;
                    }
                }
                else
                {
                    using (var db = new PhonyDbContext())
                    {
                        SearchSelectedValue = db.Items.SingleOrDefault(i => i.Group == ItemGroup.Other && i.QTY > 0 && i.Shopcode == SearchText).Id;
                    }
                }
            }
            else if (ByCard)
            {
                if (ByName)
                {
                    using (var db = new PhonyDbContext())
                    {
                        SearchSelectedValue = db.Items.SingleOrDefault(c => c.Group == ItemGroup.Card && c.QTY > 0 && c.Name.Contains(SearchText)).Id;
                    }
                }
                else if (ByBarCode)
                {
                    using (var db = new PhonyDbContext())
                    {
                        SearchSelectedValue = db.Items.SingleOrDefault(c => c.Group == ItemGroup.Card && c.QTY > 0 && c.Barcode == SearchText).Id;
                    }
                }
                else
                {
                    using (var db = new PhonyDbContext())
                    {
                        SearchSelectedValue = db.Items.SingleOrDefault(c => c.Group == ItemGroup.Card && c.QTY > 0 && c.Shopcode == SearchText).Id;
                    }
                }
            }
            else
            {
                using (var db = new PhonyDbContext())
                {
                    SearchSelectedValue = db.Services.SingleOrDefault(s => s.Name.Contains(SearchText)).Id;
                }
            }
        }

        void NewBillNo()
        {
            try
            {
                using (var db = new PhonyDbContext())
                {
                    CurrentBillNo = db.Bills.OrderByDescending(p => p.Id).FirstOrDefault().Id + 1;
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