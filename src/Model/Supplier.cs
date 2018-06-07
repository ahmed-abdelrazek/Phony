using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Supplier : BaseModel
    {
        public Supplier()
        {
            Items = new ObservableCollection<Item>();
            SuppliersMoves = new ObservableCollection<SupplierMove>();
        }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public byte[] Image { get; set; }

        public string Site { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public long SalesManId { get; set; }

        public virtual SalesMan SalesMan { get; set; }

        public virtual ObservableCollection<SupplierMove> SuppliersMoves { get; set; }

        public virtual ObservableCollection<Item> Items { get; set; }
    }
}