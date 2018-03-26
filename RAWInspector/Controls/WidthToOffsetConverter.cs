using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RAWInspector.Controls
{
    internal sealed class WidthToOffsetConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int i1 && values[1] is int i2)
                return i1 * i2;

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}