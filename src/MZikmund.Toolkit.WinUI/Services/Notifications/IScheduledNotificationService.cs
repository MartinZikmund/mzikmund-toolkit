namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Cross-platform local-notification scheduling. Apps schedule notifications by
/// app-supplied <see cref="ScheduledNotification.Id"/>; the platform fires them at the
/// requested <see cref="ScheduledNotification.DeliveryTime"/> even if the app isn't
/// running.
/// </summary>
public interface IScheduledNotificationService
{
    /// <summary>
    /// Schedules <paramref name="notification"/>. If a notification with the same
    /// <see cref="ScheduledNotification.Id"/> is already scheduled, it is replaced.
    /// </summary>
    Task ScheduleAsync(ScheduledNotification notification);

    /// <summary>
    /// Cancels a single scheduled notification by id. No-op when no matching
    /// notification is pending.
    /// </summary>
    Task CancelAsync(string id);

    /// <summary>Cancels all scheduled notifications.</summary>
    Task CancelAllAsync();

    /// <summary>Returns the currently scheduled (not-yet-fired) notifications.</summary>
    Task<IReadOnlyList<ScheduledNotification>> GetPendingAsync();
}
