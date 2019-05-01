using LiteDB;

namespace Phony.WPF.Models
{
    public class Supplier : BaseModel
    {
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public byte[] Image { get; set; }

        public string Site { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [BsonRef(nameof(Data.DBCollections.SalesMen))]
        public virtual SalesMan SalesMan { get; set; }
    }
}