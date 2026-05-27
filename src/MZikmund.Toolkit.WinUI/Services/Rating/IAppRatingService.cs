namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Tracks app launch counts and prompts the user to rate the app once a threshold
/// is reached. Pair with <see cref="AppRatingOptions"/> to configure the threshold
/// and per-platform store identifiers.
/// </summary>
public interface IAppRatingService
{
    /// <summary>The number of recorded launches.</summary>
    int LaunchCount { get; }

    /// <summary><see langword="true"/> if the user has already been prompted to rate.</summary>
    bool HasBeenAsked { get; }

    /// <summary>
    /// <see langword="true"/> when the launch threshold has been met and the user has
    /// not yet been asked. Apps usually check this on app start and call
    /// <see cref="RequestRatingAsync"/> if it's true.
    /// </summary>
    bool ShouldRequestRating { get; }

    /// <summary>Increments the persisted launch counter. Call once per app launch.</summary>
    void IncrementLaunchCount();

    /// <summary>
    /// Opens the platform's "rate this app" experience and records that the user
    /// was asked. No-ops on platforms whose identifier is not configured.
    /// </summary>
    /// <returns><see langword="true"/> if the launcher accepted the request; otherwise <see langword="false"/>.</returns>
    Task<bool> RequestRatingAsync();

    /// <summary>
    /// Resets <see cref="LaunchCount"/> and <see cref="HasBeenAsked"/>. Useful in tests
    /// and as a "ask me again later" affordance.
    /// </summary>
    void Reset();
}
