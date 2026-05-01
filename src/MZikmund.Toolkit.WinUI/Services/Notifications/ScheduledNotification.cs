namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// A scheduled local notification: when to fire, what to show, and a stable
/// app-supplied <see cref="Id"/> the platform uses to look it up later for cancel.
/// </summary>
/// <param name="Id">
/// Stable identifier the app picks (typically a GUID-based string or a
/// <see cref="Helpers.StableHash.FromGuid"/>-derived integer). Used to cancel the
/// scheduled notification later.
/// </param>
/// <param name="Title">Notification title.</param>
/// <param name="Body">Notification body text.</param>
/// <param name="DeliveryTime">When the notification should fire (local OS clock).</param>
/// <param name="Category">Optional category tag for grouping or per-channel routing.</param>
public sealed record ScheduledNotification(
    string Id,
    string Title,
    string Body,
    DateTimeOffset DeliveryTime,
    string? Category = null);
