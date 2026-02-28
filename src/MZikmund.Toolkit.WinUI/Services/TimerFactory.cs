using Microsoft.UI.Dispatching;
using MZikmund.Toolkit.WinUI.Services.Navigation;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Creates <see cref="DispatcherQueueTimer"/> instances from the window's dispatcher queue.
/// </summary>
public class TimerFactory : ITimerFactory
{
	private readonly IWindowShellProvider _provider;

	/// <summary>
	/// Initializes a new instance of the <see cref="TimerFactory"/> class.
	/// </summary>
	/// <param name="provider">The window shell provider.</param>
	public TimerFactory(IWindowShellProvider provider)
	{
		_provider = provider;
	}

	/// <inheritdoc/>
	public DispatcherQueueTimer Create() => _provider.DispatcherQueue.CreateTimer();
}
