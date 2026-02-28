using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Dispatching;
using MZikmund.Toolkit.WinUI.Infrastructure;

namespace MZikmund.Toolkit.WinUI.Services.Navigation;

/// <summary>
/// Default implementation of <see cref="IWindowShellProvider"/> and <see cref="IXamlRootProvider"/>.
/// Must be initialized by calling <see cref="SetShell"/> during app startup.
/// </summary>
public sealed class WindowShellProvider : IWindowShellProvider, IXamlRootProvider
{
	private IWindowShell? _shell;
	private Window? _window;
	private DispatcherQueue? _dispatcherQueue;

	/// <summary>
	/// Initializes the provider with the window shell and window instances.
	/// </summary>
	/// <param name="window">The application window.</param>
	/// <param name="shell">The window shell.</param>
	public void SetShell(Window window, IWindowShell shell)
	{
		_shell = shell ?? throw new ArgumentNullException(nameof(shell));
		_window = window ?? throw new ArgumentNullException(nameof(window));
		_dispatcherQueue = shell.DispatcherQueue;
	}

	/// <inheritdoc/>
	public IWindowShell Shell { get { EnsureInitialized(); return _shell; } }

	/// <inheritdoc/>
	public Window Window { get { EnsureInitialized(); return _window; } }

	/// <inheritdoc/>
	public DispatcherQueue DispatcherQueue { get { EnsureInitialized(); return _dispatcherQueue; } }

	/// <inheritdoc/>
	public Frame RootFrame { get { EnsureInitialized(); return _shell.RootFrame; } }

	/// <inheritdoc/>
	public XamlRoot XamlRoot { get { EnsureInitialized(); return _shell.XamlRoot!; } }

	[MemberNotNull(nameof(_shell))]
	[MemberNotNull(nameof(_window))]
	[MemberNotNull(nameof(_dispatcherQueue))]
	private void EnsureInitialized()
	{
		if (_shell is null || _dispatcherQueue is null || _window is null)
		{
			throw new InvalidOperationException("WindowShellProvider was not initialized. Call SetShell() first.");
		}
	}
}
