using LiteDB;

namespace Phony.Model
{
    public class ClientMove : BaseModel
    {
        [BsonRef(nameof(ViewModel.DBCollections.Clients))]
        public virtual Client Client { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}