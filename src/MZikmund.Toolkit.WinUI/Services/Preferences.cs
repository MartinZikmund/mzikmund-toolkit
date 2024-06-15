using System.Text.Json;

namespace MZikmund.Toolkit.WinUI.Services;

public class Preferences : IPreferences
{
    private readonly Dictionary<string, object?> _preferenceCache = new Dictionary<string, object?>();

    private readonly ApplicationDataContainer _container = ApplicationData.Current.LocalSettings;

    public T? Get<T>(string key, Func<T>? defaultValueBuilder = null)
    {
        if (!_preferenceCache.TryGetValue(key, out var boxedValue))
        {
            // retrieve directly from settings and cache
            var value = GetFromContainer(key, defaultValueBuilder);
            _preferenceCache.Add(key, value);
            return value;
        }
        else
        {
            return (T)boxedValue;
        }
    }

    public T? GetComplex<T>(string key, Func<T>? defaultValueBuilder = null)
    {
        if (!_preferenceCache.TryGetValue(key, out var boxedValue))
        {
            // retrieve directly from settings and cache
            var value = GetComplexFromContainer(key, defaultValueBuilder);
            _preferenceCache.Add(key, value!);
            return value;
        }
        else
        {
            return (T?)boxedValue;
        }
    }

    public void SetComplex<T>(string key, T value)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        _container.Values[key] = serializedValue;
        _preferenceCache[key] = value!;
    }

    public void Set<T>(string key, T value)
    {
        _preferenceCache[key] = value!;
    }

    private T? GetFromContainer<T>(string key, Func<T>? defaultValueBuilder = null)
    {
        if (_container.Values.TryGetValue(key, out var value))
        {
            //get existing
            try
            {
                return (T)value;
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

        if (defaultValueBuilder == null)
        {
            return default!;
        }
        else
        {
            return defaultValueBuilder();
        }
    }

    private T? GetComplexFromContainer<T>(string key, Func<T>? defaultValueBuilder = null)
    {
        if (_container.Values.TryGetValue(key, out var value))
        {
            //get existing
            try
            {
                var serialized = (string)value;
                return JsonSerializer.Deserialize<T>(serialized);
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
        if (defaultValueBuilder == null)
        {
            return default!;
        }
        else
        {
            return defaultValueBuilder();
        }
    }

    private bool TryGetSetting(string key, out object? value) => _container.Values.TryGetValue(key, out value);
}
