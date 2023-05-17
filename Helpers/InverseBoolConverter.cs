using System.Globalization;
using System.Windows.Data;
using System;

namespace Sales.Helpers;

[ValueConversion(typeof(bool), typeof(bool))]
public class InverseBoolConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        bool booleanValue = (bool)value;
        return !booleanValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        bool booleanValue = (bool)value;
        return !booleanValue;
    }
}