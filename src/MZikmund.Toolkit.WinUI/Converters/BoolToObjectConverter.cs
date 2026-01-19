using Microsoft.UI.Xaml.Data;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Converts a boolean value to one of two specified objects.
/// </summary>
public class BoolToObjectConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets the value to return when the boolean is true.
    /// </summary>
    public object? TrueValue { get; set; }

    /// <summary>
    /// Gets or sets the value to return when the boolean is false.
    /// </summary>
    public object? FalseValue { get; set; }

    /// <inheritdoc/>
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        var boolValue = value is bool b && b;
        return boolValue ? TrueValue : FalseValue;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value?.Equals(TrueValue) == true)
        {
            return true;
        }

        if (value?.Equals(FalseValue) == true)
        {
            return false;
        }

        return false;
    }
}
