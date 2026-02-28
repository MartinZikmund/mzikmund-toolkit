namespace MZikmund.Toolkit.WinUI.Navigation;

/// <summary>
/// Specifies navigation metadata for a page, such as the transition type.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class NavigationInfoAttribute : Attribute
{
	/// <summary>
	/// Gets the navigation transition to use when navigating to the decorated page.
	/// </summary>
	public NavigationTransition Transition { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="NavigationInfoAttribute"/> class.
	/// </summary>
	/// <param name="transition">The transition type for this page.</param>
	public NavigationInfoAttribute(NavigationTransition transition = NavigationTransition.Default)
	{
		Transition = transition;
	}
}
