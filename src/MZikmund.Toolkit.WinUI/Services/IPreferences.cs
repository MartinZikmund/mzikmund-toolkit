namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Handles application preferences.
/// </summary>
public interface IPreferences
{
    /// <summary>
    /// Retrieves a plain setting from the preferences.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <param name="defaultValue">Default value.</param>
    /// <returns>Value from settings or default value.</returns>
    T Get<T>(string key, T defaultValue);

    /// <summary>
    /// Sets a plain setting in the preferences.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    void Set<T>(string key, T? value);

    /// <summary>
    /// Gets a complex setting from the preferences.
    /// Value is stored as string and deserialized.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Complex setting.</returns>
    T? GetComplex<T>(string key);

    /// <summary>
    /// Sets a complex setting in the preferences.
    /// Value is serialized to string.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    void SetComplex<T>(string key, T? value);
}
