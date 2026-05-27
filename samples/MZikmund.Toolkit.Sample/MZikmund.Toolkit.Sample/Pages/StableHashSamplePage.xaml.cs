using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Helpers;

namespace MZikmund.Toolkit.Sample;

public sealed partial class StableHashSamplePage : Page
{
    public StableHashSamplePage()
    {
        this.InitializeComponent();
        GuidInput.Text = Guid.NewGuid().ToString();
    }

    private void HashGuid_Click(object sender, RoutedEventArgs e)
    {
        if (Guid.TryParse(GuidInput.Text, out var guid))
        {
            var hash = StableHash.FromGuid(guid);
            GuidResult.Text = $"FromGuid({guid:D}) = {hash} (0x{hash:X8})";
        }
        else
        {
            GuidResult.Text = "Not a valid GUID.";
        }
    }

    private void NewGuid_Click(object sender, RoutedEventArgs e)
    {
        GuidInput.Text = Guid.NewGuid().ToString();
    }

    private void HashString_Click(object sender, RoutedEventArgs e)
    {
        var input = StringInput.Text ?? string.Empty;
        var hash = StableHash.FromString(input);
        StringResult.Text = $"FromString(\"{input}\") = {hash} (0x{hash:X8})";
    }
}
