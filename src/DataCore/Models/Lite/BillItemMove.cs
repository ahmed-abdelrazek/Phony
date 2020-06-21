using System;

namespace Phony.Data.Models.Lite
{
    public class BillItemMove : BaseModel
    {
        public virtual Bill Bill { get; set; }

        public virtual Item Item { get; set; }

        public decimal ItemPrice { get; set; }

        public decimal QTY { get; set; }

        public decimal Discount { get; set; }
    }
}