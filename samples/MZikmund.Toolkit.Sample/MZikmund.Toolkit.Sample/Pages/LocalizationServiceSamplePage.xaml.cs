using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class LocalizationServiceSamplePage : Page
{
    private readonly ILocalizationService _localization;

    public LocalizationServiceSamplePage()
    {
        this.InitializeComponent();

        var preferences = new InMemoryPreferences();
        _localization = new LocalizationService(
            preferences,
            supportedLanguages: ["en-US", "cs-CZ", "de-DE", "fr-FR"],
            fallbackLanguage: "en-US");
        _localization.LanguageChanged += OnLanguageChanged;

        SystemText.Text = $"System language: {_localization.GetSystemLanguage()}";
        foreach (var language in _localization.SupportedLanguages)
        {
            LanguagePicker.Items.Add(language);
        }
        LanguagePicker.SelectedItem = _localization.CurrentLanguage;
        UpdateCurrent();
    }

    private void LanguagePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LanguagePicker.SelectedItem is string language)
        {
            try
            {
                _localization.SetLanguage(language);
            }
            catch (ArgumentException ex)
            {
                EventLog.Text = ex.Message;
            }
        }
    }

    private void OnLanguageChanged(object? sender, string language)
    {
        EventLog.Text = $"LanguageChanged → {language}";
        UpdateCurrent();
    }

    private void UpdateCurrent()
    {
        CurrentText.Text =
            $"CurrentLanguage: {_localization.CurrentLanguage} (PrimaryLanguageOverride applied)";
    }

    private sealed class InMemoryPreferences : IPreferences
    {
        private readonly Dictionary<string, object?> _values = new();

        public T Get<T>(string key, T defaultValue) =>
            _values.TryGetValue(key, out var v) && v is T typed ? typed : defaultValue;

        public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T value)
        {
            if (_values.TryGetValue(key, out var v) && v is T typed)
            {
                value = typed;
                return true;
            }
            value = default;
            return false;
        }

        public void Set<T>(string key, T? value)
        {
            if (value is null) _values.Remove(key);
            else _values[key] = value;
        }

        public T GetComplex<T>(string key, T defaultValue) => Get(key, defaultValue);

        public bool TryGetComplex<T>(string key, [MaybeNullWhen(false)] out T value) => TryGet(key, out value);

        public void SetComplex<T>(string key, T? value) => Set(key, value);
    }
}
