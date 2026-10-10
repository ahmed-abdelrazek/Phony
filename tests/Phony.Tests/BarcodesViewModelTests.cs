using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class BarcodesViewModelTests
{
    [Fact]
    public void EncodingRequiresValueAlignmentAndEncoderAndSavingRequiresEncodedValue()
    {
        UiThread.Run(() =>
        {
            var vm = new BarcodesViewModel();
            Assert.False(vm.Encode.CanExecute(null));
            Assert.False(vm.Save.CanExecute(null));
            vm.EncodeValue = "123456";
            vm.Alignment = "center";
            Assert.True(vm.Encode.CanExecute(null));
            vm.SelectedEncoder = " ";
            Assert.False(vm.Encode.CanExecute(null));
            vm.EncodedValue = "123456";
            Assert.True(vm.Save.CanExecute(null));
        });
    }
}
