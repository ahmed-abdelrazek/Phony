using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using TinyLittleMvvm;

namespace Phony.WPF.ViewModels
{
    public class PaletteSelectorViewModel : BaseViewModelWithAnnotationValidation
    {
        bool _isDark;

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

        public IEnumerable<Swatch> Swatches { get; }

        public ICommand ApplyPrimaryCommand { get; }

        public ICommand ApplyAccentCommand { get; }

        public PaletteSelectorViewModel()
        {
            Swatches = new SwatchesProvider().Swatches;
            IsDark = Properties.Settings.Default.IsDarkTheme;

            ApplyPrimaryCommand = new RelayCommand<Swatch>(ApplyPrimary);
            ApplyAccentCommand = new RelayCommand<Swatch>(ApplyAccent);
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