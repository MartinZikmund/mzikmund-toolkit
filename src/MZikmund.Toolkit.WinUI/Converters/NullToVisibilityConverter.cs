using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Maps an arbitrary object to a <see cref="Visibility"/>:
/// non-null → <see cref="Visibility.Visible"/>, null → <see cref="Visibility.Collapsed"/>.
/// Set <see cref="Invert"/> to swap that behavior.
/// </summary>
public sealed class NullToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// When <see langword="true"/>, null values become <see cref="Visibility.Visible"/>
    /// and non-null values become <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public bool Invert { get; set; }

    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var hasValue = value is not null;
        var visible = Invert ? !hasValue : hasValue;
        return visible ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotSupportedException($"{nameof(NullToVisibilityConverter)} only supports one-way binding.");
}
