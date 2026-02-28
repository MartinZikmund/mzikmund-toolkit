using Microsoft.UI.Xaml.Markup;
using MZikmund.Toolkit.WinUI.Services.Localization;

namespace MZikmund.Toolkit.WinUI.Markup;

/// <summary>
/// XAML markup extension that resolves a localized string by key.
/// </summary>
[MarkupExtensionReturnType(ReturnType = typeof(string))]
public sealed class LocalizeExtension : MarkupExtension
{
	/// <summary>
	/// Gets or sets the resource key to look up.
	/// </summary>
	public string Key { get; set; } = string.Empty;

	/// <inheritdoc/>
	protected override object ProvideValue()
	{
		if (string.IsNullOrEmpty(Key))
		{
			return string.Empty;
		}

		return Localizer.Instance.GetString(Key);
	}
}
