using Phony.Data;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests
{
    public sealed class MainPageVMTests : DatabaseTestBase
    {
        [Fact]
        public void DashboardPollingStopsAndCannotRestartAfterDisposal()
        {
            UiThread.Run(() =>
            {
                LiteDbContext.ConnectionString = _connection;
                var vm = new MainPageVM(_user);
                Assert.False(vm.IsPolling);
                vm.StartPolling();
                Assert.True(vm.IsPolling);
                vm.StopPolling();
                Assert.False(vm.IsPolling);
                vm.StartPolling();
                vm.Dispose();
                vm.StartPolling();
                Assert.False(vm.IsPolling);
            });
        }

    }
}
