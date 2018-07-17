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
            set
            {
                if (value != _cardId)
                {
                    _cardId = value;
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

        public string ChildBalance
        {
            get => _childBalance;
            set
            {
                if (value != _childBalance)
                {
                    _childBalance = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string CardsCount
        {
            get => _cardsCount;
            set
            {
                if (value != _cardsCount)
                {
                    _cardsCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string CardsPurchasePrice
        {
            get => _cardsPurchasePrice;
            set
            {
                if (value != _cardsPurchasePrice)
                {
                    _cardsPurchasePrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string CardsSalePrice
        {
            get => _cardsSalePrice;
            set
            {
                if (value != _cardsSalePrice)
                {
                    _cardsSalePrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string CardsProfit
        {
            get => _cardsProfit;
            set
            {
                if (value != _cardsProfit)
                {
                    _cardsProfit = value;
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

        public bool IsAddCardFlyoutOpen
        {
            get => _isAddCardFlyoutOpen;
            set
            {
                if (value != _isAddCardFlyoutOpen)
                {
                    _isAddCardFlyoutOpen = value;
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

        public ObservableCollection<Item> Cards
        {
            get => _cards;
            set
            {
                if (value != _cards)
                {
                    _cards = value;
                    RaisePropertyChanged();
                }
            }
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

        Users.LoginVM CurrentUser = new Users.LoginVM();

        Cards CardsMessage = Application.Current.Windows.OfType<Cards>().FirstOrDefault();

        public CardsViewModel()
        {
            LoadCommands();
            ByName = true;
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Companies = new ObservableCollection<Company>(db.GetCollection<Company>(DBCollections.Companies.ToString()).FindAll().ToList());
                Suppliers = new ObservableCollection<Supplier>(db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).FindAll().ToList());
                Cards = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items.ToString()).Find(i => i.Group == ItemGroup.Card).ToList());
                Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users.ToString()).FindAll().ToList());
            }
            new Thread(() =>
            {
                CardsCount = $"إجمالى الكروت: {Cards.Count().ToString()}";
                CardsPurchasePrice = $"اجمالى سعر الشراء: {decimal.Round(Cards.Sum(i => i.PurchasePrice * i.QTY), 2).ToString()}";
                CardsSalePrice = $"اجمالى سعر البيع: {decimal.Round(Cards.Sum(i => i.RetailPrice * i.QTY), 2).ToString()}";
                CardsProfit = $"تقدير صافى الربح: {decimal.Round((Cards.Sum(i => i.RetailPrice * i.QTY) - Cards.Sum(i => i.PurchasePrice * i.QTY)), 2).ToString()}";
            }).Start();
        }

        public void LoadCommands()
        {
            OpenAddCardFlyout = new DelegateCommand(DoOpenAddCardFlyout, CanOpenAddCardFlyout);
            SelectImage = new DelegateCommand(DoSelectImage, CanSelectImage);
            FillUI = new DelegateCommand(DoFillUI, CanFillUI);
            DeleteCard = new DelegateCommand(DoDeleteCard, CanDeleteCard);
            ReloadAllCards = new DelegateCommand(DoReloadAllCards, CanReloadAllCards);
            Search = new DelegateCommand(DoSearch, CanSearch);
            AddCard = new DelegateCommand(DoAddCard, CanAddCard);
            EditCard = new DelegateCommand(DoEditCard, CanEditCard);
        }

        private bool CanAddCard()
        {
            if (string.IsNullOrWhiteSpace(Name) || SelectedCompanyValue == 0 || SelectedSupplierValue == 0)
            {
                return false;
            }
            return true;
        }

        private void DoAddCard()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var itemCol = db.GetCollection<Item>(DBCollections.Items.ToString());
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
                    Company = db.GetCollection<Company>(DBCollections.Companies.ToString()).FindById(SelectedCompanyValue),
                    Supplier = db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).FindById(SelectedSupplierValue),
                    Notes = Notes,
                    CreateDate = DateTime.Now,
                    EditDate = null
                };
                itemCol.Insert(i);
                Cards.Add(i);
                CardsMessage.ShowMessageAsync("تمت العملية", "تم اضافة الكارت بنجاح");
            }
        }

        private bool CanEditCard()
        {
            if (string.IsNullOrWhiteSpace(Name) || CardId == 0 || SelectedCompanyValue == 0 || SelectedSupplierValue == 0 || DataGridSelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditCard()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                var itemCol = db.GetCollection<Item>(DBCollections.Items.ToString());
                var i = itemCol.Find(x => x.Id == DataGridSelectedItem.Id).FirstOrDefault();
                i.Name = Name;
                i.Barcode = Barcode;
                i.Shopcode = Shopcode;
                i.Image = Image;
                i.PurchasePrice = PurchasePrice;
                i.WholeSalePrice = WholeSalePrice;
                i.RetailPrice = RetailPrice;
                i.QTY = QTY;
                i.Company = db.GetCollection<Company>(DBCollections.Companies.ToString()).FindById(SelectedCompanyValue);
                i.Supplier = db.GetCollection<Supplier>(DBCollections.Suppliers.ToString()).FindById(SelectedSupplierValue);
                i.Notes = Notes;
                i.EditDate = DateTime.Now;
                i.Editor = db.GetCollection<User>(DBCollections.Users.ToString()).FindById(CurrentUser.Id);
                itemCol.Update(i);
                Cards[Cards.IndexOf(DataGridSelectedItem)] = i;
                CardId = 0;
                DataGridSelectedItem = null;
                CardsMessage.ShowMessageAsync("تمت العملية", "تم تعديل الكارت بنجاح");
            }
        }

        private bool CanDeleteCard()
        {
            if (DataGridSelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteCard()
        {
            var result = await CardsMessage.ShowMessageAsync("حذف الكارت", $"هل انت متاكد من حذف الكارت {DataGridSelectedItem.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                {
                    db.GetCollection<Item>(DBCollections.Items.ToString()).Delete(DataGridSelectedItem.Id);
                    Cards.Remove(DataGridSelectedItem);
                }
                DataGridSelectedItem = null;
                await CardsMessage.ShowMessageAsync("تمت العملية", "تم حذف الكارت بنجاح");
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
                        Cards = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items.ToString()).Find(i => i.Name.Contains(SearchText) && i.Group == ItemGroup.Card).ToList());
                    }
                    else if (ByBarCode)
                    {
                        Cards = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items.ToString()).Find(i => i.Barcode == SearchText && i.Group == ItemGroup.Card));
                    }
                    else
                    {
                        Cards = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items.ToString()).Find(i => i.Shopcode == SearchText && i.Group == ItemGroup.Card));
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
                        CardsMessage.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                    }
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله");
            }
        }

        private bool CanReloadAllCards()
        {
            return true;
        }

        private void DoReloadAllCards()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Cards = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items.ToString()).Find(i => i.Group == ItemGroup.Card));
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
            if (IsAddCardFlyoutOpen)
            {
                IsAddCardFlyoutOpen = false;
            }
            else
            {
                IsAddCardFlyoutOpen = true;
            }
        }
    }
}