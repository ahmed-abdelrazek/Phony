using Prism.Mvvm;
using System;

namespace Phony.DTOs
{
    public class StoreDto : BindableBase
    {
        private long id;
        private string name;
        private UserDto creator;
        private DateTime createDate;
        private UserDto editor;
        private DateTime? editDate;

        public long Id { get => id; set => SetProperty(ref id, value); }

        public string Name { get => name; set => SetProperty(ref name, value); }

        public string Motto { get; set; }

        public byte[] Image { get; set; }

        public string ImageString { get => Image == null ? null : Convert.ToBase64String(Image); }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Tel1 { get; set; }

        public string Tel2 { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string Site { get; set; }

        public UserDto Creator { get => creator; set => SetProperty(ref creator, value); }

        public DateTime CreateDate { get => createDate; set => SetProperty(ref createDate, value); }

        public UserDto Editor { get => editor; set => SetProperty(ref editor, value); }

        public DateTime? EditDate { get => editDate; set => SetProperty(ref editDate, value); }
    }
}
