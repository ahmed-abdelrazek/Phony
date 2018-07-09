using LiteDB;

namespace Phony.Model
{
    public class Bill : BaseModel
    {
        [BsonRef(nameof(ViewModel.DBCollections.Clients))]
        public virtual Client Client { get; set; }

        [BsonRef(nameof(ViewModel.DBCollections.Stores))]
        public virtual Store Store { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAfterDiscounts { get; set; }

        public decimal TotalPayed { get; set; }

        public bool IsReturned { get; set; }
    }
}