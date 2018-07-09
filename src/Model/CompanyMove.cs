using LiteDB;

namespace Phony.Model
{
    public class CompanyMove : BaseModel
    {
        [BsonRef(nameof(ViewModel.DBCollections.Companies))]
        public virtual Company Company { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}