using Microsoft.UI.Xaml.Data;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Formats an integer as a zero-padded two-digit string ("05", "12", "59").
/// Useful for clock, countdown, and timer displays where each component needs
/// fixed width.
/// </summary>
public sealed class IntToTimeComponentStringConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var number = value switch
        {
            int i => i,
            long l => (int)l,
            short s => s,
            byte b => b,
            _ => System.Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture),
        };

        return number.ToString("00", System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotSupportedException($"{nameof(IntToTimeComponentStringConverter)} only supports one-way binding.");
}
