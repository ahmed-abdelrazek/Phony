using LiteDB;

namespace Phony.Models
{
    public class Bill : BaseModel
    {
        [BsonRef(nameof(ViewModels.DBCollections.Clients))]
        public virtual Client Client { get; set; }

        [BsonRef(nameof(ViewModels.DBCollections.Stores))]
        public virtual Store Store { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAfterDiscounts { get; set; }

        public decimal TotalPayed { get; set; }

        public bool IsReturned { get; set; }
    }
}