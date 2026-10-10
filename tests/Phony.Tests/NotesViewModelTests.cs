using Phony.Models;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class NotesViewModelTests : DatabaseTestBase
{
    [Fact]
    public void AddRequiresValidInput()
    {
        UiThread.Run(() =>
        {
            var vm = new NotesViewModel();
            Assert.False(vm.AddNo.CanExecute(null));
            vm.Name = "record";

            Assert.True(vm.AddNo.CanExecute(null));
            vm.Name = " ";
            Assert.False(vm.AddNo.CanExecute(null));
        });
    }

    [Fact]
    public void EditRequiresValidInputAndSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new NotesViewModel();
            Assert.False(vm.EditNo.CanExecute(null));
            vm.Name = "record";
            
            Assert.False(vm.EditNo.CanExecute(null));
            vm.DataGridSelectedNo = new Note { Id = 2, Name = "selected" };
            vm.FillUI.Execute(null);
            Assert.Equal(2, vm.NoId);
            Assert.Equal("selected", vm.Name);
            Assert.True(vm.EditNo.CanExecute(null));
            vm.DataGridSelectedNo = null;
            Assert.False(vm.EditNo.CanExecute(null));
        });
    }

    [Fact]
    public void DeleteRequiresSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new NotesViewModel();
            Assert.False(vm.DeleteNo.CanExecute(null));
            vm.DataGridSelectedNo = new Note { Id = 2, Name = "selected" };
            Assert.True(vm.DeleteNo.CanExecute(null));
            vm.DataGridSelectedNo = null;
            Assert.False(vm.DeleteNo.CanExecute(null));
        });
    }

    [Fact]
    public void SearchRejectsBlankInputAndFlyoutToggles()
    {
        UiThread.Run(() =>
        {
            var vm = new NotesViewModel();
            vm.SearchText = " ";
            Assert.False(vm.Search.CanExecute(null));
            vm.SearchText = "record";
            Assert.True(vm.Search.CanExecute(null));
            vm.OpenAddNoFlyout.Execute(null);
            Assert.True(vm.IsAddNoFlyoutOpen);
            vm.OpenAddNoFlyout.Execute(null);
            Assert.False(vm.IsAddNoFlyoutOpen);
        });
    }
}
