namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Cross-platform notification-permission service. Wraps the per-platform "may I
/// show notifications" flow so apps can query, request, and (when denied) deeplink
/// the user to the OS settings page.
/// </summary>
public interface INotificationPermissionService
{
    /// <summary>Returns the current permission status without prompting the user.</summary>
    Task<NotificationPermissionStatus> IsGrantedAsync();

    /// <summary>
    /// Returns the current status; if it's <see cref="NotificationPermissionStatus.NotDetermined"/>,
    /// shows the platform's permission prompt and returns the post-prompt status.
    /// </summary>
    Task<NotificationPermissionStatus> EnsurePermissionAsync();

    /// <summary>
    /// Opens the OS settings page where the user can change the notification permission.
    /// Useful as a fallback when <see cref="EnsurePermissionAsync"/> returns
    /// <see cref="NotificationPermissionStatus.Denied"/>.
    /// </summary>
    /// <returns><see langword="true"/> if a settings page was launched.</returns>
    Task<bool> OpenSettingsAsync();
}
