using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Xaml;

namespace MZikmund.Toolkit.WinUI.Resources;

/// <summary>
/// Type-safe lookup of resources defined in <c>App.xaml</c> or any merged dictionary
/// reachable through <see cref="Application.Current"/>.
/// </summary>
/// <remarks>
/// Useful when XAML's <c>{StaticResource}</c> markup isn't available — for example, in
/// behaviors, converters, or code-behind helpers that need a brush, style, or string
/// from the application resource scope.
/// </remarks>
public static class ResourceAccessor
{
    /// <summary>
    /// Looks up the resource with the given <paramref name="key"/> and casts it to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Expected resource type.</typeparam>
    /// <param name="key">Resource key as declared in XAML (<c>x:Key</c>).</param>
    /// <returns>The resource cast to <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidOperationException">No <see cref="Application.Current"/> is available.</exception>
    /// <exception cref="KeyNotFoundException">The key is not present in the application resources.</exception>
    /// <exception cref="InvalidCastException">The resource exists but is not of type <typeparamref name="T"/>.</exception>
    public static T GetResource<T>(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        var resources = Application.Current?.Resources
            ?? throw new InvalidOperationException("Application.Current.Resources is not available. ResourceAccessor requires an initialized Application.");

        if (!resources.TryGetValue(key, out var raw))
        {
            throw new KeyNotFoundException($"Resource '{key}' was not found in Application.Current.Resources.");
        }

        if (raw is T typed)
        {
            return typed;
        }

        throw new InvalidCastException($"Resource '{key}' is of type {raw?.GetType().FullName ?? "null"}, not {typeof(T).FullName}.");
    }

    /// <summary>
    /// Tries to look up the resource with the given <paramref name="key"/> as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Expected resource type.</typeparam>
    /// <param name="key">Resource key.</param>
    /// <param name="value">The resource if the lookup succeeded; otherwise <c>default</c>.</param>
    /// <returns><see langword="true"/> if a resource of type <typeparamref name="T"/> was found at <paramref name="key"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetResource<T>(string key, [MaybeNullWhen(false)] out T value)
    {
        ArgumentNullException.ThrowIfNull(key);

        var resources = Application.Current?.Resources;
        if (resources is not null && resources.TryGetValue(key, out var raw) && raw is T typed)
        {
            value = typed;
            return true;
        }

        value = default;
        return false;
    }
}
