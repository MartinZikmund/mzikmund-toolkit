using System.Windows.Input;

namespace MZikmund.Toolkit.WinUI.ViewModels;

/// <summary>
/// Contract for the view-model that backs an <c>IWindowShell</c>. Exposed as an
/// interface so window-scoped services (loading indicators, navigation) can depend
/// on the abstraction rather than the concrete <see cref="WindowShellViewModel"/>.
/// </summary>
public interface IWindowShellViewModel
{
    /// <summary>The window title shown in the title bar / header.</summary>
    string Title { get; set; }

    /// <summary>Whether the shell is currently in a loading state.</summary>
    bool IsLoading { get; set; }

    /// <summary>The status message shown next to the loading indicator. May be null.</summary>
    string? LoadingStatusMessage { get; set; }

    /// <summary>Command bound to the shell's "go back" button.</summary>
    ICommand GoBackCommand { get; }
}
