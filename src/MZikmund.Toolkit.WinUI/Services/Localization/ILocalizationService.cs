namespace MZikmund.Toolkit.WinUI.Services;

/// <summary>
/// Manages the active app language at runtime: tracks the current and supported
/// languages, exposes the system language for first-launch detection, and applies
/// language changes to the platform's resource lookup pipeline.
/// </summary>
/// <remarks>
/// The service only manages <em>which</em> language is active; the actual translation
/// lookup is performed by an <c>ILocalizer</c> implementation registered separately
/// (typically backed by <c>IStringLocalizer</c>). Apps subscribe to
/// <see cref="LanguageChanged"/> to refresh views when the language changes.
/// </remarks>
public interface ILocalizationService
{
    /// <summary>The currently active language tag (e.g. <c>"en-US"</c>, <c>"cs-CZ"</c>).</summary>
    string CurrentLanguage { get; }

    /// <summary>Languages the app ships translations for, in declaration order.</summary>
    IReadOnlyList<string> SupportedLanguages { get; }

    /// <summary>Raised after a successful language change.</summary>
    event EventHandler<string>? LanguageChanged;

    /// <summary>
    /// Switches the active language. Persists the choice and applies it to
    /// <c>Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride</c>.
    /// </summary>
    /// <exception cref="ArgumentException"><paramref name="language"/> is not in <see cref="SupportedLanguages"/>.</exception>
    void SetLanguage(string language);

    /// <summary>The OS-reported UI culture, e.g. <c>"cs-CZ"</c> on a Czech device.</summary>
    string GetSystemLanguage();
}
