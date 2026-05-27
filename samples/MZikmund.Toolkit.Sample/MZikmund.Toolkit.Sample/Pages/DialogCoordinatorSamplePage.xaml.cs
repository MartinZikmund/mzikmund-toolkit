using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class DialogCoordinatorSamplePage : Page
{
    private readonly IDialogCoordinator _dialogCoordinator;

    public DialogCoordinatorSamplePage()
    {
        this.InitializeComponent();
        _dialogCoordinator = new DialogCoordinator();
    }

    private async void ShowDialog_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "Sample Dialog",
            Content = "This is a sample dialog shown via the DialogCoordinator.",
            PrimaryButtonText = "OK",
            XamlRoot = this.XamlRoot
        };

        var result = await _dialogCoordinator.ShowAsync(dialog);
        SingleDialogResult.Text = $"Dialog result: {result}";
    }

    private async void ShowMultipleDialogs_Click(object sender, RoutedEventArgs e)
    {
        MultipleDialogResult.Text = "Showing dialogs...";

        // Start showing 3 dialogs rapidly - they will be queued
        var task1 = ShowQueuedDialog(1);
        var task2 = ShowQueuedDialog(2);
        var task3 = ShowQueuedDialog(3);

        await Task.WhenAll(task1, task2, task3);

        MultipleDialogResult.Text = "All dialogs completed!";
    }

    private async Task ShowQueuedDialog(int number)
    {
        var dialog = new ContentDialog
        {
            Title = $"Dialog {number}",
            Content = $"This is dialog number {number}. It was queued by the DialogCoordinator.",
            PrimaryButtonText = "OK",
            XamlRoot = this.XamlRoot
        };

        await _dialogCoordinator.ShowAsync(dialog);
    }
}
