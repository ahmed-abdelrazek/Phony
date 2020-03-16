using LiteDB;
using Phony.WPF.Data;
using Phony.WPF.Models;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TinyLittleMvvm;

namespace Phony.WPF.ViewModels
{
    public class CardsViewModel : BaseViewModelWithAnnotationValidation, IOnLoadedHandler
    {
        long _cardId;
        long _selectedCompanyValue;
        long _selectedSupplierValue;
        string _name;
        string _barcode;
        string _shopcode;
        string _searchText;
        string _notes;
        string _childName;
        string _childPrice;
        string _childBalance;
        static string _cardsCount;
        static string _cardsPurchasePrice;
        static string _cardsSalePrice;
        static string _cardsProfit;
        byte[] _image;
        byte[] _childImage;
        ItemGroup _group;
        decimal _purchasePrice;
        decimal _wholeSalePrice;
        decimal _retailPrice;
        decimal _qty;
        bool _byName;
        bool _byBarCode;
        bool _byShopCode;
        bool _fastResult;
        bool _openFastResult;
        bool _isAddCardFlyoutOpen;
        Item _dataGridSelectedItem;

        ObservableCollection<Company> _companies;
        ObservableCollection<Supplier> _suppliers;
        ObservableCollection<Item> _cards;

        public long CardId
        {
            get => _cardId;
            set
            {
                _cardId = value;
                NotifyOfPropertyChange(() => CardId);
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

        public string ChildBalance
        {
            get => _childBalance;
            set
            {
                _childBalance = value;
                NotifyOfPropertyChange(() => ChildBalance);
            }
        }

        public string CardsCount
        {
            get => _cardsCount;
            set
            {
                _cardsCount = value;
                NotifyOfPropertyChange(() => CardsCount);
            }
        }

        public string CardsPurchasePrice
        {
            get => _cardsPurchasePrice;
            set
            {
                _cardsPurchasePrice = value;
                NotifyOfPropertyChange(() => CardsPurchasePrice);
            }
        }

        public string CardsSalePrice
        {
            get => _cardsSalePrice;
            set
            {
                _cardsSalePrice = value;
                NotifyOfPropertyChange(() => CardsSalePrice);
            }
        }

        public string CardsProfit
        {
            get => _cardsProfit;
            set
            {
                _cardsProfit = value;
                NotifyOfPropertyChange(() => CardsProfit);
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

        public bool IsAddCardFlyoutOpen
        {
            get => _isAddCardFlyoutOpen;
            set
            {
                _isAddCardFlyoutOpen = value;
                NotifyOfPropertyChange(() => IsAddCardFlyoutOpen);
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

        public ObservableCollection<Item> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                NotifyOfPropertyChange(() => Cards);
            }
        }

        public ObservableCollection<User> Users { get; set; }

        public CardsViewModel()
        {
            Title = "كروت الشحن";
            ByName = true;
        }

        public async Task OnLoadedAsync()
        {
            await Task.Run(() =>
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    Companies = new ObservableCollection<Company>(db.GetCollection<Company>(Data.DBCollections.Companies).FindAll().ToList());
                    Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(Data.DBCollections.Suppliers).FindAll().ToList());
                    Cards = new ObservableCollection<Item>(db.GetCollection<Item>(Data.DBCollections.Items.ToString()).Find(i => i.Group == ItemGroup.Card).ToList());
                    Users = new ObservableCollection<User>(db.GetCollection<User>(Data.DBCollections.Users).FindAll().ToList());
                }
                CardsCount = $"إجمالى الكروت: {Cards.Count().ToString()}";
                CardsPurchasePrice = $"اجمالى سعر الشراء: {decimal.Round(Cards.Sum(i => i.PurchasePrice * i.QTY), 2).ToString()}";
                CardsSalePrice = $"اجمالى سعر البيع: {decimal.Round(Cards.Sum(i => i.RetailPrice * i.QTY), 2).ToString()}";
                CardsProfit = $"تقدير صافى الربح: {decimal.Round((Cards.Sum(i => i.RetailPrice * i.QTY) - Cards.Sum(i => i.PurchasePrice * i.QTY)), 2).ToString()}";
            });
        }

        private bool CanAddCard()
        {
            return !string.IsNullOrWhiteSpace(Name) && SelectedCompanyValue != 0 && SelectedSupplierValue != 0;
        }

        private void DoAddCard()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            var itemCol = db.GetCollection<Item>(DBCollections.Items);
            var i = new Item
            {
                Name = Name,
                Barcode = Barcode,
                Shopcode = Shopcode,
                Image = Image,
                Group = ItemGroup.Card,
                PurchasePrice = PurchasePrice,
                WholeSalePrice = WholeSalePrice,
                RetailPrice = RetailPrice,
                QTY = QTY,
                Company = db.GetCollection<Company>(DBCollections.Companies).FindById(SelectedCompanyValue),
                Supplier = db.GetCollection<Supplier>(DBCollections.Suppliers).FindById(SelectedSupplierValue),
                Notes = Notes,
                CreateDate = DateTime.Now,
                //Creator = Core.ReadUserSession(),
                EditDate = null,
                Editor = null

            };
            itemCol.Insert(i);
            Cards.Add(i);
            MessageBox.MaterialMessageBox.Show("تم اضافة الكارت بنجاح", "تمت العملية", true);
        }

        private bool CanEditCard()
        {
            return string.IsNullOrWhiteSpace(Name) || CardId == 0 || SelectedCompanyValue == 0 || SelectedSupplierValue == 0 || DataGridSelectedItem == null
                ? false
                : true;
        }

        private void DoEditCard()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            var itemCol = db.GetCollection<Item>(DBCollections.Items);
            var i = itemCol.Find(x => x.Id == DataGridSelectedItem.Id).FirstOrDefault();
            i.Name = Name;
            i.Barcode = Barcode;
            i.Shopcode = Shopcode;
            i.Image = Image;
            i.PurchasePrice = PurchasePrice;
            i.WholeSalePrice = WholeSalePrice;
            i.RetailPrice = RetailPrice;
            i.QTY = QTY;
            i.Company = db.GetCollection<Company>(DBCollections.Companies).FindById(SelectedCompanyValue);
            i.Supplier = db.GetCollection<Supplier>(DBCollections.Suppliers).FindById(SelectedSupplierValue);
            i.Notes = Notes;
            //i.Editor = Core.ReadUserSession();
            i.EditDate = DateTime.Now;
            itemCol.Update(i);
            Cards[Cards.IndexOf(DataGridSelectedItem)] = i;
            CardId = 0;
            DataGridSelectedItem = null;
            MessageBox.MaterialMessageBox.Show("تم تعديل الكارت بنجاح", "تمت العملية", true);
        }

        private bool CanDeleteCard()
        {
            return DataGridSelectedItem == null ? false : true;
        }

        private void DoDeleteCard()
        {
            var result = MessageBox.MaterialMessageBox.ShowWithCancel($"هل انت متاكد من حذف الكارت {DataGridSelectedItem.Name}", "حذف الكارت", true);
            if (result == MessageBoxResult.OK)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    db.GetCollection<Item>(DBCollections.Items).Delete(DataGridSelectedItem.Id);
                    Cards.Remove(DataGridSelectedItem);
                }
                DataGridSelectedItem = null;
                MessageBox.MaterialMessageBox.Show("تم حذف الكارت بنجاح", "تمت العملية", true);
            }
        }

        private bool CanSearch()
        {
            return string.IsNullOrWhiteSpace(SearchText) ? false : true;
        }

        private void DoSearch()
        {
            try
            {
                using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
                Cards = ByName
                    ? new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Name.Contains(SearchText) && i.Group == ItemGroup.Card).ToList())
                    : ByBarCode
                        ? new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Barcode == SearchText && i.Group == ItemGroup.Card))
                        : new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Shopcode == SearchText && i.Group == ItemGroup.Card));
                if (Cards.Count > 0)
                {
                    if (FastResult)
                    {
                        ChildName = Cards.FirstOrDefault().Name;
                        ChildPrice = Cards.FirstOrDefault().RetailPrice.ToString();
                        ChildImage = Cards.FirstOrDefault().Image;
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

        private bool CanReloadAllCards()
        {
            return true;
        }

        private void DoReloadAllCards()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            Cards = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Group == ItemGroup.Card));
        }

        private bool CanFillUI()
        {
            return DataGridSelectedItem == null ? false : true;
        }

        private void DoFillUI()
        {
            CardId = DataGridSelectedItem.Id;
            Name = DataGridSelectedItem.Name;
            Barcode = DataGridSelectedItem.Barcode;
            Shopcode = DataGridSelectedItem.Shopcode;
            Image = DataGridSelectedItem.Image;
            PurchasePrice = DataGridSelectedItem.PurchasePrice;
            WholeSalePrice = DataGridSelectedItem.WholeSalePrice;
            RetailPrice = DataGridSelectedItem.RetailPrice;
            QTY = DataGridSelectedItem.QTY;
            SelectedCompanyValue = DataGridSelectedItem.Company.Id;
            SelectedSupplierValue = DataGridSelectedItem.Supplier.Id;
            Notes = DataGridSelectedItem.Notes;
            IsAddCardFlyoutOpen = true;
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

        private bool CanOpenAddCardFlyout()
        {
            return true;
        }

        private void DoOpenAddCardFlyout()
        {
            IsAddCardFlyoutOpen = IsAddCardFlyoutOpen ? false : true;
        }
    }
}