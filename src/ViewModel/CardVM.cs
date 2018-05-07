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
    public class CardVM : CommonBase
    {
        int _cardId;
        int _selectedCompanyValue;
        int _selectedSupplierValue;
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
        decimal _salePrice;
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

        public int CardId
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

        public decimal SalePrice
        {
            get => _salePrice;
            set
            {
                if (value != _salePrice)
                {
                    _salePrice = value;
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

        public int SelectedCompanyValue
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

        public int SelectedSupplierValue
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

        public ICommand OpenAddCardFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand DeleteCard { get; set; }
        public ICommand ReloadAllCards { get; set; }
        public ICommand Search { get; set; }
        public ICommand AddCard { get; set; }
        public ICommand EditCard { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        Cards CardsMassage = Application.Current.Windows.OfType<Cards>().FirstOrDefault();

        public CardVM()
        {
            LoadCommands();
            ByName = true;
            using (var db = new PhonyDbContext())
            {
                Companies = new ObservableCollection<Company>(db.Companies);
                Suppliers = new ObservableCollection<Supplier>(db.Suppliers);
                Cards = new ObservableCollection<Item>(db.Items.Where(i => i.Group == ItemGroup.Card));
                Users = new ObservableCollection<User>(db.Users);
            }
            new Thread(() =>
            {
                CardsCount = $"إجمالى الكروت: {Cards.Count().ToString()}";
                CardsPurchasePrice = $"اجمالى سعر الشراء: {decimal.Round(Cards.Sum(i => i.PurchasePrice * i.QTY), 2).ToString()}";
                CardsSalePrice = $"اجمالى سعر البيع: {decimal.Round(Cards.Sum(i => i.SalePrice * i.QTY), 2).ToString()}";
                CardsProfit = $"تقدير صافى الربح: {decimal.Round((Cards.Sum(i => i.SalePrice * i.QTY) - Cards.Sum(i => i.PurchasePrice * i.QTY)), 2).ToString()}";
                Thread.CurrentThread.Abort();
            }).Start();
        }

        public void LoadCommands()
        {
            OpenAddCardFlyout = new CustomCommand(DoOpenAddCardFlyout, CanOpenAddCardFlyout);
            SelectImage = new CustomCommand(DoSelectImage, CanSelectImage);
            FillUI = new CustomCommand(DoFillUI, CanFillUI);
            DeleteCard = new CustomCommand(DoDeleteCard, CanDeleteCard);
            ReloadAllCards = new CustomCommand(DoReloadAllCards, CanReloadAllCards);
            Search = new CustomCommand(DoSearch, CanSearch);
            AddCard = new CustomCommand(DoAddCard, CanAddCard);
            EditCard = new CustomCommand(DoEditCard, CanEditCard);
        }

        private bool CanEditCard(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || CardId == 0 || SelectedCompanyValue == 0 || SelectedSupplierValue == 0 || DataGridSelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditCard(object obj)
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
                i.SalePrice = SalePrice;
                i.QTY = QTY;
                i.CompanyId = SelectedCompanyValue;
                i.SupplierId = SelectedSupplierValue;
                i.Notes = Notes;
                i.EditDate = DateTime.Now;
                i.EditById = CurrentUser.Id;
                db.Complete();
                CardId = 0;
                Cards.Remove(DataGridSelectedItem);
                Cards.Add(i);
                DataGridSelectedItem = null;
                CardsMassage.ShowMessageAsync("تمت العملية", "تم تعديل الكارت بنجاح");
            }
        }

        private bool CanAddCard(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || SelectedCompanyValue == 0 || SelectedSupplierValue == 0)
            {
                return false;
            }
            return true;
        }

        private void DoAddCard(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var i = new Item
                {
                    Name = Name,
                    Barcode = Barcode,
                    Shopcode = Shopcode,
                    Image = Image,
                    Group = ItemGroup.Card,
                    PurchasePrice = PurchasePrice,
                    WholeSalePrice = WholeSalePrice,
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
                Cards.Add(i);
                CardsMassage.ShowMessageAsync("تمت العملية", "تم اضافة الكارت بنجاح");
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
                    Cards = new ObservableCollection<Item>(db.Items.Where(i => i.Name.Contains(SearchText) && i.Group == ItemGroup.Card));
                }
                else if (ByBarCode)
                {
                    Cards = new ObservableCollection<Item>(db.Items.Where(i => i.Barcode == SearchText && i.Group == ItemGroup.Card));
                }
                else
                {
                    Cards = new ObservableCollection<Item>(db.Items.Where(i => i.Shopcode == SearchText && i.Group == ItemGroup.Card));
                }
                if (Cards.Count > 0)
                {
                    if (FastResult)
                    {
                        ChildName = Cards.FirstOrDefault().Name;
                        ChildPrice = Cards.FirstOrDefault().SalePrice.ToString();
                        ChildImage = Cards.FirstOrDefault().Image;
                        OpenFastResult = true;
                    }
                }
                else
                {
                    CardsMassage.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                }
            }
        }

        private bool CanReloadAllCards(object obj)
        {
            return true;
        }

        private void DoReloadAllCards(object obj)
        {
            using (var db = new PhonyDbContext())
            {
                Cards = new ObservableCollection<Item>(db.Items.Where(i => i.Group == ItemGroup.Card));
            }
        }

        private bool CanDeleteCard(object obj)
        {
            if (DataGridSelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteCard(object obj)
        {
            var result = await CardsMassage.ShowMessageAsync("حذف الكارت", $"هل انت متاكد من حذف الكارت {DataGridSelectedItem.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new UnitOfWork(new PhonyDbContext()))
                {
                    db.Items.Remove(db.Items.Get(DataGridSelectedItem.Id));
                    db.Complete();
                    Cards.Remove(DataGridSelectedItem);
                }
                DataGridSelectedItem = null;
                await CardsMassage.ShowMessageAsync("تمت العملية", "تم حذف الكارت بنجاح");
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
            CardId = DataGridSelectedItem.Id;
            Name = DataGridSelectedItem.Name;
            Barcode = DataGridSelectedItem.Barcode;
            Shopcode = DataGridSelectedItem.Shopcode;
            Image = DataGridSelectedItem.Image;
            PurchasePrice = DataGridSelectedItem.PurchasePrice;
            WholeSalePrice = DataGridSelectedItem.WholeSalePrice;
            SalePrice = DataGridSelectedItem.SalePrice;
            QTY = DataGridSelectedItem.QTY;
            SelectedCompanyValue = DataGridSelectedItem.CompanyId;
            SelectedSupplierValue = DataGridSelectedItem.SupplierId;
            Notes = DataGridSelectedItem.Notes;
            IsAddCardFlyoutOpen = true;
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

        private bool CanOpenAddCardFlyout(object obj)
        {
            return true;
        }

        private void DoOpenAddCardFlyout(object obj)
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