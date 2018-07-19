using LiteDB;
using Phony.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace Phony.ViewModels
{
    public class ShortagesViewModel : BindableBase
    {
        static string _itemsCount;

        ObservableCollection<Item> _items;

        public string ItemsCount
        {
            get => _itemsCount;
            set => SetProperty(ref _itemsCount, value);
        }

        public ObservableCollection<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public ShortagesViewModel()
        {
            using (var db = new LiteDatabase(Properties.Settings.Default.DBFullName))
            {
                Items = new ObservableCollection<Item>(db.GetCollection<Item>(Data.DBCollections.Items.ToString()).Find(i => i.QTY <= 0));
            }
            new Thread(() =>
            {
                ItemsCount = $"إجمالى النواقص: {Items.Count().ToString()}";
            }).Start();
        }
    }
}
