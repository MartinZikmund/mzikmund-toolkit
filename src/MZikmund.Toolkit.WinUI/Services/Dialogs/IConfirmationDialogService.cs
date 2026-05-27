namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Shows a Yes/No confirmation <see cref="ContentDialog"/>.
/// </summary>
public interface IConfirmationDialogService
{
    /// <summary>
    /// Shows a confirmation dialog and returns the user's choice.
    /// </summary>
    /// <param name="title">Dialog title.</param>
    /// <param name="message">Dialog message body.</param>
    /// <param name="yesText">Override for the affirmative button text. Falls back to <see cref="DialogStrings.YesButtonText"/>.</param>
    /// <param name="noText">Override for the negative button text. Falls back to <see cref="DialogStrings.NoButtonText"/>.</param>
    /// <returns><see cref="ConfirmationResult.Yes"/> if the user clicked the primary button; otherwise <see cref="ConfirmationResult.No"/>.</returns>
    Task<ConfirmationResult> ConfirmAsync(string title, string message, string? yesText = null, string? noText = null);
}
