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
        static int _id;
        static string _name;
        static UserGroup _group;
        bool _isLogging;

        public int Id
        {
            get => _id;
            set
            {
                if (value != _id)
                {
                    _id = value;
                    RaisePropertyChanged();
                }
            }
        }

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

        public UserGroup Group
        {
            get => _group;
            set
            {
                if (value != _group)
                {
                    _group = value;
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

        public SecureString SecurePassword { private get; set; }

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
                            u = db.Users.GetLoginCredentials(Name, new NetworkCredential("", SecurePassword).Password);
                        });
                        if (u == null)
                        {
                            await Message.ShowMessageAsync("خطا", "تاكد من اسم المستخدم او كلمة المرور و ان المستخدم نشط").ConfigureAwait(false);
                        }
                        else
                        {
                            Id = u.Id;
                            Name = u.Name;
                            Group = u.Group;
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