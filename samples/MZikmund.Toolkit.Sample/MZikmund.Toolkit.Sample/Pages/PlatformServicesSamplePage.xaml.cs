using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class PlatformServicesSamplePage : Page
{
    private readonly ILauncherService _launcher = new LauncherService();
    private readonly IDisplayRequestManager _display = new DisplayRequestManager();
    private readonly IShareService _share = new ShareService();

    public PlatformServicesSamplePage()
    {
        this.InitializeComponent();
        UpdateDisplayText();
    }

    private async void LaunchUri_Click(object sender, RoutedEventArgs e)
    {
        if (Uri.TryCreate(LaunchUriInput.Text, UriKind.Absolute, out var uri))
        {
            var ok = await _launcher.LaunchUriAsync(uri);
            LauncherResult.Text = ok ? "Launched." : "Launcher refused the URI.";
        }
        else
        {
            LauncherResult.Text = "Not a valid absolute URI.";
        }
    }

    private void Acquire_Click(object sender, RoutedEventArgs e)
    {
        _display.Acquire();
        UpdateDisplayText();
    }

    private void Release_Click(object sender, RoutedEventArgs e)
    {
        _display.Release();
        UpdateDisplayText();
    }

    private void ClearDisplay_Click(object sender, RoutedEventArgs e)
    {
        _display.Clear();
        UpdateDisplayText();
    }

    private async void Share_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            await _share.ShareTextAsync(
                title: "Hello from MZikmund.Toolkit",
                text: "Sharing through ShareService.",
                webLink: new Uri("https://github.com/MartinZikmund/mzikmund-toolkit"));
            ShareResult.Text = "Share UI completed.";
        }
        catch (Exception ex)
        {
            ShareResult.Text = $"Share failed: {ex.GetType().Name}: {ex.Message}";
        }
    }

    private void UpdateDisplayText() =>
        DisplayCountText.Text = $"Active acquisitions: {_display.ActiveCount}";
}
