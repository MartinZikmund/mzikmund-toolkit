using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Converts an enum value to <see cref="Visibility"/> by comparing it to the converter parameter.
/// Returns <see cref="Visibility.Visible"/> when the bound value matches the parameter.
/// Set <see cref="Invert"/> to true to reverse the logic.
/// </summary>
public class EnumValueToVisibilityConverter : IValueConverter
{
	/// <summary>
	/// Gets or sets a value indicating whether to invert the conversion logic.
	/// </summary>
	public bool Invert { get; set; }

	/// <inheritdoc/>
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (parameter is null)
		{
			throw new ArgumentNullException(nameof(parameter));
		}

		var enumValue = value?.ToString();
		var compareValue = parameter.ToString();
		var isMatch = string.Equals(enumValue, compareValue, StringComparison.OrdinalIgnoreCase);

		if (Invert)
		{
			isMatch = !isMatch;
		}

		return isMatch ? Visibility.Visible : Visibility.Collapsed;
	}

	/// <inheritdoc/>
	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotSupportedException();
	}
}
