namespace MZikmund.Toolkit.WinUI.Disposables;

/// <summary>
/// Factories for ad-hoc <see cref="IDisposable"/> values. Avoids the boilerplate of a
/// nested class for one-off "do <em>X</em> on dispose" scopes.
/// </summary>
/// <example>
/// "Loading scope" pattern — toggle a flag for the duration of an operation:
/// <code>
/// IsLoading = true;
/// using var _ = Disposable.Create(() => IsLoading = false);
/// await DoWorkAsync();
/// </code>
/// </example>
public static class Disposable
{
    /// <summary>
    /// Returns an <see cref="IDisposable"/> that runs <paramref name="onDispose"/> exactly
    /// once, on first <see cref="IDisposable.Dispose"/>. Subsequent disposes are no-ops.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="onDispose"/> is <see langword="null"/>.</exception>
    public static IDisposable Create(Action onDispose)
    {
        ArgumentNullException.ThrowIfNull(onDispose);
        return new ActionDisposable(onDispose);
    }

    /// <summary>
    /// A no-op disposable. Useful as a default value or to short-circuit "no scope needed" branches.
    /// </summary>
    public static IDisposable Empty { get; } = new EmptyDisposable();

    private sealed class ActionDisposable : IDisposable
    {
        private Action? _onDispose;

        public ActionDisposable(Action onDispose) => _onDispose = onDispose;

        public void Dispose() =>
            Interlocked.Exchange(ref _onDispose, null)?.Invoke();
    }

    private sealed class EmptyDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
