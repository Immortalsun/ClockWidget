using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace TimeZoneHelper
{
    public class MultiBooleanToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(
            object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(t => (bool?) t ?? false))
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(
            object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
