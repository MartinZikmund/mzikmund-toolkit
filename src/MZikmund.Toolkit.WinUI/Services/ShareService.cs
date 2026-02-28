using System.Runtime.InteropServices;
using MZikmund.Toolkit.WinUI.Services.Navigation;
using Windows.ApplicationModel.DataTransfer;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Shares content via the OS native share dialog using DataTransferManager.
/// Uses COM interop on WinUI desktop and standard UWP approach on Uno targets.
/// </summary>
public class ShareService : IShareService
{
	private readonly IWindowShellProvider _windowShellProvider;

	private string? _pendingTitle;
	private string? _pendingUri;

	/// <summary>
	/// Initializes a new instance of the <see cref="ShareService"/> class.
	/// </summary>
	/// <param name="windowShellProvider">The window shell provider.</param>
	public ShareService(IWindowShellProvider windowShellProvider)
	{
		_windowShellProvider = windowShellProvider;
	}

	/// <inheritdoc/>
	public Task ShareAsync(string title, string uri)
	{
		try
		{
			_pendingTitle = title;
			_pendingUri = uri;

#if !HAS_UNO
			var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(_windowShellProvider.Window);

			IDataTransferManagerInterop interop =
				DataTransferManager.As<IDataTransferManagerInterop>();

			IntPtr result = interop.GetForWindow(hwnd, _dtm_iid);
			var dataTransferManager =
				WinRT.MarshalInterface<DataTransferManager>.FromAbi(result);

			dataTransferManager.DataRequested += OnDataRequested;

			interop.ShowShareUIForWindow(hwnd);
#else
			var dataTransferManager = DataTransferManager.GetForCurrentView();
			dataTransferManager.DataRequested += OnDataRequested;
			DataTransferManager.ShowShareUI();
#endif
		}
		catch (COMException)
		{
			// Share failure is non-critical (e.g., no share targets available)
		}

		return Task.CompletedTask;
	}

	private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
	{
		sender.DataRequested -= OnDataRequested;

		args.Request.Data.Properties.Title = _pendingTitle ?? string.Empty;
		if (_pendingUri is not null)
		{
			args.Request.Data.SetWebLink(new Uri(_pendingUri));
			args.Request.Data.SetText(_pendingUri);
		}

		_pendingTitle = null;
		_pendingUri = null;
	}

#if !HAS_UNO
	[ComImport]
	[Guid("3A3DCD6C-3EAB-43DC-BCDE-45671CE800C8")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	private interface IDataTransferManagerInterop
	{
		IntPtr GetForWindow([In] IntPtr appWindow, [In] ref Guid riid);
		void ShowShareUIForWindow(IntPtr appWindow);
	}

	private static readonly Guid _dtm_iid =
		new(0xa5caee9b, 0x8708, 0x49d1, 0x8d, 0x36, 0x67, 0xd2, 0x5a, 0x8d, 0xa0, 0x0c);
#endif
}
