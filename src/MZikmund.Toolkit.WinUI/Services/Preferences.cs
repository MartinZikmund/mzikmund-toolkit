using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Handles application preferences.
/// </summary>
public class Preferences : IPreferences
{
    private readonly Dictionary<string, object> _preferenceCache = new();

    private readonly ApplicationDataContainer _container = ApplicationData.Current.LocalSettings;

    /// <summary>
    /// Retrieves a plain setting from the preferences.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <param name="defaultValue">Default value.</param>
    /// <returns>Value from settings or default value.</returns>
    public T Get<T>(string key, T defaultValue)
    {
        if (TryGetFromCache<T>(key, out var cachedValue))
        {
            return cachedValue;
        }

        if (TryGetFromContainer<T>(key, out var value) && value is { })
        {
            _preferenceCache[key] = value;
            return value;
        }

        if (defaultValue is { })
        {
            _preferenceCache[key] = defaultValue;
        }

        return defaultValue;
    }

    /// <summary>
    /// Gets a complex setting from the preferences.
    /// Value is stored as string and deserialized.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <returns>Complex setting.</returns>
    public T? GetComplex<T>(string key)
    {
        if (TryGetFromCache<T>(key, out var cachedValue))
        {
            return cachedValue;
        }

        if (TryGetFromContainer<string>(key, out var value))
        {
            var deserializedValue = JsonSerializer.Deserialize<T>(value);
            if (deserializedValue is { })
            {
                _preferenceCache[key] = deserializedValue;
            }
            return deserializedValue;
        }

        return default;
    }

    /// <summary>
    /// Sets a plain setting in the preferences.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    public void Set<T>(string key, T? value)
    {
        if (value is null)
        {
            _preferenceCache.Remove(key);
            _container.Values.Remove(key);
            return;
        }

        _container.Values[key] = value;
        _preferenceCache[key] = value!;
    }

    /// <summary>
    /// Sets a complex setting in the preferences.
    /// Value is serialized to string.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    public void SetComplex<T>(string key, T? value)
    {
        if (value is null)
        {
            _preferenceCache.Remove(key);
            _container.Values.Remove(key);
            return;
        }

        var serializedValue = JsonSerializer.Serialize(value);
        _container.Values[key] = serializedValue;
        _preferenceCache[key] = value;
    }

    private bool TryGetFromCache<T>(string key, [MaybeNullWhen(false)] out T value)
    {
        if (_preferenceCache.TryGetValue(key, out var boxedValue))
        {
            value = (T)boxedValue;
            return true;
        }

        value = default;
        return false;
    }

    private bool TryGetFromContainer<T>(string key, [MaybeNullWhen(false)] out T value)
    {
        if (_container.Values.TryGetValue(key, out var objectValue))
        {
            //get existing
            try
            {
                value = (T)objectValue;
                return true;
            }
            catch
            {
#if DEBUG
                throw new InvalidOperationException("Value stored in the setting does not match expected type.");
#else
				//invalid value, remove
				container.Values.Remove(key);
#endif
            }
        }

        value = default;
        return false;
    }
}
