using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MZikmund.Toolkit.WinUI.ComponentModel;

/// <summary>
/// Minimal <see cref="INotifyPropertyChanged"/> base with a <see cref="SetProperty{T}"/>
/// helper. Pulled in by <see cref="ViewModels.ViewModelBase"/> so the toolkit doesn't
/// force callers to take a CommunityToolkit.Mvvm dependency for the basics.
/// </summary>
public abstract class ObservableObject : INotifyPropertyChanged
{
    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises <see cref="PropertyChanged"/> for <paramref name="propertyName"/>.
    /// </summary>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    /// Assigns <paramref name="value"/> to <paramref name="field"/> if they differ and
    /// raises <see cref="PropertyChanged"/>.
    /// </summary>
    /// <returns><see langword="true"/> if the value changed.</returns>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
