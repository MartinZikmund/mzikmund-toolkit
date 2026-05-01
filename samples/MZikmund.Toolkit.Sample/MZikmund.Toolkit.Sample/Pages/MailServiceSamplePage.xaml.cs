using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class MailServiceSamplePage : Page
{
    private readonly IMailService _mail = new MailService();

    public MailServiceSamplePage()
    {
        this.InitializeComponent();
    }

    private async void Compose_Click(object sender, RoutedEventArgs e)
    {
        var addressTo = (ToInput.Text ?? string.Empty).Trim();
        var subject = SubjectInput.Text;
        var body = BodyInput.Text;

        if (string.IsNullOrWhiteSpace(addressTo))
        {
            UriPreviewText.Text = "Recipient is required.";
            return;
        }

        var uri = MailService.BuildMailtoUri(addressTo, subject, body);
        UriPreviewText.Text = $"Launching: {uri.OriginalString}";

        var launched = await _mail.ComposeMailAsync(addressTo, subject, body);
        if (!launched)
        {
            UriPreviewText.Text += "\nLauncher returned false (no mail handler installed?).";
        }
    }
}
