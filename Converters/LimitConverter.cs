using System;
using System.Globalization;
using System.Windows.Data;

namespace PRN212_Project.Converters
{
    public class LimitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is decimal currentSpent && parameter != null)
                {
                    if (decimal.TryParse(parameter.ToString(), out decimal monthlyLimit) && monthlyLimit > 0)
                    {
                        double percentage = (double)currentSpent / (double)monthlyLimit;
                        System.Diagnostics.Debug.WriteLine($"Convert: CurrentSpent={currentSpent}, MonthlyLimit={monthlyLimit}, Percentage={percentage}");
                        if (percentage >= 1.0)
                        {
                            return "Red";
                        }
                        else if (percentage >= 0.8)
                        {
                            return "Yellow";
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Convert: Invalid MonthlyLimit={parameter}");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Convert: Invalid value={value}, parameter={parameter}");
                }
                return "Default";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Convert Error: {ex.Message}");
                return "Default";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}