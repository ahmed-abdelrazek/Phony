using LiteDB;

namespace Phony.Model
{
    public class ServiceMove : BaseModel
    {
        [BsonRef(nameof(ViewModel.DBCollections.Services))]
        public virtual Service Service { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}