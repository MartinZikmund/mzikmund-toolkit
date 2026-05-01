using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI;

namespace MZikmund.Toolkit.WinUI.Dialogs;

/// <summary>
/// A simple <see cref="ContentDialog"/> wrapping the WinUI <see cref="ColorPicker"/>
/// control. Use the static <see cref="PickAsync(XamlRoot, Color, ColorPickerDialogStrings?)"/>
/// helper for one-call usage; the dialog itself can also be constructed and shown manually
/// to integrate with <c>IDialogCoordinator</c>.
/// </summary>
public sealed partial class ColorPickerDialog : ContentDialog
{
    /// <summary>The color picker control hosted inside the dialog.</summary>
    public ColorPicker ColorPicker { get; }

    /// <summary>
    /// The currently chosen color. Bound two-way to <see cref="ColorPicker.Color"/>.
    /// </summary>
    public Color SelectedColor
    {
        get => ColorPicker.Color;
        set => ColorPicker.Color = value;
    }

    /// <summary>Initializes a new instance with default labels.</summary>
    public ColorPickerDialog() : this(strings: null)
    {
    }

    /// <summary>Initializes a new instance with optional custom labels.</summary>
    public ColorPickerDialog(ColorPickerDialogStrings? strings)
    {
        var s = strings ?? new ColorPickerDialogStrings();

        ColorPicker = new ColorPicker
        {
            IsAlphaEnabled = true,
            ColorSpectrumShape = ColorSpectrumShape.Box,
            IsMoreButtonVisible = true,
        };

        Title = s.Title;
        PrimaryButtonText = s.OkButtonText;
        CloseButtonText = s.CancelButtonText;
        DefaultButton = ContentDialogButton.Primary;
        Content = ColorPicker;
    }

    /// <summary>
    /// Convenience: builds, shows, and returns the user's chosen color.
    /// </summary>
    /// <param name="xamlRoot">XAML root to anchor the dialog to.</param>
    /// <param name="initialColor">Color to seed the picker with.</param>
    /// <param name="strings">Optional override for the title and button labels.</param>
    /// <returns>The chosen color, or <see langword="null"/> if the user cancelled.</returns>
    public static async Task<Color?> PickAsync(
        XamlRoot xamlRoot,
        Color initialColor,
        ColorPickerDialogStrings? strings = null)
    {
        ArgumentNullException.ThrowIfNull(xamlRoot);

        var dialog = new ColorPickerDialog(strings)
        {
            XamlRoot = xamlRoot,
            SelectedColor = initialColor,
        };

        var result = await dialog.ShowAsync();
        return result == ContentDialogResult.Primary
            ? dialog.SelectedColor
            : (Color?)null;
    }
}

/// <summary>Localizable labels used by <see cref="ColorPickerDialog"/>.</summary>
public sealed class ColorPickerDialogStrings
{
    /// <summary>Dialog title. Defaults to <c>"Choose a color"</c>.</summary>
    public string Title { get; init; } = "Choose a color";

    /// <summary>Affirmative button text. Defaults to <c>"OK"</c>.</summary>
    public string OkButtonText { get; init; } = "OK";

    /// <summary>Cancel button text. Defaults to <c>"Cancel"</c>.</summary>
    public string CancelButtonText { get; init; } = "Cancel";
}
