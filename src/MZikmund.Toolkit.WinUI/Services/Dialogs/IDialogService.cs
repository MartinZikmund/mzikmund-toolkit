namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Shows a single-button informational <see cref="ContentDialog"/>.
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// Shows a dialog with the given title and message and a single close button.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    /// <param name="message">Dialog message body.</param>
    /// <param name="closeButtonText">Override for the close button text. Falls back to <see cref="DialogStrings.OkButtonText"/>.</param>
    Task ShowAsync(string title, string message, string? closeButtonText = null);
}
