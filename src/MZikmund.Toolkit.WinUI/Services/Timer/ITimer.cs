namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Thin abstraction over a UI-thread timer (typically <c>DispatcherQueueTimer</c>).
/// Lets view-models and services depend on a tickable clock without referencing the
/// platform type directly, which makes them testable with a fake.
/// </summary>
public interface ITimer
{
    /// <summary>Tick interval. Setting this while the timer is running takes effect on the next tick.</summary>
    TimeSpan Interval { get; set; }

    /// <summary><see langword="true"/> while the timer is started.</summary>
    bool IsRunning { get; }

    /// <summary>Raised on each tick, on the timer's owning UI thread.</summary>
    event EventHandler? Tick;

    /// <summary>Starts ticking. No-op when already running.</summary>
    void Start();

    /// <summary>Stops ticking. No-op when already stopped.</summary>
    void Stop();
}
