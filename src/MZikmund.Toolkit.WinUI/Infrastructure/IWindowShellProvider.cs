namespace MZikmund.Toolkit.WinUI.Infrastructure;

/// <summary>
/// Resolves the active <see cref="IWindowShell"/> for the current window scope.
/// Used by services that need window-scoped access (dialogs, theme manager, navigation)
/// without holding a hard reference to the app's concrete shell type.
/// </summary>
public interface IWindowShellProvider
{
    /// <summary>
    /// The active shell. Throws <see cref="InvalidOperationException"/> if no shell
    /// has been registered through <see cref="SetShell"/> yet.
    /// </summary>
    IWindowShell Shell { get; }

    /// <summary>
    /// Registers (or replaces) the active shell. Apps call this once during shell
    /// initialization, typically from the shell page's constructor or <c>OnLoaded</c>.
    /// </summary>
    void SetShell(IWindowShell shell);
}
