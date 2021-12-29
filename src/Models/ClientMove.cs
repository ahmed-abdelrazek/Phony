using LiteDB;

namespace Phony.Models
{
    public class ClientMove : BaseModel
    {
        [BsonRef(nameof(Data.DBCollections.Clients))]
        public virtual Client Client { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}