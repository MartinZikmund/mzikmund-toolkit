using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Localization;

namespace MZikmund.Toolkit.Sample;

public sealed partial class LocalizerSamplePage : Page
{
    private static readonly DictionaryLocalizer English = new(new()
    {
        ["Greeting"] = "Hello, world!",
        ["Farewell"] = "Goodbye!",
    });

    private static readonly DictionaryLocalizer Czech = new(new()
    {
        ["Greeting"] = "Ahoj, světe!",
        ["Farewell"] = "Sbohem!",
    });

    public LocalizerSamplePage()
    {
        // Seed with English so the {toolkit:Localize} TextBlocks below have something to show.
        Localizer.Current = English;
        this.InitializeComponent();
    }

    private void UseEnglish_Click(object sender, RoutedEventArgs e)
    {
        Localizer.Current = English;
        LookupResult.Text = "Active localizer: English (custom impl).";
    }

    private void UseCzech_Click(object sender, RoutedEventArgs e)
    {
        Localizer.Current = Czech;
        LookupResult.Text = "Active localizer: Czech (custom impl).";
    }

    private void UseFallback_Click(object sender, RoutedEventArgs e)
    {
        Localizer.Current = new DictionaryLocalizer(new());
        LookupResult.Text = "Active localizer: empty dictionary — every key returns ???key???.";
    }

    private void Lookup_Click(object sender, RoutedEventArgs e)
    {
        var key = (KeyInput.Text ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(key))
        {
            LookupResult.Text = "Enter a key first.";
            return;
        }

        LookupResult.Text = $"Localizer.Current.GetString(\"{key}\") = \"{Localizer.Current.GetString(key)}\"";
    }

    private sealed class DictionaryLocalizer(Dictionary<string, string> entries) : ILocalizer
    {
        public string GetString(string key) =>
            entries.TryGetValue(key, out var value) ? value : $"???{key}???";
    }
}
