using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Bill : BaseModel
    {
        public Bill()
        {
            BillsMoves = new ObservableCollection<BillMove>();
        }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        public int StoreId { get; set; }

        public virtual Store Store { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAfterDiscounts { get; set; }

        public decimal TotalPayed { get; set; }

        public virtual ObservableCollection<BillMove> BillsMoves { get; set; }
    }
}