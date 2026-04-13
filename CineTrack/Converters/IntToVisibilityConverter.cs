using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CineTrack.Converters
{
    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            // Try to handle common numeric types and strings containing numbers
            if (value is int intVal)
            {
                return intVal > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            if (value is long longVal)
            {
                return longVal > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            if (value is short shortVal)
            {
                return shortVal > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            if (value is string s && int.TryParse(s, out var parsed))
            {
                return parsed > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            // Fallback: try to convert to int
            try
            {
                var converted = System.Convert.ToInt32(value);
                return converted > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}