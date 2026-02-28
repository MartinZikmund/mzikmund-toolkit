using Microsoft.UI.Dispatching;

namespace MZikmund.Toolkit.WinUI.Infrastructure;

/// <summary>
/// Represents the root shell of the application window.
/// </summary>
public interface IWindowShell
{
	/// <summary>
	/// Gets the XamlRoot of the shell.
	/// </summary>
	XamlRoot? XamlRoot { get; }

	/// <summary>
	/// Gets the service provider scoped to this window.
	/// </summary>
	IServiceProvider ServiceProvider { get; }

	/// <summary>
	/// Gets the dispatcher queue for the window's UI thread.
	/// </summary>
	DispatcherQueue DispatcherQueue { get; }

	/// <summary>
	/// Gets the root navigation frame.
	/// </summary>
	Frame RootFrame { get; }

	/// <summary>
	/// Sets the custom title bar element.
	/// </summary>
	/// <param name="titleBar">The title bar element, or null to reset.</param>
	void SetTitleBar(UIElement? titleBar);
}
