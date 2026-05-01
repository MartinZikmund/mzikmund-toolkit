using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class LocalizationServiceTests
{
    [TestMethod]
    public void Ctor_NullPreferences_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TestLocalizationService(null!, ["en-US"]));
    }

    [TestMethod]
    public void Ctor_NullSupportedLanguages_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TestLocalizationService(new InMemoryPreferences(), null!));
    }

    [TestMethod]
    public void Ctor_EmptySupportedLanguages_Throws()
    {
        Assert.Throws<ArgumentException>(() =>
            new TestLocalizationService(new InMemoryPreferences(), Array.Empty<string>()));
    }

    [TestMethod]
    public void Ctor_PicksUpPersistedLanguage_WhenSupported()
    {
        var prefs = new InMemoryPreferences();
        prefs.Set("MZikmund.Toolkit.Language", "cs-CZ");

        var sut = new TestLocalizationService(prefs, ["en-US", "cs-CZ", "de-DE"]);

        Assert.AreEqual("cs-CZ", sut.CurrentLanguage);
    }

    [TestMethod]
    public void Ctor_PersistedLanguageNotSupported_FallsBackToFallbackArgument()
    {
        var prefs = new InMemoryPreferences();
        prefs.Set("MZikmund.Toolkit.Language", "ja-JP"); // not in list

        var sut = new TestLocalizationService(
            prefs,
            ["en-US", "de-DE"],
            fallbackLanguage: "en-US");

        Assert.AreEqual("en-US", sut.CurrentLanguage);
    }

    [TestMethod]
    public void Ctor_NoPersisted_NoSystemMatch_NoFallback_UsesFirstSupported()
    {
        var sut = new TestLocalizationService(new InMemoryPreferences(), ["fi-FI", "is-IS"]);

        Assert.AreEqual("fi-FI", sut.CurrentLanguage);
    }

    [TestMethod]
    public void SetLanguage_UnsupportedLanguage_Throws()
    {
        var sut = new TestLocalizationService(new InMemoryPreferences(), ["en-US", "cs-CZ"]);

        Assert.Throws<ArgumentException>(() => sut.SetLanguage("ja-JP"));
    }

    [TestMethod]
    public void SetLanguage_NullLanguage_Throws()
    {
        var sut = new TestLocalizationService(new InMemoryPreferences(), ["en-US", "cs-CZ"]);

        Assert.Throws<ArgumentNullException>(() => sut.SetLanguage(null!));
    }

    [TestMethod]
    public void SetLanguage_NewLanguage_PersistsAndRaisesEvent()
    {
        var prefs = new InMemoryPreferences();
        var sut = new TestLocalizationService(prefs, ["en-US", "cs-CZ", "de-DE"], fallbackLanguage: "en-US");
        var captured = new List<string>();
        sut.LanguageChanged += (_, lang) => captured.Add(lang);

        sut.SetLanguage("cs-CZ");

        Assert.AreEqual("cs-CZ", sut.CurrentLanguage);
        CollectionAssert.AreEqual(new[] { "cs-CZ" }, captured);
        Assert.AreEqual("cs-CZ", prefs.Get("MZikmund.Toolkit.Language", string.Empty));
    }

    [TestMethod]
    public void SetLanguage_SameLanguage_DoesNotRaiseEvent()
    {
        var sut = new TestLocalizationService(new InMemoryPreferences(), ["en-US", "cs-CZ"], fallbackLanguage: "en-US");
        var fired = 0;
        sut.LanguageChanged += (_, _) => fired++;

        sut.SetLanguage(sut.CurrentLanguage);

        Assert.AreEqual(0, fired);
    }

    [TestMethod]
    public void SupportedLanguages_PreservesOrder()
    {
        var sut = new TestLocalizationService(
            new InMemoryPreferences(),
            ["en-US", "cs-CZ", "de-DE", "fr-FR"]);

        CollectionAssert.AreEqual(
            new[] { "en-US", "cs-CZ", "de-DE", "fr-FR" },
            sut.SupportedLanguages.ToList());
    }

    [TestMethod]
    public void GetSystemLanguage_ReturnsCurrentUICultureName()
    {
        var sut = new TestLocalizationService(new InMemoryPreferences(), ["en-US"]);

        Assert.AreEqual(System.Globalization.CultureInfo.CurrentUICulture.Name, sut.GetSystemLanguage());
    }

    private sealed class TestLocalizationService : LocalizationService
    {
        public TestLocalizationService(IPreferences preferences, IEnumerable<string> supportedLanguages, string? fallbackLanguage = null)
            : base(preferences, supportedLanguages, fallbackLanguage)
        {
        }

        protected override void ApplyToPlatform(string language)
        {
            // No-op: ApplicationLanguages.PrimaryLanguageOverride is unavailable in
            // the headless test host (the Windows Runtime initializer NRE's).
        }
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
