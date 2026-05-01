using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Dialogs;

namespace MZikmund.Toolkit.Sample;

public sealed partial class ColorPickerDialogSamplePage : Page
{
    public ColorPickerDialogSamplePage()
    {
        this.InitializeComponent();
    }

    private async void Pick_Click(object sender, RoutedEventArgs e)
    {
        var picked = await ColorPickerDialog.PickAsync(this.XamlRoot, SwatchBrush.Color);
        if (picked is { } color)
        {
            SwatchBrush.Color = color;
            SwatchText.Text = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            StatusText.Text = "Color picked.";
        }
        else
        {
            StatusText.Text = "Picker cancelled — color unchanged.";
        }
    }
}
