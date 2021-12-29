using Prism.Mvvm;
using System;

namespace Phony.DTOs
{
    public class SupplierDto : BindableBase
    {
        private long id;
        private string name;
        private decimal balance;
        private byte[] image;
        private string site;
        private string email;
        private string phone;
        private string notes;
        private UserDto creator;
        private DateTime createDate;
        private UserDto editor;
        private DateTime? editDate;

        public long Id { get => id; set => SetProperty(ref id, value); }

        public string Name { get => name; set => SetProperty(ref name, value); }

        public decimal Balance { get => balance; set => SetProperty(ref balance, value); }

        public byte[] Image { get => image; set => SetProperty(ref image, value); }

        public string Site { get => site; set => SetProperty(ref site, value); }

        public string Email { get => email; set => SetProperty(ref email, value); }

        public string Phone { get => phone; set => SetProperty(ref phone, value); }

        public string Notes { get => notes; set => SetProperty(ref notes, value); }

        public SalesManDto SalesMan { get; set; }

        public UserDto Creator { get => creator; set => SetProperty(ref creator, value); }

        public DateTime CreateDate { get => createDate; set => SetProperty(ref createDate, value); }

        public UserDto Editor { get => editor; set => SetProperty(ref editor, value); }

        public DateTime? EditDate { get => editDate; set => SetProperty(ref editDate, value); }
    }
}
