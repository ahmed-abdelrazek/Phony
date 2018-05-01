using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Supplier : BaseModel
    {
        public Supplier()
        {
            Items = new ObservableCollection<Item>();
        }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public string Site { get; set; }

        public byte[] Image { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int SalesManId { get; set; }

        public virtual SalesMan SalesMan { get; set; }

        public virtual ObservableCollection<Item> Items { get; set; }
    }
}