using Phony.ViewModels;
using Xunit;

namespace Phony.Tests;

public sealed class MainWindowViewModelTests
{
    [Fact]
    public void NavigationChangesSourceAndIgnoresBlankPages()
    {
        UiThread.Run(() =>
        {
            var vm = new MainWindowViewModel();
            var previousPage = vm.PageName;
            var previousSource = MainWindowViewModel.CurrentSource;
            try
            {
                vm.PageName = "Main";
                Assert.Equal("/PhonyLite;component/Views/Pages/Main.xaml", MainWindowViewModel.CurrentSource.OriginalString);
                vm.NavigateToPage(" ");
                Assert.EndsWith("Main.xaml", MainWindowViewModel.CurrentSource.OriginalString);
                Assert.True(vm.ChangeSource.CanExecute(null));
            }
            finally { vm.PageName = previousPage; MainWindowViewModel.CurrentSource = previousSource; }
        });
    }
}
