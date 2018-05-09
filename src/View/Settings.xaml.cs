using MahApps.Metro.Controls;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
        public Settings()
        {
            InitializeComponent();
        }
        public IEnumerable<Swatch> Swatches = new SwatchesProvider().Swatches;
        
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Theme == "BaseDark")
            {
                ThemeS.SelectedIndex = 1;
            }
            else if (Properties.Settings.Default.Theme == "BaseLight")
            {
                ThemeS.SelectedIndex = 0;
            }
            ThemeC.Text = Properties.Settings.Default.Color;
            foreach (var item in Swatches)
            {
                ComboBoxItem cbi1 = new ComboBoxItem();
                SolidColorBrush brush1 = new SolidColorBrush(item.ExemplarHue.Color);
                SolidColorBrush brush2 = new SolidColorBrush(item.ExemplarHue.Foreground);
                cbi1.Background = brush1;
                cbi1.Foreground = brush2;
                cbi1.Content = item.Name;
                ThemeC.Items.Add(cbi1);
                if (item.Name == Properties.Settings.Default.Color.ToLowerInvariant())
                {
                    ThemeC.SelectedItem = cbi1;
                }
            }
        }

        private void SaveB_Click(object sender, RoutedEventArgs e)
        {
            
            new PaletteHelper().ReplacePrimaryColor(ThemeC.Text);
            try
            {
                new PaletteHelper().ReplaceAccentColor(ThemeC.Text);
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
            Properties.Settings.Default.Color = ThemeC.Text;
            Properties.Settings.Default.Save();
        }
    }
}