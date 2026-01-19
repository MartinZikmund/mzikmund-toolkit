using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Converts a null/non-null value to Visibility.
/// By default: non-null = Visible, null = Collapsed.
/// Set Invert to true to reverse the logic.
/// </summary>
public class NullToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets a value indicating whether to invert the conversion logic.
    /// </summary>
    public bool Invert { get; set; }

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object parameter, string language)
    {
        var isNotNull = value is not null;

        if (Invert)
        {
            isNotNull = !isNotNull;
        }

        return isNotNull ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <inheritdoc/>
    public object? ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
