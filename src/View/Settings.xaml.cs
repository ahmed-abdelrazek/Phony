using MahApps.Metro;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using Phony.Kernel;
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
            if (Properties.Settings.Default.Theme == "BaseLight")
            {
                new PaletteHelper().SetLightDark(false);
            }
            else
            {
                new PaletteHelper().SetLightDark(true);
            }
            try
            {
                ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(Properties.Settings.Default.Color), ThemeManager.GetAppTheme(Properties.Settings.Default.Theme));
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
            }
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
                case "Emerald":
                    {
                        ThemeC.SelectedIndex = 6;
                        break;
                    }
                case "Teal":
                    {
                        ThemeC.SelectedIndex = 7;
                        break;
                    }
                case "Cyan":
                    {
                        ThemeC.SelectedIndex = 8;
                        break;
                    }
                case "Cobalt":
                    {
                        ThemeC.SelectedIndex = 9;
                        break;
                    }
                case "Indigo":
                    {
                        ThemeC.SelectedIndex = 10;
                        break;
                    }
                case "Violet":
                    {
                        ThemeC.SelectedIndex = 11;
                        break;
                    }
                case "Pink":
                    {
                        ThemeC.SelectedIndex = 12;
                        break;
                    }
                case "Magenta":
                    {
                        ThemeC.SelectedIndex = 13;
                        break;
                    }
                case "Crimson":
                    {
                        ThemeC.SelectedIndex = 14;
                        break;
                    }
                case "Amber":
                    {
                        ThemeC.SelectedIndex = 15;
                        break;
                    }
                case "Yellow":
                    {
                        ThemeC.SelectedIndex = 16;
                        break;
                    }
                case "Brown":
                    {
                        ThemeC.SelectedIndex = 17;
                        break;
                    }
                case "Olive":
                    {
                        ThemeC.SelectedIndex = 18;
                        break;
                    }
                case "Steel":
                    {
                        ThemeC.SelectedIndex = 19;
                        break;
                    }
                case "Mauve":
                    {
                        ThemeC.SelectedIndex = 20;
                        break;
                    }
                case "Taupe":
                    {
                        ThemeC.SelectedIndex = 21;
                        break;
                    }
                case "Sienna":
                    {
                        ThemeC.SelectedIndex = 22;
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
