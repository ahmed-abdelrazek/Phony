using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Company : BaseModel
    {
        public Company()
        {
            Bills = new ObservableCollection<Bill>();
            Items = new ObservableCollection<Item>();
        }

        public string Name { get; set; }

        public decimal? Balance { get; set; }

        public string Site { get; set; }

        public byte[] Image { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public virtual ObservableCollection<Bill> Bills { get; set; }

        public virtual ObservableCollection<Item> Items { get; set; }
    }
}