using Microsoft.UI.Xaml.Data;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Extracts the <see cref="ItemClickEventArgs.ClickedItem"/> from an <see cref="ItemClickEventArgs"/> instance.
/// Useful for binding ItemClick events to commands via event-to-command behaviors.
/// </summary>
public class ItemClickEventArgsConverter : IValueConverter
{
	/// <inheritdoc/>
	public object? Convert(object value, Type targetType, object parameter, string language)
	{
		return (value as ItemClickEventArgs)?.ClickedItem;
	}

	/// <inheritdoc/>
	public object? ConvertBack(object value, Type targetType, object parameter, string language)
	{
		throw new NotSupportedException();
	}
}
