using System.Windows.Controls;

namespace Phony.Views.Pages
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        public Main()
        {
            InitializeComponent();
            Loaded += (_, _) => (DataContext as ViewModels.MainPageVM)?.StartPolling();
            Unloaded += (_, _) => (DataContext as ViewModels.MainPageVM)?.StopPolling();
        }
    }
}
