using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Persistence;
using Phony.Utility;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModel.Users
{
    public class LoginVM : CommonBase
    {
        string _name;
        string _pass;
        bool _isLogging;

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

        public string Pass
        {
            get => _pass;
            set
            {
                if (value != _pass)
                {
                    _pass = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsLogging
        {
            get => _isLogging;
            set
            {
                if (value != _isLogging)
                {
                    _isLogging = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand LogIn { get; set; }

        MainWindowVM v = new MainWindowVM();

        View.MainWindow Message = Application.Current.Windows.OfType<View.MainWindow>().FirstOrDefault();

        public LoginVM()
        {
            LoadCommands();
        }

        private void LoadCommands()
        {
            LogIn = new CustomCommand(DoLogIn, CanDoLogIn);
        }

        private bool CanDoLogIn(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }

        private async void DoLogIn(object obj)
        {
            if (!IsLogging)
            {
                IsLogging = true;
                try
                {
                    using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
                    {
                        connection.Open();
                        if (connection.State == System.Data.ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                    using (var db = new UnitOfWork(new PhonyDbContext()))
                    {
                        Model.User u = null;
                        await Task.Run(() =>
                        {
                            u = db.Users.GetLoginCredentials(Name, Pass);
                        });
                        if (u == null)
                        {
                            await Message.ShowMessageAsync("خطا", "تاكد من اسم المستخدم او كلمة المرور و ان المستخدم نشط").ConfigureAwait(false);
                        }
                        else
                        {
                            Core.WriteUserSession(u);
                            v.PageName = "Main";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Core.SaveException(ex);
                    if (ex.ToString().Contains("A network-related or instance-specific error occurred while establishing a connection to SQL Server"))
                    {
                        BespokeFusion.MaterialMessageBox.ShowError($"البرنامج لا يستطيع الاتصال بقاعده البيانات {Environment.NewLine} قم باعادة تشغيل البرنامج لاعداده من البدايه");
                    }
                    Properties.Settings.Default.IsConfigured = false;
                    Properties.Settings.Default.Save();
                }
                IsLogging = false;
            }
        }

    }
}