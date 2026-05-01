using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Converters;
using MZikmund.Toolkit.WinUI.Localization;

namespace MZikmund.Toolkit.Sample;

public sealed partial class NullEnumConvertersSamplePage : Page
{
    private static readonly DictionaryLocalizer English = new(new()
    {
        ["OrderStatus_Pending"] = "Pending",
        ["OrderStatus_Shipped"] = "Shipped",
        ["OrderStatus_Delivered"] = "Delivered",
    });

    private static readonly DictionaryLocalizer Czech = new(new()
    {
        ["OrderStatus_Pending"] = "Čeká na zpracování",
        ["OrderStatus_Shipped"] = "Odesláno",
        ["OrderStatus_Delivered"] = "Doručeno",
    });

    private readonly EnumLocalizationConverter _converter = new();
    private OrderStatus _status = OrderStatus.Pending;

    public NullEnumConvertersSamplePage()
    {
        Localizer.Current = English;
        this.InitializeComponent();
        Refresh();
    }

    private void SetPending_Click(object sender, RoutedEventArgs e) => Set(OrderStatus.Pending);

    private void SetShipped_Click(object sender, RoutedEventArgs e) => Set(OrderStatus.Shipped);

    private void SetDelivered_Click(object sender, RoutedEventArgs e) => Set(OrderStatus.Delivered);

    private void UseEnglish_Click(object sender, RoutedEventArgs e)
    {
        Localizer.Current = English;
        Refresh();
    }

    private void UseCzech_Click(object sender, RoutedEventArgs e)
    {
        Localizer.Current = Czech;
        Refresh();
    }

    private void Set(OrderStatus status)
    {
        _status = status;
        Refresh();
    }

    private void Refresh()
    {
        var localized = (string)_converter.Convert(_status, typeof(string), null!, null!);
        LocalizedStatus.Text = $"{_status} → \"{localized}\"";
    }

    private enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
    }

    private sealed class DictionaryLocalizer(Dictionary<string, string> entries) : ILocalizer
    {
        public string GetString(string key) =>
            entries.TryGetValue(key, out var value) ? value : $"???{key}???";
    }
}
