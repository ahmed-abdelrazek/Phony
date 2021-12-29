using MahApps.Metro.Controls;

namespace Phony.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : MetroWindow
    {
        public Settings(int i)
        {
            InitializeComponent();
            SettingsTabControl.SelectedIndex = i;
        }
    }
}