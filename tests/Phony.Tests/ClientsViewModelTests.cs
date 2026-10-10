using Phony.DTOs;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class ClientsViewModelTests : DatabaseTestBase
{
    [Fact]
    public void AddRequiresValidInput()
    {
        UiThread.Run(() =>
        {
            var vm = new ClientsViewModel();
            Assert.False(vm.AddClient.CanExecute(null));
            vm.Name = "record";

            Assert.True(vm.AddClient.CanExecute(null));
            vm.Name = " ";
            Assert.False(vm.AddClient.CanExecute(null));
        });
    }

    [Fact]
    public void EditRequiresValidInputAndSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new ClientsViewModel();
            Assert.False(vm.EditClient.CanExecute(null));
            vm.Name = "record";
            
            Assert.False(vm.EditClient.CanExecute(null));
            vm.DataGridSelectedClient = new ClientDto { Id = 2, Name = "selected" };
            vm.FillUI.Execute(null);
            Assert.Equal(2, vm.ClientId);
            Assert.Equal("selected", vm.Name);
            Assert.True(vm.EditClient.CanExecute(null));
            vm.DataGridSelectedClient = null;
            Assert.False(vm.EditClient.CanExecute(null));
        });
    }

    [Fact]
    public void DeleteRequiresSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new ClientsViewModel();
            Assert.False(vm.DeleteClient.CanExecute(null));
            vm.DataGridSelectedClient = new ClientDto { Id = 2, Name = "selected" };
            Assert.True(vm.DeleteClient.CanExecute(null));
            vm.DataGridSelectedClient = null;
            Assert.False(vm.DeleteClient.CanExecute(null));
        });
    }

    [Fact]
    public void SearchRejectsBlankInputAndFlyoutToggles()
    {
        UiThread.Run(() =>
        {
            var vm = new ClientsViewModel();
            vm.SearchText = " ";
            Assert.False(vm.Search.CanExecute(null));
            vm.SearchText = "record";
            Assert.True(vm.Search.CanExecute(null));
            vm.OpenAddClientFlyout.Execute(null);
            Assert.True(vm.IsAddClientFlyoutOpen);
            vm.OpenAddClientFlyout.Execute(null);
            Assert.False(vm.IsAddClientFlyoutOpen);
        });
    }
    [Fact]
    public void DefaultRecordCannotBeDeleted()
    {
        UiThread.Run(() =>
        {
            var vm = new ClientsViewModel();
            vm.DataGridSelectedClient = new ClientDto { Id = 1 };
            Assert.False(vm.DeleteClient.CanExecute(null));
            vm.DataGridSelectedClient = new ClientDto { Id = 2 };
            Assert.True(vm.DeleteClient.CanExecute(null));
        });
    }
}
