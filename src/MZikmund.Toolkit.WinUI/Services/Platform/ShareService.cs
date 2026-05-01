using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="IShareService"/> implementation. Listens for <c>DataRequested</c>
/// once, populates the request, and triggers the system share UI. Subclasses override
/// <see cref="GetDataTransferManager"/> and <see cref="ShowShareUI"/> to plug in the
/// WinAppSDK COM-interop dance on Windows (where <c>DataTransferManager.GetForCurrentView</c>
/// is unavailable).
/// </summary>
/// <remarks>
/// Uno's cross-platform implementation of <see cref="DataTransferManager"/> works on
/// Android / iOS / macCatalyst / WASM out of the box. On WinAppSDK desktop, override
/// the two protected hooks to call <c>DataTransferManagerInterop.GetForWindow</c> /
/// <c>ShowShareUIForWindow</c> with the active window handle.
/// </remarks>
public class ShareService : IShareService
{
    /// <inheritdoc />
    public Task ShareTextAsync(string title, string text, Uri? webLink = null)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(text);

        var dtm = GetDataTransferManager();
        var tcs = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

        TypedEventHandler<DataTransferManager, DataRequestedEventArgs> handler = null!;
        handler = (sender, args) =>
        {
            args.Request.Data.Properties.Title = title;
            args.Request.Data.SetText(text);
            if (webLink is not null)
            {
                args.Request.Data.SetWebLink(webLink);
            }

            sender.DataRequested -= handler;
            tcs.TrySetResult(null);
        };

        dtm.DataRequested += handler;

        try
        {
            ShowShareUI();
        }
        catch
        {
            dtm.DataRequested -= handler;
            throw;
        }

        return tcs.Task;
    }

    /// <summary>
    /// Returns the <see cref="DataTransferManager"/> that hosts the share. Default
    /// implementation calls <see cref="DataTransferManager.GetForCurrentView"/>, which
    /// works on Uno cross-platform targets but not on WinAppSDK desktop. Override on
    /// Windows to use <c>DataTransferManagerInterop.GetForWindow(hWnd)</c>.
    /// </summary>
    protected virtual DataTransferManager GetDataTransferManager() =>
        DataTransferManager.GetForCurrentView();

    /// <summary>
    /// Shows the system share UI. Default implementation calls
    /// <see cref="DataTransferManager.ShowShareUI"/>. Override on WinAppSDK desktop to
    /// use <c>DataTransferManagerInterop.ShowShareUIForWindow(hWnd)</c>.
    /// </summary>
    protected virtual void ShowShareUI() => DataTransferManager.ShowShareUI();
}
