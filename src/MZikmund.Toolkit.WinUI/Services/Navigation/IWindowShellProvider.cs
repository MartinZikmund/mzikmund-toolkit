using Microsoft.UI.Dispatching;
using MZikmund.Toolkit.WinUI.Infrastructure;

namespace MZikmund.Toolkit.WinUI.Services.Navigation;

/// <summary>
/// Provides access to the application's window shell and related properties.
/// </summary>
public interface IWindowShellProvider
{
	/// <summary>
	/// Gets the application window.
	/// </summary>
	Window Window { get; }

	/// <summary>
	/// Gets the window shell.
	/// </summary>
	IWindowShell Shell { get; }

	/// <summary>
	/// Gets the dispatcher queue for the window's UI thread.
	/// </summary>
	DispatcherQueue DispatcherQueue { get; }

	/// <summary>
	/// Gets the root navigation frame.
	/// </summary>
	Frame RootFrame { get; }
}
