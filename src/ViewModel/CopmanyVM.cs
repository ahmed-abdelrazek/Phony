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
    public class CopmanyVM : CommonBase
    {
        long _companyId;
        string _name;
        string _site;
        string _email;
        string _searchText;
        string _phone;
        string _notes;
        static string _companiesCount;
        static string _companiesPurchasePrice;
        static string _companiesSalePrice;
        static string _companiesProfit;
        byte[] _image;
        decimal _balance;
        bool _isCompanyFlyoutOpen;
        Company _dataGridSelectedCompany;
        ObservableCollection<Company> _companies;

        public long CompanyId
        {
            get => _companyId;
            set
            {
                if (value != _companyId)
                {
                    _companyId = value;
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

        public string Site
        {
            get => _site;
            set
            {
                if (value != _site)
                {
                    _site = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (value != _email)
                {
                    _email = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (value != _phone)
                {
                    _phone = value;
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

        public string CompaniesCount
        {
            get => _companiesCount;
            set
            {
                if (value != _companiesCount)
                {
                    _companiesCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string CompaniesPurchasePrice
        {
            get => _companiesPurchasePrice;
            set
            {
                if (value != _companiesPurchasePrice)
                {
                    _companiesPurchasePrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string CompaniesSalePrice
        {
            get => _companiesSalePrice;
            set
            {
                if (value != _companiesSalePrice)
                {
                    _companiesSalePrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string CompaniesProfit
        {
            get => _companiesProfit;
            set
            {
                if (value != _companiesProfit)
                {
                    _companiesProfit = value;
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

        public decimal Balance
        {
            get => _balance;
            set
            {
                if (value != _balance)
                {
                    _balance = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsAddCompanyFlyoutOpen
        {
            get => _isCompanyFlyoutOpen;
            set
            {
                if (value != _isCompanyFlyoutOpen)
                {
                    _isCompanyFlyoutOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Company DataGridSelectedCompany
        {
            get => _dataGridSelectedCompany;
            set
            {
                if (value != _dataGridSelectedCompany)
                {
                    _dataGridSelectedCompany = value;
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

        public ObservableCollection<User> Users { get; set; }

        public ICommand OpenAddCompanyFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand DeleteCompany { get; set; }
        public ICommand ReloadAllCompanies { get; set; }
        public ICommand Search { get; set; }
        public ICommand AddCompany { get; set; }
        public ICommand EditCompany { get; set; }
        public ICommand AddBalance { get; set; }

        Users.LoginVM CurrentUser = new Users.LoginVM();

        Companies CompaniesMessage = Application.Current.Windows.OfType<Companies>().FirstOrDefault();

        public CopmanyVM()
        {
            LoadCommands();
            using (var db = new PhonyDbContext())
            {
                Companies = new ObservableCollection<Company>(db.Companies);
                Users = new ObservableCollection<User>(db.Users);
            }
            new Thread(() =>
            {
                CompaniesCount = $"إجمالى الشركات: {Companies.Count().ToString()}";
                CompaniesPurchasePrice = $"اجمالى لينا: {decimal.Round(Companies.Where(c => c.Balance > 0).Sum(i => i.Balance), 2).ToString()}";
                CompaniesSalePrice = $"اجمالى علينا: {decimal.Round(Companies.Where(c => c.Balance < 0).Sum(i => i.Balance), 2).ToString()}";
                CompaniesProfit = $"تقدير لصافى لينا: {decimal.Round((Companies.Where(c => c.Balance > 0).Sum(i => i.Balance) + Companies.Where(c => c.Balance < 0).Sum(i => i.Balance)), 2).ToString()}";
            }).Start();
        }

        public void LoadCommands()
        {
            OpenAddCompanyFlyout = new CustomCommand(DoOpenAddCompanyFlyout, CanOpenAddCompanyFlyout);
            SelectImage = new CustomCommand(DoSelectImage, CanSelectImage);
            FillUI = new CustomCommand(DoFillUI, CanFillUI);
            DeleteCompany = new CustomCommand(DoDeleteCompany, CanDeleteCompany);
            ReloadAllCompanies = new CustomCommand(DoReloadAllCompanies, CanReloadAllCompanies);
            Search = new CustomCommand(DoSearch, CanSearch);
            AddCompany = new CustomCommand(DoAddCompany, CanAddCompany);
            EditCompany = new CustomCommand(DoEditCompany, CanEditCompany);
            AddBalance = new CustomCommand(DoAddBalance, CanAddBalance);
        }

        private bool CanAddBalance(object obj)
        {
            if (DataGridSelectedCompany == null)
            {
                return false;
            }
            return true;
        }

        private async void DoAddBalance(object obj)
        {
            var result = await CompaniesMessage.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى تريد تدفيعه للشركة {DataGridSelectedCompany.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await CompaniesMessage.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لاضافته للرصيد");
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal compantpaymentamount);
                if (isvalidmoney)
                {
                    using (var db = new UnitOfWork(new PhonyDbContext()))
                    {
                        var s = db.Companies.Get(DataGridSelectedCompany.Id);
                        s.Balance -= compantpaymentamount;
                        s.EditDate = DateTime.Now;
                        s.EditById = CurrentUser.Id;
                        var sm = new CompanyMove
                        {
                            CompanyId = DataGridSelectedCompany.Id,
                            Amount = compantpaymentamount,
                            CreateDate = DateTime.Now,
                            CreatedById = CurrentUser.Id,
                            EditDate = null,
                            EditById = null
                        };
                        db.CompaniesMoves.Add(sm);
                        if (compantpaymentamount > 0)
                        {
                            db.TreasuriesMoves.Add(new TreasuryMove
                            {
                                TreasuryId = 1,
                                In = compantpaymentamount,
                                Out = 0,
                                Notes = $"تدفيع الشركة بكود {DataGridSelectedCompany.Id} باسم {DataGridSelectedCompany.Name}",
                                CreateDate = DateTime.Now,
                                CreatedById = CurrentUser.Id
                            });
                        }
                        else
                        {
                            db.TreasuriesMoves.Add(new TreasuryMove
                            {
                                TreasuryId = 1,
                                In = 0,
                                Out = compantpaymentamount,
                                Notes = $"استلام من الشركة بكود {DataGridSelectedCompany.Id} باسم {DataGridSelectedCompany.Name}",
                                CreateDate = DateTime.Now,
                                CreatedById = CurrentUser.Id
                            });
                        }
                        db.Complete();
                        await CompaniesMessage.ShowMessageAsync("تمت العملية", $"تم شحن الشركة {DataGridSelectedCompany.Name} مبلغ {compantpaymentamount} جنية بنجاح");
                        DataGridSelectedCompany = null;
                        CompanyId = 0;
                        Companies.Remove(DataGridSelectedCompany);
                        Companies.Add(s);
                    }
                }
                else
                {
                    await CompaniesMessage.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanEditCompany(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || CompanyId == 0 || DataGridSelectedCompany == null)
            {
                return false;
            }
            return true;
        }

        private void DoEditCompany(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var c = db.Companies.Get(DataGridSelectedCompany.Id);
                c.Name = Name;
                c.Balance = Balance;
                c.Site = Site;
                c.Email = Email;
                c.Phone = Phone;
                c.Image = Image;
                c.Notes = Notes;
                c.EditDate = DateTime.Now;
                c.EditById = CurrentUser.Id;
                db.Complete();
                Companies[Companies.IndexOf(DataGridSelectedCompany)] = c;
                CompanyId = 0;
                DataGridSelectedCompany = null;
                CompaniesMessage.ShowMessageAsync("تمت العملية", "تم تعديل الشركة بنجاح");
            }
        }

        private bool CanAddCompany(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }

        private void DoAddCompany(object obj)
        {
            using (var db = new UnitOfWork(new PhonyDbContext()))
            {
                var s = new Company
                {
                    Name = Name,
                    Balance = Balance,
                    Site = Site,
                    Email = Email,
                    Phone = Phone,
                    Image = Image,
                    Notes = Notes,
                    CreateDate = DateTime.Now,
                    CreatedById = CurrentUser.Id,
                    EditDate = null,
                    EditById = null
                };
                db.Companies.Add(s);
                db.Complete();
                Companies.Add(s);
                CompaniesMessage.ShowMessageAsync("تمت العملية", "تم اضافة الشركة بنجاح");
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
                Companies = new ObservableCollection<Company>(db.Companies.Where(i => i.Name.Contains(SearchText)));
                if (Companies.Count < 1)
                {
                    CompaniesMessage.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                }
            }
        }

        private bool CanReloadAllCompanies(object obj)
        {
            return true;
        }

        private void DoReloadAllCompanies(object obj)
        {
            using (var db = new PhonyDbContext())
            {
                Companies = new ObservableCollection<Company>(db.Companies);
            }
        }

        private bool CanDeleteCompany(object obj)
        {
            if (DataGridSelectedCompany == null || DataGridSelectedCompany.Id == 1)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteCompany(object obj)
        {
            var result = await CompaniesMessage.ShowMessageAsync("حذف الخدمة", $"هل انت متاكد من حذف الشركة {DataGridSelectedCompany.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new UnitOfWork(new PhonyDbContext()))
                {
                    db.Companies.Remove(db.Companies.Get(DataGridSelectedCompany.Id));
                    db.Complete();
                    Companies.Remove(DataGridSelectedCompany);
                }
                DataGridSelectedCompany = null;
                await CompaniesMessage.ShowMessageAsync("تمت العملية", "تم حذف الشركة بنجاح");
            }
        }

        private bool CanFillUI(object obj)
        {
            if (DataGridSelectedCompany == null)
            {
                return false;
            }
            return true;
        }

        private void DoFillUI(object obj)
        {
            CompanyId = DataGridSelectedCompany.Id;
            Name = DataGridSelectedCompany.Name;
            Balance = DataGridSelectedCompany.Balance;
            Site = DataGridSelectedCompany.Site;
            Image = DataGridSelectedCompany.Image;
            Email = DataGridSelectedCompany.Email;
            Phone = DataGridSelectedCompany.Phone;
            Notes = DataGridSelectedCompany.Notes;
            IsAddCompanyFlyoutOpen = true;
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

        private bool CanOpenAddCompanyFlyout(object obj)
        {
            return true;
        }

        private void DoOpenAddCompanyFlyout(object obj)
        {
            if (IsAddCompanyFlyoutOpen)
            {
                IsAddCompanyFlyoutOpen = false;
            }
            else
            {
                IsAddCompanyFlyoutOpen = true;
            }
        }
    }
}