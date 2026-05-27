using Windows.System;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="ILauncherService"/> backed by <see cref="Launcher.LaunchUriAsync(Uri)"/>.
/// </summary>
public sealed class LauncherService : ILauncherService
{
    /// <inheritdoc />
    public Task<bool> LaunchUriAsync(Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        return Launcher.LaunchUriAsync(uri).AsTask();
    }
}
