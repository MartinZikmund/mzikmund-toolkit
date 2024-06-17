namespace MZikmund.Toolkit.WinUI.Services;

internal record QueuedDialog(ContentDialog Dialog)
{
	public TaskCompletionSource<ContentDialogResult> CompletionSource { get; } = new();
}
