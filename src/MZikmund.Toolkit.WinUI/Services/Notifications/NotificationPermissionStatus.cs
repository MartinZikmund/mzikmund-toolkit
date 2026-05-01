namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Outcome of a notification permission query / request.
/// </summary>
public enum NotificationPermissionStatus
{
    /// <summary>The user has not yet been asked. Common on iOS / Android first launch.</summary>
    NotDetermined = 0,

    /// <summary>The user (or platform default) has granted notification permission.</summary>
    Granted,

    /// <summary>The user has denied notification permission.</summary>
    Denied,
}
