using Windows.System;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="INotificationPermissionService"/>. Treats packaged Windows apps
/// as having toast capability granted by default and routes <c>OpenSettingsAsync</c>
/// to the platform-appropriate URI (<c>ms-settings:notifications</c> on Windows,
/// <c>app-settings:</c> on iOS / Mac Catalyst).
/// </summary>
/// <remarks>
/// Apps shipping on Android and iOS subclass to override
/// <see cref="EnsurePermissionAsync"/> with the actual platform request:
/// <list type="bullet">
///   <item><description>iOS / Mac Catalyst: <c>UNUserNotificationCenter.RequestAuthorizationAsync</c>.</description></item>
///   <item><description>Android 13+: <c>POST_NOTIFICATIONS</c> runtime permission via <c>ContextCompat</c>.</description></item>
/// </list>
/// The base class returns <see cref="NotificationPermissionStatus.Granted"/> on those
/// platforms — fine for development / Windows but not a substitute for a real prompt.
/// </remarks>
public class NotificationPermissionService : INotificationPermissionService
{
    /// <inheritdoc />
    public virtual Task<NotificationPermissionStatus> IsGrantedAsync() =>
        Task.FromResult(NotificationPermissionStatus.Granted);

    /// <inheritdoc />
    public virtual Task<NotificationPermissionStatus> EnsurePermissionAsync() =>
        IsGrantedAsync();

    /// <inheritdoc />
    public virtual async Task<bool> OpenSettingsAsync()
    {
        var uri = GetSettingsUri();
        if (uri is null)
        {
            return false;
        }

        return await Launcher.LaunchUriAsync(uri);
    }

    /// <summary>
    /// Returns the platform-specific URI that opens the OS notification-permission
    /// settings page, or <see langword="null"/> when the platform has no usable URI.
    /// Tests / subclasses can override.
    /// </summary>
    protected virtual Uri? GetSettingsUri()
    {
        if (OperatingSystem.IsWindows())
        {
            return new Uri("ms-settings:notifications");
        }

        if (OperatingSystem.IsIOS() || OperatingSystem.IsMacCatalyst())
        {
            return new Uri("app-settings:");
        }

        return null;
    }
}
