using Caliburn.Micro;
using LiteDB;
using Phony.WPF.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Phony.WPF.ViewModels
{
    public class ShortagesViewModel : Screen
    {
        static string _itemsCount;

        ObservableCollection<Item> _items;

        public string ItemsCount
        {
            get => _itemsCount;
            set
            {
                _itemsCount = value;
                NotifyOfPropertyChange(() => ItemsCount);
            }
        }

        public ObservableCollection<Item> Items
        {
            get => _items;
            set
            {
                _items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        public ShortagesViewModel()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
            {
                Items = new ObservableCollection<Item>(db.GetCollection<Item>(Data.DBCollections.Items).Find(i => i.QTY <= 0));
            }
            new Thread(() =>
            {
                ItemsCount = $"إجمالى النواقص: {Items.Count().ToString()}";
            }).Start();
        }
    }
}
