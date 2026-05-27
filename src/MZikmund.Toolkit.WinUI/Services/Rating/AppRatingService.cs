using Windows.System;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="IAppRatingService"/> implementation. Stores launch count and
/// "has been asked" state through <see cref="IPreferences"/>, and dispatches the rating
/// prompt to a platform-specific store URI through <see cref="Launcher.LaunchUriAsync(Uri)"/>.
/// </summary>
public class AppRatingService : IAppRatingService
{
    private const string LaunchCountKey = "AppRating.LaunchCount";
    private const string HasBeenAskedKey = "AppRating.HasBeenAsked";

    private readonly IPreferences _preferences;
    private readonly AppRatingOptions _options;

    /// <summary>Initializes a new instance.</summary>
    public AppRatingService(IPreferences preferences, AppRatingOptions options)
    {
        ArgumentNullException.ThrowIfNull(preferences);
        ArgumentNullException.ThrowIfNull(options);
        _preferences = preferences;
        _options = options;
    }

    /// <inheritdoc />
    public int LaunchCount => _preferences.Get(LaunchCountKey, 0);

    /// <inheritdoc />
    public bool HasBeenAsked => _preferences.Get(HasBeenAskedKey, false);

    /// <inheritdoc />
    public bool ShouldRequestRating =>
        !HasBeenAsked && LaunchCount >= _options.MinLaunchCountForRating;

    /// <inheritdoc />
    public void IncrementLaunchCount() =>
        _preferences.Set(LaunchCountKey, LaunchCount + 1);

    /// <inheritdoc />
    public async Task<bool> RequestRatingAsync()
    {
        var uri = GetReviewUri();
        if (uri is null)
        {
            return false;
        }

        _preferences.Set(HasBeenAskedKey, true);
        return await LaunchAsync(uri);
    }

    /// <inheritdoc />
    public void Reset()
    {
        _preferences.Remove(LaunchCountKey);
        _preferences.Remove(HasBeenAskedKey);
    }

    /// <summary>
    /// Builds the platform-specific review URI for the current OS based on
    /// <see cref="AppRatingOptions"/>. Returns <see langword="null"/> when the platform
    /// is unsupported or the matching identifier is missing.
    /// </summary>
    /// <remarks>Override in tests to force a specific URI without depending on the host OS.</remarks>
    protected virtual Uri? GetReviewUri()
    {
        if (OperatingSystem.IsWindows() && !string.IsNullOrEmpty(_options.WindowsProductId))
        {
            return new Uri($"ms-windows-store://review/?ProductId={_options.WindowsProductId}");
        }

        if (OperatingSystem.IsAndroid() && !string.IsNullOrEmpty(_options.AndroidPackageName))
        {
            return new Uri($"market://details?id={_options.AndroidPackageName}");
        }

        if ((OperatingSystem.IsIOS() || OperatingSystem.IsMacCatalyst()) && !string.IsNullOrEmpty(_options.AppleAppId))
        {
            return new Uri($"itms-apps://itunes.apple.com/app/id{_options.AppleAppId}?action=write-review");
        }

        return null;
    }

    /// <summary>
    /// Hook used to actually launch the URI. Default implementation delegates to
    /// <see cref="Launcher.LaunchUriAsync(Uri)"/>. Tests override this to capture the URI.
    /// </summary>
    protected virtual Task<bool> LaunchAsync(Uri uri) => Launcher.LaunchUriAsync(uri).AsTask();
}
