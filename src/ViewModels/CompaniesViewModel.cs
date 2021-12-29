using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
using Phony.DTOs;
using Phony.Helpers;
using Phony.Models;
using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class CompaniesViewModel : BindableBase
    {
        long _companyId;
        string _name;
        string _site;
        string _email;
        string _searchText;
        string _phone;
        string _notes;
        static string _companiesCount;
        static string _companiesDebits;
        static string _companiesCredits;
        static string _companiesProfit;
        byte[] _image;
        decimal _balance;
        bool _isCompanyFlyoutOpen;
        CompanyDto _dataGridSelectedCompany;
        ObservableCollection<CompanyDto> _companies;

        public long CompanyId
        {
            get => _companyId;
            set => SetProperty(ref _companyId, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Site
        {
            get => _site;
            set => SetProperty(ref _site, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
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

        public string CompaniesCount
        {
            get => _companiesCount;
            set => SetProperty(ref _companiesCount, value);
        }

        public string CompaniesDebits
        {
            get => _companiesDebits;
            set => SetProperty(ref _companiesDebits, value);
        }

        public string CompaniesCredits
        {
            get => _companiesCredits;
            set => SetProperty(ref _companiesCredits, value);
        }

        public string CompaniesProfit
        {
            get => _companiesProfit;
            set => SetProperty(ref _companiesProfit, value);
        }

        public byte[] Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }

        public bool IsAddCompanyFlyoutOpen
        {
            get => _isCompanyFlyoutOpen;
            set => SetProperty(ref _isCompanyFlyoutOpen, value);
        }

        public CompanyDto DataGridSelectedCompany
        {
            get => _dataGridSelectedCompany;
            set => SetProperty(ref _dataGridSelectedCompany, value);
        }

        public ObservableCollection<CompanyDto> Companies
        {
            get => _companies;
            set => SetProperty(ref _companies, value);
        }

        public ICommand CompanyPay { get; set; }
        public ICommand PayCompany { get; set; }
        public ICommand AddCompany { get; set; }
        public ICommand EditCompany { get; set; }
        public ICommand DeleteCompany { get; set; }
        public ICommand OpenAddCompanyFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand ReloadAllCompanies { get; set; }
        public ICommand Search { get; set; }

        private readonly Companies Message = Application.Current.Windows.OfType<Companies>().FirstOrDefault();

        public CompaniesViewModel()
        {
            LoadCommands();
            using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
            {
                Companies = ObjectMapper.Mapper.Map<ObservableCollection<CompanyDto>>(db.GetCollection<Company>(DBCollections.Companies).Include(x => x.Creator).Include(x => x.Editor).FindAll());
            }
            DebitCredit();
        }

        public void LoadCommands()
        {
            CompanyPay = new DelegateCommand(DoCompanyPayAsync, CanCompanyPay).ObservesProperty(() => DataGridSelectedCompany);
            PayCompany = new DelegateCommand(DoPayCompanyAsync, CanPayCompany).ObservesProperty(() => DataGridSelectedCompany);
            AddCompany = new DelegateCommand(DoAddCompany, CanAddCompany).ObservesProperty(() => Name);
            EditCompany = new DelegateCommand(DoEditCompany, CanEditCompany).ObservesProperty(() => Name).ObservesProperty(() => CompanyId).ObservesProperty(() => DataGridSelectedCompany);
            DeleteCompany = new DelegateCommand(DoDeleteCompany, CanDeleteCompany).ObservesProperty(() => DataGridSelectedCompany).ObservesProperty(() => DataGridSelectedCompany.Id);
            OpenAddCompanyFlyout = new DelegateCommand(DoOpenAddCompanyFlyout, CanOpenAddCompanyFlyout);
            SelectImage = new DelegateCommand(DoSelectImage, CanSelectImage);
            FillUI = new DelegateCommand(DoFillUI, CanFillUI).ObservesProperty(() => DataGridSelectedCompany);
            ReloadAllCompanies = new DelegateCommand(DoReloadAllCompanies, CanReloadAllCompanies);
            Search = new DelegateCommand(DoSearch, CanSearch).ObservesProperty(() => SearchText);
        }

        async void DebitCredit()
        {
            decimal Debit = decimal.Round(Companies.Where(c => c.Balance < 0).Sum(i => i.Balance), 2);
            decimal Credit = decimal.Round(Companies.Where(c => c.Balance > 0).Sum(i => i.Balance), 2);
            await Task.Run(() =>
            {
                CompaniesCount = $"مجموع العملاء: {Companies.Count}";
            });
            await Task.Run(() =>
            {
                CompaniesDebits = $"اجمالى لينا: {Math.Abs(Debit)}";
            });
            await Task.Run(() =>
            {
                CompaniesCredits = $"اجمالى علينا: {Math.Abs(Credit)}";
            });
            await Task.Run(() =>
            {
                CompaniesProfit = $"تقدير لصافى لينا: {(Math.Abs(Debit) - Math.Abs(Credit))}";
            });
        }

        private bool CanCompanyPay()
        {
            return DataGridSelectedCompany is not null;
        }

        private async void DoCompanyPayAsync()
        {
            var result = await Message.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى تريد اضافته لرصيدك لدى شركة {DataGridSelectedCompany.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await Message.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لاضافته للرصيد");
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal compantpaymentamount);
                if (isvalidmoney)
                {
                    using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                    var s = db.GetCollection<Company>(DBCollections.Companies).FindById(DataGridSelectedCompany.Id);
                    s.Balance += compantpaymentamount;
                    //the company will give us money in form of balance or something
                    db.GetCollection<CompanyMove>(DBCollections.CompaniesMoves).Insert(new CompanyMove
                    {
                        Company = db.GetCollection<Company>(DBCollections.Companies).FindById(DataGridSelectedCompany.Id),
                        Credit = compantpaymentamount,
                        CreateDate = DateTime.Now,
                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Core.ReadUserSession().Id),
                        EditDate = null,
                        Editor = null
                    });
                    await Message.ShowMessageAsync("تمت العملية", $"تم اضافه رصيد لشركة {DataGridSelectedCompany.Name} مبلغ {compantpaymentamount} جنية بنجاح");
                    Companies[Companies.IndexOf(DataGridSelectedCompany)] = ObjectMapper.Mapper.Map<CompanyDto>(s);
                    DebitCredit();
                    DataGridSelectedCompany = null;
                    CompanyId = 0;
                }
                else
                {
                    await Message.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanPayCompany()
        {
            return DataGridSelectedCompany is not null;
        }

        private async void DoPayCompanyAsync()
        {
            var result = await Message.ShowInputAsync("تدفيع", $"ادخل المبلغ الذى تريد دفعه لشركة {DataGridSelectedCompany.Name}");
            if (string.IsNullOrWhiteSpace(result))
            {
                await Message.ShowMessageAsync("ادخل مبلغ", "لم تقم بادخال اى مبلغ لاضافته للرصيد");
            }
            else
            {
                bool isvalidmoney = decimal.TryParse(result, out decimal compantpaymentamount);
                if (isvalidmoney)
                {
                    using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                    var s = db.GetCollection<Company>(DBCollections.Companies).FindById(DataGridSelectedCompany.Id);
                    s.Balance -= compantpaymentamount;
                    //Company gets money from us
                    db.GetCollection<CompanyMove>(DBCollections.CompaniesMoves).Insert(new CompanyMove
                    {
                        Company = db.GetCollection<Company>(DBCollections.Companies).FindById(DataGridSelectedCompany.Id),
                        Debit = compantpaymentamount,
                        CreateDate = DateTime.Now,
                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Core.ReadUserSession().Id),
                        EditDate = null,
                        Editor = null
                    });
                    //the money is taken from our Treasury
                    db.GetCollection<TreasuryMove>(DBCollections.TreasuriesMoves).Insert(new TreasuryMove
                    {
                        Treasury = db.GetCollection<Treasury>(DBCollections.Treasuries).FindById(1),
                        Credit = compantpaymentamount,
                        Notes = $"دفع للشركة بكود {DataGridSelectedCompany.Id} باسم {DataGridSelectedCompany.Name}",
                        CreateDate = DateTime.Now,
                        Creator = db.GetCollection<User>(DBCollections.Users).FindById(Core.ReadUserSession().Id),
                        EditDate = null,
                        Editor = null
                    });
                    await Message.ShowMessageAsync("تمت العملية", $"تم خصم من شركة {DataGridSelectedCompany.Name} مبلغ {compantpaymentamount} جنية بنجاح");
                    Companies[Companies.IndexOf(DataGridSelectedCompany)] = ObjectMapper.Mapper.Map<CompanyDto>(s);
                    DebitCredit();
                    DataGridSelectedCompany = null;
                    CompanyId = 0;
                }
                else
                {
                    await Message.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanAddCompany()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private async void DoAddCompany()
        {
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
            var exist = db.GetCollection<Company>(DBCollections.Companies).Find(x => x.Name == Name).FirstOrDefault();
            if (exist is null)
            {
                var company = new Company
                {
                    Name = Name,
                    Balance = Balance,
                    Site = Site,
                    Email = Email,
                    Phone = Phone,
                    Image = Image,
                    Notes = Notes,
                    CreateDate = DateTime.Now,
                    Creator = Core.ReadUserSession(),
                    EditDate = null,
                    Editor = null
                };
                db.GetCollection<Company>(DBCollections.Companies).Insert(company);
                Companies.Add(ObjectMapper.Mapper.Map<CompanyDto>(company));
                await Message.ShowMessageAsync("تمت العملية", "تم اضافة الشركة بنجاح");
                DebitCredit();
            }
            else
            {
                await Message.ShowMessageAsync("موجود", "الشركة موجودة من قبل بالفعل");
            }
        }

        private bool CanEditCompany()
        {
            return !string.IsNullOrWhiteSpace(Name) && CompanyId != 0 && DataGridSelectedCompany is not null;
        }

        private async void DoEditCompany()
        {
            using var db = new LiteDatabase(LiteDbContext.ConnectionString);
            var company = db.GetCollection<Company>(DBCollections.Companies).FindById(DataGridSelectedCompany.Id);
            company.Name = Name;
            company.Balance = Balance;
            company.Site = Site;
            company.Email = Email;
            company.Phone = Phone;
            company.Image = Image;
            company.Notes = Notes;
            company.Editor = Core.ReadUserSession();
            company.EditDate = DateTime.Now;
            db.GetCollection<Company>(DBCollections.Companies).Update(company);
            await Message.ShowMessageAsync("تمت العملية", "تم تعديل الشركة بنجاح");
            Companies[Companies.IndexOf(DataGridSelectedCompany)] = ObjectMapper.Mapper.Map<CompanyDto>(company);
            DebitCredit();
            DataGridSelectedCompany = null;
            CompanyId = 0;
        }

        private bool CanDeleteCompany()
        {
            return DataGridSelectedCompany is not null && DataGridSelectedCompany.Id != 1;
        }

        private async void DoDeleteCompany()
        {
            var result = await Message.ShowMessageAsync("حذف الخدمة", $"هل انت متاكد من حذف الشركة {DataGridSelectedCompany.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
                {
                    db.GetCollection<Company>(DBCollections.Companies).Delete(DataGridSelectedCompany.Id);
                    Companies.Remove(DataGridSelectedCompany);
                }
                await Message.ShowMessageAsync("تمت العملية", "تم حذف الشركة بنجاح");
                DebitCredit();
                DataGridSelectedCompany = null;
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
                Companies = ObjectMapper.Mapper.Map<ObservableCollection<CompanyDto>>(db.GetCollection<Company>(DBCollections.Companies).Include(x => x.Creator).Include(x => x.Editor).Find(x => x.Name.Contains(SearchText)));
                if (Companies.Count < 1)
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

        private bool CanReloadAllCompanies()
        {
            return true;
        }

        private void DoReloadAllCompanies()
        {
            using (var db = new LiteDatabase(LiteDbContext.ConnectionString))
            {
                Companies = ObjectMapper.Mapper.Map<ObservableCollection<CompanyDto>>(db.GetCollection<Company>(DBCollections.Companies).Include(x => x.Creator).Include(x => x.Editor).FindAll());
            }
            DebitCredit();
        }

        private bool CanFillUI()
        {
            return DataGridSelectedCompany is not null;
        }

        private void DoFillUI()
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

        private bool CanOpenAddCompanyFlyout()
        {
            return true;
        }

        private void DoOpenAddCompanyFlyout()
        {
            IsAddCompanyFlyoutOpen = !IsAddCompanyFlyoutOpen;
        }
    }
}