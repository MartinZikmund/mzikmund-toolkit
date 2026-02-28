namespace MZikmund.Toolkit.WinUI.Services.Navigation;

/// <summary>
/// Provides ViewModel-based frame navigation.
/// </summary>
public interface INavigationService
{
	/// <summary>
	/// Gets a value indicating whether the navigation service can go back.
	/// </summary>
	bool CanGoBack { get; }

	/// <summary>
	/// Initializes the navigation service. Must be called after the frame is available.
	/// </summary>
	void Initialize();

	/// <summary>
	/// Navigates to the page associated with the specified ViewModel type.
	/// </summary>
	/// <typeparam name="TViewModel">The ViewModel type to navigate to.</typeparam>
	void Navigate<TViewModel>();

	/// <summary>
	/// Navigates to the page associated with the specified ViewModel type with a parameter.
	/// </summary>
	/// <typeparam name="TViewModel">The ViewModel type to navigate to.</typeparam>
	/// <param name="parameter">The navigation parameter.</param>
	void Navigate<TViewModel>(object? parameter);

	/// <summary>
	/// Navigates back to the previous page.
	/// </summary>
	/// <returns>True if navigation occurred; false if there was no back stack entry.</returns>
	bool GoBack();

	/// <summary>
	/// Clears the back stack.
	/// </summary>
	void ClearBackStack();

	/// <summary>
	/// Registers a ViewModel-to-View mapping.
	/// </summary>
	/// <param name="viewModelType">The ViewModel type.</param>
	/// <param name="viewType">The View (Page) type.</param>
	void RegisterView(Type viewModelType, Type viewType);
}
