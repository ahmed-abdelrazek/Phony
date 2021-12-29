using Prism.Mvvm;
using System;

namespace Phony.DTOs
{
    public class BillItemMoveDto : BindableBase
    {
        private long id;
        private string notes;
        private UserDto creator;
        private DateTime createDate;
        private UserDto editor;
        private DateTime? editDate;

        public long Id { get => id; set => SetProperty(ref id, value); }

        public virtual ItemDto Item { get; set; }

        public decimal ItemPrice { get; set; }

        public decimal QTY { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get => Discount == 0 ? (ItemPrice * QTY) : (ItemPrice * QTY) * Discount / 100; }

        public string Notes { get => notes; set => SetProperty(ref notes, value); }

        public UserDto Creator { get => creator; set => SetProperty(ref creator, value); }

        public DateTime CreateDate { get => createDate; set => SetProperty(ref createDate, value); }

        public UserDto Editor { get => editor; set => SetProperty(ref editor, value); }

        public DateTime? EditDate { get => editDate; set => SetProperty(ref editDate, value); }
    }
}
