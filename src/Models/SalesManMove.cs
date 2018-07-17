using LiteDB;

namespace Phony.Models
{
    public class SalesManMove : BaseModel
    {
        [BsonRef(nameof(ViewModels.DBCollections.SalesMen))]
        public virtual SalesMan SalesMan { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}