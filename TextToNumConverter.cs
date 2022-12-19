using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Converters
{
    [ValueConversion(typeof(string), typeof(double))]
    public class TextToNumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            double res = double.NaN;
            if (double.TryParse(strValue, out res))
            {
                return res;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double num = (double)value;
            return num.ToString();
        }
    }
}
