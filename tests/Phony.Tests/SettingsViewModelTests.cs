using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class SettingsViewModelTests
{
    [Fact]
    public void SavingRequiresPathOrDefaultAndImportRequiresSqlCredentials()
    {
        UiThread.Run(() =>
        {
            var settings = Phony.Properties.Settings.Default;
            var configured = settings.IsConfigured;
            try
            {
                settings.IsConfigured = false;
                var vm = new SettingsViewModel { LiteUseDefault = false, LiteDbFullPath = " " };
                Assert.False(vm.Save.CanExecute(null));
                vm.LiteUseDefault = true;
                Assert.True(vm.Save.CanExecute(null));
                vm.LiteUseDefault = false;
                vm.LiteDbFullPath = "test.db";
                Assert.True(vm.Save.CanExecute(null));
                vm.SQLUseDefault = false;
                Assert.False(vm.MoveData.CanExecute(null));
                vm.SQLServerName = "server";
                vm.SQLDataBase = "database";
                vm.SQLIsSQLAuth = true;
                Assert.False(vm.MoveData.CanExecute(null));
                vm.SQLUserName = "user";
                vm.SQLPassword = "password";
                Assert.True(vm.MoveData.CanExecute(null));
                settings.IsConfigured = true;
                Assert.False(vm.MoveData.CanExecute(null));
            }
            finally { settings.IsConfigured = configured; }
        });
    }
}
