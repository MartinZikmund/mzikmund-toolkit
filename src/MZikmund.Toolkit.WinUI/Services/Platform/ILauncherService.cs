namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Trivial wrapper over <see cref="Windows.System.Launcher.LaunchUriAsync(Uri)"/> so apps
/// can depend on an interface (and stub it in tests) instead of a static API.
/// </summary>
public interface ILauncherService
{
    /// <summary>
    /// Asks the OS to open <paramref name="uri"/> in the default registered handler.
    /// </summary>
    /// <returns><see langword="true"/> if a handler was launched; <see langword="false"/> otherwise.</returns>
    Task<bool> LaunchUriAsync(Uri uri);
}
