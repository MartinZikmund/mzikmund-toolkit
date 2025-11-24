using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MZikmund.Toolkit.Sample;

public sealed partial class XamlRootProviderSamplePage : Page
{
    public XamlRootProviderSamplePage()
    {
        this.InitializeComponent();
    }

    private void GetXamlRootInfo_Click(object sender, RoutedEventArgs e)
    {
        if (this.XamlRoot != null)
        {
            var size = this.XamlRoot.Size;
            var isHostVisible = this.XamlRoot.IsHostVisible;
            var rasterizationScale = this.XamlRoot.RasterizationScale;

            XamlRootInfo.Text = $"XamlRoot is available!\n" +
                                $"Size: {size.Width} x {size.Height}\n" +
                                $"Host Visible: {isHostVisible}\n" +
                                $"Rasterization Scale: {rasterizationScale}";
        }
        else
        {
            XamlRootInfo.Text = "XamlRoot is not available";
        }
    }

    private async void ShowDialogWithXamlRoot_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "Example Dialog",
            Content = "This dialog was shown using the XamlRoot from this page.",
            PrimaryButtonText = "OK",
            XamlRoot = this.XamlRoot
        };

        var result = await dialog.ShowAsync();
        DialogResult.Text = $"Dialog closed with result: {result}";
    }
}
