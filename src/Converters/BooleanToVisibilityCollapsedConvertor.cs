using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Phony.Converters
{
    public class BooleanToVisibilityCollapsedConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value == true ? Visibility.Collapsed : (object)Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}