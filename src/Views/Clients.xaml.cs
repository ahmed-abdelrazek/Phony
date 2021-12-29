using MahApps.Metro.Controls;

namespace Phony.Views
{
    /// <summary>
    /// Interaction logic for Clients.xaml
    /// </summary>
    public partial class Clients : MetroWindow
    {
        public Clients()
        {
            InitializeComponent();
            DataContext = new ViewModels.ClientsViewModel();
        }
    }
}
