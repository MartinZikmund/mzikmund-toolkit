using Microsoft.UI.Xaml;
using MZikmund.Toolkit.WinUI.Infrastructure;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="IThemeManager"/>. Persists the user's theme choice through
/// <see cref="IPreferences"/> and applies it to <see cref="IWindowShell.RootFrame"/>'s
/// <see cref="FrameworkElement.RequestedTheme"/>. Apps that need title-bar coloring
/// subscribe to <see cref="ThemeChanged"/>.
/// </summary>
public sealed class ThemeManager : IThemeManager
{
    private const string ThemePreferenceKey = "MZikmund.Toolkit.Theme";

    private readonly IWindowShellProvider _shellProvider;
    private readonly IPreferences _preferences;

    /// <summary>Initializes a new instance.</summary>
    public ThemeManager(IWindowShellProvider shellProvider, IPreferences preferences)
    {
        ArgumentNullException.ThrowIfNull(shellProvider);
        ArgumentNullException.ThrowIfNull(preferences);
        _shellProvider = shellProvider;
        _preferences = preferences;

        CurrentTheme = preferences.Get(ThemePreferenceKey, AppTheme.System);
    }

    /// <inheritdoc />
    public AppTheme CurrentTheme { get; private set; }

    /// <inheritdoc />
    public event EventHandler<AppTheme>? ThemeChanged;

    /// <inheritdoc />
    public void SetTheme(AppTheme theme)
    {
        CurrentTheme = theme;
        _preferences.Set(ThemePreferenceKey, theme);
        Apply();
        ThemeChanged?.Invoke(this, theme);
    }

    /// <inheritdoc />
    public void Apply()
    {
        if (_shellProvider.Shell.RootFrame is { } frame)
        {
            frame.RequestedTheme = ToElementTheme(CurrentTheme);
        }
    }

    /// <summary>Maps an <see cref="AppTheme"/> to a WinUI <see cref="ElementTheme"/>.</summary>
    public static ElementTheme ToElementTheme(AppTheme theme) => theme switch
    {
        AppTheme.Light => ElementTheme.Light,
        AppTheme.Dark => ElementTheme.Dark,
        _ => ElementTheme.Default,
    };
}
