namespace MZikmund.Toolkit.WinUI.Services;

public interface IPreferences
{
    T GetComplex<T>(string key, Func<T>? defaultValueBuilder = null);

    void SetComplex<T>(string key, T? value);

    T Get<T>(string key, Func<T>? defaultValueBuilder = null);

    void Set<T>(string key, T? value);
}
