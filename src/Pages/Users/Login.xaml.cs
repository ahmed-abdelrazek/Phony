using System.Windows;
using System.Windows.Controls;

namespace Phony.Pages.Users
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (GridDX.DataContext != null)
            {
                ((ViewModel.Users.LoginVM)GridDX.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword;
            }
        }
    }
}