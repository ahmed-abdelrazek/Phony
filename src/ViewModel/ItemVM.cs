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
        long _itemId;
        long _selectedCompanyValue;
        long _selectedSupplierValue;
        string _name;
        string _barcode;
        string _shopcode;
        string _searchText;
        string _notes;
        string _childName;
        string _childPrice;
        string _childQTY;
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

        public long ItemId
        {
            get => _itemId;
            set
            {
                if (value != _itemId)
                {
                    _itemId = value;
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

        public string Barcode
        {
            get => _barcode;
            set
            {
                if (value != _barcode)
                {
                    _barcode = value;
                    RaisePropertyChanged();
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

        public string ChildQTY
        {
            get => _childQTY;
            set
            {
                if (value != _childQTY)
                {
                    _childQTY = value;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        public long SelectedCompanyValue
        {
            get => _selectedCompanyValue;
            set
            {
                if (value != _selectedCompanyValue)
                {
                    _selectedCompanyValue = value;
                    RaisePropertyChanged();
                }
            }
        }

        public long SelectedSupplierValue
        {
            get => _selectedSupplierValue;
            set
            {
                if (value != _selectedSupplierValue)
                {
                    _selectedSupplierValue = value;
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

        public bool IsAddItemFlyoutOpen
        {
            get => _isAddItemFlyoutOpen;
            set
            {
                if (value != _isAddItemFlyoutOpen)
                {
                    _isAddItemFlyoutOpen = value;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<User> Users { get; set; }

        public ICommand OpenAddItemFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand DeleteItem { get; set; }
        public ICommand ReloadAllItems { get; set; }
        public ICommand Search { get; set; }
        public ICommand AddItem { get; set; }
        public ICommand EditItem { get; set; }

        Items ItemsMessage = Application.Current.Windows.OfType<Items>().FirstOrDefault();

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
                ItemsSalePrice = $"اجمالى سعر البيع: {decimal.Round(Items.Sum(i => i.RetailPrice * i.QTY), 2).ToString()}";
                ItemsProfit = $"تقدير صافى الربح: {decimal.Round((Items.Sum(i => i.RetailPrice * i.QTY) - Items.Sum(i => i.PurchasePrice * i.QTY)), 2).ToString()}";
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
            if (string.IsNullOrWhiteSpace(Name) || ItemId < 1 || SelectedCompanyValue < 1 || SelectedSupplierValue < 1 || DataGridSelectedItem == null)
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
                i.QTY = QTY;
                i.CompanyId = SelectedCompanyValue;
                i.SupplierId = SelectedSupplierValue;
                i.Notes = Notes;
                i.EditDate = DateTime.Now;
                i.EditById = Core.ReadUserSession().Id;
                db.Complete();
                Items[Items.IndexOf(DataGridSelectedItem)] = i;
                ItemId = 0;
                DataGridSelectedItem = null;
                ItemsMessage.ShowMessageAsync("تمت العملية", "تم تعديل الصنف بنجاح");
            }
        }

        private bool CanAddItem(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || SelectedCompanyValue < 1 || SelectedSupplierValue < 1)
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
                    QTY = QTY,
                    CompanyId = SelectedCompanyValue,
                    SupplierId = SelectedSupplierValue,
                    Notes = Notes,
                    CreateDate = DateTime.Now,
                    CreatedById = Core.ReadUserSession().Id,
                    EditDate = null,
                    EditById = null
                };
                db.Items.Add(i);
                db.Complete();
                Items.Add(i);
                ItemsMessage.ShowMessageAsync("تمت العملية", "تم اضافة الصنف بنجاح");
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
            try
            {
                using (var db = new PhonyDbContext())
                {
                    if (ByName)
                    {
                        Items = new ObservableCollection<Item>(db.Items.Where(i => i.Name.Contains(SearchText) && i.Group == ItemGroup.Other));
                    }
                    else if (ByBarCode)
                    {
                        Items = new ObservableCollection<Item>(db.Items.Where(i => i.Barcode == SearchText && i.Group == ItemGroup.Other));
                    }
                    else
                    {
                        Items = new ObservableCollection<Item>(db.Items.Where(i => i.Shopcode == SearchText && i.Group == ItemGroup.Other));
                    }
                    if (Items.Count > 0)
                    {
                        if (FastResult)
                        {
                            ChildName = Items.FirstOrDefault().Name;
                            ChildPrice = Items.FirstOrDefault().RetailPrice.ToString();
                            ChildQTY= Items.FirstOrDefault().QTY.ToString();
                            ChildImage = Items.FirstOrDefault().Image;
                            OpenFastResult = true;
                        }
                    }
                    else
                    {
                        ItemsMessage.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                    }
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله");
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
            var result = await ItemsMessage.ShowMessageAsync("حذف الصنف", $"هل انت متاكد من حذف الصنف {DataGridSelectedItem.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new UnitOfWork(new PhonyDbContext()))
                {
                    db.Items.Remove(db.Items.Get(DataGridSelectedItem.Id));
                    db.Complete();
                    Items.Remove(DataGridSelectedItem);
                }
                DataGridSelectedItem = null;
                await ItemsMessage.ShowMessageAsync("تمت العملية", "تم حذف الصنف بنجاح");
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