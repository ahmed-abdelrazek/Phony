using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Bill : BaseModel
    {

        public int? ClientId { get; set; }

        public virtual Client Client { get; set; }

        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public int? ServiceId { get; set; }

        public virtual Service Service { get; set; }

        public decimal? Discount { get; set; }

        public virtual ObservableCollection<BillMove> BillMoves { get; set; }
    }
}