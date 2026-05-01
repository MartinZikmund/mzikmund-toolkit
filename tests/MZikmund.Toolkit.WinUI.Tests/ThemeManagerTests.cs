using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class ThemeManagerTests
{
    [TestMethod]
    public void Ctor_NullArgs_Throw()
    {
        var prefs = new InMemoryPreferences();
        var provider = new WindowShellProvider();

        Assert.Throws<ArgumentNullException>(() => new ThemeManager(null!, prefs));
        Assert.Throws<ArgumentNullException>(() => new ThemeManager(provider, null!));
    }

    [TestMethod]
    public void CurrentTheme_DefaultsToSystem_OnFreshPreferences()
    {
        var manager = CreateManager(new InMemoryPreferences());

        Assert.AreEqual(AppTheme.System, manager.CurrentTheme);
    }

    [TestMethod]
    public void CurrentTheme_LoadsFromPreferences()
    {
        var prefs = new InMemoryPreferences();
        prefs.Set("MZikmund.Toolkit.Theme", AppTheme.Dark);

        var manager = CreateManager(prefs);

        Assert.AreEqual(AppTheme.Dark, manager.CurrentTheme);
    }

    [TestMethod]
    public void SetTheme_PersistsAndUpdatesCurrent()
    {
        var prefs = new InMemoryPreferences();
        var manager = CreateManager(prefs);

        manager.SetTheme(AppTheme.Light);

        Assert.AreEqual(AppTheme.Light, manager.CurrentTheme);
        Assert.AreEqual(AppTheme.Light, prefs.Get("MZikmund.Toolkit.Theme", AppTheme.System));
    }

    [TestMethod]
    public void SetTheme_RaisesThemeChanged_WithNewValue()
    {
        var manager = CreateManager(new InMemoryPreferences());
        AppTheme? captured = null;
        manager.ThemeChanged += (_, theme) => captured = theme;

        manager.SetTheme(AppTheme.Dark);

        Assert.AreEqual(AppTheme.Dark, captured);
    }

    [TestMethod]
    public void ToElementTheme_MapsCorrectly()
    {
        Assert.AreEqual(ElementTheme.Default, ThemeManager.ToElementTheme(AppTheme.System));
        Assert.AreEqual(ElementTheme.Light, ThemeManager.ToElementTheme(AppTheme.Light));
        Assert.AreEqual(ElementTheme.Dark, ThemeManager.ToElementTheme(AppTheme.Dark));
    }

    private static ThemeManager CreateManager(IPreferences preferences)
    {
        var provider = new WindowShellProvider();
        provider.SetShell(new StubShell());
        return new ThemeManager(provider, preferences);
    }

    private sealed class StubShell : IWindowShell
    {
        public object? ViewModel => null;

        public XamlRoot XamlRoot => null!;

        public IServiceProvider ServiceProvider => null!;

        public DispatcherQueue DispatcherQueue => null!;

        // ThemeManager.Apply is null-safe — RootFrame=null skips the assignment.
        public Frame RootFrame => null!;

        public void SetTitleBar(UIElement titleBar)
        {
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
