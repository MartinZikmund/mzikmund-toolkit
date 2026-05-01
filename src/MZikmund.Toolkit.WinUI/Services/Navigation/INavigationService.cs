using MZikmund.Toolkit.WinUI.Navigation;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Frame-based navigation service that consumes <see cref="NavigationInfoAttribute"/>
/// metadata on page types: section tag for highlighting and transition for animation.
/// </summary>
public interface INavigationService
{
    /// <summary>The section tag of the most recently navigated-to page, or <see langword="null"/> if untagged.</summary>
    string? CurrentSection { get; }

    /// <summary><see langword="true"/> when the underlying frame can navigate back.</summary>
    bool CanGoBack { get; }

    /// <summary>Raised after a successful navigation.</summary>
    event EventHandler? Navigated;

    /// <summary>
    /// Navigates the shell's root frame to <paramref name="pageType"/>, applying the
    /// transition declared on its <see cref="NavigationInfoAttribute"/> (if any).
    /// </summary>
    /// <returns><see langword="true"/> if the navigation started.</returns>
    bool Navigate(Type pageType, object? parameter = null);

    /// <summary>Navigates back if <see cref="CanGoBack"/> is true.</summary>
    /// <returns><see langword="true"/> if a back navigation occurred.</returns>
    bool GoBack();
}
