namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Base <see cref="IScheduledNotificationService"/>. The toolkit ships only the
/// abstraction; per-platform scheduling is intentionally out of scope here because
/// each OS uses a different stack (Windows toast notifier, iOS
/// <c>UNUserNotificationCenter</c>, Android <c>AlarmManager</c> + boot receiver).
/// All methods throw <see cref="PlatformNotSupportedException"/> by default.
/// </summary>
/// <remarks>
/// Apps subclass this base and override the four virtual methods with the platform's
/// own implementation. Typical pattern:
/// <code>
/// public sealed class WindowsScheduledNotificationService : ScheduledNotificationService
/// {
///     public override Task ScheduleAsync(ScheduledNotification n)
///     {
///         var doc = BuildToastXml(n);
///         var toast = new ScheduledToastNotification(doc, n.DeliveryTime) { Id = n.Id };
///         ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
///         return Task.CompletedTask;
///     }
/// }
/// </code>
/// </remarks>
public class ScheduledNotificationService : IScheduledNotificationService
{
    /// <inheritdoc />
    public virtual Task ScheduleAsync(ScheduledNotification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);
        throw NotSupported(nameof(ScheduleAsync));
    }

    /// <inheritdoc />
    public virtual Task CancelAsync(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        throw NotSupported(nameof(CancelAsync));
    }

    /// <inheritdoc />
    public virtual Task CancelAllAsync() => throw NotSupported(nameof(CancelAllAsync));

    /// <inheritdoc />
    public virtual Task<IReadOnlyList<ScheduledNotification>> GetPendingAsync() =>
        throw NotSupported(nameof(GetPendingAsync));

    private static PlatformNotSupportedException NotSupported(string member) => new(
        $"{nameof(ScheduledNotificationService)}.{member} is not implemented in the toolkit. " +
        "Subclass and override with the platform's scheduling stack (Windows toast notifier / " +
        "iOS UNUserNotificationCenter / Android AlarmManager).");
}
