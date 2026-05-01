using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Extensions;
using MZikmund.Toolkit.WinUI.Infrastructure;

namespace MZikmund.Toolkit.Sample;

public sealed partial class UIElementServicesSamplePage : Page, IWindowShell
{
    public UIElementServicesSamplePage()
    {
        this.InitializeComponent();

        ServiceProvider = new ServiceCollection()
            .AddSingleton<ITimeProvider, SystemTimeProvider>()
            .BuildServiceProvider();
    }

    public object? ViewModel => null;

    public new XamlRoot XamlRoot => base.XamlRoot;

    public IServiceProvider ServiceProvider { get; }

    public new DispatcherQueue DispatcherQueue => base.DispatcherQueue;

    public Frame RootFrame => Frame;

    public void SetTitleBar(UIElement titleBar)
    {
    }

    private void Resolve_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var services = ((Button)sender).GetServiceProvider();
            var time = services.GetRequiredService<ITimeProvider>();
            ResultText.Text = $"Resolved ITimeProvider → {time.Now:O}";
        }
        catch (Exception ex)
        {
            ResultText.Text = $"Failed: {ex.Message}";
        }
    }

    private interface ITimeProvider
    {
        DateTimeOffset Now { get; }
    }

    private sealed class SystemTimeProvider : ITimeProvider
    {
        public DateTimeOffset Now => DateTimeOffset.UtcNow;
    }
}
