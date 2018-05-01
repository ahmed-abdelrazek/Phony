using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Bill : BaseModel
    {
        public Bill()
        {
            BillMoves = new ObservableCollection<BillMove>();
        }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public int StoreId { get; set; }

        public virtual Store Store { get; set; }

        public decimal Discount { get; set; }

        public virtual ObservableCollection<BillMove> BillMoves { get; set; }
    }
}