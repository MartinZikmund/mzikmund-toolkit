using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Disposables;

namespace MZikmund.Toolkit.Sample;

public sealed partial class DisposableSamplePage : Page
{
    public DisposableSamplePage()
    {
        this.InitializeComponent();
    }

    private async void RunJob_Click(object sender, RoutedEventArgs e)
    {
        LoadingRing.IsActive = true;
        StateText.Text = "Working…";
        using var scope = Disposable.Create(() =>
        {
            LoadingRing.IsActive = false;
            StateText.Text = "Idle.";
        });

        await Task.Delay(TimeSpan.FromSeconds(2));
    }
}
