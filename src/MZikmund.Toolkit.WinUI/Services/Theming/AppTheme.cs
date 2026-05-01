namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// User-facing theme choice for an app: <see cref="System"/> follows the OS,
/// <see cref="Light"/> / <see cref="Dark"/> override it.
/// </summary>
public enum AppTheme
{
    /// <summary>Follow the operating system's current theme.</summary>
    System = 0,

    /// <summary>Light theme regardless of the OS setting.</summary>
    Light,

    /// <summary>Dark theme regardless of the OS setting.</summary>
    Dark,
}
