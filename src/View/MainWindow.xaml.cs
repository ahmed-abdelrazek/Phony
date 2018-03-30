using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Threading;

namespace Phony.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        ViewModel.MainWindowVM v = new ViewModel.MainWindowVM();
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            Timer.Interval = TimeSpan.FromMilliseconds(200);
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (FrameWithinGrid.Source != v.CurrentSource)
            {
                FrameWithinGrid.Source = v.CurrentSource;
            }
        }

        private void SettingsW_Click(object sender, RoutedEventArgs e)
        {
            Settings set = new Settings();
            set.ShowDialog();
        }

        private void metroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
