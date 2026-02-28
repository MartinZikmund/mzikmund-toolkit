using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Converts between <see cref="int"/> and <see cref="double"/> for NumberBox.Value binding.
/// NaN (cleared NumberBox) returns <see cref="DependencyProperty.UnsetValue"/> to prevent source update.
/// </summary>
public class IntToDoubleConverter : IValueConverter
{
	/// <inheritdoc/>
	public object Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is int intValue)
		{
			return (double)intValue;
		}

		return double.NaN;
	}

	/// <inheritdoc/>
	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		if (value is double doubleValue && !double.IsNaN(doubleValue))
		{
			return (int)doubleValue;
		}

		return DependencyProperty.UnsetValue;
	}
}
