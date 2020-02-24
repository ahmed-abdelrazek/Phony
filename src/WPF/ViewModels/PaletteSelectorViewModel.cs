using Caliburn.Micro;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Phony.WPF.ViewModels
{
    public class PaletteSelectorViewModel : Screen
    {
        bool _alternate;
        bool _isDark;

        public bool Alternate
        {
            get => _alternate;
            set
            {
                _alternate = value;
                ApplyStyle(_alternate);
                NotifyOfPropertyChange(() => Alternate);
            }
        }

        public bool IsDark
        {
            get => _isDark;
            set
            {
                _isDark = value;
                ApplyBase(_isDark);
                NotifyOfPropertyChange(() => IsDark);
            }
        }

        public PaletteSelectorViewModel()
        {
            Swatches = new SwatchesProvider().Swatches;
            Alternate = Properties.Settings.Default.IsAlternateStyle;
            IsDark = Properties.Settings.Default.IsDarkTheme;
            ApplyStyle(Alternate);
        }

        public IEnumerable<Swatch> Swatches { get; }

        private static void ApplyStyle(bool alternate)
        {
            var resourceDictionary = new ResourceDictionary
            {
                Source = new Uri(@"pack://application:,,,/Dragablz;component/Themes/materialdesign.xaml")
            };

            var styleKey = alternate ? "MaterialDesignAlternateTabablzControlStyle" : "MaterialDesignTabablzControlStyle";
            var style = (Style)resourceDictionary[styleKey];

            foreach (var tabablzControl in Dragablz.TabablzControl.GetLoadedInstances())
            {
                tabablzControl.Style = style;
            }
            Properties.Settings.Default.IsAlternateStyle = alternate;
            Properties.Settings.Default.Save();
        }

        public static void ApplyBase(bool isDark)
        {
            ModifyTheme(theme => theme.SetBaseTheme(isDark ? Theme.Dark : Theme.Light));

            if (Properties.Settings.Default.IsDarkTheme != isDark)
            {
                Properties.Settings.Default.IsDarkTheme = isDark;
                Properties.Settings.Default.Save();
            }
        }

        public static void ApplyPrimary(Swatch swatch)
        {
            ModifyTheme(theme => theme.SetPrimaryColor(swatch.ExemplarHue.Color));

            if (Properties.Settings.Default.PrimaryColor != swatch.Name)
            {
                Properties.Settings.Default.PrimaryColor = swatch.Name;
                Properties.Settings.Default.Save();
            }
        }

        public static void ApplyAccent(Swatch swatch)
        {
            ModifyTheme(theme => theme.SetSecondaryColor(swatch.AccentExemplarHue.Color));

            if (Properties.Settings.Default.AccentColor != swatch.Name)
            {
                Properties.Settings.Default.AccentColor = swatch.Name;
                Properties.Settings.Default.Save();
            }
        }

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}