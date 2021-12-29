using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
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
    public class CardsViewModel : BindableBase
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
            set => SetProperty(ref _cardId, value);
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

        public string ChildBalance
        {
            get => _childBalance;
            set => SetProperty(ref _childBalance, value);
        }

        public string CardsCount
        {
            get => _cardsCount;
            set => SetProperty(ref _cardsCount, value);
        }

        public string CardsPurchasePrice
        {
            get => _cardsPurchasePrice;
            set => SetProperty(ref _cardsPurchasePrice, value);
        }

        public string CardsSalePrice
        {
            get => _cardsSalePrice;
            set => SetProperty(ref _cardsSalePrice, value);
        }

        public string CardsProfit
        {
            get => _cardsProfit;
            set => SetProperty(ref _cardsProfit, value);
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

        public bool IsAddCardFlyoutOpen
        {
            get => _isAddCardFlyoutOpen;
            set => SetProperty(ref _isAddCardFlyoutOpen, value);
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

        public ObservableCollection<Item> Cards
        {
            get => _cards;
            set => SetProperty(ref _cards, value);
        }

        public ObservableCollection<User> Users { get; set; }

        public ICommand AddCard { get; set; }
        public ICommand EditCard { get; set; }
        public ICommand DeleteCard { get; set; }
        public ICommand OpenAddCardFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand ReloadAllCards { get; set; }
        public ICommand Search { get; set; }

        private readonly Cards Message = Application.Current.Windows.OfType<Cards>().FirstOrDefault();

        public CardsViewModel()
        {
            LoadCommands();
            ByName = true;
            using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
            {
                Companies = new ObservableCollection<Company>(db.GetCollection<Company>(Data.DBCollections.Companies).FindAll().ToList());
                Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(Data.DBCollections.Suppliers).FindAll().ToList());
                Cards = new ObservableCollection<Item>(db.GetCollection<Item>(Data.DBCollections.Items.ToString()).Find(i => i.Group == ItemGroup.Card).ToList());
                Users = new ObservableCollection<User>(db.GetCollection<User>(Data.DBCollections.Users).FindAll().ToList());
            }
            new Thread(() =>
            {
                CardsCount = $"إجمالى الكروت: {Cards.Count}";
                CardsPurchasePrice = $"اجمالى سعر الشراء: {decimal.Round(Cards.Sum(i => i.PurchasePrice * i.QTY), 2)}";
                CardsSalePrice = $"اجمالى سعر البيع: {decimal.Round(Cards.Sum(i => i.RetailPrice * i.QTY), 2)}";
                CardsProfit = $"تقدير صافى الربح: {decimal.Round((Cards.Sum(i => i.RetailPrice * i.QTY) - Cards.Sum(i => i.PurchasePrice * i.QTY)), 2)}";
            }).Start();
        }

        public void LoadCommands()
        {
            AddCard = new DelegateCommand(DoAddCard, CanAddCard).ObservesProperty(() => Name).ObservesProperty(() => SelectedCompanyValue).ObservesProperty(() => SelectedSupplierValue);
            EditCard = new DelegateCommand(DoEditCard, CanEditCard).ObservesProperty(() => Name).ObservesProperty(() => CardId).ObservesProperty(() => SelectedCompanyValue).ObservesProperty(() => SelectedSupplierValue).ObservesProperty(() => DataGridSelectedItem);
            DeleteCard = new DelegateCommand(DoDeleteCard, CanDeleteCard).ObservesProperty(() => DataGridSelectedItem);
            OpenAddCardFlyout = new DelegateCommand(DoOpenAddCardFlyout, CanOpenAddCardFlyout);
            SelectImage = new DelegateCommand(DoSelectImage, CanSelectImage);
            FillUI = new DelegateCommand(DoFillUI, CanFillUI).ObservesProperty(() => DataGridSelectedItem);
            ReloadAllCards = new DelegateCommand(DoReloadAllCards, CanReloadAllCards);
            Search = new DelegateCommand(DoSearch, CanSearch).ObservesProperty(() => SearchText);
        }

        private bool CanAddCard()
        {
            return !string.IsNullOrWhiteSpace(Name) && SelectedCompanyValue != 0 && SelectedSupplierValue != 0;
        }

        private async void DoAddCard()
        {
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
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
                Creator = Core.ReadUserSession(),
                EditDate = null,
                Editor = null

            };
            itemCol.Insert(i);
            Cards.Add(i);
            await Message.ShowMessageAsync("تمت العملية", "تم اضافة الكارت بنجاح");
        }

        private bool CanEditCard()
        {
            return !string.IsNullOrWhiteSpace(Name) && CardId != 0 && SelectedCompanyValue != 0 && SelectedSupplierValue != 0 && DataGridSelectedItem is not null;
        }

        private async void DoEditCard()
        {
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
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
            i.Editor = Core.ReadUserSession();
            i.EditDate = DateTime.Now;
            itemCol.Update(i);
            Cards[Cards.IndexOf(DataGridSelectedItem)] = i;
            CardId = 0;
            DataGridSelectedItem = null;
            await Message.ShowMessageAsync("تمت العملية", "تم تعديل الكارت بنجاح");
        }

        private bool CanDeleteCard()
        {
            return DataGridSelectedItem is not null;
        }

        private async void DoDeleteCard()
        {
            var result = await Message.ShowMessageAsync("حذف الكارت", $"هل انت متاكد من حذف الكارت {DataGridSelectedItem.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
                {
                    db.GetCollection<Item>(DBCollections.Items).Delete(DataGridSelectedItem.Id);
                    Cards.Remove(DataGridSelectedItem);
                }
                DataGridSelectedItem = null;
                await Message.ShowMessageAsync("تمت العملية", "تم حذف الكارت بنجاح");
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
                if (ByName)
                {
                    Cards = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Name.Contains(SearchText) && i.Group == ItemGroup.Card).ToList());
                }
                else if (ByBarCode)
                {
                    Cards = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Barcode == SearchText && i.Group == ItemGroup.Card));
                }
                else
                {
                    Cards = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Shopcode == SearchText && i.Group == ItemGroup.Card));
                }
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
                    await Message.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                await Message.ShowMessageAsync("خطأ", "لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله");
            }
        }

        private bool CanReloadAllCards()
        {
            return true;
        }

        private void DoReloadAllCards()
        {
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
            Cards = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.Group == ItemGroup.Card));
        }

        private bool CanFillUI()
        {
            return DataGridSelectedItem is not null;
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

        private bool CanOpenAddCardFlyout()
        {
            return true;
        }

        private void DoOpenAddCardFlyout()
        {
            IsAddCardFlyoutOpen = !IsAddCardFlyoutOpen;
        }
    }
}