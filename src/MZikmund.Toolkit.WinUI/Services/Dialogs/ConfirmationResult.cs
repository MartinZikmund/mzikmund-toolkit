namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Result of an <see cref="IConfirmationDialogService"/> prompt.
/// </summary>
public enum ConfirmationResult
{
    /// <summary>The user chose the affirmative option.</summary>
    Yes,

    /// <summary>The user chose the negative option (or dismissed the dialog).</summary>
    No,
}
