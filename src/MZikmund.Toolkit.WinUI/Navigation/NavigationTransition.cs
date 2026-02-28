namespace MZikmund.Toolkit.WinUI.Navigation;

/// <summary>
/// Defines the types of navigation transitions available.
/// </summary>
public enum NavigationTransition
{
	/// <summary>
	/// Default slide transition.
	/// </summary>
	Default,

	/// <summary>
	/// Slide transition (left/right).
	/// </summary>
	Slide,

	/// <summary>
	/// Drill-in transition for hierarchical navigation.
	/// </summary>
	DrillIn,

	/// <summary>
	/// Entrance transition for initial page display.
	/// </summary>
	Entrance,

	/// <summary>
	/// Suppresses the navigation transition animation.
	/// </summary>
	Suppress,
}
