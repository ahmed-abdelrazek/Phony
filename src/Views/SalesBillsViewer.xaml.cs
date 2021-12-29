using MahApps.Metro.Controls;

namespace Phony.Views
{
    /// <summary>
    /// Interaction logic for SalesBillsViewer.xaml
    /// </summary>
    public partial class SalesBillsViewer : MetroWindow
    {
        private ViewModels.SalesBillsViewerViewModel _viewModel;

        public SalesBillsViewer()
        {
            InitializeComponent();
            this.DataContext = _viewModel = new ViewModels.SalesBillsViewerViewModel(0);
        }

        public SalesBillsViewer(long billNo)
        {
            InitializeComponent();
            this.DataContext = _viewModel = new ViewModels.SalesBillsViewerViewModel(billNo);
        }

        private async void SearchButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_viewModel.BillSelectedValue > 0)
            {
                await _viewModel.LoadReportAsync(_viewModel.BillSelectedValue);
                await webView.EnsureCoreWebView2Async(null);
                webView.NavigateToString(_viewModel.Report);
            }
        }

        private async void PrintButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await webView.CoreWebView2.ExecuteScriptAsync("window.print();");
        }
    }
}