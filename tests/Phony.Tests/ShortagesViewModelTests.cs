using LiteDB;
using Phony.Data;
using Phony.Models;
using Phony.ViewModels;
using System.Linq;
using Xunit;

namespace Phony.Tests;

public sealed class ShortagesViewModelTests : DatabaseTestBase
{
    [Fact]
    public void LoadsOnlyItemsWithZeroOrNegativeStock()
    {
        using (var db = new LiteDatabase(_connection))
        {
            db.GetCollection<Item>(DBCollections.Items).Insert(new Item { Id = 2, QTY = 0 });
            db.GetCollection<Item>(DBCollections.Items).Insert(new Item { Id = 3, QTY = -1 });
        }
        UiThread.Run(() =>
        {
            var vm = new ShortagesViewModel();
            Assert.Equal([2, 3], vm.Items.Select(x => x.Id));
        });
    }
}
