using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Company : BaseModel
    {
        public Company()
        {
            Items = new ObservableCollection<Item>();
            CompaniesMoves = new ObservableCollection<CompanyMove>();
        }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public byte[] Image { get; set; }

        public string Site { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        
        public virtual ObservableCollection<Item> Items { get; set; }

        public virtual ObservableCollection<CompanyMove> CompaniesMoves { get; set; }
    }
}