using System.Windows.Input;
using MZikmund.Toolkit.WinUI.ComponentModel;

namespace MZikmund.Toolkit.WinUI.ViewModels;

/// <summary>
/// Default <see cref="IWindowShellViewModel"/> implementation. Subclass to add
/// app-specific properties (user info, sync indicators, etc.).
/// </summary>
public class WindowShellViewModel : ViewModelBase, IWindowShellViewModel
{
    private string _title = string.Empty;
    private string? _loadingStatusMessage;
    private readonly RelayCommand _goBackCommand;

    /// <summary>Initializes a new instance.</summary>
    public WindowShellViewModel()
    {
        _goBackCommand = new RelayCommand(() => GoBackRequested?.Invoke(this, EventArgs.Empty));
    }

    /// <summary>The window title shown in the title bar / header.</summary>
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    /// <summary>Status message shown next to the loading indicator. May be null.</summary>
    public string? LoadingStatusMessage
    {
        get => _loadingStatusMessage;
        set => SetProperty(ref _loadingStatusMessage, value);
    }

    /// <summary>
    /// Command bound to the shell's "go back" button. Raises <see cref="GoBackRequested"/>
    /// when executed; the shell handles the actual navigation.
    /// </summary>
    public ICommand GoBackCommand => _goBackCommand;

    /// <summary>
    /// Raised when <see cref="GoBackCommand"/> is invoked. The shell page should
    /// subscribe and call <c>Frame.GoBack()</c>.
    /// </summary>
    public event EventHandler? GoBackRequested;

    /// <summary>
    /// Notifies bound controls to re-query <see cref="GoBackCommand"/> after
    /// frame state changes (e.g. when the frame's <c>CanGoBack</c> flips).
    /// </summary>
    public void RaiseGoBackChanged() => _goBackCommand.RaiseCanExecuteChanged();
}
