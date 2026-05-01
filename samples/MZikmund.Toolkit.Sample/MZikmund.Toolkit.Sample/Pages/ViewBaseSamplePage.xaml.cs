using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.ViewModels;

namespace MZikmund.Toolkit.Sample;

public sealed partial class ViewBaseSamplePage : Page, IWindowShell
{
    public ViewBaseSamplePage()
    {
        ServiceProvider = new ServiceCollection()
            .AddTransient<DemoViewModel>()
            .BuildServiceProvider();

        this.InitializeComponent();
        HostedFrame.Navigate(typeof(DemoView));
    }

    public object? ViewModel => null;

    public new XamlRoot XamlRoot => base.XamlRoot;

    public IServiceProvider ServiceProvider { get; }

    public new DispatcherQueue DispatcherQueue => base.DispatcherQueue;

    public Frame RootFrame => HostedFrame;

    public void SetTitleBar(UIElement titleBar)
    {
    }

    private void Load_Click(object sender, RoutedEventArgs e)
    {
        // Re-navigate so OnNavigatedTo fires again with a fresh greeting parameter.
        HostedFrame.Navigate(typeof(DemoView), $"Hello at {DateTimeOffset.UtcNow:HH:mm:ss}");
    }
}

public sealed class DemoViewModel : ViewModelBase
{
    private string _greeting = "(no greeting yet)";

    public string Greeting
    {
        get => _greeting;
        set => SetProperty(ref _greeting, value);
    }

    public override void OnNavigatedTo(object? parameter)
    {
        if (parameter is string s)
        {
            Greeting = s;
        }
    }
}
