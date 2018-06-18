using MahApps.Metro.Controls;
using Phony.ViewModel;
using System;
using System.Windows.Threading;

namespace Phony.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
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
            if (FrameWithinGrid.Source != MainWindowVM.CurrentSource)
            {
                FrameWithinGrid.Source = MainWindowVM.CurrentSource;
                if (FrameWithinGrid.Source == new Uri("Phony;component/Pages/Main.xaml", UriKind.Relative))
                {
                    if (WindowState != System.Windows.WindowState.Maximized)
                    {
                        WindowState = System.Windows.WindowState.Maximized;
                    }
                }
                else
                {
                    if (WindowState != System.Windows.WindowState.Normal)
                    { 
                        WindowState = System.Windows.WindowState.Normal;
                    }
                }
            }
        }

        private void metroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
