using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.Sample;

public sealed partial class DeepLinkSamplePage : Page
{
    private readonly IDeepLinkService _service = new DeepLinkService();
    private readonly List<string> _log = new();

    public DeepLinkSamplePage()
    {
        this.InitializeComponent();
        _service.RequestReceived += (sender, request) =>
        {
            _log.Add($"[{DateTimeOffset.Now:HH:mm:ss}] RequestReceived: {request.Uri}");
            UpdateLog();
        };
    }

    private void Enqueue_Click(object sender, RoutedEventArgs e)
    {
        if (!Uri.TryCreate((UriInput.Text ?? string.Empty).Trim(), UriKind.RelativeOrAbsolute, out var uri))
        {
            _log.Add("[error] Not a valid URI.");
            UpdateLog();
            return;
        }

        _service.Enqueue(new DeepLinkRequest(uri));
        _log.Add($"[{DateTimeOffset.Now:HH:mm:ss}] Enqueued: {uri}");
        UpdateLog();
    }

    private void Replay_Click(object sender, RoutedEventArgs e)
    {
        var replayed = _service.Replay();
        _log.Add($"[{DateTimeOffset.Now:HH:mm:ss}] Replay drained {replayed} request(s).");
        UpdateLog();
    }

    private async void DequeueAsync_Click(object sender, RoutedEventArgs e)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var request = await _service.TryDequeueAsync(cts.Token);
        _log.Add(request is null
            ? $"[{DateTimeOffset.Now:HH:mm:ss}] Async dequeue timed out / cancelled."
            : $"[{DateTimeOffset.Now:HH:mm:ss}] Async dequeue: {request.Uri}");
        UpdateLog();
    }

    private void UpdateLog() =>
        LogText.Text = _log.Count == 0 ? "(empty)" : string.Join("\n", _log);
}
