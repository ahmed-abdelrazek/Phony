using LiteDB;
using System;

namespace Phony.Model
{
    public class SupplierMove : BaseModel
    {
        [BsonRef(nameof(ViewModel.DBCollections.Suppliers))]
        public virtual Supplier Supplier { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}