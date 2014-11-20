using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TimeZoneHelper
{
    public class BooleanToVisibilityConverter : IMultiValueConverter
    {


        public object Convert(
            object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            if (!(values[0] is bool) || !(values[1] is bool)
               || !(values[2] is bool))
            {
                return null;
            }
            var val1 = (bool)values[0];
            var val2 = (bool) values[1];
            var val3 = (bool) values[2];
            if (val1 || val2 || val3)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object[] ConvertBack(
            object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
