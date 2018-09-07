using MahApps.Metro.Controls;

namespace Phony.Views
{
    /// <summary>
    /// Interaction logic for SalesBillsViewer.xaml
    /// </summary>
    public partial class SalesBillsViewer : MetroWindow
    {
        public SalesBillsViewer()
        {
            InitializeComponent();
        }

        public SalesBillsViewer(long billNo)
        {
            InitializeComponent();
            this.DataContext = new ViewModels.SalesBillsViewerViewModel(billNo);
        }
    }
}