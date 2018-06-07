using System;

namespace Phony.Model
{
    public class SupplierMove : BaseModel
    {
        public long SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public decimal Amount { get; set; }
    }
}