using Windows.System.Display;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Reference-counted display request manager with generation tracking.
/// Each <see cref="RequestActive"/> call increments the count and returns a disposable
/// that decrements it. <see cref="Clear"/> resets all requests and invalidates
/// outstanding disposables via generation tracking.
/// </summary>
internal class DisplayRequestManager : IDisplayRequestManager
{
	private int _activeRequestCount;
	private int _generation;

	private readonly DisplayRequest _displayRequest = new();

	/// <inheritdoc/>
	public IDisposable RequestActive()
	{
		var currentGeneration = _generation;
		_activeRequestCount++;
		_displayRequest.RequestActive();
		return new ActionDisposable(() =>
		{
			// Ensure we did not Clear in-between.
			if (_generation == currentGeneration)
			{
				_activeRequestCount--;
				_displayRequest.RequestRelease();
			}
		});
	}

	/// <inheritdoc/>
	public void Clear()
	{
		while (_activeRequestCount > 0)
		{
			_displayRequest.RequestRelease();
			_activeRequestCount--;
		}

		_generation++;
	}

	private sealed class ActionDisposable : IDisposable
	{
		private Action? _action;

		public ActionDisposable(Action action) => _action = action;

		public void Dispose()
		{
			var action = Interlocked.Exchange(ref _action, null);
			action?.Invoke();
		}
	}
}
