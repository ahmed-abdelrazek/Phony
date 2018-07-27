using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
using Phony.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        string _name;
        string _pass;
        bool _isLogging;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Pass
        {
            get => _pass;
            set => SetProperty(ref _pass, value);
        }

        public bool IsLogging
        {
            get => _isLogging;
            set => SetProperty(ref _isLogging, value);
        }

        public ICommand LogIn { get; set; }

        MainWindowViewModel v = new MainWindowViewModel();

        Views.MainWindow Message = Application.Current.Windows.OfType<Views.MainWindow>().FirstOrDefault();
        DbConnectionStringBuilder ConnectionStringBuilder = new DbConnectionStringBuilder();

        public LoginViewModel()
        {
            LoadCommands();
            ConnectionStringBuilder.ConnectionString = Properties.Settings.Default.DBFullName;
        }

        private void LoadCommands()
        {
            LogIn = new DelegateCommand(DoLogIn, CanDoLogIn).ObservesProperty(() => Name);
        }

        private bool CanDoLogIn()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }

        private async void DoLogIn()
        {
            if (!IsLogging)
            {
                IsLogging = true;
                try
                {
                    ConnectionStringBuilder.ConnectionString = Properties.Settings.Default.DBFullName;
                    if (File.Exists(ConnectionStringBuilder["Filename"].ToString()))
                    {
                        using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
                        {
                            User u = null;
                            await Task.Run(() =>
                            {
                                u = db.GetCollection<User>(DBCollections.Users).Find(x => x.Name == Name).FirstOrDefault();
                            });
                            if (u == null)
                            {
                                await Message.ShowMessageAsync("خطا", "تاكد من اسم المستخدم او كلمة المرور و ان المستخدم نشط").ConfigureAwait(false);
                            }
                            else
                            {
                                if (SecurePasswordHasher.Verify(Pass, u.Pass))
                                {
                                    Core.WriteUserSession(u);
                                    v.PageName = "Main";
                                }
                                else
                                {
                                    await Message.ShowMessageAsync("خطا", "تاكد من اسم المستخدم او كلمة المرور و ان المستخدم نشط").ConfigureAwait(false);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Core.SaveException(ex);
                    Properties.Settings.Default.IsConfigured = false;
                    Properties.Settings.Default.Save();
                }
                IsLogging = false;
            }
        }

    }
}