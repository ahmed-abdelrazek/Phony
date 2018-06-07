using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Service : BaseModel
    {
        public Service()
        {
            BillsServicesMoves = new ObservableCollection<BillServiceMove>();
            ServicesMoves = new ObservableCollection<ServiceMove>();
        }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public byte[] Image { get; set; }

        public string Site { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        
        public virtual ObservableCollection<BillServiceMove> BillsServicesMoves { get; set; }

        public virtual ObservableCollection<ServiceMove> ServicesMoves { get; set; }
    }
}