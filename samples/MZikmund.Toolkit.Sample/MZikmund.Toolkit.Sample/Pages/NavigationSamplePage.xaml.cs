using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class NavigationSamplePage : Page, IWindowShell
{
    private readonly WindowShellProvider _shellProvider = new();
    private readonly INavigationService _navigation;

    public NavigationSamplePage()
    {
        this.InitializeComponent();
        _shellProvider.SetShell(this);
        _navigation = new NavigationService(_shellProvider);
        _navigation.Navigated += (s, e) => UpdateStatus();
        UpdateStatus();
    }

    public object? ViewModel => null;

    public new XamlRoot XamlRoot => base.XamlRoot;

    public IServiceProvider ServiceProvider => null!;

    public new DispatcherQueue DispatcherQueue => base.DispatcherQueue;

    public Frame RootFrame => HostedFrame;

    public void SetTitleBar(UIElement titleBar)
    {
    }

    private void GotoMain_Click(object sender, RoutedEventArgs e) =>
        _navigation.Navigate(typeof(NavigationDemoMainPage));

    private void GotoSettings_Click(object sender, RoutedEventArgs e) =>
        _navigation.Navigate(typeof(NavigationDemoSettingsPage));

    private void Back_Click(object sender, RoutedEventArgs e) =>
        _navigation.GoBack();

    private void UpdateStatus()
    {
        StatusText.Text =
            $"CurrentSection: {_navigation.CurrentSection ?? "(null)"}, CanGoBack: {_navigation.CanGoBack}";
    }
}
