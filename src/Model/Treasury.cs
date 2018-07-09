using LiteDB;

namespace Phony.Model
{
    public class Treasury : BaseModel
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        [BsonRef(nameof(ViewModel.DBCollections.Stores))]
        public virtual Store Store { get; set; }
    }
}