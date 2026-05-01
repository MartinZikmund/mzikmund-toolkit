using System.Globalization;
using Windows.Globalization;

namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Default <see cref="ILocalizationService"/>. Persists the user's language choice
/// through <see cref="IPreferences"/> and applies it via
/// <see cref="ApplicationLanguages.PrimaryLanguageOverride"/>.
/// </summary>
/// <remarks>
/// On first launch (or when the persisted language is no longer supported), the
/// constructor falls through this preference order:
/// <list type="number">
///   <item><description>The persisted choice, if it's still in <see cref="SupportedLanguages"/>.</description></item>
///   <item><description>The OS UI culture, if it's in <see cref="SupportedLanguages"/>.</description></item>
///   <item><description>The explicit <c>fallbackLanguage</c> constructor argument.</description></item>
///   <item><description>The first entry of <see cref="SupportedLanguages"/>.</description></item>
/// </list>
/// </remarks>
public class LocalizationService : ILocalizationService
{
    private const string LanguagePreferenceKey = "MZikmund.Toolkit.Language";

    private readonly IPreferences _preferences;
    private readonly List<string> _supported;
    private string _currentLanguage;

    /// <summary>Initializes a new instance.</summary>
    /// <param name="preferences">Preferences service for persisting the choice.</param>
    /// <param name="supportedLanguages">Languages the app ships translations for.</param>
    /// <param name="fallbackLanguage">Used when no persisted / system language matches.</param>
    public LocalizationService(
        IPreferences preferences,
        IEnumerable<string> supportedLanguages,
        string? fallbackLanguage = null)
    {
        ArgumentNullException.ThrowIfNull(preferences);
        ArgumentNullException.ThrowIfNull(supportedLanguages);

        _preferences = preferences;
        _supported = supportedLanguages.ToList();
        if (_supported.Count == 0)
        {
            throw new ArgumentException("Supported languages list cannot be empty.", nameof(supportedLanguages));
        }

        _currentLanguage = ResolveInitialLanguage(fallbackLanguage);
        ApplyToPlatform(_currentLanguage);
    }

    /// <inheritdoc />
    public string CurrentLanguage => _currentLanguage;

    /// <inheritdoc />
    public IReadOnlyList<string> SupportedLanguages => _supported;

    /// <inheritdoc />
    public event EventHandler<string>? LanguageChanged;

    /// <inheritdoc />
    public void SetLanguage(string language)
    {
        ArgumentNullException.ThrowIfNull(language);
        if (!_supported.Contains(language))
        {
            throw new ArgumentException(
                $"Language '{language}' is not in SupportedLanguages.", nameof(language));
        }

        if (_currentLanguage == language)
        {
            return;
        }

        _currentLanguage = language;
        _preferences.Set(LanguagePreferenceKey, language);
        ApplyToPlatform(language);
        LanguageChanged?.Invoke(this, language);
    }

    /// <inheritdoc />
    public string GetSystemLanguage() => CultureInfo.CurrentUICulture.Name;

    private string ResolveInitialLanguage(string? fallbackLanguage)
    {
        var saved = _preferences.Get(LanguagePreferenceKey, string.Empty);
        if (!string.IsNullOrEmpty(saved) && _supported.Contains(saved))
        {
            return saved;
        }

        var system = GetSystemLanguage();
        if (_supported.Contains(system))
        {
            return system;
        }

        if (!string.IsNullOrEmpty(fallbackLanguage) && _supported.Contains(fallbackLanguage))
        {
            return fallbackLanguage!;
        }

        return _supported[0];
    }

    /// <summary>
    /// Pushes the language change to the platform. Default implementation sets
    /// <see cref="ApplicationLanguages.PrimaryLanguageOverride"/>. Tests override
    /// to avoid the platform call (the Windows Runtime type isn't initializable
    /// in a console test host).
    /// </summary>
    protected virtual void ApplyToPlatform(string language) =>
        ApplicationLanguages.PrimaryLanguageOverride = language;
}
