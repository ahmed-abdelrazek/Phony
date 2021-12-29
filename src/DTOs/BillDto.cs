using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace Phony.DTOs
{
    public class BillDto : BindableBase
    {
        private long id;
        private string notes;
        private UserDto creator;
        private DateTime createDate;
        private UserDto editor;
        private DateTime? editDate;

        public long Id { get => id; set => SetProperty(ref id, value); }

        public ClientDto Client { get; set; }

        public StoreDto Store { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAfterDiscounts { get; set; }

        public decimal TotalPayed { get; set; }

        public decimal Change { get; set; }

        public bool IsReturned { get; set; }

        public List<BillItemMoveDto> ItemsMoves { get; set; }

        public List<BillServiceMoveDto> ServicesMoves { get; set; }

        public string Notes { get => notes; set => SetProperty(ref notes, value); }

        public UserDto Creator { get => creator; set => SetProperty(ref creator, value); }

        public DateTime CreateDate { get => createDate; set => SetProperty(ref createDate, value); }

        public UserDto Editor { get => editor; set => SetProperty(ref editor, value); }

        public DateTime? EditDate { get => editDate; set => SetProperty(ref editDate, value); }
    }
}
