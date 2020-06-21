using Phony.Data.Core;

namespace Phony.Data.Models.Lite
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

        public virtual Company Company { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}