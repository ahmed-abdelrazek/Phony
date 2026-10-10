using Phony.Data;
using Phony.DTOs;
using Phony.Models;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class ItemVMTests : DatabaseTestBase
{
    [Fact]
    public void LoadsOnlyOrdinaryItemsFromDatabase()
    {
        using (var db = new LiteDB.LiteDatabase(_connection))
        {
            db.GetCollection<Item>(DBCollections.Items).Insert(new Item { Id = 2, Group = ItemGroup.Card });
            db.GetCollection<Item>(DBCollections.Items).Insert(new Item { Id = 3, Group = ItemGroup.Other });
        }
        UiThread.Run(() =>
        {
            var vm = new ItemVM();
            Assert.Equal(3, Assert.Single(vm.Items).Id);
        });
    }

    [Fact]
    public void AddRequiresValidInput()
    {
        UiThread.Run(() =>
        {
            var vm = new ItemVM();
            Assert.False(vm.AddItem.CanExecute(null));
            vm.Name = "record";
            vm.SelectedCompanyValue = 1; vm.SelectedSupplierValue = 1;
            Assert.True(vm.AddItem.CanExecute(null));
            vm.Name = " ";
            Assert.False(vm.AddItem.CanExecute(null));
        });
    }

    [Fact]
    public void EditRequiresValidInputAndSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new ItemVM();
            Assert.False(vm.EditItem.CanExecute(null));
            vm.Name = "record";
            vm.SelectedCompanyValue = 1; vm.SelectedSupplierValue = 1;
            Assert.False(vm.EditItem.CanExecute(null));
            vm.DataGridSelectedItem = new ItemDto { Id = 2, Name = "selected" };
            vm.FillUI.Execute(null);
            vm.SelectedCompanyValue = 1; vm.SelectedSupplierValue = 1;
            Assert.Equal(2, vm.ItemId);
            Assert.Equal("selected", vm.Name);
            Assert.True(vm.EditItem.CanExecute(null));
            vm.DataGridSelectedItem = null;
            Assert.False(vm.EditItem.CanExecute(null));
        });
    }

    [Fact]
    public void DeleteRequiresSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new ItemVM();
            Assert.False(vm.DeleteItem.CanExecute(null));
            vm.DataGridSelectedItem = new ItemDto { Id = 2, Name = "selected" };
            Assert.True(vm.DeleteItem.CanExecute(null));
            vm.DataGridSelectedItem = null;
            Assert.False(vm.DeleteItem.CanExecute(null));
        });
    }

    [Fact]
    public void SearchRejectsBlankInputAndFlyoutToggles()
    {
        UiThread.Run(() =>
        {
            var vm = new ItemVM();
            vm.SearchText = " ";
            Assert.False(vm.Search.CanExecute(null));
            vm.SearchText = "record";
            Assert.True(vm.Search.CanExecute(null));
            vm.OpenAddItemFlyout.Execute(null);
            Assert.True(vm.IsAddItemFlyoutOpen);
            vm.OpenAddItemFlyout.Execute(null);
            Assert.False(vm.IsAddItemFlyoutOpen);
        });
    }
}

