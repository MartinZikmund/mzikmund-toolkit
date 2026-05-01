namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Per-app identifiers used by <see cref="IStoreLauncherService"/> to launch
/// platform-specific store URIs.
/// </summary>
/// <remarks>
/// Apps populate the platforms they ship to and leave the others <see langword="null"/>.
/// Store-launching methods return <see langword="false"/> when the active platform's
/// identifier (or the publisher name, for <c>MoreApps</c>) is missing.
/// </remarks>
public sealed class StoreLauncherOptions
{
    /// <summary>Microsoft Store product ID, used in the <c>ms-windows-store://</c> URIs.</summary>
    public string? WindowsProductId { get; init; }

    /// <summary>Google Play package name, used in the <c>market://</c> URIs.</summary>
    public string? AndroidPackageName { get; init; }

    /// <summary>Apple App Store app ID (numeric), used in the <c>itms-apps://</c> URIs.</summary>
    public string? AppleAppId { get; init; }

    /// <summary>Publisher / developer display name, used by the <c>MoreApps</c> URIs.</summary>
    public string? PublisherName { get; init; }
}
