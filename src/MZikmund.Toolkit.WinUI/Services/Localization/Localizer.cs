using Microsoft.Extensions.Localization;
using MZikmund.Toolkit.WinUI.Infrastructure;

namespace MZikmund.Toolkit.WinUI.Services.Localization;

/// <summary>
/// Provides static access to localized strings via <see cref="IStringLocalizer"/>.
/// The localizer is resolved lazily from the IoC container on first use.
/// </summary>
public class Localizer
{
	private static readonly Lazy<IStringLocalizer> _stringLocalizer =
		new(() => IoC.GetRequiredService<IStringLocalizer>());

	private Localizer()
	{
	}

	/// <summary>
	/// Gets the singleton instance.
	/// </summary>
	public static Localizer Instance { get; } = new Localizer();

	/// <summary>
	/// Gets the localized string for the specified key.
	/// </summary>
	/// <param name="key">The resource key.</param>
	/// <returns>The localized string, or a placeholder if the resource is not found.</returns>
	public string GetString(string key)
	{
		var result = _stringLocalizer.Value.GetString(key);
		return !result.ResourceNotFound ? result.Value : $"???{key}???";
	}

	/// <summary>
	/// Gets the localized string for the specified key.
	/// </summary>
	/// <param name="key">The resource key.</param>
	/// <returns>The localized string, or a placeholder if the resource is not found.</returns>
	public string this[string key] => GetString(key);
}
