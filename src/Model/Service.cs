using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Service : BaseModel
    {

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public string Site { get; set; }

        public byte[] Image { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public virtual ObservableCollection<Bill> Bills { get; set; }

        public virtual ObservableCollection<BillMove> BillMoves { get; set; }
    }
}