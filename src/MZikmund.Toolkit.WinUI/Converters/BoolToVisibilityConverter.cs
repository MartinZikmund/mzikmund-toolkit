using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Converts a boolean value to a Visibility value.
/// Set Invert to true to reverse the logic (false = Visible, true = Collapsed).
/// </summary>
public class BoolToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets a value indicating whether to invert the conversion logic.
    /// </summary>
    public bool Invert { get; set; }

    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var boolValue = value is bool b && b;

        if (Invert)
        {
            boolValue = !boolValue;
        }

        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        var visibility = value is Visibility v ? v : Visibility.Collapsed;
        var result = visibility == Visibility.Visible;

        return Invert ? !result : result;
    }
}
