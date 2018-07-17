using LiteDB;

namespace Phony.Models
{
    public class Treasury : BaseModel
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        [BsonRef(nameof(ViewModels.DBCollections.Stores))]
        public virtual Store Store { get; set; }
    }
}