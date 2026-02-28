namespace MZikmund.Toolkit.WinUI.Services.Theming;

/// <summary>
/// Manages the application theme, including title bar theming on Windows.
/// </summary>
public interface IThemeManager : IDisposable
{
	/// <summary>
	/// Sets the application theme.
	/// </summary>
	/// <param name="theme">The theme to apply.</param>
	void SetTheme(ElementTheme theme);

	/// <summary>
	/// Gets the currently requested theme.
	/// </summary>
	ElementTheme CurrentTheme { get; }

	/// <summary>
	/// Gets the actual resolved theme (accounting for system default).
	/// </summary>
	ApplicationTheme ActualTheme { get; }
}
