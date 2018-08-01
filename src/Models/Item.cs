using LiteDB;
using Phony.Data;

namespace Phony.Models
{
    public class Item : BaseModel
    {
        public string Name { get; set; }

        public string Barcode { get; set; }

        public string Shopcode { get; set; }

        public byte[] Image { get; set; }

        public ItemGroup Group { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal WholeSalePrice { get; set; }

        public decimal HalfWholeSalePrice { get; set; }

        public decimal RetailPrice { get; set; }

        public decimal QTY { get; set; }

        [BsonRef(nameof(DBCollections.Companies))]
        public Company Company { get; set; }

        [BsonRef(nameof(DBCollections.Suppliers))]
        public virtual Supplier Supplier { get; set; }
    }
}