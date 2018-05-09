using MahApps.Metro.Controls.Dialogs;
using Phony.Kernel;
using Phony.Model;
using Phony.Persistence;
using Phony.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModel
{
    public class UserVM : CommonBase
    {
        int _userId;
        string _name;
        UserGroup _selectedGroup;
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
            set
            {
                if (value != _userId)
                {
                    _userId = value;
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

        public SecureString Password1 { private get; set; }

        public SecureString Password2 { private get; set; }

        public UserGroup SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (value != _selectedGroup)
                {
                    _selectedGroup = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (value != _phone)
                {
                    _phone = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (value != _searchText)
                {
                    _searchText = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (value != _notes)
                {
                    _notes = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value != _isActive)
                {
                    _isActive = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsAddUserFlyoutOpen
        {
            get => _isAddUserFlyoutOpen;
            set
            {
                if (value != _isAddUserFlyoutOpen)
                {
                    _isAddUserFlyoutOpen = value;
                    RaisePropertyChanged();
                }
            }
        }

        public User DataGridSelectedUser
        {
            get => _dataGridSelectedUser;
            set
            {
                if (value != _dataGridSelectedUser)
                {
                    _dataGridSelectedUser = value;
                    RaisePropertyChanged();
                }
            }
        }

        public IEnumerable<UserGroup> Groups { get; set; }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                if (value != _users)
                {
                    _users = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand OpenAddUserFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand DeleteUser { get; set; }
        public ICommand ReloadAllUsers { get; set; }
        public ICommand Search { get; set; }
        public ICommand AddUser { get; set; }
        public ICommand EditUser { get; set; }

        View.Users Message = Application.Current.Windows.OfType<View.Users>().FirstOrDefault();

        public UserVM()
        {
            LoadCommands();
            IsActive = true;
            Groups = Enum.GetValues(typeof(UserGroup))
            .OfType<UserGroup>();
            using (var db = new PhonyDbContext())
            {
                Users = new ObservableCollection<User>(db.Users);
            }
        }

        public void LoadCommands()
        {
            OpenAddUserFlyout = new CustomCommand(DoOpenAddUserFlyout, CanOpenAddUserFlyout);
            FillUI = new CustomCommand(DoFillUI, CanFillUI);
            DeleteUser = new CustomCommand(DoDeleteUser, CanDeleteUser);
            ReloadAllUsers = new CustomCommand(DoReloadAllUsers, CanReloadAllUsers);
            Search = new CustomCommand(DoSearch, CanSearch);
            AddUser = new CustomCommand(DoAddUser, CanAddUser);
            EditUser = new CustomCommand(DoEditUser, CanEditUser);
        }

        private bool CanEditUser(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || UserId == 0 || DataGridSelectedUser == null ||string.IsNullOrWhiteSpace(new NetworkCredential("", Password1).Password) || string.IsNullOrWhiteSpace(new NetworkCredential("", Password2).Password))
            {
                //if (DataGridSelectedUser.Name != ViewModel.Users.CurrentUser.Name && ViewModel.Users.CurrentUser.Group != UserGroup.Manager)
                //{
                return false;
                //}
            }
            return true;
        }

        private void DoEditUser(object obj)
        {
            if (new NetworkCredential("", Password1).Password == new NetworkCredential("", Password2).Password)
            {
                using (var db = new UnitOfWork(new PhonyDbContext()))
                {
                    var u = db.Users.Get(DataGridSelectedUser.Id);
                    u.Name = Name;
                    u.Pass = SecurePasswordHasher.Hash(new NetworkCredential("", Password1).Password);
                    u.Group = SelectedGroup;
                    u.Phone = Phone;
                    u.Notes = Notes;
                    u.IsActive = IsActive;
                    db.Complete();
                    UserId = 0;
                    Users.Remove(DataGridSelectedUser);
                    Users.Add(u);
                    DataGridSelectedUser = null;
                    Message.ShowMessageAsync("تمت العملية", "تم تعديل المستخدم بنجاح");
                }
            }
            else
            {
                Message.ShowMessageAsync("تاكد من الباسورد", "كلمتى المرور غير متطابقتين");
            }
        }

        private bool CanAddUser(object obj)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(new NetworkCredential("", Password1).Password) || string.IsNullOrWhiteSpace(new NetworkCredential("", Password2).Password))
            {
                return false;
            }
            return true;
        }

        private void DoAddUser(object obj)
        {
            if (new NetworkCredential("", Password1).Password == new NetworkCredential("", Password2).Password)
            {
                using (var db = new UnitOfWork(new PhonyDbContext()))
                {

                    var u = new User
                    {
                        Name = Name,
                        Pass = SecurePasswordHasher.Hash(new NetworkCredential("", Password1).Password),
                        Group = SelectedGroup,
                        Phone = Phone,
                        Notes = Notes,
                        IsActive = IsActive
                    };
                    db.Users.Add(u);
                    db.Complete();
                    Users.Add(u);
                    Message.ShowMessageAsync("تمت العملية", "تم اضافة المستخدم بنجاح");
                }

            }
            else
            {
                Message.ShowMessageAsync("تاكد من الباسورد", "كلمتى المرور غير متطابقتين");
            }
        }

        private bool CanSearch(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return false;
            }
            return true;
        }

        private void DoSearch(object obj)
        {
            using (var db = new PhonyDbContext())
            {
                Users = new ObservableCollection<User>(db.Users);
                if (Users.Count < 1)
                {
                    Message.ShowMessageAsync("غير موجود", "لم يتم العثور على شئ");
                }
            }
        }

        private bool CanReloadAllUsers(object obj)
        {
            return true;
        }

        private void DoReloadAllUsers(object obj)
        {
            using (var db = new PhonyDbContext())
            {
                Users = new ObservableCollection<User>(db.Users);
            }
        }

        private bool CanDeleteUser(object obj)
        {
            if (DataGridSelectedUser == null || DataGridSelectedUser.Id == 1)
            {
                return false;
            }
            return true;
        }

        private async void DoDeleteUser(object obj)
        {
            var result = await Message.ShowMessageAsync("حذف الرقم", $"هل انت متاكد من حذف الرقم {DataGridSelectedUser.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new UnitOfWork(new PhonyDbContext()))
                {
                    db.Users.Remove(db.Users.Get(DataGridSelectedUser.Id));
                    db.Complete();
                    Users.Remove(DataGridSelectedUser);
                }
                DataGridSelectedUser = null;
                await Message.ShowMessageAsync("تمت العملية", "تم حذف الكارت بنجاح");
            }
        }

        private bool CanFillUI(object obj)
        {
            if (DataGridSelectedUser == null)
            {
                return false;
            }
            return true;
        }

        private void DoFillUI(object obj)
        {
            UserId = DataGridSelectedUser.Id;
            Name = DataGridSelectedUser.Name;
            SelectedGroup = DataGridSelectedUser.Group;
            Phone = DataGridSelectedUser.Phone;
            Notes = DataGridSelectedUser.Notes;
            IsActive = DataGridSelectedUser.IsActive;
            IsAddUserFlyoutOpen = true;
        }

        private bool CanOpenAddUserFlyout(object obj)
        {
            return true;
        }

        private void DoOpenAddUserFlyout(object obj)
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