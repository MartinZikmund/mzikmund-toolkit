using System.Diagnostics.CodeAnalysis;

namespace MZikmund.Toolkit.WinUI.Infrastructure;

/// <summary>
/// Static service locator for resolving services from the application's dependency injection container.
/// </summary>
public static class IoC
{
	private static IServiceProvider? _serviceProvider;

	/// <summary>
	/// Sets the service provider instance used for service resolution.
	/// </summary>
	/// <param name="serviceProvider">The service provider to use.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceProvider"/> is null.</exception>
	public static void SetProvider(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
	}

	/// <summary>
	/// Gets a service of the specified type, or null if not registered.
	/// </summary>
	/// <typeparam name="T">The type of service to resolve.</typeparam>
	/// <returns>The service instance, or null if not registered.</returns>
	public static T? GetService<T>() where T : class
	{
		EnsureServiceProvider();
		return (T?)_serviceProvider.GetService(typeof(T));
	}

	/// <summary>
	/// Gets a required service of the specified type.
	/// </summary>
	/// <typeparam name="T">The type of service to resolve.</typeparam>
	/// <returns>The service instance.</returns>
	/// <exception cref="InvalidOperationException">Thrown when the service is not registered.</exception>
	public static T GetRequiredService<T>() where T : class
	{
		EnsureServiceProvider();
		return (T?)_serviceProvider.GetService(typeof(T))
			?? throw new InvalidOperationException($"Service of type {typeof(T).Name} is not registered.");
	}

	[MemberNotNull(nameof(_serviceProvider))]
	private static void EnsureServiceProvider()
	{
		if (_serviceProvider is null)
		{
			throw new InvalidOperationException("Service provider was not yet initialized. Call IoC.SetProvider() first.");
		}
	}
}
