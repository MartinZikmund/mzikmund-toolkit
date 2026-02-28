namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Launches URIs using the platform's default handler.
/// </summary>
public class LauncherService : ILauncherService
{
	/// <inheritdoc/>
	public async Task<bool> LaunchUriAsync(Uri uri)
	{
		return await Windows.System.Launcher.LaunchUriAsync(uri);
	}
}
