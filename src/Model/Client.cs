using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Client : BaseModel
    {
        public Client()
        {
            Bills = new ObservableCollection<Bill>();
        }

        public string Name { get; set; }

        public decimal? Balance { get; set; }

        public virtual ObservableCollection<Bill> Bills { get; set; }
    }
}