using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Phony.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Phony.View
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

        public IEnumerable<Swatch> Swatches = new SwatchesProvider().Swatches;

        SqlConnectionStringBuilder ClientConnectionStringBuilder = new SqlConnectionStringBuilder();

        void FillConnectionString()
        {
            ClientConnectionStringBuilder.ConnectionString = null;

            if ((bool)UseLocalDefaultCheckBox.IsChecked)
            {
                ClientConnectionStringBuilder.DataSource = ".\\SQLExpress";
                ClientConnectionStringBuilder.InitialCatalog = "PhonyDb";
                ClientConnectionStringBuilder.IntegratedSecurity = true;
                ClientConnectionStringBuilder.MultipleActiveResultSets = true;
            }
            else
            {
                ClientConnectionStringBuilder.DataSource = ClientServerTextBox.Text;
                ClientConnectionStringBuilder.InitialCatalog = ClientDataBaseTextBox.Text;
                if ((bool)ClientWinAuthRadioButton.IsChecked)
                {
                    ClientConnectionStringBuilder.IntegratedSecurity = true;
                }
                else
                {
                    ClientConnectionStringBuilder.UserID = ClientUsernameTextBox.Text;
                    ClientConnectionStringBuilder.Password = ClientPasswordTextBox.Text;
                }
                ClientConnectionStringBuilder.MultipleActiveResultSets = true;
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Theme == "BaseDark")
            {
                ThemeS.SelectedIndex = 1;
            }
            else
            {
                ThemeS.SelectedIndex = 0;
            }
            if (Properties.Settings.Default.SalesBillsPaperSize == "A4")
            {
                BillReportPaperSizeCb.SelectedIndex = 0;
            }
            else
            {
                BillReportPaperSizeCb.SelectedIndex = 1;
            }
            ThemePC.Text = Properties.Settings.Default.PrimaryColor;
            foreach (var item in Swatches)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                SolidColorBrush brush1 = new SolidColorBrush(item.ExemplarHue.Color);
                SolidColorBrush brush2 = new SolidColorBrush(item.ExemplarHue.Foreground);
                cbi.Background = brush1;
                cbi.Foreground = brush2;
                cbi.Content = item.Name;
                ThemePC.Items.Add(cbi);
                if (item.Name == Properties.Settings.Default.PrimaryColor.ToLowerInvariant())
                {
                    ThemePC.SelectedItem = cbi;
                }
            }
            ThemeAC.Text = Properties.Settings.Default.AccentColor;
            foreach (var item in Swatches)
            {
                if (item.AccentExemplarHue == null)
                {
                    continue;
                }
                ComboBoxItem cbi = new ComboBoxItem();
                SolidColorBrush brush1 = new SolidColorBrush(item.AccentExemplarHue.Color);
                SolidColorBrush brush2 = new SolidColorBrush(item.AccentExemplarHue.Foreground);
                cbi.Background = brush1;
                cbi.Foreground = brush2;
                cbi.Content = item.Name;
                ThemeAC.Items.Add(cbi);
                if (item.Name == Properties.Settings.Default.AccentColor.ToLowerInvariant())
                {
                    ThemeAC.SelectedItem = cbi;
                }
            }
            if (!string.IsNullOrWhiteSpace(ClientConnectionStringBuilder.ConnectionString))
            {
                if (ClientConnectionStringBuilder.DataSource == ".\\SQLExpress" && ClientConnectionStringBuilder.InitialCatalog == "PhonyDb" && ClientConnectionStringBuilder.IntegratedSecurity == true)
                {
                    UseLocalDefaultCheckBox.IsChecked = true;
                }
                else
                {
                    UseLocalDefaultCheckBox.IsChecked = false;
                }
                if (ClientConnectionStringBuilder.IntegratedSecurity == true)
                {
                    ClientWinAuthRadioButton.IsChecked = true;
                }
                else
                {
                    ClientSQLAuthRadioButton.IsChecked = true;
                }
                ClientServerTextBox.Text = ClientConnectionStringBuilder.DataSource;
                ClientDataBaseTextBox.Text = ClientConnectionStringBuilder.InitialCatalog;
                ClientUsernameTextBox.Text = ClientConnectionStringBuilder.UserID;
                ClientPasswordTextBox.Text = ClientConnectionStringBuilder.Password;
            }
        }

        private async void SaveB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new PaletteHelper().ReplacePrimaryColor(ThemePC.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            try
            {
                new PaletteHelper().ReplaceAccentColor(ThemeAC.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (ThemeS.SelectedIndex == 0)
            {
                Properties.Settings.Default.Theme = "BaseLight";
                new PaletteHelper().SetLightDark(false);
            }
            else
            {
                Properties.Settings.Default.Theme = "BaseDark";
                new PaletteHelper().SetLightDark(true);
            }
            if (!(bool)UseLocalDefaultCheckBox.IsChecked)
            {
                if (string.IsNullOrWhiteSpace(ClientServerTextBox.Text))
                {
                    await this.ShowMessageAsync("تحذير", "من فضلك ادخل بيانات السيرفر");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(ClientDataBaseTextBox.Text))
                {
                    await this.ShowMessageAsync("تحذير", "من فضلك ادخل اسم قاعدة البيانات");
                    return;
                }
                else if ((bool)ClientSQLAuthRadioButton.IsChecked)
                {
                    if (string.IsNullOrWhiteSpace(ClientUsernameTextBox.Text))
                    {
                        await this.ShowMessageAsync("تحذير", "من فضلك ادخل اسم مستخدم قاعدة البيانات");
                        return;
                    }
                    else if (string.IsNullOrWhiteSpace(ClientPasswordTextBox.Text))
                    {
                        await this.ShowMessageAsync("تحذير", "من فضلك ادخل كلمة مرور مستخدم قاعدة البيانات");
                        return;
                    }
                }
            }
            FillConnectionString();
            Properties.Settings.Default.PrimaryColor = ThemePC.Text;
            Properties.Settings.Default.AccentColor = ThemeAC.Text;
            Properties.Settings.Default.SalesBillsPaperSize = BillReportPaperSizeCb.Text;
            Properties.Settings.Default.ConnectionString = ClientConnectionStringBuilder.ConnectionString;
            Properties.Settings.Default.Save();
            if (!Properties.Settings.Default.IsConfigured)
            {
                try
                {
                    using (var db = new PhonyDbContext())
                    {
                        var i = await db.Items.FirstOrDefaultAsync();
                    }
                    Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhonyDbContext, Migrations.Configuration>());
                    Properties.Settings.Default.IsConfigured = true;
                    Properties.Settings.Default.Save();
                }
                catch (Exception ex)
                {
                    Kernel.Core.SaveException(ex);
                    BespokeFusion.MaterialMessageBox.ShowError("هناك مشكله فى الاتصال بقاعدة البيانات");
                }
                Close();
            }
            await this.ShowMessageAsync("تم الحفظ", "لقد تم تغيير اعدادات البرنامج و حفظها بنجاح");
        }
    }
}