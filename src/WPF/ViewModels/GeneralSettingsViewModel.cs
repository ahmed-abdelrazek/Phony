using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data.Core;
using Phony.Data.Models.Lite;
using Phony.WPF.Data;
using System;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using TinyLittleMvvm;

namespace Phony.WPF.ViewModels
{
    public class GeneralSettingsViewModel : BaseViewModelWithAnnotationValidation, IOnLoadedHandler
    {
        int _reportsSizeIndex;
        string _reportsSize;
        string _dbFullPath;
        string _dbPassword;
        bool _liteUseDefault;

        public int ReportsSizeIndex
        {
            get => _reportsSizeIndex;
            set
            {
                _reportsSizeIndex = value;
                NotifyOfPropertyChange(() => ReportsSizeIndex);
            }
        }

        public string ReportsSize
        {
            get => _reportsSize;
            set
            {
                _reportsSize = value;
                NotifyOfPropertyChange(() => ReportsSize);
            }
        }

        public string LiteDbFullPath
        {
            get => _dbFullPath;
            set
            {
                _dbFullPath = value;
                NotifyOfPropertyChange(() => LiteDbFullPath);
            }
        }

        public string LiteDbPassword
        {
            get => _dbPassword;
            set
            {
                _dbPassword = value;
                NotifyOfPropertyChange(() => LiteDbPassword);
            }
        }

        public bool LiteUseDefault
        {
            get => _liteUseDefault;
            set
            {
                _liteUseDefault = value;
                NotifyOfPropertyChange(() => LiteUseDefault);
            }
        }

        public ICommand SaveDbConfig { get; }

        DbConnectionStringBuilder LiteConnectionStringBuilder = new DbConnectionStringBuilder();

        public GeneralSettingsViewModel()
        {
            SaveDbConfig = new RelayCommand(DoSaveDbConfig, CanSaveDbConfig);
        }

        public async Task OnLoadedAsync()
        {
            await Task.Run(() =>
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    if (Properties.Settings.Default.IsConfigured)
                    {
                        LiteConnectionStringBuilder.ConnectionString = Properties.Settings.Default.LiteDbConnectionString;
                        if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.LiteDbConnectionString))
                        {
                            LiteDbFullPath = LiteConnectionStringBuilder["Filename"].ToString();
                            if (LiteConnectionStringBuilder.ContainsKey("Password"))
                            {
                                LiteDbPassword = LiteConnectionStringBuilder["Password"].ToString();
                            }
                            if (LiteDbFullPath == Core.UserLocalAppFolderPath() + "..\\Phony.db" && string.IsNullOrWhiteSpace(LiteDbPassword))
                            {
                                LiteUseDefault = true;
                            }
                        }
                        else
                        {
                            DialogCoordinator.Instance.ShowMessageAsync(this, "خطا", "لم يستطع البرنامج تحميل اعدادات قاعدة البيانات");
                        }
                        ReportsSizeIndex = Properties.Settings.Default.SalesBillsPaperSize == "A4" ? 0 : 1;
                    }
                    else
                    {
                        LiteUseDefault = true;
                        LiteDbFullPath = Core.UserLocalAppFolderPath() + "..\\Phony.db";
                    }
                }
            });
        }

        private bool CanSaveDbConfig()
        {
            return !string.IsNullOrEmpty(LiteDbFullPath) || LiteUseDefault;
        }

        public void DoSaveDbConfig()
        {
            if (!CanSaveDbConfig())
            {
                return;
            }
            if (Properties.Settings.Default.IsConfigured)
            {
                if (string.IsNullOrWhiteSpace(LiteDbFullPath))
                {
                    MessageBox.MaterialMessageBox.Show("من فضلك اختار مكان لحفظ قاعده البيانات", "خطا", true);
                    return;
                }
                else
                {
                    if (Directory.Exists(LiteDbFullPath))
                    {
                        if (LiteDbFullPath.EndsWith("Phony.db"))
                        {
                            LiteConnectionStringBuilder["Filename"] = LiteDbFullPath;
                        }
                        else
                        {
                            if (!LiteDbFullPath.EndsWith("\\"))
                            {
                                LiteDbFullPath += "\\";
                            }
                            LiteConnectionStringBuilder["Filename"] = LiteDbFullPath + "Phony.db";
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(LiteDbPassword))
                {
                    LiteConnectionStringBuilder["Password"] = LiteDbPassword;
                }
                Properties.Settings.Default.SalesBillsPaperSize = ReportsSize;
                Properties.Settings.Default.LiteDbConnectionString = LiteConnectionStringBuilder.ConnectionString;
                Properties.Settings.Default.Save();
                MessageBox.MaterialMessageBox.Show("لقد تم تغيير اعدادات البرنامج و حفظها بنجاح", "تم الحفظ", true);
            }
            else
            {
                if (LiteUseDefault)
                {
                    LiteConnectionStringBuilder["Filename"] = LiteDbFullPath;
                }
                else
                {
                    if (LiteDbFullPath.EndsWith("Phony.db"))
                    {
                        LiteConnectionStringBuilder["Filename"] = LiteDbFullPath;
                    }
                    else
                    {
                        if (!LiteDbFullPath.EndsWith("\\"))
                        {
                            LiteDbFullPath += "\\";
                        }
                        LiteConnectionStringBuilder["Filename"] = LiteDbFullPath + "Phony.db";
                    }
                    if (!string.IsNullOrWhiteSpace(LiteDbPassword))
                    {
                        LiteConnectionStringBuilder["Password"] = LiteDbPassword;
                    }
                }
                Properties.Settings.Default.LiteDbConnectionString = LiteConnectionStringBuilder.ConnectionString;
                Properties.Settings.Default.Save();
                try
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                    {
                        var userCol = db.GetCollection<User>(DBCollections.Users);
                        var user = userCol.FindById(1);
                        if (user == null)
                        {
                            userCol.Insert(new User
                            {
                                Id = 1,
                                Name = "admin",
                                Pass = SecurePasswordHasher.Hash("admin"),
                                Group = UserGroup.Manager,
                                IsActive = true
                            });
                        }
                        var clientCol = db.GetCollection<Client>(DBCollections.Clients);
                        var client = clientCol.FindById(1);
                        if (client == null)
                        {
                            clientCol.Insert(new Client
                            {
                                Id = 1,
                                Name = "كاش",
                                Balance = 0,
                                CreatedAt = DateTime.Now,
                                Creator = CurrentUser,
                                Editor = CurrentUser
                            });
                        }
                        var companyCol = db.GetCollection<Company>(DBCollections.Companies);
                        var company = companyCol.FindById(1);
                        if (company == null)
                        {
                            companyCol.Insert(new Company
                            {
                                Id = 1,
                                Name = "لا يوجد",
                                Balance = 0,
                                CreatedAt = DateTime.Now,
                                Creator = CurrentUser,
                                Editor = CurrentUser
                            });
                        }
                        var salesMenCol = db.GetCollection<SalesMan>(DBCollections.SalesMen);
                        var salesMen = salesMenCol.FindById(1);
                        if (salesMen == null)
                        {
                            salesMenCol.Insert(new SalesMan
                            {
                                Id = 1,
                                Name = "لا يوجد",
                                Balance = 0,
                                CreatedAt = DateTime.Now,
                                Creator = CurrentUser,
                                Editor = CurrentUser
                            });
                        }
                        var suppliersCol = db.GetCollection<Supplier>(DBCollections.Suppliers);
                        var supplier = suppliersCol.FindById(1);
                        if (supplier == null)
                        {
                            suppliersCol.Insert(new Supplier
                            {
                                Id = 1,
                                Name = "لا يوجد",
                                Balance = 0,
                                SalesMan = db.GetCollection<SalesMan>(DBCollections.SalesMen).FindById(1),
                                CreatedAt = DateTime.Now,
                                Creator = CurrentUser,
                                Editor = CurrentUser
                            });
                        }
                        var storesCol = db.GetCollection<Store>(DBCollections.Stores);
                        var store = storesCol.FindById(1);
                        if (store == null)
                        {
                            storesCol.Insert(new Store
                            {
                                Id = 1,
                                Name = "التوكل",
                                Motto = "لخدمات المحمول",
                                CreatedAt = DateTime.Now,
                                Creator = CurrentUser,
                                Editor = CurrentUser
                            });
                        }
                        var treasuriesCol = db.GetCollection<Treasury>(DBCollections.Treasuries);
                        var treasury = treasuriesCol.FindById(1);
                        if (treasury == null)
                        {
                            treasuriesCol.Insert(new Treasury
                            {
                                Id = 1,
                                Name = "الرئيسية",
                                Store = db.GetCollection<Store>(DBCollections.Stores).FindById(1),
                                Balance = 0,
                                CreatedAt = DateTime.Now,
                                Creator = CurrentUser,
                                Editor = CurrentUser
                            });
                        }
                    }
                    Properties.Settings.Default.IsConfigured = true;
                }
                catch (Exception ex)
                {
                    Properties.Settings.Default.IsConfigured = false;
                    Core.SaveException(ex);
                    MessageBox.MaterialMessageBox.ShowError("هناك مشكله فى اعداد البرانامج تاكد من البيانات التى ادخلتها", "مشكلة", true);
                }
                finally
                {
                    Properties.Settings.Default.Save();
                    MessageBox.MaterialMessageBox.Show("تم اعداد البرنامج بنجاح", "تمت العملية", true);
                }
            }
        }

        public void SelectLiteDbFolder()
        {
            //todo FolderBrowserDialog
            //var dlg = new System.Windows.Forms.FolderBrowserDialog
            //{
            //    RootFolder = Environment.SpecialFolder.MyDocuments
            //};

            //dlg.ShowDialog();
            //if (string.IsNullOrWhiteSpace(dlg.SelectedPath))
            //{
            //    LiteDbFullPath = dlg.SelectedPath;
            //    if (!LiteDbFullPath.EndsWith("Phony.db"))
            //    {
            //        if (!LiteDbFullPath.EndsWith("\\"))
            //        {
            //            LiteDbFullPath = LiteDbFullPath + "\\";
            //        }
            //        LiteDbFullPath += "Phony.db";
            //    }
            //}
        }
    }
}
