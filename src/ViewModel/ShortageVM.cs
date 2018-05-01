using Phony.Kernel;
using Phony.Model;
using Phony.Persistence;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Phony.ViewModel
{
    public class ShortageVM : CommonBase
    {
        static string _itemsCount;

        ObservableCollection<Item> _items;

        public string ItemsCount
        {
            get => _itemsCount;
            set
            {
                if (value != _itemsCount)
                {
                    _itemsCount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<Item> Items
        {
            get => _items;
            set
            {
                if (value != _items)
                {
                    _items = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ShortageVM()
        {
            using (var db = new PhonyDbContext())
            {
                Items = new ObservableCollection<Item>(db.Items.Where(i => i.QTY <= 0));
            }
            new Thread(() =>
            {
                ItemsCount = $"إجمالى النواقص: {Items.Count().ToString()}";
                Thread.CurrentThread.Abort();
            }).Start();
        }
    }
}
