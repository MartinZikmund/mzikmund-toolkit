using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using MZikmund.Toolkit.WinUI.Infrastructure;

namespace MZikmund.Toolkit.Sample;

public sealed partial class WindowShellSamplePage : Page
{
    public WindowShellSamplePage()
    {
        this.InitializeComponent();
    }

    private void RegisterAndResolve_Click(object sender, RoutedEventArgs e)
    {
        var provider = new WindowShellProvider();
        var shell = new InlineWindowShell(this);

        provider.SetShell(shell);

        var resolved = provider.Shell;
        ResultText.Text =
            $"Shell registered: same instance = {ReferenceEquals(shell, resolved)}\n" +
            $"DispatcherQueue: {resolved.DispatcherQueue?.GetType().Name ?? "(null)"}\n" +
            $"RootFrame: {resolved.RootFrame?.GetType().Name ?? "(null)"}\n" +
            $"XamlRoot: {(resolved.XamlRoot is null ? "(null)" : "(set)")}\n" +
            $"ServiceProvider: {(resolved.ServiceProvider is null ? "(null)" : "(set)")}\n" +
            $"ViewModel: {resolved.ViewModel ?? "(null)"}";
    }

    private sealed class InlineWindowShell(Page page) : IWindowShell
    {
        public object? ViewModel => null;

        public XamlRoot XamlRoot => page.XamlRoot;

        public IServiceProvider ServiceProvider { get; } =
            new ServiceCollection().BuildServiceProvider();

        public DispatcherQueue DispatcherQueue => page.DispatcherQueue;

        public Frame RootFrame => page.Frame;

        public void SetTitleBar(UIElement titleBar)
        {
            // Sample shell doesn't own a Window — no-op.
        }
    }
}
