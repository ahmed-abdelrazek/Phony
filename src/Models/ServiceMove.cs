using LiteDB;

namespace Phony.Models
{
    public class ServiceMove : BaseModel
    {
        [BsonRef(nameof(Data.DBCollections.Services))]
        public virtual Service Service { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}