using System;
using System.Globalization;
using System.Windows.Data;

namespace Phony.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return targetType != typeof(bool) ? throw new InvalidOperationException("The target must be a boolean") : !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}