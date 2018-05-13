using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Client : BaseModel
    {
        public Client()
        {
            Bills = new ObservableCollection<Bill>();
            ClientsMoves = new ObservableCollection<ClientMove>();
        }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public string Site { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public virtual ObservableCollection<Bill> Bills { get; set; }

        public virtual ObservableCollection<ClientMove> ClientsMoves { get; set; }
    }
}