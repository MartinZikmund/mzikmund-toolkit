using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class StoreLauncherSamplePage : Page
{
    private static readonly StoreLauncherOptions DemoOptions = new()
    {
        WindowsProductId = "9P1234567890",
        AndroidPackageName = "com.example.app",
        AppleAppId = "1234567890",
        PublisherName = "Contoso",
    };

    private readonly IStoreLauncherService _service = new StoreLauncherService(DemoOptions);

    public StoreLauncherSamplePage()
    {
        this.InitializeComponent();
        Refresh(StorePlatform.Windows);
    }

    private void PlatformPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PlatformPicker.SelectedItem is ComboBoxItem item && item.Tag is string tag &&
            Enum.TryParse<StorePlatform>(tag, out var platform))
        {
            Refresh(platform);
        }
    }

    private void Refresh(StorePlatform platform)
    {
        RateUriText.Text = StoreLauncherUris.Rate(platform, DemoOptions)?.ToString() ?? "(none)";
        ListingUriText.Text = StoreLauncherUris.Listing(platform, DemoOptions)?.ToString() ?? "(none)";
        MoreAppsUriText.Text = StoreLauncherUris.MoreApps(platform, DemoOptions)?.ToString() ?? "(none)";
    }

    private async void RateApp_Click(object sender, RoutedEventArgs e)
    {
        LaunchResult.Text = $"RateAppAsync returned {await _service.RateAppAsync()}.";
    }

    private async void ShowListing_Click(object sender, RoutedEventArgs e)
    {
        LaunchResult.Text = $"ShowAppListingAsync returned {await _service.ShowAppListingAsync()}.";
    }

    private async void MoreApps_Click(object sender, RoutedEventArgs e)
    {
        LaunchResult.Text = $"MoreAppsByPublisherAsync returned {await _service.MoreAppsByPublisherAsync()}.";
    }
}
