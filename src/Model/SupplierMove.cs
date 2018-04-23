using System;

namespace Phony.Model
{
    public class SupplierMove : BaseModel
    {
        public SupplierMove()
        {
            Date = DateTime.Now;
        }

        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}