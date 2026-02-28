using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Converts a 6-character hex color string (without # prefix) to a <see cref="SolidColorBrush"/>.
/// Returns a gray brush for invalid input.
/// </summary>
public class HexColorToBrushConverter : IValueConverter
{
	/// <inheritdoc/>
	public object? Convert(object value, Type targetType, object parameter, string language)
	{
		if (value is not string hex || hex.Length != 6)
		{
			return new SolidColorBrush(Colors.Gray);
		}

		try
		{
			var r = System.Convert.ToByte(hex[..2], 16);
			var g = System.Convert.ToByte(hex[2..4], 16);
			var b = System.Convert.ToByte(hex[4..6], 16);
			return new SolidColorBrush(Color.FromArgb(255, r, g, b));
		}
		catch
		{
			return new SolidColorBrush(Colors.Gray);
		}
	}

	/// <inheritdoc/>
	public object ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotSupportedException();
	}
}
