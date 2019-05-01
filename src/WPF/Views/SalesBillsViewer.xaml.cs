using MahApps.Metro.Controls;

namespace Phony.WPF.Views
{
    /// <summary>
    /// Interaction logic for SalesBillsViewer.xaml
    /// </summary>
    public partial class SalesBillsViewer : MetroWindow
    {
        public SalesBillsViewer()
        {
            InitializeComponent();
            //wb.BeginInit();
            //wb.EndInit();
            //wb. = "Reports\\SalesBillA8.html";
        }

        public SalesBillsViewer(long billNo)
        {
            InitializeComponent();
            this.DataContext = new ViewModels.SalesBillsViewerViewModel(billNo);
        }
    }
}