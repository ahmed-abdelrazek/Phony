using Prism.Mvvm;
using System;

namespace Phony.DTOs
{
    public class ItemDto : BindableBase
    {
        private long id;
        private string name;
        private string notes;
        private UserDto creator;
        private DateTime createDate;
        private UserDto editor;
        private DateTime? editDate;

        public long Id { get => id; set => SetProperty(ref id, value); }

        public string Name { get => name; set => SetProperty(ref name, value); }

        public string Barcode { get; set; }

        public string Shopcode { get; set; }

        public byte[] Image { get; set; }

        public EnumDto Group { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal WholeSalePrice { get; set; }

        public decimal HalfWholeSalePrice { get; set; }

        public decimal RetailPrice { get; set; }

        public decimal QTY { get; set; }

        public CompanyDto Company { get; set; }

        public SupplierDto Supplier { get; set; }

        public string Notes { get => notes; set => SetProperty(ref notes, value); }

        public UserDto Creator { get => creator; set => SetProperty(ref creator, value); }

        public DateTime CreateDate { get => createDate; set => SetProperty(ref createDate, value); }

        public UserDto Editor { get => editor; set => SetProperty(ref editor, value); }

        public DateTime? EditDate { get => editDate; set => SetProperty(ref editDate, value); }
    }
}
