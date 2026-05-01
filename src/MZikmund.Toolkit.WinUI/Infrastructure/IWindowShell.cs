using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MZikmund.Toolkit.WinUI.Infrastructure;

/// <summary>
/// Generic abstraction for the per-window shell (the visual chrome plus the services
/// scoped to it). Apps implement this on their actual window shell page; the toolkit
/// uses it where it needs window-scoped access without depending on app-specific types.
/// </summary>
public interface IWindowShell
{
    /// <summary>The view-model bound to this shell. Apps choose the concrete type.</summary>
    object? ViewModel { get; }

    /// <summary>The active <see cref="XamlRoot"/> for this window.</summary>
    XamlRoot XamlRoot { get; }

    /// <summary>Per-window service provider scope. Pulled from the host's DI container.</summary>
    IServiceProvider ServiceProvider { get; }

    /// <summary>The <see cref="DispatcherQueue"/> that owns this window's UI thread.</summary>
    DispatcherQueue DispatcherQueue { get; }

    /// <summary>The frame that hosts top-level page navigation inside the shell.</summary>
    Frame RootFrame { get; }

    /// <summary>
    /// Sets the custom title-bar element for this window (passes through to
    /// <see cref="Window.SetTitleBar"/> on the owning window).
    /// </summary>
    void SetTitleBar(UIElement titleBar);
}
