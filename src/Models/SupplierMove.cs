using LiteDB;
using System;

namespace Phony.Models
{
    public class SupplierMove : BaseModel
    {
        [BsonRef(nameof(ViewModels.DBCollections.Suppliers))]
        public virtual Supplier Supplier { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}