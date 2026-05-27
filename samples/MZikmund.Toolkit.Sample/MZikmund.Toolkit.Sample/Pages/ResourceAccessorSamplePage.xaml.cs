using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using MZikmund.Toolkit.WinUI.Resources;
using Windows.UI;

namespace MZikmund.Toolkit.Sample;

public sealed partial class ResourceAccessorSamplePage : Page
{
    public ResourceAccessorSamplePage()
    {
        this.InitializeComponent();
    }

    private void LookupColor_Click(object sender, RoutedEventArgs e)
    {
        Lookup<Color>(value => $"Color {value} (A={value.A}, R={value.R}, G={value.G}, B={value.B})");
    }

    private void LookupBrush_Click(object sender, RoutedEventArgs e)
    {
        Lookup<Brush>(value =>
        {
            if (value is SolidColorBrush solid)
            {
                return $"SolidColorBrush {solid.Color}";
            }
            return $"Brush of type {value.GetType().Name}";
        });
    }

    private void LookupStyle_Click(object sender, RoutedEventArgs e)
    {
        Lookup<Style>(value => $"Style targeting {value.TargetType?.Name ?? "(none)"}");
    }

    private void Lookup<T>(Func<T, string> describe)
    {
        var key = (ResourceKeyInput.Text ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(key))
        {
            LookupResult.Text = "Enter a resource key first.";
            return;
        }

        if (ResourceAccessor.TryGetResource<T>(key, out var value))
        {
            LookupResult.Text = $"Found '{key}' as {typeof(T).Name}: {describe(value)}";
        }
        else
        {
            LookupResult.Text = $"No '{key}' resource of type {typeof(T).Name} was found.";
        }
    }
}
