using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircleControl
{
    public class WidthToCenterPoint : IValueConverter 
    {
        object IValueConverter.Convert(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            double  width = (Double)value;
            if (width != null) return new Point(width / 2, width / 2); ;
            return Binding.DoNothing;
        }
        object IValueConverter.ConvertBack(object value, Type targetType,
                                           object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
