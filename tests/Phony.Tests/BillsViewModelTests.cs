using Phony.Data;
using Phony.Models;
using Phony.ViewModels;
using Xunit;

namespace Phony.Tests
{
    public sealed class BillsViewModelTests : DatabaseTestBase
    {
        [Theory]
        [InlineData(-1, 0, false)]
        [InlineData(0, -1, false)]
        [InlineData(0, 101, false)]
        [InlineData(0, 0, true)]
        [InlineData(100, 100, true)]
        public void SavingRequiresClientLinesAndValidPaymentAndDiscount(decimal payment, decimal discount, bool enabled)
        {
            UiThread.Run(() =>
            {
                var vm = new BillsViewModel();
                Assert.False(vm.SaveBill.CanExecute(null));
                vm.SelectedClient = new Client { Id = 2 };
                Assert.False(vm.SaveBill.CanExecute(null));
                vm.BillItemsMoves.Add(Item());
                vm.BillClientPayment = payment;
                vm.BillDiscount = discount;
                Assert.Equal(enabled, vm.SaveBill.CanExecute(null));
                Assert.Equal(enabled, vm.SaveAndShow.CanExecute(null));
                vm.SelectedClient = null;
                Assert.False(vm.SaveBill.CanExecute(null));
            });
        }

        [Fact]
        public void ChangeRefreshesWhenTotalsOrDiscountsChange()
        {
            UiThread.Run(() =>
            {
                LiteDbContext.ConnectionString = _connection;
                var vm = new BillsViewModel { BillTotalAfterEachDiscount = 100, BillTotalAfterDiscount = 100, BillClientPayment = 100 };
                vm.BillDiscount = 10;
                Assert.Equal(10, vm.BillClientPaymentChange);
                vm.BillTotalAfterDiscount = 120;
                Assert.Equal(0, vm.BillClientPaymentChange);
                vm.BillTotalAfterDiscount = 40;
                Assert.Equal(60, vm.BillClientPaymentChange);
            });
        }

    }
}
