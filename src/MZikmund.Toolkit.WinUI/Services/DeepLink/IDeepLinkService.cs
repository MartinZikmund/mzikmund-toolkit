namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Captures deep-link / pending-navigation requests when the shell isn't ready
/// yet, then replays them after init. Pairs with notification taps and tile
/// activation: the OS hands the app a URI before any UI is alive, and the shell
/// drains the queue once it's ready to navigate.
/// </summary>
public interface IDeepLinkService
{
    /// <summary>Raised by <see cref="Replay"/> for each queued request, in arrival order.</summary>
    event EventHandler<DeepLinkRequest>? RequestReceived;

    /// <summary>Adds a request to the queue. Safe to call from any thread.</summary>
    void Enqueue(DeepLinkRequest request);

    /// <summary>
    /// Awaits the next queued request. Returns <see langword="null"/> if the wait is cancelled.
    /// </summary>
    Task<DeepLinkRequest?> TryDequeueAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Drains all currently queued requests synchronously, raising
    /// <see cref="RequestReceived"/> for each in order.
    /// </summary>
    /// <returns>The number of requests replayed.</returns>
    int Replay();
}
