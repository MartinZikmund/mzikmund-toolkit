using MZikmund.Toolkit.WinUI.ComponentModel;

namespace MZikmund.Toolkit.WinUI.ViewModels;

/// <summary>
/// Base class for view-models that participate in page lifecycle. Provides
/// observable <see cref="IsLoading"/> and <see cref="PageTitle"/> properties plus
/// virtual hooks for view creation, loading, navigation, and unloading.
/// </summary>
/// <remarks>
/// Apps wire the lifecycle methods from their pages, e.g. in <c>OnNavigatedTo</c>:
/// <code>
/// protected override void OnNavigatedTo(NavigationEventArgs e)
/// {
///     base.OnNavigatedTo(e);
///     ViewModel.OnNavigatedTo(e.Parameter);
/// }
/// </code>
/// </remarks>
public abstract class ViewModelBase : ObservableObject
{
    private bool _isLoading;
    private string _pageTitle = string.Empty;

    /// <summary>Whether the view-model is currently performing async work.</summary>
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    /// <summary>Page title shown in the shell title bar / header.</summary>
    public string PageTitle
    {
        get => _pageTitle;
        set => SetProperty(ref _pageTitle, value);
    }

    /// <summary>Called when the page hosting this view-model is constructed.</summary>
    public virtual void ViewCreated()
    {
    }

    /// <summary>Called when the page is about to be shown.</summary>
    public virtual void ViewLoading()
    {
    }

    /// <summary>Called after the page's <c>Loaded</c> event has fired.</summary>
    public virtual void ViewLoaded()
    {
    }

    /// <summary>Called when the page's <c>Unloaded</c> event fires.</summary>
    public virtual void ViewUnloaded()
    {
    }

    /// <summary>Called when the frame navigates to the page hosting this view-model.</summary>
    /// <param name="parameter">The navigation parameter passed to <c>Frame.Navigate</c>.</param>
    public virtual void OnNavigatedTo(object? parameter)
    {
    }

    /// <summary>Called when the frame navigates away from the page hosting this view-model.</summary>
    public virtual void OnNavigatedFrom()
    {
    }
}
