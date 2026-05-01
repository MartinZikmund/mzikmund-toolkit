namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Localizable button labels used by <see cref="IDialogService"/> and <see cref="IConfirmationDialogService"/>.
/// Defaults to English; apps can supply localized strings via the service constructors.
/// </summary>
public sealed class DialogStrings
{
    /// <summary>
    /// Text used for the close button on a single-button dialog. Defaults to <c>"OK"</c>.
    /// </summary>
    public string OkButtonText { get; init; } = "OK";

    /// <summary>
    /// Text used for the affirmative button on a confirmation dialog. Defaults to <c>"Yes"</c>.
    /// </summary>
    public string YesButtonText { get; init; } = "Yes";

    /// <summary>
    /// Text used for the negative button on a confirmation dialog. Defaults to <c>"No"</c>.
    /// </summary>
    public string NoButtonText { get; init; } = "No";
}
