using System.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Converts a collection count or integer to Visibility.
/// By default: count > 0 = Visible, count = 0 = Collapsed.
/// Set Invert to true to reverse the logic.
/// </summary>
public class CountToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets a value indicating whether to invert the conversion logic.
    /// </summary>
    public bool Invert { get; set; }

    /// <inheritdoc/>
    public object Convert(object? value, Type targetType, object parameter, string language)
    {
        var count = value switch
        {
            int i => i,
            ICollection collection => collection.Count,
            IEnumerable enumerable => enumerable.Cast<object>().Count(),
            _ => 0
        };

        var hasItems = count > 0;

        if (Invert)
        {
            hasItems = !hasItems;
        }

        return hasItems ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <inheritdoc/>
    public object? ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
