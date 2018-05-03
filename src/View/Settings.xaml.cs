using MahApps.Metro;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;

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

        public void ChangeAppStyle()
        {
            new PaletteHelper().ReplacePrimaryColor(Properties.Settings.Default.Color);
            new PaletteHelper().ReplaceAccentColor(Properties.Settings.Default.Color);
            if (Properties.Settings.Default.Theme == "BaseLight")
            {
                new PaletteHelper().SetLightDark(false);
            }
            else
            {
                new PaletteHelper().SetLightDark(true);
            }
            ThemeManager.ChangeAppTheme(Application.Current, Properties.Settings.Default.Theme);
        }

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
            switch (Properties.Settings.Default.Color)
            {
                case "Red":
                    {
                        ThemeC.SelectedIndex = 0;
                        break;
                    }
                case "Green":
                    {
                        ThemeC.SelectedIndex = 1;
                        break;
                    }
                case "Blue":
                    {
                        ThemeC.SelectedIndex = 2;
                        break;
                    }
                case "Purple":
                    {
                        ThemeC.SelectedIndex = 3;
                        break;
                    }
                case "Orange":
                    {
                        ThemeC.SelectedIndex = 4;
                        break;
                    }
                case "Lime":
                    {
                        ThemeC.SelectedIndex = 5;
                        break;
                    }
                case "Teal":
                    {
                        ThemeC.SelectedIndex = 6;
                        break;
                    }
                case "Cyan":
                    {
                        ThemeC.SelectedIndex = 7;
                        break;
                    }
                case "Indigo":
                    {
                        ThemeC.SelectedIndex = 8;
                        break;
                    }
                case "Pink":
                    {
                        ThemeC.SelectedIndex = 9;
                        break;
                    }
                case "Amber":
                    {
                        ThemeC.SelectedIndex = 10;
                        break;
                    }
                case "Yellow":
                    {
                        ThemeC.SelectedIndex = 11;
                        break;
                    }
                case "Brown":
                    {
                        ThemeC.SelectedIndex = 12;
                        break;
                    }
                default:
                    {
                        ThemeC.SelectedIndex = 0;
                        break;
                    }
            }
        }

        private void SaveB_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeS.SelectedIndex == 0)
            {
                Properties.Settings.Default.Theme = "BaseLight";
            }
            else
            {
                Properties.Settings.Default.Theme = "BaseDark";
            }
            Properties.Settings.Default.Color = ThemeC.Text;
            Properties.Settings.Default.Save();
            ChangeAppStyle();
        }

    }
}