namespace MZikmund.Services.Dialogs;

internal record QueuedDialog(ContentDialog Dialog)
{
	public TaskCompletionSource<ContentDialogResult> CompletionSource { get; } = new();
}
