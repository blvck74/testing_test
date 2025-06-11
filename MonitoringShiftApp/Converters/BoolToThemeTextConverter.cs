using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace MonitoringShiftApp.Converters;

public class BoolToThemeTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isDark)
        {
            return isDark ? "🌙 Темная" : "☀️ Светлая";
        }
        return "☀️ Светлая";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}