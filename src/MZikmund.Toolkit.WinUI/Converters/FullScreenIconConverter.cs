using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace MZikmund.Toolkit.WinUI.Converters;

/// <summary>
/// Maps a <see cref="bool"/> "is full screen" state to a <see cref="Symbol"/>:
/// <see cref="Symbol.BackToWindow"/> when full-screen, <see cref="Symbol.FullScreen"/> otherwise.
/// Useful for the icon on a fullscreen toggle button.
/// </summary>
public sealed class FullScreenIconConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, string language) =>
        value is true ? Symbol.BackToWindow : Symbol.FullScreen;

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        value is Symbol.BackToWindow;
}
