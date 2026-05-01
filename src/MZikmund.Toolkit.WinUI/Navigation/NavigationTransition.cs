namespace MZikmund.Toolkit.WinUI.Navigation;

/// <summary>
/// Page-to-page transition style used by <see cref="NavigationInfoAttribute"/> and
/// the navigation service. Maps to a <c>NavigationTransitionInfo</c> at navigate time.
/// </summary>
public enum NavigationTransition
{
    /// <summary>Use the frame's default transition.</summary>
    Default = 0,

    /// <summary>Suppress the transition animation.</summary>
    Suppress,

    /// <summary>Slide in from the right (typical "drill-down").</summary>
    FromRight,

    /// <summary>Slide in from the left.</summary>
    FromLeft,

    /// <summary>Slide in from the bottom (typical "modal sheet").</summary>
    FromBottom,
}
