namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Launches platform-specific store URIs for the three common actions: rating the app,
/// opening its listing, and showing "more apps by this publisher".
/// </summary>
public interface IStoreLauncherService
{
    /// <summary>
    /// Opens the store's "rate / write a review" experience for the current app.
    /// </summary>
    /// <returns><see langword="true"/> when the launcher accepted the request; <see langword="false"/> when the platform is unsupported, the matching identifier is missing in options, or the launcher refused.</returns>
    Task<bool> RateAppAsync();

    /// <summary>
    /// Opens the current app's listing in the store.
    /// </summary>
    /// <returns><see langword="true"/> when the launcher accepted the request; otherwise <see langword="false"/>.</returns>
    Task<bool> ShowAppListingAsync();

    /// <summary>
    /// Opens "more apps by this publisher" in the store.
    /// </summary>
    /// <returns><see langword="true"/> when the launcher accepted the request; otherwise <see langword="false"/>.</returns>
    Task<bool> MoreAppsByPublisherAsync();
}
