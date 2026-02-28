using Microsoft.UI.Xaml.Data;
using MZikmund.Toolkit.WinUI.Services.Localization;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Converts an enum value to a localized string using the convention <c>EnumTypeName_ValueName</c>
/// as the resource key.
/// </summary>
public class EnumLocalizationConverter : IValueConverter
{
	/// <inheritdoc/>
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is Enum enumValue)
		{
			var enumType = enumValue.GetType();
			var enumName = Enum.GetName(enumType, enumValue);
			var key = $"{enumType.Name}_{enumName}";
			return Localizer.Instance.GetString(key);
		}

		return "";
	}

	/// <inheritdoc/>
	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotSupportedException();
	}
}
