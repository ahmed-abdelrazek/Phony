using Phony.DTOs;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class SalesMenViewModelTests : DatabaseTestBase
{
    [Fact]
    public void AddRequiresValidInput()
    {
        UiThread.Run(() =>
        {
            var vm = new SalesMenViewModel();
            Assert.False(vm.AddSalesMan.CanExecute(null));
            vm.Name = "record";

            Assert.True(vm.AddSalesMan.CanExecute(null));
            vm.Name = " ";
            Assert.False(vm.AddSalesMan.CanExecute(null));
        });
    }

    [Fact]
    public void EditRequiresValidInputAndSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new SalesMenViewModel();
            Assert.False(vm.EditSalesMan.CanExecute(null));
            vm.Name = "record";
            
            Assert.False(vm.EditSalesMan.CanExecute(null));
            vm.DataGridSelectedSalesMan = new SalesManDto { Id = 2, Name = "selected" };
            vm.FillUI.Execute(null);
            Assert.Equal(2, vm.SalesManId);
            Assert.Equal("selected", vm.Name);
            Assert.True(vm.EditSalesMan.CanExecute(null));
            vm.DataGridSelectedSalesMan = null;
            Assert.False(vm.EditSalesMan.CanExecute(null));
        });
    }

    [Fact]
    public void DeleteRequiresSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new SalesMenViewModel();
            Assert.False(vm.DeleteSalesMan.CanExecute(null));
            vm.DataGridSelectedSalesMan = new SalesManDto { Id = 2, Name = "selected" };
            Assert.True(vm.DeleteSalesMan.CanExecute(null));
            vm.DataGridSelectedSalesMan = null;
            Assert.False(vm.DeleteSalesMan.CanExecute(null));
        });
    }

    [Fact]
    public void SearchRejectsBlankInputAndFlyoutToggles()
    {
        UiThread.Run(() =>
        {
            var vm = new SalesMenViewModel();
            vm.SearchText = " ";
            Assert.False(vm.Search.CanExecute(null));
            vm.SearchText = "record";
            Assert.True(vm.Search.CanExecute(null));
            vm.OpenAddSalesManFlyout.Execute(null);
            Assert.True(vm.IsAddSalesManFlyoutOpen);
            vm.OpenAddSalesManFlyout.Execute(null);
            Assert.False(vm.IsAddSalesManFlyoutOpen);
        });
    }
    [Fact]
    public void DefaultRecordCannotBeDeleted()
    {
        UiThread.Run(() =>
        {
            var vm = new SalesMenViewModel();
            vm.DataGridSelectedSalesMan = new SalesManDto { Id = 1 };
            Assert.False(vm.DeleteSalesMan.CanExecute(null));
            vm.DataGridSelectedSalesMan = new SalesManDto { Id = 2 };
            Assert.True(vm.DeleteSalesMan.CanExecute(null));
        });
    }
}
