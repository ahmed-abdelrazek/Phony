using LiteDB;

namespace Phony.Models
{
    public class SupplierMove : BaseModel
    {
        [BsonRef(nameof(Data.DBCollections.Suppliers))]
        public virtual Supplier Supplier { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}