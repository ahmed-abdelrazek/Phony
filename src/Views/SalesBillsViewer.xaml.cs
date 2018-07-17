using System.Windows;

namespace Phony.Views
{
    /// <summary>
    /// Interaction logic for SalesBillsViewer.xaml
    /// </summary>
    public partial class SalesBillsViewer : Window
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