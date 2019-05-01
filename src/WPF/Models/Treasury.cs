using LiteDB;

namespace Phony.WPF.Models
{
    public class Treasury : BaseModel
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        [BsonRef(nameof(Data.DBCollections.Stores))]
        public virtual Store Store { get; set; }
    }
}