using Phony.WPF.Data;
using System;
using System.Data.SqlClient;
using System.Windows.Input;
using TinyLittleMvvm;

namespace Phony.WPF.ViewModels
{
    public class MSSQLMovementViewModel : BaseViewModelWithAnnotationValidation
    {
        string _sqlServerName;
        string _sqlUserName;
        string _sqlPassword;
        string _sqlDataBase;
        bool _sqlUseDefault;
        bool _sqlIsWinAuth;
        bool _sqlIsSQLAuth;
        bool _sqlIsImporting;

        public string SQLServerName
        {
            get => _sqlServerName;
            set
            {
                _sqlServerName = value;
                NotifyOfPropertyChange(() => SQLServerName);
            }
        }

        public string SQLUserName
        {
            get => _sqlUserName;
            set
            {
                _sqlUserName = value;
                NotifyOfPropertyChange(() => SQLUserName);
            }
        }

        public string SQLPassword
        {
            get => _sqlPassword;
            set
            {
                _sqlPassword = value;
                NotifyOfPropertyChange(() => SQLPassword);
            }
        }

        public string SQLDataBase
        {
            get => _sqlDataBase;
            set
            {
                _sqlDataBase = value;
                NotifyOfPropertyChange(() => SQLDataBase);
            }
        }

        public bool SQLUseDefault
        {
            get => _sqlUseDefault;
            set
            {
                _sqlUseDefault = value;
                NotifyOfPropertyChange(() => SQLUseDefault);
            }
        }

        public bool SQLIsWinAuth
        {
            get => _sqlIsWinAuth;
            set
            {
                _sqlIsWinAuth = value;
                NotifyOfPropertyChange(() => SQLIsWinAuth);
            }
        }

        public bool SQLIsSQLAuth
        {
            get => _sqlIsSQLAuth;
            set
            {
                _sqlIsSQLAuth = value;
                NotifyOfPropertyChange(() => SQLIsSQLAuth);
            }
        }

        public bool SQLIsImporting
        {
            get => _sqlIsImporting;
            set
            {
                _sqlIsImporting = value;
                NotifyOfPropertyChange(() => SQLIsImporting);
            }
        }

        public ICommand MoveData { get; }

        SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();

        public MSSQLMovementViewModel()
        {
            SQLUseDefault = true;
            SQLIsWinAuth = true;
            SQLServerName = ".\\SQLExpress";
            SQLDataBase = "PhonyDb";

            MoveData = new RelayCommand(DoMoveData, CanMoveData);
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

        public void DoMoveData()
        {
            SQLIsImporting = true;
            if (SQLUseDefault)
            {
                sqlConnectionStringBuilder.DataSource = ".\\SQLExpress";
                sqlConnectionStringBuilder.InitialCatalog = "PhonyDb";
                sqlConnectionStringBuilder.IntegratedSecurity = true;
                sqlConnectionStringBuilder.MultipleActiveResultSets = true;
            }
            else
            {
                sqlConnectionStringBuilder.DataSource = SQLServerName;
                sqlConnectionStringBuilder.InitialCatalog = SQLDataBase;
                if (SQLIsWinAuth)
                {
                    sqlConnectionStringBuilder.IntegratedSecurity = true;
                }
                else
                {
                    sqlConnectionStringBuilder.UserID = SQLUserName;
                    sqlConnectionStringBuilder.Password = SQLPassword;
                }
                sqlConnectionStringBuilder.MultipleActiveResultSets = true;
            }
            try
            {
                new SQL(sqlConnectionStringBuilder.ConnectionString).ImportFromMSSQL();
                Properties.Settings.Default.IsConfigured = true;

            }
            catch (Exception ex)
            {
                Properties.Settings.Default.IsConfigured = false;
                Core.SaveException(ex);
                MessageBox.MaterialMessageBox.ShowError("حدثت مشكلة اثناء عمليه النقل من فضلك تاكد من صحه البيانات", "خطأ", true);
            }
            finally
            {
                Properties.Settings.Default.Save();
                SQLIsImporting = false;
                if (Properties.Settings.Default.IsConfigured)
                {
                    MessageBox.MaterialMessageBox.Show("تم نقل بياناتك بنجاح", "تمت العملية", true);
                }
            }
        }


    }
}
