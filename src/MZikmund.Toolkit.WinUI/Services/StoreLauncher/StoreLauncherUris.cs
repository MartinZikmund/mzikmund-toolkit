namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Pure URI-builder for the three <see cref="IStoreLauncherService"/> actions.
/// Separated from the launcher service so callers (and tests) can inspect the
/// URIs without invoking the platform launcher.
/// </summary>
public static class StoreLauncherUris
{
    /// <summary>
    /// Returns the URI that opens a "rate this app" / "write a review" prompt
    /// on the given <paramref name="platform"/>. Returns <see langword="null"/>
    /// when the platform isn't supported or the matching identifier in
    /// <paramref name="options"/> is missing.
    /// </summary>
    public static Uri? Rate(StorePlatform platform, StoreLauncherOptions options) =>
        platform switch
        {
            StorePlatform.Windows when !string.IsNullOrEmpty(options.WindowsProductId)
                => new Uri($"ms-windows-store://review/?ProductId={options.WindowsProductId}"),
            StorePlatform.Android when !string.IsNullOrEmpty(options.AndroidPackageName)
                => new Uri($"market://details?id={options.AndroidPackageName}"),
            StorePlatform.Apple when !string.IsNullOrEmpty(options.AppleAppId)
                => new Uri($"itms-apps://itunes.apple.com/app/id{options.AppleAppId}?action=write-review"),
            _ => null,
        };

    /// <summary>
    /// Returns the URI that opens this app's listing page on the given <paramref name="platform"/>.
    /// </summary>
    public static Uri? Listing(StorePlatform platform, StoreLauncherOptions options) =>
        platform switch
        {
            StorePlatform.Windows when !string.IsNullOrEmpty(options.WindowsProductId)
                => new Uri($"ms-windows-store://pdp/?ProductId={options.WindowsProductId}"),
            StorePlatform.Android when !string.IsNullOrEmpty(options.AndroidPackageName)
                => new Uri($"market://details?id={options.AndroidPackageName}"),
            StorePlatform.Apple when !string.IsNullOrEmpty(options.AppleAppId)
                => new Uri($"itms-apps://itunes.apple.com/app/id{options.AppleAppId}"),
            _ => null,
        };

    /// <summary>
    /// Returns the URI that opens "more apps by this publisher" on the given <paramref name="platform"/>.
    /// Requires <see cref="StoreLauncherOptions.PublisherName"/>.
    /// </summary>
    public static Uri? MoreApps(StorePlatform platform, StoreLauncherOptions options)
    {
        if (string.IsNullOrEmpty(options.PublisherName))
        {
            return null;
        }

        var encoded = Uri.EscapeDataString(options.PublisherName);
        return platform switch
        {
            StorePlatform.Windows => new Uri($"ms-windows-store://publisher/?name={encoded}"),
            StorePlatform.Android => new Uri($"market://search?q=pub:{encoded}"),
            StorePlatform.Apple => new Uri($"itms-apps://itunes.apple.com/search?term={encoded}"),
            _ => null,
        };
    }
}
