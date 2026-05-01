using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class DialogServicesSamplePage : Page
{
    private readonly IDialogCoordinator _coordinator = new DialogCoordinator();

    public DialogServicesSamplePage()
    {
        this.InitializeComponent();
    }

    private async void ShowInfo_Click(object sender, RoutedEventArgs e)
    {
        var dialogService = new DialogService(new ThisPageXamlRootProvider(this), _coordinator);
        await dialogService.ShowAsync("Saved", "Your changes have been saved.");
    }

    private async void ShowInfoCustom_Click(object sender, RoutedEventArgs e)
    {
        var dialogService = new DialogService(new ThisPageXamlRootProvider(this), _coordinator);
        await dialogService.ShowAsync("Custom button", "Notice the close button text below.", closeButtonText: "Got it");
    }

    private async void ConfirmEnglish_Click(object sender, RoutedEventArgs e)
    {
        var service = new ConfirmationDialogService(new ThisPageXamlRootProvider(this), _coordinator);
        var result = await service.ConfirmAsync("Delete file?", "This cannot be undone.");
        ConfirmResult.Text = $"Result: {result}";
    }

    private async void ConfirmCzech_Click(object sender, RoutedEventArgs e)
    {
        var service = new ConfirmationDialogService(
            new ThisPageXamlRootProvider(this),
            _coordinator,
            new DialogStrings { YesButtonText = "Ano", NoButtonText = "Ne" });
        var result = await service.ConfirmAsync("Smazat soubor?", "Tuto akci nelze vrátit zpět.");
        ConfirmResult.Text = $"Result: {result}";
    }

    private sealed class ThisPageXamlRootProvider(Page page) : IXamlRootProvider
    {
        public XamlRoot XamlRoot => page.XamlRoot;
    }
}
