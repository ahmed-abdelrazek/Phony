using Caliburn.Micro;
using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.WPF.Data;
using Phony.WPF.EventModels;
using Phony.WPF.Models;
using System;
using System.Data.Common;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Phony.WPF.ViewModels
{
    public class GeneralSettingsViewModel : Screen
    {
        int _reportsSizeIndex;
        string _reportsSize;
        string _dbFullPath;
        string _dbPassword;
        bool _liteUseDefault;

        IEventAggregator _events;
        SettingsEvents settingsEvents;

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
                NotifyOfPropertyChange(() => CanSaveDbConfig);
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
                NotifyOfPropertyChange(() => CanSaveDbConfig);
            }
        }

        DbConnectionStringBuilder LiteConnectionStringBuilder = new DbConnectionStringBuilder();

        public GeneralSettingsViewModel(IEventAggregator events)
        {
            _events = events;
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
                if (Properties.Settings.Default.SalesBillsPaperSize == "A4")
                {
                    ReportsSizeIndex = 0;
                }
                else
                {
                    ReportsSizeIndex = 1;
                }
            }
            else
            {
                LiteUseDefault = true;
                LiteDbFullPath = Core.UserLocalAppFolderPath() + "..\\Phony.db";
            }
        }

        private bool CanSaveDbConfig
        {
            get
            {
                return !string.IsNullOrEmpty(LiteDbFullPath) || LiteUseDefault;
            }
        }

        public async Task SaveDbConfig()
        {
            settingsEvents = new SettingsEvents();

            if (Properties.Settings.Default.IsConfigured)
            {
                if (string.IsNullOrWhiteSpace(LiteDbFullPath))
                {
                    await DialogCoordinator.Instance.ShowMessageAsync(this, "خطا", "من فضلك اختار مكان لحفظ قاعده البيانات");
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
                                LiteDbFullPath = LiteDbFullPath + "\\";
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
                await DialogCoordinator.Instance.ShowMessageAsync(this, "تم الحفظ", "لقد تم تغيير اعدادات البرنامج و حفظها بنجاح");
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
                            LiteDbFullPath = LiteDbFullPath + "\\";
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
                                Creator = db.GetCollection<User>(DBCollections.Users).FindById(1),
                                CreateDate = DateTime.Now,
                                Editor = null,
                                EditDate = null
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
                                Creator = db.GetCollection<User>(DBCollections.Users).FindById(1),
                                CreateDate = DateTime.Now,
                                Editor = null,
                                EditDate = null
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
                                Creator = db.GetCollection<User>(DBCollections.Users).FindById(1),
                                CreateDate = DateTime.Now,
                                Editor = null,
                                EditDate = null
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
                                Creator = db.GetCollection<User>(DBCollections.Users).FindById(1),
                                CreateDate = DateTime.Now,
                                Editor = null,
                                EditDate = null
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
                                Creator = db.GetCollection<User>(DBCollections.Users).FindById(1),
                                CreateDate = DateTime.Now,
                                Editor = null,
                                EditDate = null
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
                                Creator = db.GetCollection<User>(DBCollections.Users).FindById(1),
                                CreateDate = DateTime.Now,
                                Editor = null,
                                EditDate = null
                            });
                        }
                    }
                    Properties.Settings.Default.IsConfigured = true;
                    settingsEvents.CloseWindow = true;
                }
                catch (Exception ex)
                {
                    Properties.Settings.Default.IsConfigured = false;
                    Core.SaveException(ex);
                    await DialogCoordinator.Instance.ShowMessageAsync(this, "مشكلة", "هناك مشكله فى اعداد البرانامج تاكد من البيانات التى ادخلتها");
                }
                finally
                {
                    Properties.Settings.Default.Save();
                    await DialogCoordinator.Instance.ShowMessageAsync(this, "تمت العملية", "تم اعداد البرنامج بنجاح");
                    await _events.PublishOnBackgroundThreadAsync(settingsEvents);
                }
            }
        }

        public void SelectLiteDbFolder()
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyDocuments
            };

            dlg.ShowDialog();
            if (string.IsNullOrWhiteSpace(dlg.SelectedPath))
            {
                LiteDbFullPath = dlg.SelectedPath;
                if (!LiteDbFullPath.EndsWith("Phony.db"))
                {
                    if (!LiteDbFullPath.EndsWith("\\"))
                    {
                        LiteDbFullPath = LiteDbFullPath + "\\";
                    }
                    LiteDbFullPath += "Phony.db";
                }
            }
        }
    }
}
