using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class NotificationPermissionSamplePage : Page
{
    private readonly INotificationPermissionService _service = new NotificationPermissionService();

    public NotificationPermissionSamplePage()
    {
        this.InitializeComponent();
    }

    private async void IsGranted_Click(object sender, RoutedEventArgs e)
    {
        var status = await _service.IsGrantedAsync();
        ResultText.Text = $"IsGrantedAsync = {status}";
    }

    private async void Ensure_Click(object sender, RoutedEventArgs e)
    {
        var status = await _service.EnsurePermissionAsync();
        ResultText.Text = $"EnsurePermissionAsync = {status}";
    }

    private async void OpenSettings_Click(object sender, RoutedEventArgs e)
    {
        var launched = await _service.OpenSettingsAsync();
        ResultText.Text = launched
            ? "OpenSettingsAsync launched the settings URI."
            : "OpenSettingsAsync didn't launch — platform has no settings URI in the base impl. Subclass to override.";
    }
}
