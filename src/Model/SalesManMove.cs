using LiteDB;

namespace Phony.Model
{
    public class SalesManMove : BaseModel
    {
        [BsonRef(nameof(ViewModel.DBCollections.SalesMen))]
        public virtual SalesMan SalesMan { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}