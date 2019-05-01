using System;

namespace Phony.WPF.Models.Lite
{
    public class BillItemMove : IBaseModel
    {
        public virtual Bill Bill { get; set; }

        public virtual Item Item { get; set; }

        public decimal ItemPrice { get; set; }

        public decimal QTY { get; set; }

        public decimal Discount { get; set; }

        public uint Id { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedOn { get; set; }

        public User Creator { get; set; }

        public DateTime? EditedOn { get; set; }

        public User Editor { get; set; }
    }
}