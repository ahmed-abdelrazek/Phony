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
            if (FrameWithinGrid.Source != ViewModel.MainWindowVM.CurrentSource)
            {
                FrameWithinGrid.Source = ViewModel.MainWindowVM.CurrentSource;
            }
        }

        private void SettingsW_Click(object sender, RoutedEventArgs e)
        {
            Settings set = new Settings();
            set.ShowDialog();
        }

        private void IncomeAddB_Click(object sender, RoutedEventArgs e)
        {
            //using (var db = new PhonyDbContext())
            //{
            //    // Get customer collection
            //    var Incomes = db.GetCollection<IncomeM>("Incomes");
            //    // Create your new customer instance
            //    var Income = new IncomeM
            //    {
            //        Name = IncomeNameTb.Text,
            //        Balance = uint.Parse(IncomeBalanceTb.Text),
            //        PeriodType = IncomePeriodTypeCb.SelectedIndex,
            //        Month = DateTime.Now.Month,
            //        Year = DateTime.Now.Year,
            //        CreateDate = DateTime.Now,
            //        IsActive = true
            //    };
            //    // Insert new customer document (Id will be auto-incremented)
            //    Incomes.Insert(Income);
            //    IncomesListDGV.Items.Refresh();
            //    // Update a document inside a collection
            //    //Income.Name = "Joana Doe";
            //    //Incomes.Update(Income);
            //    // Index document using a document property
            //    //Incomes.EnsureIndex(x => x.Name);
            //}
        }
    }
}
