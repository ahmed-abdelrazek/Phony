using MaterialDesignColors;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Phony.ViewModels
{
    public class PaletteSelectorViewModel : BindableBase
    {
        bool _alternate;
        bool _isDark;

        public bool Alternate
        {
            get => _alternate;
            set => SetProperty(ref _alternate, value);
        }

        public bool IsDark
        {
            get => _isDark;
            set => SetProperty(ref _isDark, value);
        }

        public PaletteSelectorViewModel()
        {
            Swatches = new SwatchesProvider().Swatches;
            Alternate = Properties.Settings.Default.IsAlternateStyle;
            IsDark = Properties.Settings.Default.IsDarkTheme;
            ApplyStyle(Alternate);
        }

        public ICommand ToggleStyleCommand { get; } = new DelegateCommand<object>(o => ApplyStyle((bool)o));

        public ICommand ToggleBaseCommand { get; } = new DelegateCommand<object>(o => ApplyBase((bool)o));

        public IEnumerable<Swatch> Swatches { get; }

        public ICommand ApplyPrimaryCommand { get; } = new DelegateCommand<object>(o => ApplyPrimary((Swatch)o));

        public ICommand ApplyAccentCommand { get; } = new DelegateCommand<object>(o => ApplyAccent((Swatch)o));

        private static void ApplyStyle(bool alternate)
        {
            Helpers.ThemeHelper.ApplyStyle(alternate);
            Properties.Settings.Default.IsAlternateStyle = alternate;
            Properties.Settings.Default.Save();
        }

        private static void ApplyBase(bool isDark)
        {
            Helpers.ThemeHelper.ApplyBase(isDark);
            Properties.Settings.Default.IsDarkTheme = isDark;
            Properties.Settings.Default.Save();
        }

        private static void ApplyPrimary(Swatch swatch)
        {
            Helpers.ThemeHelper.ApplyPrimary(swatch);
            Properties.Settings.Default.PrimaryColor = swatch.Name;
            Properties.Settings.Default.Save();
        }

        private static void ApplyAccent(Swatch swatch)
        {
            Helpers.ThemeHelper.ApplyAccent(swatch);
            Properties.Settings.Default.AccentColor = swatch.Name;
            Properties.Settings.Default.Save();
        }
    }
}