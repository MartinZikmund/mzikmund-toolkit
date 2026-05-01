using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.ViewModels;

namespace MZikmund.Toolkit.Sample;

public sealed partial class CoreInfrastructureSamplePage : Page
{
    private int _goBackCount;

    public CoreInfrastructureSamplePage()
    {
        ShellViewModel = new WindowShellViewModel { Title = "Sample shell" };
        ShellViewModel.GoBackRequested += (s, e) =>
        {
            _goBackCount++;
            GoBackCounterText.Text = $"GoBackRequested fired {_goBackCount} times.";
        };
        this.InitializeComponent();
    }

    public WindowShellViewModel ShellViewModel { get; }

    private void IoC_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var services = new ServiceCollection()
                .AddSingleton<ISampleGreeter, SampleGreeter>()
                .BuildServiceProvider();

            IoC.SetProvider(services);

            var greeter = IoC.GetRequiredService<ISampleGreeter>();
            IoCResultText.Text = $"IoC.GetRequiredService<ISampleGreeter>() → {greeter.Greet()}";
        }
        catch (Exception ex)
        {
            IoCResultText.Text = $"Failed: {ex.Message}";
        }
    }

    private interface ISampleGreeter
    {
        string Greet();
    }

    private sealed class SampleGreeter : ISampleGreeter
    {
        public string Greet() => $"Hello at {DateTimeOffset.UtcNow:HH:mm:ss}";
    }
}
