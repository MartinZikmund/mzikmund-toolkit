using Microsoft.UI.Xaml.Markup;
using MZikmund.Toolkit.WinUI.Localization;

namespace MZikmund.Toolkit.WinUI.Markup;

/// <summary>
/// XAML markup extension that resolves a localization key through <see cref="Localizer.Current"/>:
/// <c>{toolkit:Localize Key=Greeting}</c>.
/// </summary>
/// <remarks>
/// Markup extensions are evaluated once at element construction. Live language switching is
/// not supported here — apps that need that should bind to a property on a view-model and let
/// the view-model raise <c>PropertyChanged</c> when the language changes.
/// </remarks>
[MarkupExtensionReturnType(ReturnType = typeof(string))]
public sealed class LocalizeExtension : MarkupExtension
{
    /// <summary>
    /// Resource key.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override object ProvideValue() =>
        string.IsNullOrEmpty(Key)
            ? string.Empty
            : Localizer.Current.GetString(Key);
}
