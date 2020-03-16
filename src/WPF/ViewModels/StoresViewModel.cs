using LiteDB;
using MahApps.Metro.Controls.Dialogs;
using Phony.WPF.Data;
using Phony.WPF.Models;
using Phony.WPF.Views;
using System;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;

namespace Phony.WPF.ViewModels
{
    public class StoresViewModel : BaseViewModelWithAnnotationValidation
    {
        int _storeId;
        string _name;
        string _motto;
        byte[] _image;
        string _address1;
        string _address2;
        string _tel1;
        string _tel2;
        string _phone1;
        string _phone2;
        string _email1;
        string _email2;
        string _site;
        string _notes;

        public int StoreId
        {
            get => _storeId;
            set
            {
                _storeId = value;
                NotifyOfPropertyChange(() => StoreId);
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

        public string Motto
        {
            get => _motto;
            set
            {
                _motto = value;
                NotifyOfPropertyChange(() => Motto);
            }
        }

        public byte[] Image
        {
            get => _image;
            set
            {
                _image = value;
                NotifyOfPropertyChange(() => Image);
            }
        }

        public string Address1
        {
            get => _address1;
            set
            {
                _address1 = value;
                NotifyOfPropertyChange(() => Address1);
            }
        }

        public string Address2
        {
            get => _address2;
            set
            {
                _address2 = value;
                NotifyOfPropertyChange(() => Address2);
            }
        }

        public string Tel1
        {
            get => _tel1;
            set
            {
                _tel1 = value;
                NotifyOfPropertyChange(() => Tel1);
            }
        }

        public string Tel2
        {
            get => _tel2;
            set
            {
                _tel2 = value;
                NotifyOfPropertyChange(() => Tel2);
            }
        }

        public string Phone1
        {
            get => _phone1;
            set
            {
                _phone1 = value;
                NotifyOfPropertyChange(() => Phone1);
            }
        }

        public string Phone2
        {
            get => _phone2;
            set
            {
                _phone2 = value;
                NotifyOfPropertyChange(() => Phone2);
            }
        }

        public string Email1
        {
            get => _email1;
            set
            {
                _email1 = value;
                NotifyOfPropertyChange(() => Email1);
            }
        }

        public string Email2
        {
            get => _email2;
            set
            {
                _email2 = value;
                NotifyOfPropertyChange(() => Email2);
            }
        }

        public string Site
        {
            get => _site;
            set
            {
                _site = value;
                NotifyOfPropertyChange(() => Site);
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

        Store store;

        public ObservableCollection<User> Users { get; set; }

        public StoresViewModel()
        {
            Title = "بيانات المحل";
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            Users = new ObservableCollection<User>(db.GetCollection<User>(DBCollections.Users).FindAll());
            store = db.GetCollection<Store>(DBCollections.Stores).FindById(1);
            Name = store.Name;
            Motto = store.Motto;
            Image = store.Image;
            Address1 = store.Address1;
            Address2 = store.Address2;
            Tel1 = store.Tel1;
            Tel2 = store.Tel2;
            Phone1 = store.Phone1;
            Phone2 = store.Phone2;
            Email1 = store.Email1;
            Email2 = store.Email2;
            Site = store.Site;
            Notes = store.Notes;
        }

        private bool CanEdit()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }

        private void DoEdit()
        {
            using var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString);
            store = db.GetCollection<Store>(DBCollections.Stores).FindById(1);
            store.Name = Name;
            store.Motto = Motto;
            store.Image = Image;
            store.Address1 = Address1;
            store.Address2 = Address2;
            store.Tel1 = Tel1;
            store.Tel2 = Tel2;
            store.Phone1 = Phone1;
            store.Phone2 = Phone2;
            store.Email1 = Email1;
            store.Email2 = Email2;
            store.Site = Site;
            store.Notes = Notes;
            //store.Editor = Core.ReadUserSession();
            store.EditDate = DateTime.Now;
            db.GetCollection<Store>(DBCollections.Stores).Update(store);
            MessageBox.MaterialMessageBox.Show("تم حفظ بيانات المحل بنجاح", "تمت العملية", true);
        }

        private bool CanSelectImage()
        {
            return true;
        }

        private void DoSelectImage()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            var codecs = ImageCodecInfo.GetImageEncoders();
            dlg.Filter = string.Format("All image files ({1})|{1}|All files|*",
            string.Join("|", codecs.Select(codec =>
            string.Format("({1})|{1}", codec.CodecName, codec.FilenameExtension)).ToArray()),
            string.Join(";", codecs.Select(codec => codec.FilenameExtension).ToArray()));
            var result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                Image = File.ReadAllBytes(filename);
            }
        }
    }
}