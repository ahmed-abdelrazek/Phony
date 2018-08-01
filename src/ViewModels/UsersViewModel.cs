using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
using Phony.Extensions;
using Phony.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class UsersViewModel : BindableBase
    {
        int _userId;
        string _name;
        string _password1;
        string _password2;
        byte _selectedGroup;
        string _phone;
        string _searchText;
        string _notes;
        bool _isActive;
        bool _isAddUserFlyoutOpen;
        User _dataGridSelectedUser;

        ObservableCollection<User> _users;

        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Password1
        {
            get => _password1;
            set => SetProperty(ref _password1, value);
        }

        public string Password2
        {
            get => _password2;
            set => SetProperty(ref _password2, value);
        }

        public byte SelectedGroup
        {
            get => _selectedGroup;
            set => SetProperty(ref _selectedGroup, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public bool IsAddUserFlyoutOpen
        {
            get => _isAddUserFlyoutOpen;
            set => SetProperty(ref _isAddUserFlyoutOpen, value);
        }

        public User DataGridSelectedUser
        {
            get => _dataGridSelectedUser;
            set => SetProperty(ref _dataGridSelectedUser, value);
        }

        public ObservableCollection<Enumeration<byte>> Groups { get; set; }

        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public ICommand AddUser { get; set; }
        public ICommand EditUser { get; set; }
        public ICommand DeleteUser { get; set; }
        public ICommand OpenAddUserFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand ReloadAllUsers { get; set; }
        public ICommand Search { get; set; }

        Views.Users Message = Application.Current.Windows.OfType<Views.Users>().FirstOrDefault();

        public UsersViewModel()
        {
            LoadCommands();
            IsActive = true;
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

        public void LoadCommands()
        {
            AddUser = new DelegateCommand(DoAddUser, CanAddUser).ObservesProperty(() => Name).ObservesProperty(() => Password1).ObservesProperty(() => Password2);
            EditUser = new DelegateCommand(DoEditUser, CanEditUser).ObservesProperty(() => Name).ObservesProperty(() => Password1).ObservesProperty(() => Password2).ObservesProperty(() => UserId).ObservesProperty(() => DataGridSelectedUser);
            DeleteUser = new DelegateCommand(DoDeleteUser, CanDeleteUser).ObservesProperty(() => DataGridSelectedUser);
            OpenAddUserFlyout = new DelegateCommand(DoOpenAddUserFlyout, CanOpenAddUserFlyout);
            FillUI = new DelegateCommand(DoFillUI, CanFillUI).ObservesProperty(() => DataGridSelectedUser);
            ReloadAllUsers = new DelegateCommand(DoReloadAllUsers, CanReloadAllUsers);
            Search = new DelegateCommand(DoSearch, CanSearch).ObservesProperty(() => SearchText);
        }

        private bool CanAddUser()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Password1) || string.IsNullOrWhiteSpace(Password2))
            {
                return false;
            }
            return true;
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
            if (string.IsNullOrWhiteSpace(Name) || UserId == 0 || DataGridSelectedUser == null || string.IsNullOrWhiteSpace(Password1) || string.IsNullOrWhiteSpace(Password2))
            {
                return false;
            }
            return true;
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
            if (DataGridSelectedUser == null || DataGridSelectedUser.Id == 1)
            {
                return false;
            }
            return true;
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
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return false;
            }
            return true;
        }

        private void DoSearch()
        {
            try
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users).FindAll().ToList());
                    if (Users.Count < 1)
                    {
                        Message.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                    }
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
                BespokeFusion.MaterialMessageBox.ShowError("لم يستطع ايجاد ما تبحث عنه تاكد من صحه البيانات المدخله");
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
            IsActive = DataGridSelectedUser.IsActive;
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