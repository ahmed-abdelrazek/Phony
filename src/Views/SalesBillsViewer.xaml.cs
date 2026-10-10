using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Web.WebView2.Core;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Phony.Views
{
    public partial class SalesBillsViewer : MetroWindow
    {
        private readonly ViewModels.SalesBillsViewerViewModel _viewModel;
        private bool _isLoading;
        private bool _isPrinting;
        private bool _isClosed;
        private long _pendingBillNo;
        private long _loadedBillNo;

        public SalesBillsViewer() : this(0) { }

        public SalesBillsViewer(long billNo)
        {
            InitializeComponent();
            DataContext = _viewModel = new ViewModels.SalesBillsViewerViewModel(billNo);
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            Loaded += Viewer_Loaded;
            Closed += (_, _) =>
            {
                _isClosed = true;
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            };
            UpdateButtons();
        }

        private bool HasSelectedBill => _viewModel.BillSelectedValue > 0
            && _viewModel.Bills.Any(b => b.Id == _viewModel.BillSelectedValue);

        private void UpdateButtons()
        {
            if (_isClosed) return;
            SearchButton.IsEnabled = HasSelectedBill && !_isLoading && !_isPrinting;
            PrintButton.IsEnabled = HasSelectedBill && !_isLoading && !_isPrinting
                && _loadedBillNo == _viewModel.BillSelectedValue && webView.CoreWebView2 != null;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.BillSelectedValue)
                || e.PropertyName == nameof(_viewModel.Bills))
            {
                _loadedBillNo = 0;
                UpdateButtons();
            }
        }

        private async void Viewer_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= Viewer_Loaded;
            if (HasSelectedBill) await LoadSelectedReportAsync();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadSelectedReportAsync();
        }

        private async Task LoadSelectedReportAsync()
        {
            if (!HasSelectedBill || _isLoading || _isPrinting || _isClosed) return;
            _isLoading = true;
            _loadedBillNo = 0;
            _pendingBillNo = 0;
            var selectedBillNo = _viewModel.BillSelectedValue;
            UpdateButtons();
            try
            {
                await _viewModel.LoadReportAsync(selectedBillNo);
                if (_isClosed) return;
                if (string.IsNullOrWhiteSpace(_viewModel.Report)
                    || selectedBillNo != _viewModel.BillSelectedValue)
                {
                    _isLoading = false;
                    UpdateButtons();
                    return;
                }
                await webView.EnsureCoreWebView2Async();
                if (_isClosed) return;
                if (selectedBillNo != _viewModel.BillSelectedValue)
                {
                    _isLoading = false;
                    UpdateButtons();
                    return;
                }
                _pendingBillNo = selectedBillNo;
                webView.NavigateToString(_viewModel.Report);
                // Print stays disabled until the browser finishes navigation.
            }
            catch (Exception)
            {
                _isLoading = false;
                UpdateButtons();
                if (!_isClosed)
                    await this.ShowMessageAsync("خطأ", "تعذر تحميل الفاتورة. تأكد من تثبيت WebView2 ثم حاول مرة أخرى.");
            }
        }

        private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (_pendingBillNo == 0) return;
            _loadedBillNo = e.IsSuccess ? _pendingBillNo : 0;
            _pendingBillNo = 0;
            _isLoading = false;
            UpdateButtons();
        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateButtons();
            if (!PrintButton.IsEnabled || webView.CoreWebView2 == null) return;
            _isPrinting = true;
            UpdateButtons();
            try
            {
                await webView.CoreWebView2.ExecuteScriptAsync("window.print();");
            }
            catch (Exception)
            {
                _loadedBillNo = 0;
                if (!_isClosed)
                    await this.ShowMessageAsync("خطأ", "تعذر طباعة الفاتورة. أعد تحميلها ثم حاول مرة أخرى.");
            }
            finally
            {
                _isPrinting = false;
                UpdateButtons();
            }
        }
    }
}
