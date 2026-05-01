using MZikmund.Toolkit.WinUI.Infrastructure;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="IConfirmationDialogService"/> implementation. Builds a Yes/No
/// <see cref="ContentDialog"/> with the primary button as the default and translates
/// the result into <see cref="ConfirmationResult"/>.
/// </summary>
public class ConfirmationDialogService : IConfirmationDialogService
{
    private readonly IXamlRootProvider _xamlRootProvider;
    private readonly IDialogCoordinator _coordinator;
    private readonly DialogStrings _strings;

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="xamlRootProvider">Provider for the active <see cref="XamlRoot"/>.</param>
    /// <param name="coordinator">Dialog coordinator that serializes concurrent shows.</param>
    /// <param name="strings">Optional override for the default English button labels.</param>
    public ConfirmationDialogService(
        IXamlRootProvider xamlRootProvider,
        IDialogCoordinator coordinator,
        DialogStrings? strings = null)
    {
        _xamlRootProvider = xamlRootProvider;
        _coordinator = coordinator;
        _strings = strings ?? new DialogStrings();
    }

    /// <inheritdoc />
    public async Task<ConfirmationResult> ConfirmAsync(string title, string message, string? yesText = null, string? noText = null)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            PrimaryButtonText = yesText ?? _strings.YesButtonText,
            CloseButtonText = noText ?? _strings.NoButtonText,
            DefaultButton = ContentDialogButton.Primary,
            XamlRoot = _xamlRootProvider.XamlRoot,
        };

        var result = await _coordinator.ShowAsync(dialog);
        return result == ContentDialogResult.Primary ? ConfirmationResult.Yes : ConfirmationResult.No;
    }
}
