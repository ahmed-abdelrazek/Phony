using LiteDB;

namespace Phony.Model
{
    public class BillItemMove : BaseModel
    {
        [BsonRef(nameof(ViewModel.DBCollections.Bills))]
        public virtual Bill Bill { get; set; }

        [BsonRef(nameof(ViewModel.DBCollections.Items))]
        public virtual Item Item { get; set; }

        public decimal ItemPrice { get; set; }

        public decimal QTY { get; set; }

        public decimal Discount { get; set; }
    }
}