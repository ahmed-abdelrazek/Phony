using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Model;
using Phony.Persistence;
using Phony.Utility;
using Phony.View;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModel
{
    public class ItemVM : CommonBase
    {
        int _itemId;
        int _selectedCompanyValue;
        int _selectedSupplierValue;
        string _name;
        string _barcode;
        string _shopcode;
        string _searchText;
        string _notes;
        string _childName;
        string _childPrice;
        static string _itemsCount;
        static string _itemsPurchasePrice;
        static string _itemsSalePrice;
        static string _itemsProfit;
        byte[] _image;
        byte[] _childImage;
        ItemGroup _group;
        decimal _purchasePrice;
        decimal _wholeSalePrice;
        decimal _halfWholeSalePrice;
        decimal _retailPrice;
        decimal _salePrice;
        decimal _qty;
        bool _byName;
        bool _byBarCode;
        bool _byShopCode;
        bool _fastResult;
        bool _openFastResult;
        bool _isAddItemFlyoutOpen;
        Item _dataGridSelectedItem;

        ObservableCollection<Company> _companies;
        ObservableCollection<Supplier> _suppliers;
        ObservableCollection<Item> _items;

        public int ItemId
        {
            get => _itemId;
            set
            {
                if (value != _itemId)
                {
                    _itemId = value;
                    RaisePropertyChanged(nameof(ItemId));
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
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        public string Barcode
        {
            get => _barcode;
            set
            {
                if (value != _barcode)
                {
                    _barcode = value;
                    RaisePropertyChanged(nameof(Barcode));
                }
            }
        }

        public string Shopcode
        {
            get => _shopcode;
            set
            {
                if (value != _shopcode)
                {
                    _shopcode = value;
                    RaisePropertyChanged(nameof(Shopcode));
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
                    RaisePropertyChanged(nameof(SearchText));
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
                    RaisePropertyChanged(nameof(Notes));
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
                    RaisePropertyChanged(nameof(ChildName));
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
                    RaisePropertyChanged(nameof(ChildPrice));
                }
            }
        }

        public string ItemsCount
        {
            get => _itemsCount;
            set
            {
                if (value != _itemsCount)
                {
                    _itemsCount = value;
                    RaisePropertyChanged(nameof(ItemsCount));
                }
            }
        }

        public string ItemsPurchasePrice
        {
            get => _itemsPurchasePrice;
            set
            {
                if (value != _itemsPurchasePrice)
                {
                    _itemsPurchasePrice = value;
                    RaisePropertyChanged(nameof(ItemsPurchasePrice));
                }
            }
        }

        public string ItemsSalePrice
        {
            get => _itemsSalePrice;
            set
            {
                if (value != _itemsSalePrice)
                {
                    _itemsSalePrice = value;
                    RaisePropertyChanged(nameof(ItemsSalePrice));
                }
            }
        }

        public string ItemsProfit
        {
            get => _itemsProfit;
            set
            {
                if (value != _itemsProfit)
                {
                    _itemsProfit = value;
                    RaisePropertyChanged(nameof(ItemsProfit));
                }
            }
        }

        public byte[] Image
        {
            get => _image;
            set
            {
                if (value != _image)
                {
                    _image = value;
                    RaisePropertyChanged(nameof(Image));
                }
            }
        }

        public byte[] ChildImage
        {
            get => _childImage;
            set
            {
                if (value != _childImage)
                {
                    _childImage = value;
                    RaisePropertyChanged(nameof(ChildImage));
                }
            }
        }

        public ItemGroup Group
        {
            get => _group;
            set
            {
                if (value != _group)
                {
                    _group = value;
                    RaisePropertyChanged(nameof(Group));
                }
            }
        }

        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                if (value != _purchasePrice)
                {
                    _purchasePrice = value;
                    RaisePropertyChanged(nameof(PurchasePrice));
                }
            }
        }

        public decimal WholeSalePrice
        {
            get => _wholeSalePrice;
            set
            {
                if (value != _wholeSalePrice)
                {
                    _wholeSalePrice = value;
                    RaisePropertyChanged(nameof(WholeSalePrice));
                }
            }
        }

        public decimal HalfWholeSalePrice
        {
            get => _halfWholeSalePrice;
            set
            {
                if (value != _halfWholeSalePrice)
                {
                    _halfWholeSalePrice = value;
                    RaisePropertyChanged(nameof(HalfWholeSalePrice));
                }
            }
        }

        public decimal RetailPrice
        {
            get => _retailPrice;
            set
            {
                if (value != _retailPrice)
                {
                    _retailPrice = value;
                    RaisePropertyChanged(nameof(RetailPrice));
                }
            }
        }

        public decimal SalePrice
        {
            get => _salePrice;
            set
            {
                if (value != _salePrice)
                {
                    _salePrice = value;
                    RaisePropertyChanged(nameof(SalePrice));
                }
            }
        }

        public decimal QTY
        {
            get => _qty;
            set
            {
                if (value != _qty)
                {
                    _qty = value;
                    RaisePropertyChanged(nameof(QTY));
                }
            }
        }

        public int SelectedCompanyValue
        {
            get => _selectedCompanyValue;
            set
            {
                if (value != _selectedCompanyValue)
                {
                    _selectedCompanyValue = value;
                    RaisePropertyChanged(nameof(SelectedCompanyValue));
                }
            }
        }

        public int SelectedSupplierValue
        {
            get => _selectedSupplierValue;
            set
            {
                if (value != _selectedSupplierValue)
                {
                    _selectedSupplierValue = value;
                    RaisePropertyChanged(nameof(SelectedSupplierValue));
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
                    RaisePropertyChanged(nameof(ByName));
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
                    RaisePropertyChanged(nameof(ByBarCode));
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
                    RaisePropertyChanged(nameof(ByShopCode));
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
                    RaisePropertyChanged(nameof(FastResult));
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
                    RaisePropertyChanged(nameof(OpenFastResult));
                }
            }
        }

        public Item DataGridSelectedItem
        {
            get => _dataGridSelectedItem;
            set
            {
                if (value != _dataGridSelectedItem)
                {
                    _dataGridSelectedItem = value;
                    RaisePropertyChanged(nameof(DataGridSelectedItem));
                }
            }
        }

        public ObservableCollection<Company> Companies
        {
            get => _companies;
            set
            {
                if (value != _companies)
                {
                    _companies = value;
                    RaisePropertyChanged(nameof(Companies));
                }
            }
        }

        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                if (value != _suppliers)
                {
                    _suppliers = value;
                    RaisePropertyChanged(nameof(Suppliers));
                }
            }
        }

        public ObservableCollection<Item> Items
        {
            get => _items;
            set
            {
                if (value != _items)
                {
                    _items = value;
                    RaisePropertyChanged(nameof(Items));
                }
            }
        }

        public ObservableCollection<User> Users { get; set; }

        public bool IsAddItemFlyoutOpen
        {
            get => _isAddItemFlyoutOpen;
            set
            {
                if (value != _isAddItemFlyoutOpen)
                {
                    _isAddItemFlyoutOpen = value;
                    RaisePropertyChanged(nameof(IsAddItemFlyoutOpen));
                }
            }
        }

        public ICommand OpenAddItemFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand DeleteItem { get; set; }
        public ICommand ReloadAllItems { get; set; }
        public ICommand Search { get; set; }
        public ICommand AddItem { get; set; }
        public ICommand EditItem { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        Items ItemsMassage = Application.Current.Windows.OfType<Items>().FirstOrDefault();


        public ItemVM()
        {
            LoadCommands();
            ByName = true;
            using (var db = new PhonyDbContext())
            {
                Companies = new ObservableCollection<Company>(db.Companies);
                Suppliers = new ObservableCollection<Supplier>(db.Suppliers);
                Items = new ObservableCollection<Item>(db.Items.Where(i => i.Group == ItemGroup.Other));
                Users = new ObservableCollection<User>(db.Users);
            }
            new Thread(() =>
            {
                ItemsCount = $"إجمالى الاصناف: {Items.Count().ToString()}";
                ItemsPurchasePrice = $"اجمالى سعر الشراء: {decimal.Round(Items.Sum(i => i.PurchasePrice * i.QTY), 2).ToString()}";
                ItemsSalePrice = $"اجمالى سعر البيع: {decimal.Round(Items.Sum(i => i.SalePrice * i.QTY), 2).ToString()}";
                ItemsProfit = $"تقدير صافى الربح: {decimal.Round((Items.Sum(i => i.SalePrice * i.QTY) - Items.Sum(i => i.PurchasePrice * i.QTY)), 2).ToString()}";
                Thread.CurrentThread.Abort();
            }).Start();
        }

        public void LoadCommands()
        {
            OpenAddItemFlyout = new CustomCommand(DoOpenAddItemFlyout, CanOpenAddItemFlyout);
            SelectImage = new CustomCommand(DoSelectImage, CanSelectImage);
            FillUI = new CustomCommand(DoFillUI, CanFillUI);
            DeleteItem = new CustomCommand(DoDeleteItem, CanDeleteItem);
            ReloadAllItems = new CustomCommand(DoReloadAllItems, CanReloadAllItems);
            Search = new CustomCommand(DoSearch, CanSearch);
            AddItem = new CustomCommand(DoAddItem, CanAddItem);
            EditItem = new CustomCommand(DoEditItem, CanEditItem);
        }

        private bool CanEditItem(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || ItemId == 0 || SelectedCompanyValue == 0 || SelectedSupplierValue == 0 || DataGridSelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditItem(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var i = db.Items.Get(DataGridSelectedItem.Id);
                i.Name = Name;
                i.Barcode = Barcode;
                i.Shopcode = Shopcode;
                i.Image = Image;
                i.PurchasePrice = PurchasePrice;
                i.WholeSalePrice = WholeSalePrice;
                i.HalfWholeSalePrice = HalfWholeSalePrice;
                i.RetailPrice = RetailPrice;
                i.SalePrice = SalePrice;
                i.QTY = QTY;
                i.CompanyId = SelectedCompanyValue;
                i.SupplierId = SelectedSupplierValue;
                i.Notes = Notes;
                i.EditDate = DateTime.Now;
                i.EditById = CurrentUser.Id;
                db.Complete();
                ItemId = 0;
                Items.Remove(DataGridSelectedItem);
                Items.Add(i);
                DataGridSelectedItem = null;
                ItemsMassage.ShowMessageAsync("تمت العملية", "تم تعديل الصنف بنجاح");
            }
        }

        private bool CanAddItem(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || SelectedCompanyValue == 0 || SelectedSupplierValue == 0)
            {
                return false;
            }
            return true;
        }

        private void DoAddItem(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var i = new Item
                {
                    Name = Name,
                    Barcode = Barcode,
                    Shopcode = Shopcode,
                    Image = Image,
                    Group = ItemGroup.Other,
                    PurchasePrice = PurchasePrice,
                    WholeSalePrice = WholeSalePrice,
                    HalfWholeSalePrice = HalfWholeSalePrice,
                    RetailPrice = RetailPrice,
                    SalePrice = SalePrice,
                    QTY = QTY,
                    CompanyId = SelectedCompanyValue,
                    SupplierId = SelectedSupplierValue,
                    Notes = Notes,
                    CreateDate = DateTime.Now,
                    CreatedById = CurrentUser.Id,
                    EditDate = null,
                    EditById = null
                };
                db.Items.Add(i);
                db.Complete();
                Items.Add(i);
                ItemsMassage.ShowMessageAsync("تمت العملية", "تم اضافة الصنف بنجاح");
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
            using (var db = new PhonyDbContext())
            {
                if (ByName)
                {
                    Items = new ObservableCollection<Item>(db.Items.Where(i => i.Name.Contains(SearchText) && i.Group == ItemGroup.Other));
                    if (FastResult)
                    {
                        ChildName = Items.FirstOrDefault().Name;
                        ChildPrice = Items.FirstOrDefault().SalePrice.ToString();
                        ChildImage = Items.FirstOrDefault().Image;
                        OpenFastResult = true;
                    }
                }
                else if (ByBarCode)
                {
                    Items = new ObservableCollection<Item>(db.Items.Where(i => i.Barcode == SearchText && i.Group == ItemGroup.Other));
                }
                else
                {
                    Items = new ObservableCollection<Item>(db.Items.Where(i => i.Shopcode == SearchText && i.Group == ItemGroup.Other));
                }
            }
        }

        private bool CanReloadAllItems(object obj)
        {
            return true;
        }

        private void DoReloadAllItems(object obj)
        {
            using (var db = new PhonyDbContext())
            {
                Items = new ObservableCollection<Item>(db.Items.Where(i => i.Group == ItemGroup.Other));
            }
        }

        private bool CanDeleteItem(object obj)
        {
            if (DataGridSelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteItem(object obj)
        {
            var result = await ItemsMassage.ShowMessageAsync("حذف الصنف", $"هل انت متاكد من حذف الصنف {DataGridSelectedItem.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new UnitOfWork(new PhonyDbContext()))
                {
                    db.Items.Remove(db.Items.Get(DataGridSelectedItem.Id));
                    db.Complete();
                    Items.Remove(DataGridSelectedItem);
                }
                DataGridSelectedItem = null;
                await ItemsMassage.ShowMessageAsync("تمت العملية", "تم حذف الصنف بنجاح");
            }
        }

        private bool CanFillUI(object obj)
        {
            if (DataGridSelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private void DoFillUI(object obj)
        {
            ItemId = DataGridSelectedItem.Id;
            Name = DataGridSelectedItem.Name;
            Barcode = DataGridSelectedItem.Barcode;
            Shopcode = DataGridSelectedItem.Shopcode;
            Image = DataGridSelectedItem.Image;
            PurchasePrice = DataGridSelectedItem.PurchasePrice;
            WholeSalePrice = DataGridSelectedItem.WholeSalePrice;
            HalfWholeSalePrice = DataGridSelectedItem.HalfWholeSalePrice;
            RetailPrice = DataGridSelectedItem.RetailPrice;
            SalePrice = DataGridSelectedItem.SalePrice;
            QTY = DataGridSelectedItem.QTY;
            SelectedCompanyValue = DataGridSelectedItem.CompanyId;
            SelectedSupplierValue = DataGridSelectedItem.SupplierId;
            Notes = DataGridSelectedItem.Notes;
            IsAddItemFlyoutOpen = true;
        }

        private bool CanSelectImage(object obj)
        {
            return true;
        }

        private void DoSelectImage(object obj)
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

        private bool CanOpenAddItemFlyout(object obj)
        {
            return true;
        }

        private void DoOpenAddItemFlyout(object obj)
        {
            if (IsAddItemFlyoutOpen)
            {
                IsAddItemFlyoutOpen = false;
            }
            else
            {
                IsAddItemFlyoutOpen = true;
            }
        }
    }
}