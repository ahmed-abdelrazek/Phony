using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Phony.Converters
{
    public class BinaryImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not null && value is byte[])
            {
                byte[] bytes = value as byte[];
                MemoryStream stream = new(bytes);
                BitmapImage image = new();
                image.BeginInit();
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }

            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}