using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.Generated;

namespace MZikmund.Toolkit.Sample;

public sealed partial class PackageInfoSamplePage : Page
{
    public PackageInfoSamplePage()
    {
        this.InitializeComponent();

        var packages = GeneratedPackageInfo.GetPackages();
        CountText.Text = $"{packages.Count} user-facing reference(s):";
        PackagesList.ItemsSource = packages
            .Select(p => $"{p.Id} {p.Version}")
            .ToList();
    }
}
