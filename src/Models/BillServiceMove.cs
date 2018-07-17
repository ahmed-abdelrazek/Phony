using LiteDB;

namespace Phony.Models
{
    public class BillServiceMove : BaseModel
    {
        [BsonRef(nameof(ViewModels.DBCollections.Bills))]
        public virtual Bill Bill { get; set; }

        [BsonRef(nameof(ViewModels.DBCollections.Services))]
        public virtual Service Service { get; set; }

        public decimal Balance { get; set; }

        public decimal ServicePayment { get; set; }

        public decimal Discount { get; set; }
    }
}
