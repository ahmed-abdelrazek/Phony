using Phony.DTOs;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class CompaniesViewModelTests : DatabaseTestBase
{
    [Fact]
    public void AddRequiresValidInput()
    {
        UiThread.Run(() =>
        {
            var vm = new CompaniesViewModel();
            Assert.False(vm.AddCompany.CanExecute(null));
            vm.Name = "record";

            Assert.True(vm.AddCompany.CanExecute(null));
            vm.Name = " ";
            Assert.False(vm.AddCompany.CanExecute(null));
        });
    }

    [Fact]
    public void EditRequiresValidInputAndSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new CompaniesViewModel();
            Assert.False(vm.EditCompany.CanExecute(null));
            vm.Name = "record";
            
            Assert.False(vm.EditCompany.CanExecute(null));
            vm.DataGridSelectedCompany = new CompanyDto { Id = 2, Name = "selected" };
            vm.FillUI.Execute(null);
            Assert.Equal(2, vm.CompanyId);
            Assert.Equal("selected", vm.Name);
            Assert.True(vm.EditCompany.CanExecute(null));
            vm.DataGridSelectedCompany = null;
            Assert.False(vm.EditCompany.CanExecute(null));
        });
    }

    [Fact]
    public void DeleteRequiresSelection()
    {
        UiThread.Run(() =>
        {
            var vm = new CompaniesViewModel();
            Assert.False(vm.DeleteCompany.CanExecute(null));
            vm.DataGridSelectedCompany = new CompanyDto { Id = 2, Name = "selected" };
            Assert.True(vm.DeleteCompany.CanExecute(null));
            vm.DataGridSelectedCompany = null;
            Assert.False(vm.DeleteCompany.CanExecute(null));
        });
    }

    [Fact]
    public void SearchRejectsBlankInputAndFlyoutToggles()
    {
        UiThread.Run(() =>
        {
            var vm = new CompaniesViewModel();
            vm.SearchText = " ";
            Assert.False(vm.Search.CanExecute(null));
            vm.SearchText = "record";
            Assert.True(vm.Search.CanExecute(null));
            vm.OpenAddCompanyFlyout.Execute(null);
            Assert.True(vm.IsAddCompanyFlyoutOpen);
            vm.OpenAddCompanyFlyout.Execute(null);
            Assert.False(vm.IsAddCompanyFlyoutOpen);
        });
    }
    [Fact]
    public void DefaultRecordCannotBeDeleted()
    {
        UiThread.Run(() =>
        {
            var vm = new CompaniesViewModel();
            vm.DataGridSelectedCompany = new CompanyDto { Id = 1 };
            Assert.False(vm.DeleteCompany.CanExecute(null));
            vm.DataGridSelectedCompany = new CompanyDto { Id = 2 };
            Assert.True(vm.DeleteCompany.CanExecute(null));
        });
    }
}
