using CommunityToolkit.Mvvm.ComponentModel;

namespace MZikmund.Toolkit.WinUI.ViewModels;

/// <summary>
/// Base class for all page ViewModels. Provides lifecycle methods
/// that are called by <c>PageBase</c>.
/// </summary>
public abstract partial class ViewModelBase : ObservableObject
{
	/// <summary>
	/// Gets or sets a value indicating whether the page is loading.
	/// </summary>
	[ObservableProperty]
	public partial bool IsLoading { get; set; }

	/// <summary>
	/// Gets or sets the page title.
	/// </summary>
	[ObservableProperty]
	public partial string? PageTitle { get; set; }

	/// <summary>
	/// Called once when the View is first created and ViewModel is resolved.
	/// </summary>
	public virtual void ViewCreated() { }

	/// <summary>
	/// Called when the page is loading (before Loaded event).
	/// </summary>
	public virtual void ViewLoading() { }

	/// <summary>
	/// Called when the page has fully loaded.
	/// </summary>
	public virtual void ViewLoaded() { }

	/// <summary>
	/// Called when the page is unloaded.
	/// </summary>
	public virtual void ViewUnloaded() { }

	/// <summary>
	/// Called when the page is navigated to.
	/// </summary>
	/// <param name="parameter">Navigation parameter passed during navigation.</param>
	public virtual void OnNavigatedTo(object? parameter) { }

	/// <summary>
	/// Called when the page is navigated away from.
	/// </summary>
	public virtual void OnNavigatedFrom() { }
}
