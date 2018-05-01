using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Store : BaseModel
    {
        public Store()
        {
            Bills = new ObservableCollection<Bill>();
        }
        public string Name { get; set; }

        public byte[] Image { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Tel1 { get; set; }

        public string Tel2 { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Email1 { get; set; }

        public string Email2 { get; set; }

        public string Site { get; set; }

        public virtual ObservableCollection<Bill> Bills { get; set; }
    }
}