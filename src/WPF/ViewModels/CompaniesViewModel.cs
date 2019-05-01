using Caliburn.Micro;
using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.WPF.Data;
using Phony.WPF.Models;
using Phony.WPF.Views;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.WPF.ViewModels
{
    public class CompaniesViewModel : Screen
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
        Company _dataGridSelectedCompany;
        ObservableCollection<Company> _companies;

        public long CompanyId
        {
            get => _companyId;
            set
            {
                _companyId = value;
                NotifyOfPropertyChange(() => CompanyId);
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

        public string Site
        {
            get => _site;
            set
            {
                _site = value;
                NotifyOfPropertyChange(() => Site);
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                NotifyOfPropertyChange(() => Email);
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                NotifyOfPropertyChange(() => Phone);
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

        public string CompaniesCount
        {
            get => _companiesCount;
            set
            {
                _companiesCount = value;
                NotifyOfPropertyChange(() => CompaniesCount);
            }
        }

        public string CompaniesDebits
        {
            get => _companiesDebits;
            set
            {
                _companiesDebits = value;
                NotifyOfPropertyChange(() => CompaniesDebits);
            }
        }

        public string CompaniesCredits
        {
            get => _companiesCredits;
            set
            {
                _companiesCredits = value;
                NotifyOfPropertyChange(() => CompaniesCredits);
            }
        }

        public string CompaniesProfit
        {
            get => _companiesProfit;
            set
            {
                _companiesProfit = value;
                NotifyOfPropertyChange(() => CompaniesProfit);
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

        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                NotifyOfPropertyChange(() => Balance);
            }
        }

        public bool IsAddCompanyFlyoutOpen
        {
            get => _isCompanyFlyoutOpen;
            set
            {
                _isCompanyFlyoutOpen = value;
                NotifyOfPropertyChange(() => IsAddCompanyFlyoutOpen);
            }
        }

        public Company DataGridSelectedCompany
        {
            get => _dataGridSelectedCompany;
            set
            {
                _dataGridSelectedCompany = value;
                NotifyOfPropertyChange(() => DataGridSelectedCompany);
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

        public ObservableCollection<User> Users { get; set; }

        Companies Message = Application.Current.Windows.OfType<Companies>().FirstOrDefault();

        public CompaniesViewModel()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                Companies = new ObservableCollection<Company>(db.GetCollection<Company>(Data.DBCollections.Companies).FindAll());
                Users = new ObservableCollection<User>(db.GetCollection<User>(Data.DBCollections.Users).FindAll());
            }
            DebitCredit();
        }

        async void DebitCredit()
        {
            decimal Debit = decimal.Round(Companies.Where(c => c.Balance < 0).Sum(i => i.Balance), 2);
            decimal Credit = decimal.Round(Companies.Where(c => c.Balance > 0).Sum(i => i.Balance), 2);
            await Task.Run(() =>
            {
                CompaniesCount = $"مجموع العملاء: {Companies.Count().ToString()}";
            });
            await Task.Run(() =>
            {
                CompaniesDebits = $"اجمالى لينا: {Math.Abs(Debit).ToString()}";
            });
            await Task.Run(() =>
            {
                CompaniesCredits = $"اجمالى علينا: {Math.Abs(Credit).ToString()}";
            });
            await Task.Run(() =>
            {
                CompaniesProfit = $"تقدير لصافى لينا: {(Math.Abs(Debit) - Math.Abs(Credit)).ToString()}";
            });
        }

        private bool CanCompanyPay()
        {
            return DataGridSelectedCompany == null ? false : true;
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
                    using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                    {
                        var s = db.GetCollection<Company>(DBCollections.Companies).FindById(DataGridSelectedCompany.Id);
                        s.Balance += compantpaymentamount;
                        //the company will give us money in form of balance or something
                        db.GetCollection<CompanyMove>(DBCollections.CompaniesMoves).Insert(new CompanyMove
                        {
                            Company = db.GetCollection<Company>(DBCollections.Companies).FindById(DataGridSelectedCompany.Id),
                            Credit = compantpaymentamount,
                            CreateDate = DateTime.Now,
                            //Creator = db.GetCollection<User>(DBCollections.Users).FindById(Core.ReadUserSession().Id),
                            EditDate = null,
                            Editor = null
                        });
                        await Message.ShowMessageAsync("تمت العملية", $"تم اضافه رصيد لشركة {DataGridSelectedCompany.Name} مبلغ {compantpaymentamount} جنية بنجاح");
                        Companies[Companies.IndexOf(DataGridSelectedCompany)] = s;
                        DebitCredit();
                        DataGridSelectedCompany = null;
                        CompanyId = 0;
                    }
                }
                else
                {
                    await Message.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanPayCompany()
        {
            return DataGridSelectedCompany == null ? false : true;
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
                    using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                    {
                        var s = db.GetCollection<Company>(DBCollections.Companies).FindById(DataGridSelectedCompany.Id);
                        s.Balance -= compantpaymentamount;
                        //Company gets money from us
                        db.GetCollection<CompanyMove>(DBCollections.CompaniesMoves).Insert(new CompanyMove
                        {
                            Company = db.GetCollection<Company>(DBCollections.Companies).FindById(DataGridSelectedCompany.Id),
                            Debit = compantpaymentamount,
                            CreateDate = DateTime.Now,
                            //Creator = db.GetCollection<User>(DBCollections.Users).FindById(Core.ReadUserSession().Id),
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
                            //Creator = db.GetCollection<User>(DBCollections.Users).FindById(Core.ReadUserSession().Id),
                            EditDate = null,
                            Editor = null
                        });
                        await Message.ShowMessageAsync("تمت العملية", $"تم خصم من شركة {DataGridSelectedCompany.Name} مبلغ {compantpaymentamount} جنية بنجاح");
                        Companies[Companies.IndexOf(DataGridSelectedCompany)] = s;
                        DebitCredit();
                        DataGridSelectedCompany = null;
                        CompanyId = 0;
                    }
                }
                else
                {
                    await Message.ShowMessageAsync("خطاء فى المبلغ", "ادخل مبلغ صحيح بعلامه عشرية واحدة");
                }
            }
        }

        private bool CanAddCompany()
        {
            return string.IsNullOrWhiteSpace(Name) ? false : true;
        }

        private async void DoAddCompany()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                var exist = db.GetCollection<Company>(DBCollections.Companies).Find(x => x.Name == Name).FirstOrDefault();
                if (exist == null)
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
                        //Creator = Core.ReadUserSession(),
                        EditDate = null,
                        Editor = null
                    };
                    db.GetCollection<Company>(DBCollections.Companies).Insert(s);
                    Companies.Add(s);
                    await Message.ShowMessageAsync("تمت العملية", "تم اضافة الشركة بنجاح");
                    DebitCredit();
                }
                else
                {
                    await Message.ShowMessageAsync("موجود", "الشركة موجودة من قبل بالفعل");
                }
            }
        }

        private bool CanEditCompany()
        {
            return string.IsNullOrWhiteSpace(Name) || CompanyId == 0 || DataGridSelectedCompany == null ? false : true;
        }

        private async void DoEditCompany()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                var c = db.GetCollection<Company>(DBCollections.Companies).FindById(DataGridSelectedCompany.Id);
                c.Name = Name;
                c.Balance = Balance;
                c.Site = Site;
                c.Email = Email;
                c.Phone = Phone;
                c.Image = Image;
                c.Notes = Notes;
                //c.Editor = Core.ReadUserSession();
                c.EditDate = DateTime.Now;
                db.GetCollection<Company>(DBCollections.Companies).Update(c);
                await Message.ShowMessageAsync("تمت العملية", "تم تعديل الشركة بنجاح");
                Companies[Companies.IndexOf(DataGridSelectedCompany)] = c;
                DebitCredit();
                DataGridSelectedCompany = null;
                CompanyId = 0;
            }
        }

        private bool CanDeleteCompany()
        {
            return DataGridSelectedCompany == null || DataGridSelectedCompany.Id == 1 ? false : true;
        }

        private async void DoDeleteCompany()
        {
            var result = await Message.ShowMessageAsync("حذف الخدمة", $"هل انت متاكد من حذف الشركة {DataGridSelectedCompany.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
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
            return string.IsNullOrWhiteSpace(SearchText) ? false : true;
        }

        private async void DoSearch()
        {
            try
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    Companies = new ObservableCollection<Company>(db.GetCollection<Company>(DBCollections.Companies).Find(x => x.Name.Contains(SearchText)));
                    if (Companies.Count < 1)
                    {
                        await Message.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                    }
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
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                Companies = new ObservableCollection<Company>(db.GetCollection<Company>(DBCollections.Companies).FindAll());
            }
            DebitCredit();
        }

        private bool CanFillUI()
        {
            return DataGridSelectedCompany == null ? false : true;
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

        private bool CanOpenAddCompanyFlyout()
        {
            return true;
        }

        private void DoOpenAddCompanyFlyout()
        {
            IsAddCompanyFlyoutOpen = IsAddCompanyFlyoutOpen ? false : true;
        }
    }
}