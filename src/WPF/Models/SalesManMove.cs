using LiteDB;

namespace Phony.WPF.Models
{
    public class SalesManMove : BaseModel
    {
        [BsonRef(nameof(Data.DBCollections.SalesMen))]
        public virtual SalesMan SalesMan { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}