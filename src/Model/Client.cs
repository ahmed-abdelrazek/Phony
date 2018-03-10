using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Client : BaseModel
    {

        public string Name { get; set; }

        public decimal? Balance { get; set; }

        public virtual ObservableCollection<Bill> Bills { get; set; }
    }
}