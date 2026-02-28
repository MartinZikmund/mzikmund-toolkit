namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Manages display requests to keep the screen active.
/// </summary>
public interface IDisplayRequestManager
{
	/// <summary>
	/// Requests the display to stay active. Dispose the returned value to release the request.
	/// </summary>
	/// <returns>A disposable that releases the display request when disposed.</returns>
	IDisposable RequestActive();

	/// <summary>
	/// Clears all active display requests.
	/// </summary>
	void Clear();
}
