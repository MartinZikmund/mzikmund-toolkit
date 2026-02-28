namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Service for showing content dialogs, supporting ViewModel-to-Dialog convention mapping.
/// </summary>
public interface IDialogService
{
	/// <summary>
	/// Shows a dialog mapped to the specified ViewModel by convention
	/// (strips "ViewModel" suffix to find the dialog type).
	/// </summary>
	/// <param name="viewModel">The ViewModel to use as the dialog's DataContext.</param>
	/// <returns>The dialog result.</returns>
	Task<ContentDialogResult> ShowAsync(object viewModel);

	/// <summary>
	/// Shows a simple dialog with a title and content text.
	/// </summary>
	/// <param name="title">The dialog title.</param>
	/// <param name="content">The dialog content text.</param>
	/// <returns>The dialog result.</returns>
	Task<ContentDialogResult> ShowAsync(string title, string content);

	/// <summary>
	/// Shows the specified content dialog directly.
	/// </summary>
	/// <param name="contentDialog">The content dialog to show.</param>
	/// <returns>The dialog result.</returns>
	Task<ContentDialogResult> ShowAsync(ContentDialog contentDialog);

	/// <summary>
	/// Registers a dialog type mapped to a ViewModel type.
	/// </summary>
	/// <param name="viewModelType">The ViewModel type.</param>
	/// <param name="dialogType">The ContentDialog type.</param>
	void RegisterDialog(Type viewModelType, Type dialogType);
}
