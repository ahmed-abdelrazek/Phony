using MaterialDesignExtensions.Controls;

namespace Phony.WPF.Views
{
    /// <summary>
    /// Interaction logic for SalesBillsViewer.xaml
    /// </summary>
    public partial class SalesBillsViewerView : MaterialWindow
    {
        public SalesBillsViewerView()
        {
            InitializeComponent();
        }

        public SalesBillsViewerView(long billNo)
        {
            InitializeComponent();
            this.DataContext = new ViewModels.SalesBillsViewerViewModel(billNo);
        }
    }
}