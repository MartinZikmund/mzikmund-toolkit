using Windows.System;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="IStoreLauncherService"/> implementation. Uses
/// <see cref="OperatingSystem"/> probes to pick a <see cref="StorePlatform"/>,
/// builds the URI through <see cref="StoreLauncherUris"/>, and dispatches it through
/// <see cref="Launcher.LaunchUriAsync(Uri)"/>.
/// </summary>
public class StoreLauncherService : IStoreLauncherService
{
    private readonly StoreLauncherOptions _options;

    /// <summary>Initializes a new instance with the supplied <paramref name="options"/>.</summary>
    public StoreLauncherService(StoreLauncherOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options;
    }

    /// <summary>
    /// Currently detected <see cref="StorePlatform"/> for store URI lookup.
    /// </summary>
    /// <remarks>Override in tests or unusual hosts to force a specific platform.</remarks>
    protected virtual StorePlatform CurrentPlatform =>
        OperatingSystem.IsWindows() ? StorePlatform.Windows :
        OperatingSystem.IsAndroid() ? StorePlatform.Android :
        OperatingSystem.IsIOS() || OperatingSystem.IsMacCatalyst() ? StorePlatform.Apple :
        StorePlatform.Unsupported;

    /// <inheritdoc />
    public Task<bool> RateAppAsync() =>
        LaunchAsync(StoreLauncherUris.Rate(CurrentPlatform, _options));

    /// <inheritdoc />
    public Task<bool> ShowAppListingAsync() =>
        LaunchAsync(StoreLauncherUris.Listing(CurrentPlatform, _options));

    /// <inheritdoc />
    public Task<bool> MoreAppsByPublisherAsync() =>
        LaunchAsync(StoreLauncherUris.MoreApps(CurrentPlatform, _options));

    /// <summary>
    /// Hook used by the public methods to actually launch the URI. Default implementation
    /// delegates to <see cref="Launcher.LaunchUriAsync(Uri)"/>; tests can override to capture
    /// the URI without dispatching to the platform launcher.
    /// </summary>
    protected virtual Task<bool> LaunchAsync(Uri? uri) =>
        uri is null ? Task.FromResult(false) : Launcher.LaunchUriAsync(uri).AsTask();
}
