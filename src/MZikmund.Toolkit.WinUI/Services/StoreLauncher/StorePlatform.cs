namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Target platform for an <see cref="IStoreLauncherService"/> URI lookup.
/// </summary>
public enum StorePlatform
{
    /// <summary>Platform is unsupported or not yet detected — store URIs are not available.</summary>
    Unsupported = 0,

    /// <summary>Microsoft Store (Windows).</summary>
    Windows,

    /// <summary>Google Play (Android).</summary>
    Android,

    /// <summary>Apple App Store (iOS / macCatalyst).</summary>
    Apple,
}
