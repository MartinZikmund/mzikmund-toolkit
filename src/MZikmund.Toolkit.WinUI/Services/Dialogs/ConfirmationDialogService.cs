using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.Services.Localization;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Shows a confirmation dialog with localized Yes/No buttons.
/// Requires "Yes" and "No" resource keys in the consuming app's string resources.
/// </summary>
public sealed class ConfirmationDialogService : IConfirmationDialogService
{
	private readonly IDialogCoordinator _dialogCoordinator;
	private readonly IXamlRootProvider _xamlRootProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="ConfirmationDialogService"/> class.
	/// </summary>
	/// <param name="dialogCoordinator">The dialog coordinator for queued dialog display.</param>
	/// <param name="xamlRootProvider">The XamlRoot provider.</param>
	public ConfirmationDialogService(IDialogCoordinator dialogCoordinator, IXamlRootProvider xamlRootProvider)
	{
		_dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
		_xamlRootProvider = xamlRootProvider ?? throw new ArgumentNullException(nameof(xamlRootProvider));
	}

	/// <inheritdoc/>
	public async Task<ConfirmationResult> ShowAsync(string title, string text)
	{
		var dialog = new ContentDialog
		{
			Title = title,
			Content = text,
			PrimaryButtonText = Localizer.Instance.GetString("Yes"),
			SecondaryButtonText = Localizer.Instance.GetString("No"),
			DefaultButton = ContentDialogButton.Secondary,
			XamlRoot = _xamlRootProvider.XamlRoot,
		};

		var result = await _dialogCoordinator.ShowAsync(dialog);
		return result == ContentDialogResult.Primary ? ConfirmationResult.Confirmed : ConfirmationResult.Denied;
	}
}
