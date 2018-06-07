using System.Collections.ObjectModel;

namespace Phony.Model
{
    public class Treasury : BaseModel
    {
        public Treasury()
        {
            TreasuriesMoves = new ObservableCollection<TreasuryMove>();
        }

        public string Name { get; set; }

        public decimal Balance { get; set; }

        public long StoreId { get; set; }

        public virtual Store Store { get; set; }

        public virtual ObservableCollection<TreasuryMove> TreasuriesMoves { get; set; }
    }
}