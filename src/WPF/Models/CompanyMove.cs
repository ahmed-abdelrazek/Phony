using LiteDB;

namespace Phony.WPF.Models
{
    public class CompanyMove : BaseModel
    {
        [BsonRef(nameof(Data.DBCollections.Companies))]
        public virtual Company Company { get; set; }

        public decimal Debit { get; set; }

        public decimal Credit { get; set; }
    }
}