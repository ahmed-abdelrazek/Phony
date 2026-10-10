using Phony.Data;
using Phony.Models;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class CardsViewModelTests : DatabaseTestBase
{
    [Fact]
    public void LoadsOnlyCardsFromDatabase()
    {
        using (var db = new LiteDB.LiteDatabase(_connection))
        {
            db.GetCollection<Item>(DBCollections.Items).Insert(new Item { Id = 2, Group = ItemGroup.Card });
            db.GetCollection<Item>(DBCollections.Items).Insert(new Item { Id = 3, Group = ItemGroup.Other });
        }
        UiThread.Run(() =>
        {
            var vm = new CardsViewModel();
            Assert.Equal(2, Assert.Single(vm.Cards).Id);
        });
    }

    [Fact]
    public void AddRequiresValidInput()
    {
        UiThread.Run(() =>
        {
            var vm = new CardsViewModel();
            Assert.False(vm.AddCard.CanExecute(null));
            vm.Name = "record";
            vm.SelectedCompanyValue = 1; vm.SelectedSupplierValue = 1;
            Assert.True(vm.AddCard.CanExecute(null));
            vm.Name = " ";
            Assert.False(vm.AddCard.CanExecute(null));
        });
    }

    [Fact]
    public void EditRequiresValidInputAndSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new CardsViewModel();
            Assert.False(vm.EditCard.CanExecute(null));
            vm.Name = "record";
            vm.SelectedCompanyValue = 1; vm.SelectedSupplierValue = 1;
            Assert.False(vm.EditCard.CanExecute(null));
            vm.DataGridSelectedItem = new Item { Id = 2, Name = "selected" };
            vm.FillUI.Execute(null);
            vm.SelectedCompanyValue = 1; vm.SelectedSupplierValue = 1;
            Assert.Equal(2, vm.CardId);
            Assert.Equal("selected", vm.Name);
            Assert.True(vm.EditCard.CanExecute(null));
            vm.DataGridSelectedItem = null;
            Assert.False(vm.EditCard.CanExecute(null));
        });
    }

    [Fact]
    public void DeleteRequiresSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new CardsViewModel();
            Assert.False(vm.DeleteCard.CanExecute(null));
            vm.DataGridSelectedItem = new Item { Id = 2, Name = "selected" };
            Assert.True(vm.DeleteCard.CanExecute(null));
            vm.DataGridSelectedItem = null;
            Assert.False(vm.DeleteCard.CanExecute(null));
        });
    }

    [Fact]
    public void SearchRejectsBlankInputAndFlyoutToggles()
    {
        UiThread.Run(() =>
        {
            var vm = new CardsViewModel();
            vm.SearchText = " ";
            Assert.False(vm.Search.CanExecute(null));
            vm.SearchText = "record";
            Assert.True(vm.Search.CanExecute(null));
            vm.OpenAddCardFlyout.Execute(null);
            Assert.True(vm.IsAddCardFlyoutOpen);
            vm.OpenAddCardFlyout.Execute(null);
            Assert.False(vm.IsAddCardFlyoutOpen);
        });
    }
}

