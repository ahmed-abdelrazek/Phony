using Phony.Models;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class UsersViewModelTests : DatabaseTestBase
{
    [Fact]
    public void AddRequiresValidInput()
    {
        UiThread.Run(() =>
        {
            var vm = new UsersViewModel();
            Assert.False(vm.AddUser.CanExecute(null));
            vm.Name = "record";
            vm.Password1 = "password"; vm.Password2 = "password";
            Assert.True(vm.AddUser.CanExecute(null));
            vm.Name = " ";
            Assert.False(vm.AddUser.CanExecute(null));
        });
    }

    [Fact]
    public void EditRequiresValidInputAndSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new UsersViewModel();
            Assert.False(vm.EditUser.CanExecute(null));
            vm.Name = "record";
            vm.Password1 = "password"; vm.Password2 = "password";
            Assert.False(vm.EditUser.CanExecute(null));
            vm.DataGridSelectedUser = new User { Id = 2, Name = "selected" };
            vm.FillUI.Execute(null);
            Assert.Equal(2, vm.UserId);
            Assert.Equal("selected", vm.Name);
            Assert.True(vm.EditUser.CanExecute(null));
            vm.DataGridSelectedUser = null;
            Assert.False(vm.EditUser.CanExecute(null));
        });
    }

    [Fact]
    public void DeleteRequiresSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new UsersViewModel();
            Assert.False(vm.DeleteUser.CanExecute(null));
            vm.DataGridSelectedUser = new User { Id = 2, Name = "selected" };
            Assert.True(vm.DeleteUser.CanExecute(null));
            vm.DataGridSelectedUser = null;
            Assert.False(vm.DeleteUser.CanExecute(null));
        });
    }

    [Fact]
    public void SearchRejectsBlankInputAndFlyoutToggles()
    {
        UiThread.Run(() =>
        {
            var vm = new UsersViewModel();
            vm.SearchText = " ";
            Assert.False(vm.Search.CanExecute(null));
            vm.SearchText = "record";
            Assert.True(vm.Search.CanExecute(null));
            vm.OpenAddUserFlyout.Execute(null);
            Assert.True(vm.IsAddUserFlyoutOpen);
            vm.OpenAddUserFlyout.Execute(null);
            Assert.False(vm.IsAddUserFlyoutOpen);
        });
    }
    [Fact]
    public void DefaultRecordCannotBeDeleted()
    {
        UiThread.Run(() =>
        {
            var vm = new UsersViewModel();
            vm.DataGridSelectedUser = new User { Id = 1 };
            Assert.False(vm.DeleteUser.CanExecute(null));
            vm.DataGridSelectedUser = new User { Id = 2 };
            Assert.True(vm.DeleteUser.CanExecute(null));
        });
    }
}
