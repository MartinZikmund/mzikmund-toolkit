namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Service for showing confirmation dialogs with Yes/No options.
/// </summary>
public interface IConfirmationDialogService
{
	/// <summary>
	/// Shows a confirmation dialog with the specified title and text.
	/// </summary>
	/// <param name="title">The dialog title.</param>
	/// <param name="text">The dialog content text.</param>
	/// <returns>The user's confirmation result.</returns>
	Task<ConfirmationResult> ShowAsync(string title, string text);
}
