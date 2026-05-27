namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Configuration for <see cref="AppRatingService"/>: per-platform store identifiers
/// and the launch-count threshold before the rating prompt is offered.
/// </summary>
public sealed class AppRatingOptions
{
    /// <summary>Microsoft Store product ID, used in <c>ms-windows-store://review/?ProductId=…</c>.</summary>
    public string? WindowsProductId { get; init; }

    /// <summary>Google Play package name, used in <c>market://details?id=…</c>.</summary>
    public string? AndroidPackageName { get; init; }

    /// <summary>Apple App Store app ID (numeric), used in <c>itms-apps://itunes.apple.com/app/id…?action=write-review</c>.</summary>
    public string? AppleAppId { get; init; }

    /// <summary>
    /// Number of recorded launches required before <see cref="IAppRatingService.ShouldRequestRating"/>
    /// flips to <see langword="true"/>. Defaults to <c>5</c>.
    /// </summary>
    public int MinLaunchCountForRating { get; init; } = 5;
}
