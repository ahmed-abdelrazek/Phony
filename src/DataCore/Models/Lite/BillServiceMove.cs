using System;

namespace Phony.Data.Models.Lite
{
    public class BillServiceMove : BaseModel
    {
        public virtual Bill Bill { get; set; }

        public virtual Service Service { get; set; }

        public decimal Balance { get; set; }

        public decimal Cost { get; set; }

        public decimal Discount { get; set; }
    }
}
