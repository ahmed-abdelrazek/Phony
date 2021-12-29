using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
using Phony.Models;
using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
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

        private readonly MainWindowViewModel v = new();
        private readonly Views.MainWindow Message = Application.Current.Windows.OfType<Views.MainWindow>().FirstOrDefault();

        public LoginViewModel()
        {
            LoadCommands();
            if (Properties.Settings.Default.IsConfigured)
            {
                LiteDbContext.ConnectionString  = LiteDbContext.GetConnectionString(Properties.Settings.Default.LiteDbConnectionString);
            }
        }

        private void LoadCommands()
        {
            LogIn = new DelegateCommand(DoLogIn, CanDoLogIn).ObservesProperty(() => Name);
        }

        private bool CanDoLogIn()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private async void DoLogIn()
        {
            if (!IsLogging)
            {
                IsLogging = true;
                try
                {
                    if (File.Exists(LiteDbContext.ConnectionString.Filename))
                    {
                        using var db = new LiteDatabase(LiteDbContext.ConnectionString);
                        User u = null;
                        await Task.Run(() =>
                        {
                            u = db.GetCollection<User>(DBCollections.Users).Find(x => x.Name == Name).FirstOrDefault();
                        });
                        if (u is null)
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
                    else
                    {
                        Properties.Settings.Default.IsConfigured = false;
                        Properties.Settings.Default.Save();
                        new Settings(0).ShowDialog();
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