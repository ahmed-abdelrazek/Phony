using LiteDB;

namespace Phony.Models
{
    public class BillItemMove : BaseModel
    {
        [BsonRef(nameof(ViewModels.DBCollections.Bills))]
        public virtual Bill Bill { get; set; }

        [BsonRef(nameof(ViewModels.DBCollections.Items))]
        public virtual Item Item { get; set; }

        public decimal ItemPrice { get; set; }

        public decimal QTY { get; set; }

        public decimal Discount { get; set; }
    }
}