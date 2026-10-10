using LiteDB;
using Phony.Data;
using Phony.Models;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class StoresViewModelTests : DatabaseTestBase
{
    [Fact]
    public void LoadsStoreAndRequiresNameToEdit()
    {
        using (var db = new LiteDatabase(_connection))
        {
            db.GetCollection<Store>(DBCollections.Stores).Update(new Store { Id = 1, Name = "shop", Phone1 = "123" });
        }

        UiThread.Run(() =>
        {
            var vm = new StoresViewModel();
            Assert.Equal("shop", vm.Name);
            Assert.Equal("123", vm.Phone1);
            Assert.True(vm.Edit.CanExecute(null));
            vm.Name = " ";
            Assert.False(vm.Edit.CanExecute(null));
        });
    }
}
