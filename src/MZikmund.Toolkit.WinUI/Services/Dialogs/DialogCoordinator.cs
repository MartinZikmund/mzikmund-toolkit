using EventCountdowns.Services.Navigation;
using MZikmund.Toolkit.WinUI.Infrastructure;

namespace MZikmund.Services.Dialogs;

/// <summary>
/// Allows coordination of content dialog display.
/// This is needed to ensure that only one dialog is shown at a time.
/// Otherwise WinUI throws an exception.
/// </summary>
public class DialogCoordinator : IDialogCoordinator
{
	private readonly Queue<QueuedDialog> _dialogQueue = new();
    private bool _isProcessing;

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogCoordinator"/> class.
    /// </summary>
    public DialogCoordinator()
	{
    }

    /// <summary>
    /// Shows a content dialog.
    /// </summary>
    /// <param name="dialog">Dialog to show.</param>
    /// <returns>Result of the dialog.</returns>
    public async Task<ContentDialogResult> ShowAsync(ContentDialog dialog)
	{
		if (dialog is null)
		{
			throw new ArgumentNullException(nameof(dialog));
		}

		if (dialog.XamlRoot is null)
		{
			throw new InvalidOperationException("Dialog must have XamlRoot set before showing.");
		}

		var queuedDialog = EnqueueDialog(dialog);
		await ProcessQueueAsync();
		return await queuedDialog.CompletionSource.Task;
	}

	private QueuedDialog EnqueueDialog(ContentDialog dialog)
	{
		var queuedDialog = new QueuedDialog(dialog);
		_dialogQueue.Enqueue(queuedDialog);
		return queuedDialog;
	}

	private async Task ProcessQueueAsync()
	{
		if (!_isProcessing)
		{
			try
			{
				_isProcessing = true;
				while (_dialogQueue.Count > 0)
				{
					var queuedDialog = _dialogQueue.Dequeue();
					var result = await queuedDialog.Dialog.ShowAsync();
					queuedDialog.CompletionSource.SetResult(result);
				}
			}
			finally
			{
				_isProcessing = false;
			}
		}
	}
}
