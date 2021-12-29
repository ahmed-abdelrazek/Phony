using Prism.Mvvm;
using System;

namespace Phony.DTOs
{
    public class BillServiceMoveDto : BindableBase
    {
        private long id;
        private string notes;
        private UserDto creator;
        private DateTime createDate;
        private UserDto editor;
        private DateTime? editDate;

        public long Id { get => id; set => SetProperty(ref id, value); }

        public virtual ServiceDto Service { get; set; }

        public decimal Balance { get; set; }

        public decimal Cost { get; set; }

        public decimal QTY { get; set; } = 1;

        public decimal Discount { get; set; }

        public decimal Total { get => Discount == 0 ? (Cost * QTY) : (Cost * QTY) * Discount / 100; }

        public string Notes { get => notes; set => SetProperty(ref notes, value); }

        public UserDto Creator { get => creator; set => SetProperty(ref creator, value); }

        public DateTime CreateDate { get => createDate; set => SetProperty(ref createDate, value); }

        public UserDto Editor { get => editor; set => SetProperty(ref editor, value); }

        public DateTime? EditDate { get => editDate; set => SetProperty(ref editDate, value); }
    }
}
