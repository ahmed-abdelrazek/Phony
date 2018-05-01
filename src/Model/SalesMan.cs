using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class SalesMan : BaseModel
    {
        public SalesMan()
        {
            Suppliers = new ObservableCollection<Supplier>();
        }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public string Site { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public virtual ObservableCollection<Supplier> Suppliers { get; set; }
    }
}