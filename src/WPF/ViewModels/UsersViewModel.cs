using Caliburn.Micro;
using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.WPF.Data;
using Phony.WPF.Extensions;
using Phony.WPF.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace Phony.WPF.ViewModels
{
    public class UsersViewModel : Screen
    {
        int _userId;
        string _name;
        string _password1;
        string _password2;
        byte _selectedGroup;
        string _phone;
        string _searchText;
        string _notes;
        bool _isUserActive;
        bool _isAddUserFlyoutOpen;
        User _dataGridSelectedUser;

        ObservableCollection<User> _users;

        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                NotifyOfPropertyChange(() => UserId);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string Password1
        {
            get => _password1;
            set
            {
                _password1 = value;
                NotifyOfPropertyChange(() => Password1);
            }
        }

        public string Password2
        {
            get => _password2;
            set
            {
                _password2 = value;
                NotifyOfPropertyChange(() => Password2);
            }
        }

        public byte SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                NotifyOfPropertyChange(() => SelectedGroup);
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                NotifyOfPropertyChange(() => Phone);
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                NotifyOfPropertyChange(() => SearchText);
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                NotifyOfPropertyChange(() => Notes);
            }
        }

        public bool IsUserActive
        {
            get => _isUserActive;
            set
            {
                _isUserActive = value;
                NotifyOfPropertyChange(() => IsUserActive);
            }
        }

        public bool IsAddUserFlyoutOpen
        {
            get => _isAddUserFlyoutOpen;
            set
            {
                _isAddUserFlyoutOpen = value;
                NotifyOfPropertyChange(() => IsAddUserFlyoutOpen);
            }
        }

        public User DataGridSelectedUser
        {
            get => _dataGridSelectedUser;
            set
            {
                _dataGridSelectedUser = value;
                NotifyOfPropertyChange(() => DataGridSelectedUser);
            }
        }

        public ObservableCollection<Enumeration<byte>> Groups { get; set; }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        Views.Users Message = Application.Current.Windows.OfType<Views.Users>().FirstOrDefault();

        public UsersViewModel()
        {
            IsUserActive = true;
            Groups = new ObservableCollection<Enumeration<byte>>();
            foreach (var group in Enum.GetValues(typeof(UserGroup)))
            {
                Groups.Add(new Enumeration<byte>
                {
                    Id = (byte)group,
                    Name = Enumerations.GetEnumDescription((UserGroup)group).ToString()
                });
            }
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users).FindAll().ToList());
            }
        }

        private bool CanAddUser()
        {
            return string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Password1) || string.IsNullOrWhiteSpace(Password2) ? false : true;
        }

        private void DoAddUser()
        {
            if (new NetworkCredential("", Password1).Password == new NetworkCredential("", Password2).Password)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    var userCol = db.GetCollection<User>(DBCollections.Users);
                    var user = userCol.Find(x => x.Name == Name).FirstOrDefault();
                    if (user == null)
                    {
                        var u = new User
                        {
                            Name = Name,
                            Pass = SecurePasswordHasher.Hash(Password1),
                            Group = (UserGroup)SelectedGroup,
                            Phone = Phone,
                            Notes = Notes,
                            IsActive = IsActive
                        };
                        userCol.Insert(u);
                        Users.Add(u);
                        Message.ShowMessageAsync("تمت العملية", "تم اضافة المستخدم بنجاح");
                    }
                    else
                    {
                        Message.ShowMessageAsync("تكرار مستخدمين", "هناك مستخدم بنفس الاسم بالفعل");
                    }
                }
            }
            else
            {
                Message.ShowMessageAsync("تاكد من الباسورد", "كلمتى المرور غير متطابقتين");
            }
        }

        private bool CanEditUser()
        {
            return string.IsNullOrWhiteSpace(Name) || UserId == 0 || DataGridSelectedUser == null || string.IsNullOrWhiteSpace(Password1) || string.IsNullOrWhiteSpace(Password2)
                ? false
                : true;
        }

        private void DoEditUser()
        {
            if (new NetworkCredential("", Password1).Password == new NetworkCredential("", Password2).Password)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    var userCol = db.GetCollection<User>(DBCollections.Users);
                    var u = userCol.Find(x => x.Id == DataGridSelectedUser.Id).FirstOrDefault();
                    u.Name = Name;
                    u.Pass = SecurePasswordHasher.Hash(Password1);
                    u.Group = (UserGroup)SelectedGroup;
                    u.Phone = Phone;
                    u.Notes = Notes;
                    u.IsActive = IsActive;
                    userCol.Update(u);
                    Users[Users.IndexOf(DataGridSelectedUser)] = u;
                    UserId = 0;
                    DataGridSelectedUser = null;
                    Message.ShowMessageAsync("تمت العملية", "تم تعديل المستخدم بنجاح");
                }
            }
            else
            {
                Message.ShowMessageAsync("تاكد من الباسورد", "كلمتى المرور غير متطابقتين");
            }
        }

        private bool CanDeleteUser()
        {
            return DataGridSelectedUser == null || DataGridSelectedUser.Id == 1 ? false : true;
        }

        private async void DoDeleteUser()
        {
            var result = await Message.ShowMessageAsync("حذف الرقم", $"هل انت متاكد من حذف الرقم {DataGridSelectedUser.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    db.GetCollection<User>(DBCollections.Users).Delete(DataGridSelectedUser.Id);
                    Users.Remove(DataGridSelectedUser);
                }
                DataGridSelectedUser = null;
                await Message.ShowMessageAsync("تمت العملية", "تم حذف الكارت بنجاح");
            }
        }

        private bool CanSearch()
        {
            return string.IsNullOrWhiteSpace(SearchText) ? false : true;
        }

        private async void DoSearch()
        {
            try
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users).FindAll().ToList());
                    if (Users.Count < 1)
                    {
                        await Message.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                    }
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                await Message.ShowMessageAsync("خطأ", "لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله");
            }
        }

        private bool CanReloadAllUsers()
        {
            return true;
        }

        private void DoReloadAllUsers()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users).FindAll().ToList());
            }
        }

        private bool CanFillUI()
        {
            if (DataGridSelectedUser == null)
            {
                return false;
            }
            return true;
        }

        private void DoFillUI()
        {
            UserId = DataGridSelectedUser.Id;
            Name = DataGridSelectedUser.Name;
            SelectedGroup = (byte)DataGridSelectedUser.Group;
            Phone = DataGridSelectedUser.Phone;
            Notes = DataGridSelectedUser.Notes;
            IsUserActive = DataGridSelectedUser.IsActive;
            IsAddUserFlyoutOpen = true;
        }

        private bool CanOpenAddUserFlyout()
        {
            return true;
        }

        private void DoOpenAddUserFlyout()
        {
            if (IsAddUserFlyoutOpen)
            {
                IsAddUserFlyoutOpen = false;
            }
            else
            {
                IsAddUserFlyoutOpen = true;
            }
        }
    }
}