using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class StoreServiceSamplePage : Page
{
    private IStoreService _store = new FakeStoreService(startsAsPro: false);
    private string _activeName = "FakeStoreService (starts not Pro)";

    public StoreServiceSamplePage()
    {
        this.InitializeComponent();
        UpdateImplName();
    }

    private void UseFakeNotPro_Click(object sender, RoutedEventArgs e)
    {
        _store = new FakeStoreService(startsAsPro: false);
        _activeName = "FakeStoreService (starts not Pro)";
        UpdateImplName();
    }

    private void UseFakePro_Click(object sender, RoutedEventArgs e)
    {
        _store = new FakeStoreService(startsAsPro: true);
        _activeName = "FakeStoreService (starts Pro)";
        UpdateImplName();
    }

    private void UsePro_Click(object sender, RoutedEventArgs e)
    {
        _store = new ProStoreService();
        _activeName = "ProStoreService (always Pro)";
        UpdateImplName();
    }

    private async void HasPro_Click(object sender, RoutedEventArgs e)
    {
        ResultText.Text = $"HasProAsync = {await _store.HasProAsync()}";
    }

    private async void TryPurchase_Click(object sender, RoutedEventArgs e)
    {
        ResultText.Text = $"TryPurchaseProAsync = {await _store.TryPurchaseProAsync()} (now HasPro = {await _store.HasProAsync()})";
    }

    private async void GetPrice_Click(object sender, RoutedEventArgs e)
    {
        ResultText.Text = $"GetPriceAsync = \"{await _store.GetPriceAsync() ?? "(null)"}\"";
    }

    private async void TryRestore_Click(object sender, RoutedEventArgs e)
    {
        ResultText.Text = $"TryRestorePurchasesAsync = {await _store.TryRestorePurchasesAsync()}";
    }

    private void UpdateImplName()
    {
        ImplName.Text = $"Active: {_activeName}";
        ResultText.Text = "No call yet.";
    }
}
