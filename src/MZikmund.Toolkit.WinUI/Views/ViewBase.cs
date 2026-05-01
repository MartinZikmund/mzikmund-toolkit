using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.ViewModels;

namespace MZikmund.Toolkit.WinUI.Views;

/// <summary>
/// Generic <see cref="Page"/> base that resolves <typeparamref name="TViewModel"/> from the
/// hosting <see cref="IWindowShell"/>'s scoped <see cref="IServiceProvider"/>, sets it as the
/// page's <c>DataContext</c>, and wires page lifecycle events (<c>Loaded</c>, <c>Unloaded</c>,
/// <c>OnNavigatedTo</c>, <c>OnNavigatedFrom</c>) to the matching hooks on
/// <see cref="ViewModelBase"/>.
/// </summary>
/// <typeparam name="TViewModel">View-model type. Must derive from <see cref="ViewModelBase"/>.</typeparam>
/// <remarks>
/// The view-model is resolved on the first <c>Loaded</c> event because the visual tree
/// (and thus the <see cref="IWindowShell"/> ancestor) isn't reachable yet at construction
/// time. Navigation parameters that arrive before <c>Loaded</c> are buffered and replayed
/// on the view-model when it becomes available.
/// </remarks>
public abstract partial class ViewBase<TViewModel> : Page where TViewModel : ViewModelBase
{
    private TViewModel? _viewModel;
    private object? _pendingNavigationParameter;
    private bool _hasPendingNavigation;

    /// <summary>Initializes a new instance and wires the page's <c>Loaded</c> / <c>Unloaded</c> events.</summary>
    protected ViewBase()
    {
        Loaded += OnLoadedInternal;
        Unloaded += OnUnloadedInternal;
    }

    /// <summary>
    /// The view-model bound to this page. Available after the first <c>Loaded</c> event.
    /// Throws if accessed before resolution.
    /// </summary>
    public TViewModel ViewModel =>
        _viewModel ?? throw new InvalidOperationException(
            $"{GetType().Name}.ViewModel is not yet resolved. Wait for the page's Loaded event.");

    /// <inheritdoc />
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (_viewModel is not null)
        {
            _viewModel.OnNavigatedTo(e.Parameter);
        }
        else
        {
            _pendingNavigationParameter = e.Parameter;
            _hasPendingNavigation = true;
        }
    }

    /// <inheritdoc />
    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        _viewModel?.OnNavigatedFrom();
    }

    private void OnLoadedInternal(object sender, RoutedEventArgs e)
    {
        if (_viewModel is null)
        {
            _viewModel = ResolveViewModel();
            DataContext = _viewModel;
            _viewModel.ViewCreated();
        }

        _viewModel.ViewLoading();

        if (_hasPendingNavigation)
        {
            _viewModel.OnNavigatedTo(_pendingNavigationParameter);
            _hasPendingNavigation = false;
            _pendingNavigationParameter = null;
        }

        _viewModel.ViewLoaded();
    }

    private void OnUnloadedInternal(object sender, RoutedEventArgs e) =>
        _viewModel?.ViewUnloaded();

    private TViewModel ResolveViewModel()
    {
        var services = FindShellServiceProvider()
            ?? throw new InvalidOperationException(
                $"{GetType().Name}: no IWindowShell ancestor found. Pages deriving from ViewBase<T> must be hosted under a shell that implements IWindowShell.");

        return (TViewModel?)services.GetService(typeof(TViewModel))
            ?? throw new InvalidOperationException(
                $"{GetType().Name}: type {typeof(TViewModel).FullName} is not registered in the shell's IServiceProvider.");
    }

    private IServiceProvider? FindShellServiceProvider()
    {
        DependencyObject? current = this;
        while (current is not null)
        {
            if (current is IWindowShell shell)
            {
                return shell.ServiceProvider;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return null;
    }
}
