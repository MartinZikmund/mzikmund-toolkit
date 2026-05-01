using Microsoft.UI.Dispatching;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="ITimerFactory"/> that produces <see cref="ITimer"/> instances
/// backed by <see cref="DispatcherQueueTimer"/>.
/// </summary>
public sealed class TimerFactory : ITimerFactory
{
    private readonly DispatcherQueue _dispatcherQueue;

    /// <summary>
    /// Initializes a new instance bound to the supplied <paramref name="dispatcherQueue"/>.
    /// </summary>
    public TimerFactory(DispatcherQueue dispatcherQueue)
    {
        ArgumentNullException.ThrowIfNull(dispatcherQueue);
        _dispatcherQueue = dispatcherQueue;
    }

    /// <summary>
    /// Initializes a new instance using the dispatcher queue of the calling thread.
    /// </summary>
    /// <exception cref="InvalidOperationException">No <see cref="DispatcherQueue"/> is associated with the calling thread.</exception>
    public TimerFactory()
        : this(DispatcherQueue.GetForCurrentThread()
            ?? throw new InvalidOperationException("No DispatcherQueue is associated with the calling thread. Construct TimerFactory on a UI thread, or pass a DispatcherQueue explicitly."))
    {
    }

    /// <inheritdoc />
    public ITimer CreateTimer(TimeSpan interval)
    {
        var inner = _dispatcherQueue.CreateTimer();
        inner.Interval = interval;
        return new DispatcherQueueTimerAdapter(inner);
    }

    private sealed class DispatcherQueueTimerAdapter : ITimer
    {
        private readonly DispatcherQueueTimer _inner;

        public DispatcherQueueTimerAdapter(DispatcherQueueTimer inner)
        {
            _inner = inner;
            _inner.Tick += (s, e) => Tick?.Invoke(this, EventArgs.Empty);
        }

        public TimeSpan Interval
        {
            get => _inner.Interval;
            set => _inner.Interval = value;
        }

        public bool IsRunning => _inner.IsRunning;

        public event EventHandler? Tick;

        public void Start() => _inner.Start();

        public void Stop() => _inner.Stop();
    }
}
