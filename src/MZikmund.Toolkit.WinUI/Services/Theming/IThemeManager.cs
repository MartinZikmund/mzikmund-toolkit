namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Manages the user-facing app theme. Persists the choice through the toolkit's
/// preferences service and applies it to the active <c>IWindowShell</c>'s root
/// element. Apps that need title-bar coloring or Mica adjustments subscribe to
/// <see cref="ThemeChanged"/> and update from there.
/// </summary>
public interface IThemeManager
{
    /// <summary>The currently selected theme.</summary>
    AppTheme CurrentTheme { get; }

    /// <summary>Raised after <see cref="SetTheme"/> applies a new theme.</summary>
    event EventHandler<AppTheme>? ThemeChanged;

    /// <summary>
    /// Persists <paramref name="theme"/>, applies it to the shell's root element,
    /// and raises <see cref="ThemeChanged"/>.
    /// </summary>
    void SetTheme(AppTheme theme);

    /// <summary>
    /// Re-applies <see cref="CurrentTheme"/> to the shell. Useful at shell startup
    /// to restore the persisted theme on a freshly-created root element.
    /// </summary>
    void Apply();
}
