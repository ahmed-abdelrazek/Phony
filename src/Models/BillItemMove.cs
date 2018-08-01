using LiteDB;

namespace Phony.Models
{
    public class BillItemMove : BaseModel
    {
        [BsonRef(nameof(Data.DBCollections.Bills))]
        public virtual Bill Bill { get; set; }

        [BsonRef(nameof(Data.DBCollections.Items))]
        public virtual Item Item { get; set; }

        public decimal ItemPrice { get; set; }

        public decimal QTY { get; set; }

        public decimal Discount { get; set; }
    }
}