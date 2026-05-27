using MZikmund.Toolkit.WinUI.Infrastructure;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="IDialogService"/> implementation. Builds a <see cref="ContentDialog"/>,
/// attaches the current <see cref="XamlRoot"/> from <see cref="IXamlRootProvider"/>, and
/// shows it through <see cref="IDialogCoordinator"/> so concurrent dialog requests are serialized.
/// </summary>
public class DialogService : IDialogService
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
    public DialogService(
        IXamlRootProvider xamlRootProvider,
        IDialogCoordinator coordinator,
        DialogStrings? strings = null)
    {
        _xamlRootProvider = xamlRootProvider;
        _coordinator = coordinator;
        _strings = strings ?? new DialogStrings();
    }

    /// <inheritdoc />
    public async Task ShowAsync(string title, string message, string? closeButtonText = null)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = closeButtonText ?? _strings.OkButtonText,
            XamlRoot = _xamlRootProvider.XamlRoot,
        };

        await _coordinator.ShowAsync(dialog);
    }
}
