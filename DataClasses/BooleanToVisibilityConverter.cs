using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TimeZoneHelper.DataClasses
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                return null;
            }

            var val = (bool) value;

            if (val)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
