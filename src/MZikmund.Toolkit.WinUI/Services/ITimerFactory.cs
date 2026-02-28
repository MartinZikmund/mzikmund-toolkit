using Microsoft.UI.Dispatching;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Factory for creating UI-thread-bound dispatcher queue timers.
/// </summary>
public interface ITimerFactory
{
	/// <summary>
	/// Creates a new <see cref="DispatcherQueueTimer"/> bound to the UI thread.
	/// </summary>
	/// <returns>A new dispatcher queue timer.</returns>
	DispatcherQueueTimer Create();
}
