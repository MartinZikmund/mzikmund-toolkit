using Microsoft.UI.Xaml.Data;
using MZikmund.Toolkit.WinUI.Localization;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Resolves an enum value to a localized string using the convention
/// <c>{EnumType}_{EnumName}</c>, looked up through <see cref="Localizer.Current"/>.
/// </summary>
/// <remarks>
/// Example: a <c>OrderStatus.Pending</c> value is resolved against the key
/// <c>OrderStatus_Pending</c>. Apps can replace <see cref="Localizer.Current"/> at startup
/// with their own implementation backed by <c>IStringLocalizer</c>.
/// </remarks>
public sealed class EnumLocalizationConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is null)
        {
            return string.Empty;
        }

        var type = value.GetType();
        if (!type.IsEnum)
        {
            return value.ToString() ?? string.Empty;
        }

        var key = $"{type.Name}_{value}";
        return Localizer.Current.GetString(key);
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotSupportedException($"{nameof(EnumLocalizationConverter)} only supports one-way binding.");
}
