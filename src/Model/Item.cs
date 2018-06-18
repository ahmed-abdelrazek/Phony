using Phony.ViewModel;
using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Item : BaseModel
    {
        public Item()
        {
            BillsItemsMoves = new ObservableCollection<BillItemMove>();
        }

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

        public long CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public long SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual ObservableCollection<BillItemMove> BillsItemsMoves { get; set; }
    }
}