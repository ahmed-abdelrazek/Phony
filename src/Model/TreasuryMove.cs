using LiteDB;

namespace Phony.Model
{
    public class TreasuryMove : BaseModel
    {
        [BsonRef(nameof(ViewModel.DBCollections.TreasuriesMoves))]
        public virtual Treasury Treasury { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}