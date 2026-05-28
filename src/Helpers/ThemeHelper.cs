using MaterialDesignColors; // Required for Swatch and SwatchesProvider
using MaterialDesignThemes.Wpf; // Required for PaletteHelper, ITheme, BaseTheme
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Phony.Helpers
{
    public class ThemeHelper
    {
        public static IEnumerable<Swatch> Swatches { get; } = new SwatchesProvider().Swatches;

        public static void ApplyStyle(bool alternate)
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
        }

        public static void ApplyBase(bool isDark)
            => ModifyTheme(theme => theme.SetBaseTheme(isDark ? BaseTheme.Dark : BaseTheme.Light));

        public static void ApplyPrimary(Swatch swatch)
        {
            var primaryColor = swatch.PrimaryHues.FirstOrDefault()?.Color;
            if (primaryColor.HasValue)
            {
                ModifyTheme(theme => theme.SetPrimaryColor(primaryColor.Value));
            }
        }

        public static void ApplyAccent(Swatch swatch)
        {
            var secondaryColor = swatch.SecondaryHues.FirstOrDefault()?.Color;
            if (secondaryColor.HasValue)
            {
                ModifyTheme(theme => theme.SetSecondaryColor(secondaryColor.Value));
            }
        }

        public static void ModifyTheme(Action<Theme> modificationAction)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}
