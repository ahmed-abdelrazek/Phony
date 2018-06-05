using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Bill : BaseModel
    {
        public Bill()
        {
            BillsItemsMoves = new ObservableCollection<BillItemMove>();
            BillsServicesMoves = new ObservableCollection<BillServiceMove>();
        }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        public int StoreId { get; set; }

        public virtual Store Store { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAfterDiscounts { get; set; }

        public decimal TotalPayed { get; set; }

        public bool IsReturned { get; set; }

        public virtual ObservableCollection<BillItemMove> BillsItemsMoves { get; set; }

        public virtual ObservableCollection<BillServiceMove> BillsServicesMoves { get; set; }
    }
}