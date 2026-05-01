namespace MZikmund.Toolkit.WinUI.Navigation;

/// <summary>
/// Tags a page with metadata the navigation service uses: which logical "section" it
/// belongs to (for highlighting NavigationView items, etc.) and which transition to play
/// when navigating to it.
/// </summary>
/// <remarks>
/// <para>The toolkit uses string section tags rather than a hard-coded enum so apps
/// own their own section taxonomy. A typical app declares a static helper class like:</para>
/// <code>
/// public static class AppSections
/// {
///     public const string Main = "main";
///     public const string Settings = "settings";
/// }
/// </code>
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class NavigationInfoAttribute : Attribute
{
    /// <summary>Initializes a new instance.</summary>
    /// <param name="sectionTag">Logical section the page belongs to.</param>
    /// <param name="transition">Transition to play when navigating to the page.</param>
    public NavigationInfoAttribute(string sectionTag, NavigationTransition transition = NavigationTransition.Default)
    {
        ArgumentNullException.ThrowIfNull(sectionTag);
        SectionTag = sectionTag;
        Transition = transition;
    }

    /// <summary>Logical section tag.</summary>
    public string SectionTag { get; }

    /// <summary>Transition to play when navigating to the decorated page.</summary>
    public NavigationTransition Transition { get; }
}
