using LiteDB;

namespace Phony.Models
{
    public class TreasuryMove : BaseModel
    {
        [BsonRef(nameof(ViewModels.DBCollections.TreasuriesMoves))]
        public virtual Treasury Treasury { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}