using System;
using System.Globalization;
using System.Windows.Data;

namespace PRN212_Project
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal amount)
                return amount.ToString("C", CultureInfo.GetCultureInfo("vi-VN"));
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (decimal.TryParse(value?.ToString().Replace("₫", "").Trim(), NumberStyles.Currency, CultureInfo.GetCultureInfo("vi-VN"), out decimal result))
                return result;
            return 0m;
        }
    }
}