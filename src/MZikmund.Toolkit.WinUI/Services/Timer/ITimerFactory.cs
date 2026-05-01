namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Creates <see cref="ITimer"/> instances on the owning UI thread. The default
/// <see cref="TimerFactory"/> delegates to <c>DispatcherQueue.CreateTimer</c>;
/// tests use <see cref="FakeTimerFactory"/> to advance time deterministically.
/// </summary>
public interface ITimerFactory
{
    /// <summary>
    /// Creates a stopped timer with the given <paramref name="interval"/>.
    /// </summary>
    ITimer CreateTimer(TimeSpan interval);
}
