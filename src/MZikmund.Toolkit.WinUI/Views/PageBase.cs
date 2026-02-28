using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Navigation;
using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.ViewModels;

namespace MZikmund.Toolkit.WinUI.Views;

/// <summary>
/// Generic base class for all pages. Provides automatic ViewModel resolution
/// from the <see cref="IWindowShell"/> service provider and lifecycle management.
/// </summary>
/// <typeparam name="TViewModel">The ViewModel type for this page.</typeparam>
public abstract partial class PageBase<TViewModel> : Page
	where TViewModel : ViewModelBase
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PageBase{TViewModel}"/> class.
	/// </summary>
	protected PageBase()
	{
		Loading += OnPageLoading;
		Loaded += OnPageLoaded;
		Unloaded += OnPageUnloaded;
	}

	/// <summary>
	/// Gets the ViewModel for this page. Resolved from DI on first access.
	/// </summary>
	public TViewModel? ViewModel { get; private set; }

	[MemberNotNull(nameof(ViewModel))]
	private void EnsureViewModel()
	{
		if (ViewModel is not null)
		{
			return;
		}

		if (FindWindowShell(Frame.XamlRoot?.Content) is not IWindowShell windowShell)
		{
			throw new InvalidOperationException("View must be hosted inside an IWindowShell implementation.");
		}

		ViewModel = windowShell.ServiceProvider.GetRequiredService<TViewModel>();
		DataContext = ViewModel;
		ViewModel.ViewCreated();
	}

	private static IWindowShell? FindWindowShell(UIElement? windowRoot)
	{
		if (windowRoot is IWindowShell shell)
		{
			return shell;
		}

		// This happens when Hot Design takes over the root.
		if (windowRoot is ContentControl { Content: IWindowShell contentShell })
		{
			return contentShell;
		}

		return null;
	}

	private void OnPageLoading(FrameworkElement sender, object args)
	{
		EnsureViewModel();
		ViewModel.ViewLoading();
	}

	private void OnPageLoaded(object sender, RoutedEventArgs e)
	{
		ViewModel?.ViewLoaded();
	}

	private void OnPageUnloaded(object sender, RoutedEventArgs e)
	{
		ViewModel?.ViewUnloaded();
	}

	/// <inheritdoc/>
	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		base.OnNavigatedTo(e);
		EnsureViewModel();
		ViewModel.OnNavigatedTo(e.Parameter);
	}

	/// <inheritdoc/>
	protected override void OnNavigatedFrom(NavigationEventArgs e)
	{
		base.OnNavigatedFrom(e);
		ViewModel?.OnNavigatedFrom();
	}
}
