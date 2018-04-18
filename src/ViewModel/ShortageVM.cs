using Phony.Kernel;
using Phony.Model;
using Phony.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                    RaisePropertyChanged(nameof(ItemsCount));
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
                    RaisePropertyChanged(nameof(Items));
                }
            }
        }

        public ShortageVM()
        {
            using (var db = new PhonyDbContext())
            {
                Items = new ObservableCollection<Item>(db.Items.Where(i => i.QTY > 0));
            }
            new Thread(() =>
            {
                ItemsCount = $"إجمالى النواقص: {Items.Count().ToString()}";
                Thread.CurrentThread.Abort();
            }).Start();
        }
    }
}
