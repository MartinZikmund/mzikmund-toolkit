using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Converts a string to Visibility based on whether it's empty or not.
/// By default: non-empty = Visible, empty/null = Collapsed.
/// Set Invert to true to reverse the logic.
/// </summary>
public class StringToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets a value indicating whether to invert the conversion logic.
    /// </summary>
    public bool Invert { get; set; }

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object parameter, string language)
    {
        var hasValue = !string.IsNullOrEmpty(value as string);

        if (Invert)
        {
            hasValue = !hasValue;
        }

        return hasValue ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <inheritdoc/>
    public object? ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
