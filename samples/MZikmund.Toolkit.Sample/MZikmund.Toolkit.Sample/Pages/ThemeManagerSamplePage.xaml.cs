using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class ThemeManagerSamplePage : Page, IWindowShell
{
    private readonly WindowShellProvider _shellProvider = new();
    private readonly InMemoryPreferences _preferences = new();
    private readonly IThemeManager _themeManager;

    public ThemeManagerSamplePage()
    {
        this.InitializeComponent();
        _shellProvider.SetShell(this);
        _themeManager = new ThemeManager(_shellProvider, _preferences);
        _themeManager.Apply();
        UpdateText();
    }

    public object? ViewModel => null;

    public new XamlRoot XamlRoot => base.XamlRoot;

    public IServiceProvider ServiceProvider => null!;

    public new DispatcherQueue DispatcherQueue => base.DispatcherQueue;

    public Frame RootFrame => ThemedFrame;

    public void SetTitleBar(UIElement titleBar)
    {
    }

    private void System_Click(object sender, RoutedEventArgs e) => Apply(AppTheme.System);

    private void Light_Click(object sender, RoutedEventArgs e) => Apply(AppTheme.Light);

    private void Dark_Click(object sender, RoutedEventArgs e) => Apply(AppTheme.Dark);

    private void Apply(AppTheme theme)
    {
        _themeManager.SetTheme(theme);
        UpdateText();
    }

    private void UpdateText()
    {
        CurrentText.Text = $"Current theme: {_themeManager.CurrentTheme} (frame.RequestedTheme = {ThemedFrame.RequestedTheme})";
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
