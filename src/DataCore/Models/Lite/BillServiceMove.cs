using System;

namespace Phony.WPF.Models.Lite
{
    public class BillServiceMove : IBaseModel
    {
        public virtual Bill Bill { get; set; }

        public virtual Service Service { get; set; }

        public decimal Balance { get; set; }

        public decimal Cost { get; set; }

        public decimal Discount { get; set; }

        public uint Id { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedOn { get; set; }

        public User Creator { get; set; }

        public DateTime? EditedOn { get; set; }

        public User Editor { get; set; }
    }
}
