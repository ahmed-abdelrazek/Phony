using LiteDB;

namespace Phony.Models
{
    public class BillServiceMove : BaseModel
    {
        [BsonRef(nameof(Data.DBCollections.Bills))]
        public virtual Bill Bill { get; set; }

        [BsonRef(nameof(Data.DBCollections.Services))]
        public virtual Service Service { get; set; }

        public decimal Balance { get; set; }

        public decimal Cost { get; set; }

        public decimal Discount { get; set; }
    }
}
