using LiteDB;

namespace Phony.Models
{
    public class ClientMove : BaseModel
    {
        [BsonRef(nameof(ViewModels.DBCollections.Clients))]
        public virtual Client Client { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}