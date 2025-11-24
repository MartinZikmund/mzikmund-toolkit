using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Extensions;
using Windows.ApplicationModel;

namespace MZikmund.Toolkit.Sample;

public sealed partial class PackageVersionSamplePage : Page
{
    public PackageVersionSamplePage()
    {
        this.InitializeComponent();
    }

    private void FormatVersion_Click(object sender, RoutedEventArgs e)
    {
        if (ushort.TryParse(MajorInput.Text, out var major) &&
            ushort.TryParse(MinorInput.Text, out var minor) &&
            ushort.TryParse(BuildInput.Text, out var build) &&
            ushort.TryParse(RevisionInput.Text, out var revision))
        {
            var version = new PackageVersion
            {
                Major = major,
                Minor = minor,
                Build = build,
                Revision = revision
            };

            FullVersionResult.Text = $"Full version: {version.ToVersionString()}";
            ShortVersionResult.Text = $"Short version (major.minor only): {version.ToVersionString(majorMinorOnly: true)}";
        }
        else
        {
            FullVersionResult.Text = "Please enter valid numbers";
            ShortVersionResult.Text = "";
        }
    }

    private void GetAppVersion_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var package = Package.Current;
            var version = package.Id.Version;

            var fullVersion = version.ToVersionString();
            var shortVersion = version.ToVersionString(majorMinorOnly: true);

            AppVersionResult.Text = $"App Version: {fullVersion} (Short: {shortVersion})";
        }
        catch (Exception ex)
        {
            AppVersionResult.Text = $"Error: {ex.Message}";
        }
    }
}
