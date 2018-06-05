using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class BillItemMove : BaseModel
    {
        public BillItemMove()
        {
            Bills = new ObservableCollection<Bill>();
        }
        public int BillId { get; set; }

        public virtual Bill Bill { get; set; }

        public int ItemId { get; set; }

        public virtual Item Item { get; set; }

        public decimal QTY { get; set; }

        public decimal Discount { get; set; }

        public virtual ObservableCollection<Bill> Bills { get; set; }
    }
}