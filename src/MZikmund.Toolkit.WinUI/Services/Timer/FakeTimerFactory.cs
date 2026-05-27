namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Test-only <see cref="ITimerFactory"/> that returns <see cref="FakeTimer"/> instances
/// whose ticks are driven manually. Use in unit tests to assert behavior under a
/// known number of ticks without flaky time-based waits.
/// </summary>
public sealed class FakeTimerFactory : ITimerFactory
{
    private readonly List<FakeTimer> _timers = new();

    /// <summary>All timers created so far, in creation order.</summary>
    public IReadOnlyList<FakeTimer> Timers => _timers;

    /// <inheritdoc />
    public ITimer CreateTimer(TimeSpan interval)
    {
        var timer = new FakeTimer { Interval = interval };
        _timers.Add(timer);
        return timer;
    }
}

/// <summary>
/// Test-only <see cref="ITimer"/> implementation. Tracks <see cref="IsRunning"/>
/// from <see cref="Start"/> / <see cref="Stop"/> calls and lets tests fire ticks
/// manually with <see cref="RaiseTick"/>.
/// </summary>
public sealed class FakeTimer : ITimer
{
    /// <inheritdoc />
    public TimeSpan Interval { get; set; }

    /// <inheritdoc />
    public bool IsRunning { get; private set; }

    /// <inheritdoc />
    public event EventHandler? Tick;

    /// <inheritdoc />
    public void Start() => IsRunning = true;

    /// <inheritdoc />
    public void Stop() => IsRunning = false;

    /// <summary>
    /// Manually raises the <see cref="Tick"/> event. No-ops while the timer is stopped.
    /// </summary>
    public void RaiseTick()
    {
        if (!IsRunning)
        {
            return;
        }

        Tick?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Convenience for raising <see cref="Tick"/> multiple times in a row.
    /// </summary>
    public void RaiseTick(int count)
    {
        for (var i = 0; i < count; i++)
        {
            RaiseTick();
        }
    }
}
