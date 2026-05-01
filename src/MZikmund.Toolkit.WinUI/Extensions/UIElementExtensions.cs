using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using MZikmund.Toolkit.WinUI.Infrastructure;

namespace MZikmund.Toolkit.WinUI.Extensions;

/// <summary>
/// Extensions for <see cref="UIElement"/> that bridge into toolkit infrastructure.
/// </summary>
public static class UIElementExtensions
{
    /// <summary>
    /// Walks up the visual tree from <paramref name="element"/> looking for an ancestor
    /// (or the element itself) that implements <see cref="IWindowShell"/>, and returns
    /// that shell's <see cref="IWindowShell.ServiceProvider"/>.
    /// </summary>
    /// <param name="element">Starting element.</param>
    /// <returns>The DI scope tied to the owning shell.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">No <see cref="IWindowShell"/> ancestor was found.</exception>
    /// <remarks>
    /// Useful in event handlers, attached behaviors, and any other code-behind path where
    /// constructor injection isn't available — you have a UI element in hand and need to
    /// reach the same DI scope the rest of the window uses.
    /// </remarks>
    public static IServiceProvider GetServiceProvider(this UIElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.TryGetServiceProvider()
            ?? throw new InvalidOperationException(
                "No IWindowShell ancestor was found in the visual tree. Ensure the element is hosted inside a window shell that implements IWindowShell.");
    }

    /// <summary>
    /// Non-throwing variant of <see cref="GetServiceProvider(UIElement)"/>. Returns
    /// <see langword="null"/> when no <see cref="IWindowShell"/> ancestor is found, which
    /// can happen during construction or when the element is detached.
    /// </summary>
    public static IServiceProvider? TryGetServiceProvider(this UIElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        DependencyObject? current = element;
        while (current is not null)
        {
            if (current is IWindowShell shell)
            {
                return shell.ServiceProvider;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return null;
    }
}
