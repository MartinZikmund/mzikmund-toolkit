namespace MZikmund.Toolkit.WinUI.Localization;

/// <summary>
/// Resolves a localization key to a translated string.
/// </summary>
/// <remarks>
/// The toolkit ships only this abstraction plus a static <see cref="Localizer.Current"/> accessor;
/// applications register their own implementation (typically a thin wrapper over
/// <c>Microsoft.Extensions.Localization.IStringLocalizer</c>) at startup.
/// </remarks>
public interface ILocalizer
{
    /// <summary>
    /// Returns the translated string for <paramref name="key"/>, or a non-empty fallback
    /// (e.g. <c>???key???</c>) when the key is missing. Implementations must never return null.
    /// </summary>
    /// <param name="key">Resource key.</param>
    /// <returns>Translated string or a clearly marked fallback.</returns>
    string GetString(string key);
}
