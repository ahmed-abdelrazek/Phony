using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Models;
using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class ItemVM : BindableBase
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
            set => SetProperty(ref _itemId, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Barcode
        {
            get => _barcode;
            set => SetProperty(ref _barcode, value);
        }

        public string Shopcode
        {
            get => _shopcode;
            set => SetProperty(ref _shopcode, value);
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

        public string ChildQTY
        {
            get => _childQTY;
            set => SetProperty(ref _childQTY, value);
        }

        public string ItemsCount
        {
            get => _itemsCount;
            set => SetProperty(ref _itemsCount, value);
        }

        public string ItemsPurchasePrice
        {
            get => _itemsPurchasePrice;
            set => SetProperty(ref _itemsPurchasePrice, value);
        }

        public string ItemsSalePrice
        {
            get => _itemsSalePrice;
            set => SetProperty(ref _itemsSalePrice, value);
        }

        public string ItemsProfit
        {
            get => _itemsProfit;
            set => SetProperty(ref _itemsProfit, value);
        }

        public byte[] Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public byte[] ChildImage
        {
            get => _childImage;
            set => SetProperty(ref _childImage, value);
        }

        public ItemGroup Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
        }

        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set => SetProperty(ref _purchasePrice, value);
        }

        public decimal WholeSalePrice
        {
            get => _wholeSalePrice;
            set => SetProperty(ref _wholeSalePrice, value);
        }

        public decimal HalfWholeSalePrice
        {
            get => _halfWholeSalePrice;
            set => SetProperty(ref _halfWholeSalePrice, value);
        }

        public decimal RetailPrice
        {
            get => _retailPrice;
            set => SetProperty(ref _retailPrice, value);
        }

        public decimal QTY
        {
            get => _qty;
            set => SetProperty(ref _qty, value);
        }

        public long SelectedCompanyValue
        {
            get => _selectedCompanyValue;
            set => SetProperty(ref _selectedCompanyValue, value);
        }

        public long SelectedSupplierValue
        {
            get => _selectedSupplierValue;
            set => SetProperty(ref _selectedSupplierValue, value);
        }

        public bool ByName
        {
            get => _byName;
            set => SetProperty(ref _byName, value);
        }

        public bool ByBarCode
        {
            get => _byBarCode;
            set => SetProperty(ref _byBarCode, value);
        }

        public bool ByShopCode
        {
            get => _byShopCode;
            set => SetProperty(ref _byShopCode, value);
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

        public bool IsAddItemFlyoutOpen
        {
            get => _isAddItemFlyoutOpen;
            set => SetProperty(ref _isAddItemFlyoutOpen, value);
        }

        public Item DataGridSelectedItem
        {
            get => _dataGridSelectedItem;
            set => SetProperty(ref _dataGridSelectedItem, value);
        }

        public ObservableCollection<Company> Companies
        {
            get => _companies;
            set => SetProperty(ref _companies, value);
        }

        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set => SetProperty(ref _suppliers, value);
        }

        public ObservableCollection<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public ObservableCollection<User> Users { get; set; }

        public ICommand AddItem { get; set; }
        public ICommand EditItem { get; set; }
        public ICommand DeleteItem { get; set; }
        public ICommand OpenAddItemFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand ReloadAllItems { get; set; }
        public ICommand Search { get; set; }

        Items ItemsMessage = Application.Current.Windows.OfType<Items>().FirstOrDefault();

        public ItemVM()
        {
            LoadCommands();
            ByName = true;
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Companies = new ObservableCollection<Company>(db.GetCollection<Company>(Data.DBCollections.Companies).FindAll());
                Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(Data.DBCollections.Suppliers).FindAll());
                Items = new ObservableCollection<Item>(db.GetCollection<Item>(Data.DBCollections.Items.ToString()).Find(i => i.Group == ItemGroup.Other));
                Users = new ObservableCollection<User>(db.GetCollection<User>(Data.DBCollections.Users).FindAll());
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
            AddItem = new DelegateCommand(DoAddItem, CanAddItem).ObservesProperty(() => Name).ObservesProperty(() => SelectedCompanyValue).ObservesProperty(() => SelectedSupplierValue);
            EditItem = new DelegateCommand(DoEditItem, CanEditItem).ObservesProperty(() => Name).ObservesProperty(() => ItemId).ObservesProperty(() => SelectedCompanyValue).ObservesProperty(() => SelectedSupplierValue).ObservesProperty(() => DataGridSelectedItem);
            DeleteItem = new DelegateCommand(DoDeleteItem, CanDeleteItem).ObservesProperty(() => DataGridSelectedItem);
            OpenAddItemFlyout = new DelegateCommand(DoOpenAddItemFlyout, CanOpenAddItemFlyout);
            SelectImage = new DelegateCommand(DoSelectImage, CanSelectImage);
            FillUI = new DelegateCommand(DoFillUI, CanFillUI).ObservesProperty(() => DataGridSelectedItem);
            ReloadAllItems = new DelegateCommand(DoReloadAllItems, CanReloadAllItems);
            Search = new DelegateCommand(DoSearch, CanSearch).ObservesProperty(() => SearchText);
        }

        private bool CanAddItem()
        {
            if (string.IsNullOrWhiteSpace(Name) || SelectedCompanyValue < 1 || SelectedSupplierValue < 1)
            {
                return false;
            }
            return true;
        }

        private void DoAddItem()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
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
                    Company = db.GetCollection<Company>(Data.DBCollections.Companies.ToString()).FindById(SelectedCompanyValue),
                    Supplier = db.GetCollection<Supplier>(Data.DBCollections.Suppliers.ToString()).FindById(SelectedSupplierValue),
                    Notes = Notes,
                    CreateDate = DateTime.Now,
                    Creator = Core.ReadUserSession(),
                    EditDate = null,
                    Editor = null
                };
                db.GetCollection<Item>(Data.DBCollections.Items.ToString()).Insert(i);
                Items.Add(i);
                ItemsMessage.ShowMessageAsync("تمت العملية", "تم اضافة الصنف بنجاح");
            }
        }

        private bool CanEditItem()
        {
            if (string.IsNullOrWhiteSpace(Name) || ItemId < 1 || SelectedCompanyValue < 1 || SelectedSupplierValue < 1 || DataGridSelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditItem()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var i = db.GetCollection<Item>(Data.DBCollections.Items.ToString()).FindById(DataGridSelectedItem.Id);
                i.Name = Name;
                i.Barcode = Barcode;
                i.Shopcode = Shopcode;
                i.Image = Image;
                i.PurchasePrice = PurchasePrice;
                i.WholeSalePrice = WholeSalePrice;
                i.HalfWholeSalePrice = HalfWholeSalePrice;
                i.RetailPrice = RetailPrice;
                i.QTY = QTY;
                i.Company = db.GetCollection<Company>(Data.DBCollections.Companies.ToString()).FindById(SelectedCompanyValue);
                i.Supplier = db.GetCollection<Supplier>(Data.DBCollections.Suppliers.ToString()).FindById(SelectedSupplierValue);
                i.Notes = Notes;
                i.Editor = Core.ReadUserSession();
                i.EditDate = DateTime.Now;
                db.GetCollection<Item>(Data.DBCollections.Items.ToString()).Update(i);
                Items[Items.IndexOf(DataGridSelectedItem)] = i;
                ItemId = 0;
                DataGridSelectedItem = null;
                ItemsMessage.ShowMessageAsync("تمت العملية", "تم تعديل الصنف بنجاح");
            }
        }

        private bool CanDeleteItem()
        {
            if (DataGridSelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteItem()
        {
            var result = await ItemsMessage.ShowMessageAsync("حذف الصنف", $"هل انت متاكد من حذف الصنف {DataGridSelectedItem.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    db.GetCollection<Item>(Data.DBCollections.Items.ToString()).Delete(DataGridSelectedItem.Id);
                    Items.Remove(DataGridSelectedItem);
                }
                DataGridSelectedItem = null;
                await ItemsMessage.ShowMessageAsync("تمت العملية", "تم حذف الصنف بنجاح");
            }
        }

        private bool CanSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return false;
            }
            return true;
        }

        private void DoSearch()
        {
            try
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    if (ByName)
                    {
                        Items = new ObservableCollection<Item>(db.GetCollection<Item>(Data.DBCollections.Items.ToString()).Find(i => i.Name.Contains(SearchText) && i.Group == ItemGroup.Other));
                    }
                    else if (ByBarCode)
                    {
                        Items = new ObservableCollection<Item>(db.GetCollection<Item>(Data.DBCollections.Items.ToString()).Find(i => i.Barcode == SearchText && i.Group == ItemGroup.Other));
                    }
                    else
                    {
                        Items = new ObservableCollection<Item>(db.GetCollection<Item>(Data.DBCollections.Items.ToString()).Find(i => i.Shopcode == SearchText && i.Group == ItemGroup.Other));
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

        private bool CanReloadAllItems()
        {
            return true;
        }

        private void DoReloadAllItems()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Items = new ObservableCollection<Item>(db.GetCollection<Item>(Data.DBCollections.Items.ToString()).Find(i => i.Group == ItemGroup.Other));
            }
        }

        private bool CanFillUI()
        {
            if (DataGridSelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private void DoFillUI()
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
            SelectedCompanyValue = DataGridSelectedItem.Company.Id;
            SelectedSupplierValue = DataGridSelectedItem.Supplier.Id;
            Notes = DataGridSelectedItem.Notes;
            IsAddItemFlyoutOpen = true;
        }

        private bool CanSelectImage()
        {
            return true;
        }

        private void DoSelectImage()
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

        private bool CanOpenAddItemFlyout()
        {
            return true;
        }

        private void DoOpenAddItemFlyout()
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