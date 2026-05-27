using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MZikmund.Toolkit.WinUI.Http;

namespace MZikmund.Toolkit.Sample;

public sealed partial class DebugHttpHandlerSamplePage : Page
{
    private readonly List<string> _capturedLog = new();
    private readonly StubHttpHandler _stub = new();
    private readonly HttpClient _client;

    public DebugHttpHandlerSamplePage()
    {
        this.InitializeComponent();

        var debugHandler = new DebugHttpHandler(_stub)
        {
            Log = message =>
            {
                _capturedLog.Add(message);
                DispatcherQueue.TryEnqueue(UpdateLogText);
            },
        };
        _client = new HttpClient(debugHandler);
    }

    private void SendOk_Click(object sender, RoutedEventArgs e) => _ = SendAsync(HttpStatusCode.OK, "Hello world");

    private void SendNotFound_Click(object sender, RoutedEventArgs e) => _ = SendAsync(HttpStatusCode.NotFound, "{\"error\":\"not found\"}");

    private void SendServerError_Click(object sender, RoutedEventArgs e) => _ = SendAsync(HttpStatusCode.InternalServerError, "boom");

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        _capturedLog.Clear();
        UpdateLogText();
    }

    private async Task SendAsync(HttpStatusCode statusCode, string body)
    {
        _stub.NextStatus = statusCode;
        _stub.NextBody = body;

        try
        {
            var response = await _client.GetAsync("https://example.test/api/sample");
            LastResultText.Text = $"GET https://example.test/api/sample → {(int)response.StatusCode} {response.ReasonPhrase}";
        }
        catch (Exception ex)
        {
            LastResultText.Text = $"Request threw: {ex.Message}";
        }
    }

    private void UpdateLogText()
    {
        LogText.Text = _capturedLog.Count == 0 ? "(empty)" : string.Join("\n\n", _capturedLog);
    }

    private sealed class StubHttpHandler : HttpMessageHandler
    {
        public HttpStatusCode NextStatus { get; set; } = HttpStatusCode.OK;

        public string NextBody { get; set; } = string.Empty;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(NextStatus)
            {
                Content = new StringContent(NextBody),
                RequestMessage = request,
            });
    }
}
