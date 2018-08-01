using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
using Phony.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        int _reportsSizeIndex;
        string _reportsSize;
        string _dbFullPath;
        string _dbPassword;
        string _sqlServerName;
        string _sqlUserName;
        string _sqlPassword;
        string _sqlDataBase;
        bool _liteUseDefault;
        bool _sqlUseDefault;
        bool _sqlIsWinAuth;
        bool _sqlIsSQLAuth;
        bool _sqlIsImporting;


        public int ReportsSizeIndex
        {
            get => _reportsSizeIndex;
            set => SetProperty(ref _reportsSizeIndex, value);
        }

        public string ReportsSize
        {
            get => _reportsSize;
            set => SetProperty(ref _reportsSize, value);
        }

        public string LiteDbFullPath
        {
            get => _dbFullPath;
            set => SetProperty(ref _dbFullPath, value);
        }

        public string LiteDbPassword
        {
            get => _dbPassword;
            set => SetProperty(ref _dbPassword, value);
        }

        public string SQLServerName
        {
            get => _sqlServerName;
            set => SetProperty(ref _sqlServerName, value);
        }

        public string SQLUserName
        {
            get => _sqlUserName;
            set => SetProperty(ref _sqlUserName, value);
        }

        public string SQLPassword
        {
            get => _sqlPassword;
            set => SetProperty(ref _sqlPassword, value);
        }

        public string SQLDataBase
        {
            get => _sqlDataBase;
            set => SetProperty(ref _sqlDataBase, value);
        }

        public bool LiteUseDefault
        {
            get => _liteUseDefault;
            set => SetProperty(ref _liteUseDefault, value);
        }

        public bool SQLUseDefault
        {
            get => _sqlUseDefault;
            set => SetProperty(ref _sqlUseDefault, value);
        }

        public bool SQLIsWinAuth
        {
            get => _sqlIsWinAuth;
            set => SetProperty(ref _sqlIsWinAuth, value);
        }

        public bool SQLIsSQLAuth
        {
            get => _sqlIsSQLAuth;
            set => SetProperty(ref _sqlIsSQLAuth, value);
        }

        public bool SQLIsImporting
        {
            get => _sqlIsImporting;
            set => SetProperty(ref _sqlIsImporting, value);
        }

        public ICommand Save { get; set; }
        public ICommand SelectLiteDbFolder { get; set; }
        public ICommand MoveData { get; set; }

        SqlConnectionStringBuilder SQLConnectionStringBuilder = new SqlConnectionStringBuilder();

        DbConnectionStringBuilder LiteConnectionStringBuilder = new DbConnectionStringBuilder();

        readonly Views.Settings Window = Application.Current.Windows.OfType<Views.Settings>().FirstOrDefault();

        public SettingsViewModel()
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
                    Window.ShowMessageAsync("خطا", "لم يستطع البرنامج تحميل اعدادات قاعدة البيانات");
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
            SQLUseDefault = true;
            SQLIsWinAuth = true;
            Save = new DelegateCommand(DoSave, CanSave).ObservesProperty(() => LiteDbFullPath).ObservesProperty(() => LiteDbPassword).ObservesProperty(() => LiteUseDefault).ObservesProperty(() => ReportsSizeIndex).ObservesProperty(() => ReportsSize);
            SelectLiteDbFolder = new DelegateCommand(DoSelectLiteDbFolder);
            MoveData = new DelegateCommand(DoMoveData, CanMoveData).ObservesProperty(() => SQLUseDefault).ObservesProperty(() => SQLIsWinAuth).ObservesProperty(() => SQLIsSQLAuth).ObservesProperty(() => SQLServerName).ObservesProperty(() => SQLUserName).ObservesProperty(() => SQLPassword).ObservesProperty(() => SQLDataBase);
        }

        private bool CanSave()
        {
            if (!string.IsNullOrWhiteSpace(LiteDbFullPath) || LiteUseDefault)
            {
                return true;
            }
            return false;
        }

        private async void DoSave()
        {
            if (Properties.Settings.Default.IsConfigured)
            {
                if (string.IsNullOrWhiteSpace(LiteDbFullPath))
                {
                    await Window.ShowMessageAsync("خطا", "من فضلك اختار مكان لحفظ قاعده البيانات");
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
                await Window.ShowMessageAsync("تم الحفظ", "لقد تم تغيير اعدادات البرنامج و حفظها بنجاح");
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
                    if (!SQLIsImporting)
                    {
                        Properties.Settings.Default.IsConfigured = true;
                        Window.Close();
                    }
                }
                catch (Exception ex)
                {
                    Properties.Settings.Default.IsConfigured = false;
                    Core.SaveException(ex);
                    await Window.ShowMessageAsync("مشكلة", "هناك مشكله فى اعداد البرانامج تاكد من البيانات التى ادخلتها");
                }
                finally
                {
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void DoSelectLiteDbFolder()
        {
            throw new NotImplementedException();
        }

        private bool CanMoveData()
        {
            if (Properties.Settings.Default.IsConfigured)
            {
                return false;
            }
            if (SQLUseDefault)
            {
                return true;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(SQLServerName) && !string.IsNullOrWhiteSpace(SQLDataBase))
                {
                    if (SQLIsSQLAuth && (string.IsNullOrWhiteSpace(SQLUserName) || string.IsNullOrWhiteSpace(SQLPassword)))
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        private void DoMoveData()
        {
            SQLIsImporting = true;
            DoSave();
            if (SQLUseDefault)
            {
                SQLConnectionStringBuilder.DataSource = ".\\SQLExpress";
                SQLConnectionStringBuilder.InitialCatalog = "PhonyDb";
                SQLConnectionStringBuilder.IntegratedSecurity = true;
                SQLConnectionStringBuilder.MultipleActiveResultSets = true;
            }
            else
            {
                SQLConnectionStringBuilder.DataSource = SQLServerName;
                SQLConnectionStringBuilder.InitialCatalog = SQLDataBase;
                if (SQLIsWinAuth)
                {
                    SQLConnectionStringBuilder.IntegratedSecurity = true;
                }
                else
                {
                    SQLConnectionStringBuilder.UserID = SQLUserName;
                    SQLConnectionStringBuilder.Password = SQLPassword;
                }
                SQLConnectionStringBuilder.MultipleActiveResultSets = true;
            }
            try
            {
                SQL s = new SQL(SQLConnectionStringBuilder.ConnectionString);
                s.Import();
                Properties.Settings.Default.IsConfigured = true;
            }
            catch (Exception ex)
            {
                Properties.Settings.Default.IsConfigured = false;
                Core.SaveException(ex);
            }
            finally
            {
                Properties.Settings.Default.Save();
                SQLIsImporting = false;
                Window.Close();
            }
        }
    }
}