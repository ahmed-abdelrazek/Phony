using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using Phony.WPF.Data;
using Phony.WPF.EventModels;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Phony.WPF.ViewModels
{
    public class MSSQLMovementViewModel : Screen
    {
        string _sqlServerName;
        string _sqlUserName;
        string _sqlPassword;
        string _sqlDataBase;
        bool _sqlUseDefault;
        bool _sqlIsWinAuth;
        bool _sqlIsSQLAuth;
        bool _sqlIsImporting;

        IEventAggregator _events;
        SettingsEvents settingsEvents;

        public string SQLServerName
        {
            get => _sqlServerName;
            set
            {
                _sqlServerName = value;
                NotifyOfPropertyChange(() => SQLServerName);
                NotifyOfPropertyChange(() => CanMoveData);
            }
        }

        public string SQLUserName
        {
            get => _sqlUserName;
            set
            {
                _sqlUserName = value;
                NotifyOfPropertyChange(() => SQLUserName);
                NotifyOfPropertyChange(() => CanMoveData);
            }
        }

        public string SQLPassword
        {
            get => _sqlPassword;
            set
            {
                _sqlPassword = value;
                NotifyOfPropertyChange(() => SQLPassword);
                NotifyOfPropertyChange(() => CanMoveData);
            }
        }

        public string SQLDataBase
        {
            get => _sqlDataBase;
            set
            {
                _sqlDataBase = value;
                NotifyOfPropertyChange(() => SQLDataBase);
                NotifyOfPropertyChange(() => CanMoveData);
            }
        }

        public bool SQLUseDefault
        {
            get => _sqlUseDefault;
            set
            {
                _sqlUseDefault = value;
                NotifyOfPropertyChange(() => SQLUseDefault);
                NotifyOfPropertyChange(() => CanMoveData);
            }
        }

        public bool SQLIsWinAuth
        {
            get => _sqlIsWinAuth;
            set
            {
                _sqlIsWinAuth = value;
                NotifyOfPropertyChange(() => SQLIsWinAuth);
                NotifyOfPropertyChange(() => CanMoveData);
            }
        }

        public bool SQLIsSQLAuth
        {
            get => _sqlIsSQLAuth;
            set
            {
                _sqlIsSQLAuth = value;
                NotifyOfPropertyChange(() => SQLIsSQLAuth);
                NotifyOfPropertyChange(() => CanMoveData);
            }
        }

        public bool SQLIsImporting
        {
            get => _sqlIsImporting;
            set
            {
                _sqlIsImporting = value;
                NotifyOfPropertyChange(() => SQLIsImporting);
                NotifyOfPropertyChange(() => CanMoveData);
            }
        }

        SqlConnectionStringBuilder SQLConnectionStringBuilder = new SqlConnectionStringBuilder();

        public MSSQLMovementViewModel(IEventAggregator events)
        {
            _events = events;
            SQLUseDefault = true;
            SQLIsWinAuth = true;
            SQLServerName = ".\\SQLExpress";
            SQLDataBase = "PhonyDb";
        }

        private bool CanMoveData
        {
            get
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
        }

        public async Task MoveData()
        {
            SQLIsImporting = true;
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
                new SQL(SQLConnectionStringBuilder.ConnectionString).ImportFromMSSQL();

                if (!Properties.Settings.Default.IsConfigured)
                {
                    settingsEvents.CloseWindow = true;
                }

                Properties.Settings.Default.IsConfigured = true;
            }
            catch (Exception ex)
            {
                Properties.Settings.Default.IsConfigured = false;
                Core.SaveException(ex);
                await DialogCoordinator.Instance.ShowMessageAsync(this, "خطا", "حدثت مشكلة اثناء عمليه النقل من فضلك تاكد من صحه البيانات");
            }
            finally
            {
                Properties.Settings.Default.Save();
                SQLIsImporting = false;
                if (Properties.Settings.Default.IsConfigured)
                {
                    await DialogCoordinator.Instance.ShowMessageAsync(this, "تمت العملية", "تم نقل بياناتك بنجاح");
                }
                await _events.PublishOnBackgroundThreadAsync(settingsEvents);
            }
        }


    }
}
