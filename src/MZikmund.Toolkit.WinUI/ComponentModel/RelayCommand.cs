using System.Windows.Input;

namespace MZikmund.Toolkit.WinUI.ComponentModel;

/// <summary>
/// Minimal <see cref="ICommand"/> implementation: parameterless execute + optional
/// <c>canExecute</c> predicate, with manual <see cref="RaiseCanExecuteChanged"/>.
/// Use this when you don't want to take a CommunityToolkit.Mvvm dependency.
/// </summary>
public sealed class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    /// <summary>Initializes a new instance.</summary>
    /// <param name="execute">Delegate invoked when the command runs.</param>
    /// <param name="canExecute">Optional predicate. Defaults to "always executable".</param>
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        ArgumentNullException.ThrowIfNull(execute);
        _execute = execute;
        _canExecute = canExecute;
    }

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged;

    /// <inheritdoc />
    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    /// <inheritdoc />
    public void Execute(object? parameter)
    {
        if (CanExecute(parameter))
        {
            _execute();
        }
    }

    /// <summary>
    /// Raises <see cref="CanExecuteChanged"/> so bound controls re-query
    /// <see cref="CanExecute(object?)"/>.
    /// </summary>
    public void RaiseCanExecuteChanged() =>
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
