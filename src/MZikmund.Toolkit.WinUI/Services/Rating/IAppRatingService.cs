namespace MZikmund.Toolkit.WinUI.Services.Rating;

/// <summary>
/// Service for prompting the user to rate the application.
/// Implementation is platform-specific.
/// </summary>
public interface IAppRatingService
{
	/// <summary>
	/// Asks the user to rate the application using the platform's native rating dialog.
	/// </summary>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task AskUserForRatingAsync();
}
