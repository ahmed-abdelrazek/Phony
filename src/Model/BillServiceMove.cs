using LiteDB;

namespace Phony.Model
{
    public class BillServiceMove : BaseModel
    {
        [BsonRef(nameof(ViewModel.DBCollections.Bills))]
        public virtual Bill Bill { get; set; }

        [BsonRef(nameof(ViewModel.DBCollections.Services))]
        public virtual Service Service { get; set; }

        public decimal Balance { get; set; }

        public decimal ServicePayment { get; set; }

        public decimal Discount { get; set; }
    }
}
