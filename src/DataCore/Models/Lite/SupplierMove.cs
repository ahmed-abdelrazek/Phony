using System;

namespace Phony.Data.Models.Lite
{
    public class SupplierMove : BaseModel
    {
        public virtual Supplier Supplier { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}