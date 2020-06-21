using LiteDB;
using Phony.Data.Models.Lite;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TinyLittleMvvm;

namespace Phony.WPF.ViewModels
{
    public class ShortagesViewModel : BaseViewModelWithAnnotationValidation, IOnLoadedHandler
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
            Title = "النواقص";
        }

        public async Task OnLoadedAsync()
        {
            await Task.Run(() =>
            {
                using (var db = new LiteDatabase(Properties.Settings.Default.LiteDbConnectionString))
                {
                    Items = new ObservableCollection<Item>(db.GetCollection<Item>(DBCollections.Items).Find(i => i.QTY <= 0));
                    ItemsCount = $"إجمالى النواقص: {Items.Count()}";
                }
            });
        }
    }
}
