using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class BillServiceMove : BaseModel
    {
        public BillServiceMove()
        {
            Bills = new ObservableCollection<Bill>();
        }
        public int BillId { get; set; }

        public virtual Bill Bill { get; set; }

        public int ServiceId { get; set; }

        public virtual Service Service { get; set; }

        public decimal ServicePayment { get; set; }

        public decimal Discount { get; set; }

        public virtual ObservableCollection<Bill> Bills { get; set; }
    }
}
