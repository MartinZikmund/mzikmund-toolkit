using Microsoft.UI.Xaml.Markup;

namespace MZikmund.Toolkit.WinUI.Markup;

/// <summary>
/// XAML markup extension that creates a <see cref="FontIcon"/> from a glyph string.
/// </summary>
[MarkupExtensionReturnType(ReturnType = typeof(FontIcon))]
public class FontIconExtension : MarkupExtension
{
	/// <summary>
	/// Gets or sets the glyph character for the icon.
	/// </summary>
	public string Glyph { get; set; } = "";

	/// <summary>
	/// Gets or sets the optional font size for the icon.
	/// </summary>
	public double FontSize { get; set; } = 0;

	/// <inheritdoc/>
	protected override object ProvideValue()
	{
		var icon = new FontIcon { Glyph = Glyph };

		if (FontSize > 0)
		{
			icon.FontSize = FontSize;
		}

		return icon;
	}
}
