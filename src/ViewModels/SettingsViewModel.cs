using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
using Phony.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;

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
        string _appVersion;
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

        public string AppVersion
        {
            get => _appVersion;
            set => SetProperty(ref _appVersion, value);
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

        private readonly SqlConnectionStringBuilder SQLConnectionStringBuilder = new();

        readonly Views.Settings Window = System.Windows.Application.Current.Windows.OfType<Views.Settings>().FirstOrDefault();

        public SettingsViewModel()
        {
            AssemblyInfo ai = new(Assembly.GetEntryAssembly());
            AppVersion = "رقم الاصدار: " + ai.Version;

            LiteDbFullPath = Properties.Settings.Default.LiteDbConnectionString;

            if (Properties.Settings.Default.IsConfigured)
            {
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.LiteDbConnectionString))
                {
                    if (LiteDbFullPath == Core.UserLocalAppFolderPath() + "..\\Phony.db" && string.IsNullOrWhiteSpace(LiteDbPassword))
                    {
                        LiteUseDefault = true;
                    }
                }
                else
                {
                    Window.ShowMessageAsync("خطا", "لم يستطع البرنامج تحميل اعدادات قاعدة البيانات");
                }
                ReportsSizeIndex =Properties.Settings.Default.SalesBillsPaperSize == "A4" ? 0 : 1;
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

        private bool CanSave() => !string.IsNullOrWhiteSpace(LiteDbFullPath) || LiteUseDefault;

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
                            Properties.Settings.Default.LiteDbConnectionString = LiteDbFullPath;
                        }
                        else
                        {
                            if (!LiteDbFullPath.EndsWith("\\"))
                            {
                                LiteDbFullPath = LiteDbFullPath + "\\";
                            }
                            Properties.Settings.Default.LiteDbConnectionString = LiteDbFullPath + "Phony.db";
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(LiteDbPassword))
                {
                    Properties.Settings.Default.LiteDbConnectionPassword = LiteDbPassword;
                }
                Properties.Settings.Default.SalesBillsPaperSize = ReportsSize;
                Properties.Settings.Default.Save();
                await Window.ShowMessageAsync("تم الحفظ", "لقد تم تغيير اعدادات البرنامج و حفظها بنجاح");
            }
            else
            {
                if (LiteUseDefault)
                {
                    Properties.Settings.Default.LiteDbConnectionString = LiteDbFullPath;
                }
                else
                {
                    if (LiteDbFullPath.EndsWith("Phony.db"))
                    {
                        Properties.Settings.Default.LiteDbConnectionString = LiteDbFullPath;
                    }
                    else
                    {
                        if (!LiteDbFullPath.EndsWith("\\"))
                        {
                            LiteDbFullPath = LiteDbFullPath + "\\";
                        }
                        Properties.Settings.Default.LiteDbConnectionString = LiteDbFullPath + "Phony.db";
                    }
                    if (!string.IsNullOrWhiteSpace(LiteDbPassword))
                    {
                        Properties.Settings.Default.LiteDbConnectionPassword = LiteDbPassword;
                    }
                }
                Properties.Settings.Default.Save();
                try
                {
                    LiteDbContext.ConnectionString = LiteDbContext.GetConnectionString(Properties.Settings.Default.LiteDbConnectionString, Properties.Settings.Default.LiteDbConnectionPassword);
                    LiteDbContext.InitializeDatabase();
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
            var dlg = new FolderBrowserDialog
            {
                Description = "مجلد لحفظ قاعدة البيانات",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            };
            if (dlg.ShowDialog() == DialogResult.OK)
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
                    return !SQLIsSQLAuth || !string.IsNullOrWhiteSpace(SQLUserName) && !string.IsNullOrWhiteSpace(SQLPassword);
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