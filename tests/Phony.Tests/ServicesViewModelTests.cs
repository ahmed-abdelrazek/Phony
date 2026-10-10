using Phony.Models;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class ServicesViewModelTests : DatabaseTestBase
{
    [Fact]
    public void AddRequiresValidInput()
    {
        UiThread.Run(() =>
        {
            var vm = new ServicesViewModel();
            Assert.False(vm.AddService.CanExecute(null));
            vm.Name = "record";

            Assert.True(vm.AddService.CanExecute(null));
            vm.Name = " ";
            Assert.False(vm.AddService.CanExecute(null));
        });
    }

    [Fact]
    public void EditRequiresValidInputAndSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new ServicesViewModel();
            Assert.False(vm.EditService.CanExecute(null));
            vm.Name = "record";

            Assert.False(vm.EditService.CanExecute(null));
            vm.DataGridSelectedService = new Service { Id = 2, Name = "selected" };
            vm.FillUI.Execute(null);
            Assert.Equal(2, vm.ServiceId);
            Assert.Equal("selected", vm.Name);
            Assert.True(vm.EditService.CanExecute(null));
            vm.DataGridSelectedService = null;
            Assert.False(vm.EditService.CanExecute(null));
        });
    }

    [Fact]
    public void DeleteRequiresSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new ServicesViewModel();
            Assert.False(vm.DeleteService.CanExecute(null));
            vm.DataGridSelectedService = new Service { Id = 2, Name = "selected" };
            Assert.True(vm.DeleteService.CanExecute(null));
            vm.DataGridSelectedService = null;
            Assert.False(vm.DeleteService.CanExecute(null));
        });
    }

    [Fact]
    public void SearchRejectsBlankInputAndFlyoutToggles()
    {
        UiThread.Run(() =>
        {
            var vm = new ServicesViewModel();
            vm.SearchText = " ";
            Assert.False(vm.Search.CanExecute(null));
            vm.SearchText = "record";
            Assert.True(vm.Search.CanExecute(null));
            vm.OpenAddServiceFlyout.Execute(null);
            Assert.True(vm.IsAddServiceFlyoutOpen);
            vm.OpenAddServiceFlyout.Execute(null);
            Assert.False(vm.IsAddServiceFlyoutOpen);
        });
    }
    [Fact]
    public void DefaultRecordCannotBeDeleted()
    {
        UiThread.Run(() =>
        {
            var vm = new ServicesViewModel();
            vm.DataGridSelectedService = new Service { Id = 1 };
            Assert.False(vm.DeleteService.CanExecute(null));
            vm.DataGridSelectedService = new Service { Id = 2 };
            Assert.True(vm.DeleteService.CanExecute(null));
        });
    }
}
