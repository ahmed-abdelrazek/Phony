using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.Data;
using Phony.Models;
using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class NotesViewModel : BindableBase
    {
        long _noId;
        string _name;
        string _phone;
        string _searchText;
        string _notes;
        string _childName;
        string _childNo;
        bool _byName;
        bool _fastResult;
        bool _openFastResult;
        bool _isAddNoFlyoutOpen;
        Note _dataGridSelectedNo;

        ObservableCollection<Note> _numbers;

        public long NoId
        {
            get => _noId;
            set => SetProperty(ref _noId, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
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

        public string ChildName
        {
            get => _childName;
            set => SetProperty(ref _childName, value);
        }

        public string ChildNo
        {
            get => _childNo;
            set => SetProperty(ref _childNo, value);
        }

        public bool ByName
        {
            get => _byName;
            set => SetProperty(ref _byName, value);
        }

        public bool FastResult
        {
            get => _fastResult;
            set => SetProperty(ref _fastResult, value);
        }

        public bool OpenFastResult
        {
            get => _openFastResult;
            set => SetProperty(ref _openFastResult, value);
        }

        public bool IsAddNoFlyoutOpen
        {
            get => _isAddNoFlyoutOpen;
            set => SetProperty(ref _isAddNoFlyoutOpen, value);
        }

        public Note DataGridSelectedNo
        {
            get => _dataGridSelectedNo;
            set => SetProperty(ref _dataGridSelectedNo, value);
        }

        public ObservableCollection<Note> Numbers
        {
            get => _numbers;
            set => SetProperty(ref _numbers, value);
        }

        public ObservableCollection<User> Users { get; set; }

        public ICommand AddNo { get; set; }
        public ICommand EditNo { get; set; }
        public ICommand DeleteNo { get; set; }
        public ICommand OpenAddNoFlyout { get; set; }
        public ICommand SelectImage { get; set; }
        public ICommand FillUI { get; set; }
        public ICommand ReloadAllNos { get; set; }
        public ICommand Search { get; set; }

        Notes Message = Application.Current.Windows.OfType<Notes>().FirstOrDefault();

        public NotesViewModel()
        {
            LoadCommands();
            ByName = true;
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                Numbers = new ObservableCollection<Note>(db.GetCollection<Note>(DBCollections.Notes).FindAll());
                Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users).FindAll());
            }
        }

        public void LoadCommands()
        {
            AddNo = new DelegateCommand(DoAddNo, CanAddNo).ObservesProperty(() => Name);
            EditNo = new DelegateCommand(DoEditNo, CanEditNo).ObservesProperty(() => Name).ObservesProperty(() => NoId).ObservesProperty(() => DataGridSelectedNo);
            DeleteNo = new DelegateCommand(DoDeleteNo, CanDeleteNo).ObservesProperty(() => DataGridSelectedNo);
            OpenAddNoFlyout = new DelegateCommand(DoOpenAddNoFlyout, CanOpenAddNoFlyout);
            FillUI = new DelegateCommand(DoFillUI, CanFillUI).ObservesProperty(() => DataGridSelectedNo);
            ReloadAllNos = new DelegateCommand(DoReloadAllNos, CanReloadAllNos);
            Search = new DelegateCommand(DoSearch, CanSearch).ObservesProperty(() => SearchText);
        }

        private bool CanAddNo()
        {
            return string.IsNullOrWhiteSpace(Name) ? false : true;
        }

        private void DoAddNo()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                var n = new Note
                {
                    Name = Name,
                    Phone = Phone,
                    Group = NoteGroup.Numbers,
                    Notes = Notes,
                    CreateDate = DateTime.Now,
                    Creator = Core.ReadUserSession(),
                    EditDate = null,
                    Editor = null
                };
                db.GetCollection<Note>(DBCollections.Notes).Insert(n);
                Numbers.Add(n);
                Message.ShowMessageAsync("تمت العملية", "تم اضافة الرقم بنجاح");
            }
        }

        private bool CanEditNo()
        {
            return string.IsNullOrWhiteSpace(Name) || NoId == 0 || DataGridSelectedNo == null ? false : true;
        }

        private void DoEditNo()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                var n = db.GetCollection<Note>(DBCollections.Notes).FindById(DataGridSelectedNo.Id);
                n.Name = Name;
                n.Phone = Phone;
                n.Notes = Notes;
                n.Editor = Core.ReadUserSession();
                n.EditDate = DateTime.Now;
                db.GetCollection<Note>(DBCollections.Notes).Update(n);
                Numbers[Numbers.IndexOf(DataGridSelectedNo)] = n;
                NoId = 0;
                DataGridSelectedNo = null;
                Message.ShowMessageAsync("تمت العملية", "تم تعديل الرقم بنجاح");
            }
        }

        private bool CanDeleteNo()
        {
            return DataGridSelectedNo == null ? false : true;
        }

        private async void DoDeleteNo()
        {
            var result = await Message.ShowMessageAsync("حذف الرقم", $"هل انت متاكد من حذف الرقم {DataGridSelectedNo.Name}", MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    db.GetCollection<Note>(DBCollections.Notes).Delete(DataGridSelectedNo.Id);
                    Numbers.Remove(DataGridSelectedNo);
                }
                DataGridSelectedNo = null;
                await Message.ShowMessageAsync("تمت العملية", "تم حذف الرقم بنجاح");
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
                    Numbers = new ObservableCollection<Note>(db.GetCollection<Note>(DBCollections.Notes).Find(i => i.Name.Contains(SearchText)));
                    if (Numbers.Count > 0)
                    {
                        if (FastResult)
                        {
                            ChildName = Numbers.FirstOrDefault().Name;
                            ChildNo = Numbers.FirstOrDefault().Phone;
                            OpenFastResult = true;
                        }
                    }
                    else
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

        private bool CanReloadAllNos()
        {
            return true;
        }

        private void DoReloadAllNos()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                Numbers = new ObservableCollection<Note>(db.GetCollection<Note>(DBCollections.Notes).FindAll());
            }
        }

        private bool CanFillUI()
        {
            return DataGridSelectedNo == null ? false : true;
        }

        private void DoFillUI()
        {
            NoId = DataGridSelectedNo.Id;
            Name = DataGridSelectedNo.Name;
            Phone = DataGridSelectedNo.Phone;
            Notes = DataGridSelectedNo.Notes;
            IsAddNoFlyoutOpen = true;
        }

        private bool CanOpenAddNoFlyout()
        {
            return true;
        }

        private void DoOpenAddNoFlyout()
        {
            IsAddNoFlyoutOpen = IsAddNoFlyoutOpen ? false : true;
        }
    }
}