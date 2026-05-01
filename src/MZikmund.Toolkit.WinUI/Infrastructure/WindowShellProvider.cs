namespace MZikmund.Toolkit.WinUI.Infrastructure;

/// <summary>
/// Default <see cref="IWindowShellProvider"/> implementation. Holds a single
/// <see cref="IWindowShell"/> reference set by the host on shell creation.
/// Thread-safe for the simple "set once, read many" pattern.
/// </summary>
public class WindowShellProvider : IWindowShellProvider
{
    private IWindowShell? _shell;

    /// <inheritdoc />
    public IWindowShell Shell =>
        _shell ?? throw new InvalidOperationException(
            "No IWindowShell has been registered. Call SetShell during window initialization.");

    /// <inheritdoc />
    public void SetShell(IWindowShell shell)
    {
        ArgumentNullException.ThrowIfNull(shell);
        _shell = shell;
    }
}
