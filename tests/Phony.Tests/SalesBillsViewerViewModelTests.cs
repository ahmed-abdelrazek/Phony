using Phony.Data;
using Phony.ViewModels;
using System.Threading.Tasks;
using Xunit;

namespace Phony.Tests
{
    public sealed class SalesBillsViewerViewModelTests : DatabaseTestBase
    {
        [Theory]
        [InlineData("A4")]
        [InlineData("A8")]
        public async Task ChangingInvoiceSelectionDisablesReturningThePreviousInvoice(string paperSize)
        {
            var first = Save();
            var second = Save();
            await UiThread.RunAsync(async () =>
            {
                LiteDbContext.ConnectionString = _connection;
                Phony.Properties.Settings.Default.SalesBillsPaperSize = paperSize;
                var vm = new SalesBillsViewerViewModel(first);
                await vm.LoadReportAsync(first);
                var date = System.Text.RegularExpressions.Regex.Match(vm.Report, "<bdi dir=\"ltr\">([^<]+)</bdi>").Groups[1].Value;
                Assert.NotEmpty(date);
                Assert.Contains(date, c => c >= '\u0660' && c <= '\u0669');
                Assert.DoesNotContain(date, c => c >= '0' && c <= '9');
                vm.IsReturned = true;
                Assert.True(vm.SaveReturned.CanExecute(null));
                vm.BillSelectedValue = second;
                Assert.Equal(0, vm.LoadedBillId);
                Assert.False(vm.IsReturned);
                Assert.False(vm.SaveReturned.CanExecute(null));
                // Checking the toggle without loading the new invoice is insufficient.
                vm.IsReturned = true;
                Assert.False(vm.SaveReturned.CanExecute(null));
            });
        }

    }
}
