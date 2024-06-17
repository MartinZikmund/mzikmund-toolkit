namespace MZikmund.Services.Dialogs;

/// <summary>
/// Allows coordination of content dialog display.
/// This is needed to ensure that only one dialog is shown at a time.
/// Otherwise WinUI throws an exception.
/// </summary>
public interface IDialogCoordinator
{
    /// <summary>
    /// Shows a content dialog.
    /// </summary>
    /// <param name="dialog">Dialog to show.</param>
    /// <returns>Result of the dialog.</returns>
    Task<ContentDialogResult> ShowAsync(ContentDialog dialog);
}
