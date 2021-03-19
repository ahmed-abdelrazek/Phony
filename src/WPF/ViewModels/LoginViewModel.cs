﻿using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Phony.Data.Models.Lite;
using Phony.WPF.Data;
using Phony.WPF.Views;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TinyLittleMvvm;

namespace Phony.WPF.ViewModels
{
    public class LoginViewModel : BaseViewModelWithAnnotationValidation, IOnLoadedHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWindowManager _windowManager;
        private readonly ILogger _logger;
        string _userName;
        string _password;
        bool _isLogging;

        [Required(ErrorMessage = "حقل مطلوب")]
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        [Required(ErrorMessage = "حقل مطلوب")]
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
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

        public ICommand Login { get; }
        public ICommand OpenSettingsWindow { get; }

        private readonly DbConnectionStringBuilder connectionStringBuilder = new();

        public LoginViewModel(IServiceProvider serviceProvider, IWindowManager windowManager, ILogger<LoginViewModel> logger)
        {
            this._serviceProvider = serviceProvider;
            this._windowManager = windowManager;
            this._logger = logger;

            Login = new AsyncRelayCommand<PasswordBox>(DoLogin, CanLogin);
            OpenSettingsWindow = new RelayCommand(DoOpenSettingsWindow);
        }

        public Task OnLoadedAsync()
        {
            connectionStringBuilder.ConnectionString = Properties.Settings.Default.LiteDbConnectionString;
            if (string.IsNullOrWhiteSpace(connectionStringBuilder.ConnectionString) || !File.Exists(connectionStringBuilder["Filename"].ToString()) || !Properties.Settings.Default.IsConfigured)
            {
                _logger.LogInformation("Opening the setting to config the app");
                _windowManager.ShowDialog<SettingsViewModel>();
                if (!Properties.Settings.Default.IsConfigured)
                {
                    _logger.LogWarning("The app isn't configured and will now shutdown");
                    Environment.Exit(0);
                }
            }
            return Task.CompletedTask;
        }

        public bool CanLogin(PasswordBox pass)
        {
            var password = pass.Password;
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(password);
        }

        public async Task DoLogin(PasswordBox pass)
        {
            var password = pass.Password;

            if (!IsLogging)
            {
                IsLogging = true;
                try
                {
                    using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
                    User u = null;
                    await Task.Run(() =>
                    {
                        u = db.GetCollection<User>(DBCollections.Users).Find(x => x.Name == UserName).FirstOrDefault();
                    });
                    if (u == null)
                    {
                        MessageBox.MaterialMessageBox.ShowWarning("تاكد من اسم المستخدم او كلمة المرور و ان المستخدم نشط", "خطا", true);
                    }
                    else
                    {
                        if (SecurePasswordHasher.Verify(password, u.Pass))
                        {
                            var m = _serviceProvider.GetRequiredService<MainViewModel>();
                            m.CurrentUser = new User
                            {
                                Id = u.Id,
                                Name = u.Name,
                                Group = u.Group,
                                Phone = u.Phone,
                                Notes = u.Notes,
                                IsActive = u.IsActive
                            };
                            _windowManager.ShowWindow(m);
                            Application.Current.Windows.OfType<LoginView>().FirstOrDefault().Close();
                        }
                        else
                        {
                            MessageBox.MaterialMessageBox.ShowWarning("تاكد من اسم المستخدم او كلمة المرور و ان المستخدم نشط", "خطا", true);
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

        public void DoOpenSettingsWindow()
        {
            _windowManager.ShowWindow(_serviceProvider.GetRequiredService<SettingsViewModel>());
        }
    }
}