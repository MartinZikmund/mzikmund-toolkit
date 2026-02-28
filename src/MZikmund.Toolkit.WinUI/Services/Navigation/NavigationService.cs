using System.Reflection;
using Microsoft.UI.Xaml.Media.Animation;
using MZikmund.Toolkit.WinUI.Navigation;

#if HAS_UNO
using Windows.UI.Core;
#endif

namespace MZikmund.Toolkit.WinUI.Services.Navigation;

/// <summary>
/// ViewModel-based navigation service that maps ViewModel types to Page types
/// and handles frame navigation with transition support.
/// </summary>
public sealed class NavigationService : INavigationService
{
	private readonly IWindowShellProvider _shellProvider;
	private readonly Dictionary<Type, Type> _viewModelToViewMap = new();
	private bool _initialized;
	private bool _backRequestedSubscribed;

	/// <summary>
	/// Initializes a new instance of the <see cref="NavigationService"/> class.
	/// </summary>
	/// <param name="shellProvider">The window shell provider.</param>
	public NavigationService(IWindowShellProvider shellProvider)
	{
		_shellProvider = shellProvider;
	}

	private Frame Frame => _shellProvider.RootFrame;

	/// <inheritdoc/>
	public bool CanGoBack => _initialized && Frame.CanGoBack;

	/// <inheritdoc/>
	public void Initialize()
	{
		_initialized = true;
#if HAS_UNO
		Frame.Navigated += OnFrameNavigated;
		UpdateBackRequestedSubscription();
#endif
	}

	/// <inheritdoc/>
	public void RegisterView(Type viewModelType, Type viewType)
		=> _viewModelToViewMap[viewModelType] = viewType;

	/// <inheritdoc/>
	public void Navigate<TViewModel>() => Navigate<TViewModel>(null);

	/// <inheritdoc/>
	public void Navigate<TViewModel>(object? parameter)
	{
		if (!_initialized)
		{
			throw new InvalidOperationException("NavigationService not initialized. Call Initialize() first.");
		}

		if (!_viewModelToViewMap.TryGetValue(typeof(TViewModel), out var viewType))
		{
			throw new InvalidOperationException($"No view registered for ViewModel {typeof(TViewModel).Name}.");
		}

		var navInfo = GetNavigationInfo(viewType);
		var transitionInfo = GetTransitionInfo(navInfo?.Transition ?? NavigationTransition.Default, isForward: true);
		Frame.Navigate(viewType, parameter, transitionInfo);

#if HAS_UNO
		UpdateBackRequestedSubscription();
#endif
	}

	/// <inheritdoc/>
	public bool GoBack()
	{
		if (!CanGoBack)
		{
			return false;
		}

		var backEntry = Frame.BackStack.LastOrDefault();
		var transition = NavigationTransition.Default;

		if (backEntry is not null)
		{
			var navInfo = GetNavigationInfo(backEntry.SourcePageType);
			transition = navInfo?.Transition ?? NavigationTransition.Default;
		}

		Frame.GoBack(GetTransitionInfo(transition, isForward: false));

#if HAS_UNO
		UpdateBackRequestedSubscription();
#endif

		return true;
	}

	/// <inheritdoc/>
	public void ClearBackStack()
	{
		Frame.BackStack.Clear();
#if HAS_UNO
		UpdateBackRequestedSubscription();
#endif
	}

	private static NavigationInfoAttribute? GetNavigationInfo(Type viewType)
	{
		var attr = viewType.GetCustomAttribute<NavigationInfoAttribute>();
		if (attr is not null)
		{
			return attr;
		}

		// Walk the inheritance chain - needed because the sealed view class
		// may inherit from an intermediate PageBase<T> that carries the attribute.
		var baseType = viewType.BaseType;
		while (baseType is not null && baseType != typeof(Page))
		{
			attr = baseType.GetCustomAttribute<NavigationInfoAttribute>();
			if (attr is not null)
			{
				return attr;
			}

			baseType = baseType.BaseType;
		}

		return null;
	}

	private static NavigationTransitionInfo GetTransitionInfo(NavigationTransition transition, bool isForward)
	{
		return transition switch
		{
			NavigationTransition.DrillIn => new DrillInNavigationTransitionInfo(),
			NavigationTransition.Entrance => new EntranceNavigationTransitionInfo(),
			NavigationTransition.Suppress => new SuppressNavigationTransitionInfo(),
			_ => new SlideNavigationTransitionInfo
			{
				Effect = isForward
					? SlideNavigationTransitionEffect.FromRight
					: SlideNavigationTransitionEffect.FromLeft,
			},
		};
	}

#if HAS_UNO
	private void OnFrameNavigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
	{
		UpdateBackRequestedSubscription();
	}

	/// <summary>
	/// Manages BackRequested subscription. On Android 16+, subscribing/unsubscribing
	/// (not just Handled) controls whether the app or system handles the back gesture.
	/// </summary>
	private void UpdateBackRequestedSubscription()
	{
		var manager = SystemNavigationManager.GetForCurrentView();
		if (Frame.CanGoBack && !_backRequestedSubscribed)
		{
			manager.BackRequested += OnBackRequested;
			_backRequestedSubscribed = true;
		}
		else if (!Frame.CanGoBack && _backRequestedSubscribed)
		{
			manager.BackRequested -= OnBackRequested;
			_backRequestedSubscribed = false;
		}
	}

	private void OnBackRequested(object? sender, BackRequestedEventArgs e)
	{
		if (GoBack())
		{
			e.Handled = true;
		}
	}
#endif
}
