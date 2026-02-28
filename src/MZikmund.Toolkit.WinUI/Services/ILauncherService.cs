namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Service for launching URIs in the device's default browser.
/// </summary>
public interface ILauncherService
{
	/// <summary>
	/// Launches the specified URI in the default external browser.
	/// </summary>
	/// <param name="uri">The URI to launch.</param>
	/// <returns>True if the URI was launched successfully; otherwise, false.</returns>
	Task<bool> LaunchUriAsync(Uri uri);
}
