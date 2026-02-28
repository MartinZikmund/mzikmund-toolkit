using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.Services.Localization;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Dialog service that maps ViewModel types to ContentDialog types by convention.
/// ViewModel names ending with "ViewModel" are mapped to dialog types by stripping the suffix.
/// </summary>
public class DialogService : IDialogService
{
	private readonly Dictionary<string, Type> _dialogs = new();
	private readonly IDialogCoordinator _dialogCoordinator;
	private readonly IXamlRootProvider _xamlRootProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="DialogService"/> class.
	/// </summary>
	/// <param name="dialogCoordinator">The dialog coordinator for queued dialog display.</param>
	/// <param name="xamlRootProvider">The XamlRoot provider.</param>
	public DialogService(IDialogCoordinator dialogCoordinator, IXamlRootProvider xamlRootProvider)
	{
		_dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
		_xamlRootProvider = xamlRootProvider ?? throw new ArgumentNullException(nameof(xamlRootProvider));
	}

	/// <inheritdoc/>
	public async Task<ContentDialogResult> ShowAsync(object viewModel)
	{
		var viewModelType = viewModel.GetType();
		if (!viewModelType.Name.EndsWith("ViewModel", StringComparison.OrdinalIgnoreCase))
		{
			throw new InvalidOperationException("ViewModel name must end with 'ViewModel' by convention.");
		}

		var viewModelName = viewModelType.Name;
		var dialogTypeName = viewModelName[..^"ViewModel".Length];

		if (!_dialogs.TryGetValue(dialogTypeName, out var dialogType))
		{
			throw new InvalidOperationException($"Dialog for {viewModelType.Name} not found. Register it first using RegisterDialog().");
		}

		var dialog = (ContentDialog?)Activator.CreateInstance(dialogType);
		if (dialog is null)
		{
			throw new InvalidOperationException($"Instance of {dialogType} could not be created.");
		}

		dialog.DataContext = viewModel;
		dialog.XamlRoot = _xamlRootProvider.XamlRoot;
		return await _dialogCoordinator.ShowAsync(dialog);
	}

	/// <inheritdoc/>
	public async Task<ContentDialogResult> ShowAsync(string title, string content)
	{
		var dialog = new ContentDialog
		{
			Title = title,
			Content = content,
			PrimaryButtonText = Localizer.Instance.GetString("Ok"),
			XamlRoot = _xamlRootProvider.XamlRoot,
		};

		return await _dialogCoordinator.ShowAsync(dialog);
	}

	/// <inheritdoc/>
	public async Task<ContentDialogResult> ShowAsync(ContentDialog contentDialog)
	{
		if (contentDialog.XamlRoot is null)
		{
			contentDialog.XamlRoot = _xamlRootProvider.XamlRoot;
		}

		return await _dialogCoordinator.ShowAsync(contentDialog);
	}

	/// <inheritdoc/>
	public void RegisterDialog(Type viewModelType, Type dialogType)
	{
		var viewModelName = viewModelType.Name;
		if (viewModelName.EndsWith("ViewModel", StringComparison.OrdinalIgnoreCase))
		{
			viewModelName = viewModelName[..^"ViewModel".Length];
		}

		_dialogs[viewModelName] = dialogType;
	}
}
