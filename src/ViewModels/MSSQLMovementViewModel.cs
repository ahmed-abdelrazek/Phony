using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class MSSQLMovementViewModel : BindableBase
    {
        string _serverName;
        string _userName;
        string _password;
        string _dataBase;
        bool _useDefault;
        bool _isWinAuth;
        bool _isSQLAuth;
        bool _isImporting;

        public string ServerName
        {
            get => _serverName;
            set => SetProperty(ref _serverName, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string DataBase
        {
            get => _dataBase;
            set => SetProperty(ref _dataBase, value);
        }

        public bool UseDefault
        {
            get => _useDefault;
            set => SetProperty(ref _useDefault, value);
        }

        public bool IsWinAuth
        {
            get => _isWinAuth;
            set => SetProperty(ref _isWinAuth, value);
        }

        public bool IsSQLAuth
        {
            get => _isSQLAuth;
            set => SetProperty(ref _isSQLAuth, value);
        }

        public bool IsImporting
        {
            get => _isImporting;
            set => SetProperty(ref _isImporting, value);
        }

        public ICommand MoveData { get; set; }

        SqlConnectionStringBuilder ConnectionStringBuilder = new SqlConnectionStringBuilder();

        public MSSQLMovementViewModel()
        {
            UseDefault = true;
            IsWinAuth = true;
            MoveData = new DelegateCommand(DoMoveData, CanMoveData).ObservesProperty(() => UseDefault).ObservesProperty(() => IsWinAuth).ObservesProperty(() => IsSQLAuth).ObservesProperty(() => ServerName).ObservesProperty(() => UserName).ObservesProperty(() => Password).ObservesProperty(() => DataBase);
        }

        private bool CanMoveData()
        {
            if (UseDefault)
            {
                return true;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(ServerName) && !string.IsNullOrWhiteSpace(DataBase))
                {
                    if (IsSQLAuth && (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password)))
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
            IsImporting = true;
            if (UseDefault)
            {
                ConnectionStringBuilder.DataSource = ".\\SQLExpress";
                ConnectionStringBuilder.InitialCatalog = "PhonyDb";
                ConnectionStringBuilder.IntegratedSecurity = true;
                ConnectionStringBuilder.MultipleActiveResultSets = true;
            }
            else
            {
                ConnectionStringBuilder.DataSource = ServerName;
                ConnectionStringBuilder.InitialCatalog = DataBase;
                if (IsWinAuth)
                {
                    ConnectionStringBuilder.IntegratedSecurity = true;
                }
                else
                {
                    ConnectionStringBuilder.UserID = UserName;
                    ConnectionStringBuilder.Password = Password;
                }
                ConnectionStringBuilder.MultipleActiveResultSets = true;
            }
            try
            {
                Data.SQL s = new Data.SQL(ConnectionStringBuilder.ConnectionString);
                s.Import();
            }
            catch(Exception ex)
            {
                Data.Core.SaveException(ex);
            }
            finally
            {
                IsImporting = false;
                Application.Current.Windows.OfType<Views.Settings>().FirstOrDefault().Close();
            }
        }
    }
}