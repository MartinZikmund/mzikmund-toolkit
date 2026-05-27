using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace MZikmund.Toolkit.WinUI.Http;

/// <summary>
/// <see cref="DelegatingHandler"/> that logs unsuccessful HTTP responses to a sink
/// (defaulting to <see cref="Debug.WriteLine(string?)"/>). Intended to sit at the top
/// of an <see cref="HttpClient"/>'s handler chain in development builds.
/// </summary>
/// <remarks>
/// Successful responses pass through untouched. Failures are logged with method, URI,
/// status code, reason phrase, and the response body. The body read is best-effort —
/// failures while reading are caught and recorded as <c>"(unable to read body)"</c>
/// so the handler never throws.
/// </remarks>
public sealed class DebugHttpHandler : DelegatingHandler
{
    /// <summary>
    /// Sink for failure messages. Defaults to <see cref="Debug.WriteLine(string?)"/>.
    /// Replace this in tests or to forward into a logging framework.
    /// </summary>
    public Action<string> Log { get; init; } = static message => Debug.WriteLine(message);

    /// <summary>Initializes a new instance with no inner handler.</summary>
    public DebugHttpHandler()
    {
    }

    /// <summary>Initializes a new instance wrapping the supplied <paramref name="innerHandler"/>.</summary>
    public DebugHttpHandler(HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var body = await TryReadBodyAsync(response, cancellationToken).ConfigureAwait(false);
            Log($"[HTTP] {request.Method} {request.RequestUri} → {(int)response.StatusCode} {response.ReasonPhrase}{Environment.NewLine}{body}");
        }

        return response;
    }

    private static async Task<string> TryReadBodyAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            return "(unable to read body)";
        }
    }
}
