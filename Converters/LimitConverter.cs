using System;
using System.Globalization;
using System.Windows.Data;

namespace PRN212_Project
{
    public class LimitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal currentSpent && parameter is decimal limit)
            {
                if (currentSpent >= limit) return "Red";
                if (currentSpent >= limit * 0.8m) return "Yellow";
                return "Green";
            }
            return "Green";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}