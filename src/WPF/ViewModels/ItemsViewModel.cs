using LiteDB;
using Phony.Data.Core;
using Phony.Data.Models.Lite;
using Phony.WPF.Data;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TinyLittleMvvm;

namespace Phony.WPF.ViewModels
{
    public class ItemsViewModel : BaseViewModelWithAnnotationValidation, IOnLoadedHandler
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
                _itemId = value;
                NotifyOfPropertyChange(() => ItemId);
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

        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                NotifyOfPropertyChange(() => Barcode);
            }
        }

        public string Shopcode
        {
            get => _shopcode;
            set
            {
                _shopcode = value;
                NotifyOfPropertyChange(() => Shopcode);
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

        public string ChildQTY
        {
            get => _childQTY;
            set
            {
                _childQTY = value;
                NotifyOfPropertyChange(() => ChildQTY);
            }
        }

        public string ItemsCount
        {
            get => _itemsCount;
            set
            {
                _itemsCount = value;
                NotifyOfPropertyChange(() => ItemsCount);
            }
        }

        public string ItemsPurchasePrice
        {
            get => _itemsPurchasePrice;
            set
            {
                _itemsPurchasePrice = value;
                NotifyOfPropertyChange(() => ItemsPurchasePrice);
            }
        }

        public string ItemsSalePrice
        {
            get => _itemsSalePrice;
            set
            {
                _itemsSalePrice = value;
                NotifyOfPropertyChange(() => ItemsSalePrice);
            }
        }

        public string ItemsProfit
        {
            get => _itemsProfit;
            set
            {
                _itemsProfit = value;
                NotifyOfPropertyChange(() => ItemsProfit);
            }
        }

        public byte[] Image
        {
            get => _image;
            set
            {
                _image = value;
                NotifyOfPropertyChange(() => Image);
            }
        }

        public byte[] ChildImage
        {
            get => _childImage;
            set
            {
                _childImage = value;
                NotifyOfPropertyChange(() => ChildImage);
            }
        }

        public ItemGroup Group
        {
            get => _group;
            set
            {
                _group = value;
                NotifyOfPropertyChange(() => Group);
            }
        }

        public decimal PurchasePrice
        {
            get => _purchasePrice;
            set
            {
                _purchasePrice = Math.Round(value, 2);
                NotifyOfPropertyChange(() => PurchasePrice);
            }
        }

        public decimal WholeSalePrice
        {
            get => _wholeSalePrice;
            set
            {
                _wholeSalePrice = Math.Round(value, 2);
                NotifyOfPropertyChange(() => WholeSalePrice);
            }
        }

        public decimal HalfWholeSalePrice
        {
            get => _halfWholeSalePrice;
            set
            {
                _halfWholeSalePrice = Math.Round(value, 2);
                NotifyOfPropertyChange(() => HalfWholeSalePrice);
            }
        }

        public decimal RetailPrice
        {
            get => _retailPrice;
            set
            {
                _retailPrice = Math.Round(value, 2);
                NotifyOfPropertyChange(() => RetailPrice);
            }
        }

        public decimal QTY
        {
            get => _qty;
            set
            {
                _qty = value;
                NotifyOfPropertyChange(() => QTY);
            }
        }

        public long SelectedCompanyValue
        {
            get => _selectedCompanyValue;
            set
            {
                _selectedCompanyValue = value;
                NotifyOfPropertyChange(() => SelectedCompanyValue);
            }
        }

        public long SelectedSupplierValue
        {
            get => _selectedSupplierValue;
            set
            {
                _selectedSupplierValue = value;
                NotifyOfPropertyChange(() => SelectedSupplierValue);
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

        public bool ByBarCode
        {
            get => _byBarCode;
            set
            {
                _byBarCode = value;
                NotifyOfPropertyChange(() => ByBarCode);
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

        public bool IsAddItemFlyoutOpen
        {
            get => _isAddItemFlyoutOpen;
            set
            {
                _isAddItemFlyoutOpen = value;
                NotifyOfPropertyChange(() => IsAddItemFlyoutOpen);
            }
        }

        public Item DataGridSelectedItem
        {
            get => _dataGridSelectedItem;
            set
            {
                _dataGridSelectedItem = value;
                NotifyOfPropertyChange(() => DataGridSelectedItem);
            }
        }

        public ObservableCollection<Company> Companies
        {
            get => _companies;
            set
            {
                _companies = value;
                NotifyOfPropertyChange(() => Companies);
            }
        }

        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
                NotifyOfPropertyChange(() => Suppliers);
            }
        }

        public ObservableCollection<Item> Items
        {
            get => _items;
            set
            {
                _items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        public ItemsViewModel()
        {
            Title = "الاصناف";
            ByName = true;
        }

        public async Task OnLoadedAsync()
        {
            await Task.Run(() =>
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    Companies = new ObservableCollection<Company>(db.GetCollection<Company>(DBCollections.Companies).FindAll());
                    Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(DBCollections.Suppliers).FindAll());
                    Items = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Group == ItemGroup.Other));
                }
            });

            await Task.Run(() =>
            {
                ItemsCount = $"إجمالى الاصناف: {Items.Count}";
                ItemsPurchasePrice = $"اجمالى سعر الشراء: {decimal.Round(Items.Sum(i => i.PurchasePrice * i.QTY), 2)}";
                ItemsSalePrice = $"اجمالى سعر البيع: {decimal.Round(Items.Sum(i => i.RetailPrice * i.QTY), 2)}";
                ItemsProfit = $"تقدير صافى الربح: {decimal.Round(Items.Sum(i => i.RetailPrice * i.QTY) - Items.Sum(i => i.PurchasePrice * i.QTY), 2)}";
            });
        }

        private bool CanAddItem()
        {
            return !string.IsNullOrWhiteSpace(Name) && SelectedCompanyValue >= 1 && SelectedSupplierValue >= 1;
        }

        private void DoAddItem()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
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
                Company = db.GetCollection<Company>(DBCollections.Companies).FindById(SelectedCompanyValue),
                Supplier = db.GetCollection<Supplier>(DBCollections.Suppliers).FindById(SelectedSupplierValue),
                Notes = Notes,
                CreatedAt = DateTime.Now,
                Creator = CurrentUser,
                Editor = CurrentUser
            };
            db.GetCollection<Item>(DBCollections.Items).Insert(i);
            Items.Add(i);
            MessageBox.MaterialMessageBox.Show("تم اضافة الصنف بنجاح", "تمت العملية", true);
        }

        private bool CanEditItem()
        {
            return !string.IsNullOrWhiteSpace(Name) && ItemId >= 1 && SelectedCompanyValue >= 1 && SelectedSupplierValue >= 1 && DataGridSelectedItem != null;
        }

        private void DoEditItem()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            var i = db.GetCollection<Item>(DBCollections.Items).FindById(DataGridSelectedItem.Id);
            i.Name = Name;
            i.Barcode = Barcode;
            i.Shopcode = Shopcode;
            i.Image = Image;
            i.PurchasePrice = PurchasePrice;
            i.WholeSalePrice = WholeSalePrice;
            i.HalfWholeSalePrice = HalfWholeSalePrice;
            i.RetailPrice = RetailPrice;
            i.QTY = QTY;
            i.Company = db.GetCollection<Company>(DBCollections.Companies).FindById(SelectedCompanyValue);
            i.Supplier = db.GetCollection<Supplier>(DBCollections.Suppliers).FindById(SelectedSupplierValue);
            i.Notes = Notes;
            i.Editor = CurrentUser;
            i.EditedAt = DateTime.Now;
            db.GetCollection<Item>(DBCollections.Items).Update(i);
            Items[Items.IndexOf(DataGridSelectedItem)] = i;
            ItemId = 0;
            DataGridSelectedItem = null;
            MessageBox.MaterialMessageBox.Show("تم تعديل الصنف بنجاح", "تمت العملية", true);
        }

        private bool CanDeleteItem()
        {
            return DataGridSelectedItem != null;
        }

        private void DoDeleteItem()
        {
            var result = MessageBox.MaterialMessageBox.ShowWithCancel($"هل انت متاكد من حذف الصنف {DataGridSelectedItem.Name}", "حذف الصنف", true);
            if (result == MessageBoxResult.OK)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    db.GetCollection<Item>(DBCollections.Items).Delete(DataGridSelectedItem.Id);
                    Items.Remove(DataGridSelectedItem);
                }
                DataGridSelectedItem = null;
                MessageBox.MaterialMessageBox.Show("تم حذف الصنف بنجاح", "تمت العملية", true);
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
                using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
                Items = ByName
                    ? new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Name.Contains(SearchText) && i.Group == ItemGroup.Other))
                    : ByBarCode
                        ? new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Barcode == SearchText && i.Group == ItemGroup.Other))
                        : new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Shopcode == SearchText && i.Group == ItemGroup.Other));
                if (Items.Count > 0)
                {
                    if (FastResult)
                    {
                        ChildName = Items.FirstOrDefault().Name;
                        ChildPrice = Items.FirstOrDefault().RetailPrice.ToString();
                        ChildQTY = Items.FirstOrDefault().QTY.ToString();
                        ChildImage = Items.FirstOrDefault().Image;
                        OpenFastResult = true;
                    }
                }
                else
                {
                    MessageBox.MaterialMessageBox.ShowWarning("لم يتم العثور على شئ", "غير موجود", true);
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                MessageBox.MaterialMessageBox.ShowError("لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله", "خطأ", true);
            }
        }

        private bool CanReloadAllItems()
        {
            return true;
        }

        private void DoReloadAllItems()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            Items = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Group == ItemGroup.Other));
        }

        private bool CanFillUI()
        {
            return DataGridSelectedItem != null;
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
            Microsoft.Win32.OpenFileDialog dlg = new();
            var codecs = ImageCodecInfo.GetImageEncoders();
            dlg.Filter = $"All image files ({string.Join(";", codecs.Select(codec => codec.FilenameExtension).ToArray())})|{string.Join(";", codecs.Select(codec => codec.FilenameExtension).ToArray())}|All files|*";
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
            IsAddItemFlyoutOpen = !IsAddItemFlyoutOpen;
        }
    }
}