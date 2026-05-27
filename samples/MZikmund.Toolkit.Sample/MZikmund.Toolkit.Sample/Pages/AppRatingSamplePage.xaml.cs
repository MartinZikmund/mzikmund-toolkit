using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class AppRatingSamplePage : Page
{
    private readonly IAppRatingService _service = new AppRatingService(
        new Preferences(),
        new AppRatingOptions
        {
            WindowsProductId = "9P1234567890",
            AndroidPackageName = "com.example.app",
            AppleAppId = "1234567890",
            MinLaunchCountForRating = 3,
        });

    public AppRatingSamplePage()
    {
        this.InitializeComponent();
        Refresh();
    }

    private void Increment_Click(object sender, RoutedEventArgs e)
    {
        _service.IncrementLaunchCount();
        ResultText.Text = $"Launch count is now {_service.LaunchCount}.";
        Refresh();
    }

    private async void Request_Click(object sender, RoutedEventArgs e)
    {
        var launched = await _service.RequestRatingAsync();
        ResultText.Text = launched
            ? "RequestRatingAsync launched the store URI."
            : "RequestRatingAsync didn't launch — check that AppRatingOptions has an identifier for the current platform.";
        Refresh();
    }

    private void Reset_Click(object sender, RoutedEventArgs e)
    {
        _service.Reset();
        ResultText.Text = "Reset. Launch count and 'has been asked' are cleared.";
        Refresh();
    }

    private void Refresh()
    {
        LaunchCountText.Text = _service.LaunchCount.ToString();
        HasBeenAskedText.Text = _service.HasBeenAsked.ToString();
        ShouldRequestText.Text = _service.ShouldRequestRating.ToString();
    }
}
