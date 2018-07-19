using LiteDB;

namespace Phony.Models
{
    public class TreasuryMove : BaseModel
    {
        [BsonRef(nameof(Data.DBCollections.TreasuriesMoves))]
        public virtual Treasury Treasury { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}