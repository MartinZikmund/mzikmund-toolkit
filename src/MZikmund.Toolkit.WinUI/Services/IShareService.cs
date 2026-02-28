namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Service for sharing content via the OS native share dialog.
/// </summary>
public interface IShareService
{
	/// <summary>
	/// Opens the native share dialog with the specified title and URI.
	/// </summary>
	/// <param name="title">The display title for the shared content.</param>
	/// <param name="uri">The URL to share.</param>
	Task ShareAsync(string title, string uri);
}
