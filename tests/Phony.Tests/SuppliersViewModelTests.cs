using Phony.Models;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class SuppliersViewModelTests : DatabaseTestBase
{
    [Fact]
    public void AddRequiresValidInput()
    {
        UiThread.Run(() =>
        {
            var vm = new SuppliersViewModel();
            Assert.False(vm.AddSupplier.CanExecute(null));
            vm.Name = "record";
            vm.SelectedSalesMan = 1;
            Assert.True(vm.AddSupplier.CanExecute(null));
            vm.Name = " ";
            Assert.False(vm.AddSupplier.CanExecute(null));
        });
    }

    [Fact]
    public void EditRequiresValidInputAndSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new SuppliersViewModel();
            Assert.False(vm.EditSupplier.CanExecute(null));
            vm.Name = "record";
            vm.SelectedSalesMan = 1;
            Assert.False(vm.EditSupplier.CanExecute(null));
            vm.DataGridSelectedSupplier = new Supplier { Id = 2, Name = "selected" };
            vm.FillUI.Execute(null);
            vm.SelectedSalesMan = 1;
            Assert.Equal(2, vm.SupplierId);
            Assert.Equal("selected", vm.Name);
            Assert.True(vm.EditSupplier.CanExecute(null));
            vm.DataGridSelectedSupplier = null;
            Assert.False(vm.EditSupplier.CanExecute(null));
        });
    }

    [Fact]
    public void DeleteRequiresSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new SuppliersViewModel();
            Assert.False(vm.DeleteSupplier.CanExecute(null));
            vm.DataGridSelectedSupplier = new Supplier { Id = 2, Name = "selected" };
            Assert.True(vm.DeleteSupplier.CanExecute(null));
            vm.DataGridSelectedSupplier = null;
            Assert.False(vm.DeleteSupplier.CanExecute(null));
        });
    }

    [Fact]
    public void SearchRejectsBlankInputAndFlyoutToggles()
    {
        UiThread.Run(() =>
        {
            var vm = new SuppliersViewModel();
            vm.SearchText = " ";
            Assert.False(vm.Search.CanExecute(null));
            vm.SearchText = "record";
            Assert.True(vm.Search.CanExecute(null));
            vm.OpenAddSupplierFlyout.Execute(null);
            Assert.True(vm.IsAddSupplierFlyoutOpen);
            vm.OpenAddSupplierFlyout.Execute(null);
            Assert.False(vm.IsAddSupplierFlyoutOpen);
        });
    }
    [Fact]
    public void DefaultRecordCannotBeDeleted()
    {
        UiThread.Run(() =>
        {
            var vm = new SuppliersViewModel();
            vm.DataGridSelectedSupplier = new Supplier { Id = 1 };
            Assert.False(vm.DeleteSupplier.CanExecute(null));
            vm.DataGridSelectedSupplier = new Supplier { Id = 2 };
            Assert.True(vm.DeleteSupplier.CanExecute(null));
        });
    }
}
