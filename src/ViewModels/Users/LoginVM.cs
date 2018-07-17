﻿using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels.Users
{
    public class LoginVM : BindableBase
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

        MainWindowViewModel v = new MainWindowViewModel();

        Views.MainWindow Message = Application.Current.Windows.OfType<Views.MainWindow>().FirstOrDefault();
        DbConnectionStringBuilder ConnectionStringBuilder = new DbConnectionStringBuilder();

        public LoginVM()
        {
            LoadCommands();
            ConnectionStringBuilder.ConnectionString = Properties.Settings.Default.DBFullName;
        }

        private void LoadCommands()
        {
            LogIn = new DelegateCommand(DoLogIn, CanDoLogIn).ObservesProperty(()=> Name); ;
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
                                u = db.GetCollection<User>(DBCollections.Users.ToString()).Find(x => x.Name == Name).FirstOrDefault();
                            });
                            if (u == null)
                            {
                                await Message.ShowMessageAsync("خطا", "تاكد من اسم المستخدم او كلمة المرور و ان المستخدم نشط").ConfigureAwait(false);
                            }
                            else
                            {
                                if (SecurePasswordHasher.Verify(new NetworkCredential("", SecurePassword).Password, u.Pass))
                                {
                                    Id = u.Id;
                                    Name = u.Name;
                                    Group = u.Group;
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