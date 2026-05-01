namespace MZikmund.Toolkit.WinUI.Infrastructure;

/// <summary>
/// Static service-locator with a single global <see cref="IServiceProvider"/>.
/// Apps call <see cref="SetProvider(IServiceProvider)"/> exactly once at startup,
/// and any code (XAML markup, static helpers, places without DI) can resolve
/// services through <see cref="GetService{T}"/> / <see cref="GetRequiredService{T}"/>.
/// </summary>
/// <remarks>
/// Service location is generally an anti-pattern — prefer constructor injection.
/// This is provided for the unavoidable cases (XAML markup extensions, attached
/// behaviors, static converters) where DI isn't reachable.
/// </remarks>
public static class IoC
{
    private static IServiceProvider? _provider;

    /// <summary>
    /// Registers the global service provider. Subsequent calls overwrite the previous one.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="provider"/> is <see langword="null"/>.</exception>
    public static void SetProvider(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        _provider = provider;
    }

    /// <summary>
    /// Resolves <typeparamref name="T"/> from the registered provider, or returns
    /// <see langword="null"/> if not registered.
    /// </summary>
    /// <exception cref="InvalidOperationException">No provider has been registered yet.</exception>
    public static T? GetService<T>() where T : notnull
    {
        if (_provider is null)
        {
            throw new InvalidOperationException(
                "IoC has not been initialized. Call IoC.SetProvider during app startup before requesting services.");
        }

        return (T?)_provider.GetService(typeof(T));
    }

    /// <summary>
    /// Resolves <typeparamref name="T"/> from the registered provider, throwing if it
    /// can't be resolved.
    /// </summary>
    /// <exception cref="InvalidOperationException">No provider has been registered, or <typeparamref name="T"/> is not registered.</exception>
    public static T GetRequiredService<T>() where T : notnull
    {
        var service = GetService<T>();
        if (service is null)
        {
            throw new InvalidOperationException($"No service has been registered for type {typeof(T).FullName}.");
        }

        return service;
    }

    /// <summary>
    /// Clears the registered provider. Intended for test isolation; production code
    /// should not need to reset the global container.
    /// </summary>
    public static void Reset() => _provider = null;
}
