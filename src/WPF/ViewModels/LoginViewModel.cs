using Caliburn.Micro;
using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.WPF.Data;
using Phony.WPF.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Phony.WPF.ViewModels
{
    public class LoginViewModel : Screen
    {
        string _userName;
        string _password;
        bool _isLogging;
        SimpleContainer _container;

        IEventAggregator _events;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogin);
            }
        }

        public bool IsLogging
        {
            get => _isLogging;
            set
            {
                _isLogging = value;
                NotifyOfPropertyChange(() => IsLogging);
            }
        }

        public LoginViewModel(IEventAggregator events, SimpleContainer container)
        {
            _events = events;
            _container = container;
        }

        public bool CanLogin
        {
            get
            {
                return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
            }
        }

        public async Task Login()
        {
            if (!IsLogging)
            {
                IsLogging = true;
                try
                {
                    using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                    {
                        User u = null;
                        await Task.Run(() =>
                        {
                            u = db.GetCollection<User>(DBCollections.Users).Find(x => x.Name == UserName).FirstOrDefault();
                        });
                        if (u == null)
                        {
                            await DialogCoordinator.Instance.ShowMessageAsync(this, "خطا", "تاكد من اسم المستخدم او كلمة المرور و ان المستخدم نشط").ConfigureAwait(false);
                        }
                        else
                        {
                            if (SecurePasswordHasher.Verify(Password, u.Pass))
                            {
                                u.Pass = null;
                                await _events.PublishOnUIThreadAsync(u);
                            }
                            else
                            {
                                await DialogCoordinator.Instance.ShowMessageAsync(this, "خطا", "تاكد من اسم المستخدم او كلمة المرور و ان المستخدم نشط").ConfigureAwait(false);
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