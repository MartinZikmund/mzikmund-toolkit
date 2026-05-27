using System.Diagnostics.CodeAnalysis;

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
    /// Retrieves a plain setting from the preferences.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    /// <returns>Value from settings or default value.</returns>
    bool TryGet<T>(string key, [MaybeNullWhen(false)] out T value);

    /// <summary>
    /// Sets a plain setting in the preferences.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    void Set<T>(string key, T? value);

    /// <summary>
    /// Retrieves a complex setting from the preferences.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <param name="defaultValue">Default value.</param>
    /// <returns>Value from settings or default value.</returns>
    T GetComplex<T>(string key, T defaultValue);

    /// <summary>
    /// Gets a complex setting from the preferences.
    /// Value is stored as string and deserialized.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    /// <returns>Complex setting.</returns>
    bool TryGetComplex<T>(string key, [MaybeNullWhen(false)] out T value);

    /// <summary>
    /// Sets a complex setting in the preferences.
    /// Value is serialized to string.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    void SetComplex<T>(string key, T? value);

    /// <summary>
    /// Determines whether a setting with the given key exists.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <returns><see langword="true"/> if a value is stored under <paramref name="key"/>.</returns>
    bool ContainsKey(string key);

    /// <summary>
    /// Removes the setting stored under the given key. Does nothing if no such setting exists.
    /// </summary>
    /// <param name="key">Key.</param>
    void Remove(string key);

    /// <summary>
    /// Removes all stored preferences.
    /// </summary>
    void Clear();
}
