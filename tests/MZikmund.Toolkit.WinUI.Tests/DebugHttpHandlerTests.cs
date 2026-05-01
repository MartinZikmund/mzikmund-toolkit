using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Http;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class DebugHttpHandlerTests
{
    [TestMethod]
    public async Task Successful_Response_DoesNotLog()
    {
        var captured = new List<string>();
        var stub = new StubHandler(HttpStatusCode.OK, "ok body");
        using var client = new HttpClient(new DebugHttpHandler(stub)
        {
            Log = captured.Add,
        });

        var response = await client.GetAsync("https://example.test/");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(0, captured.Count, "Successful responses must not be logged.");
    }

    [TestMethod]
    public async Task Failure_Response_LogsMethodUriStatusAndBody()
    {
        var captured = new List<string>();
        var stub = new StubHandler(HttpStatusCode.NotFound, "{\"error\":\"missing\"}");
        using var client = new HttpClient(new DebugHttpHandler(stub)
        {
            Log = captured.Add,
        });

        await client.GetAsync("https://example.test/api/widgets/9");

        Assert.AreEqual(1, captured.Count);
        var entry = captured[0];
        StringAssert.Contains(entry, "GET");
        StringAssert.Contains(entry, "https://example.test/api/widgets/9");
        StringAssert.Contains(entry, "404");
        StringAssert.Contains(entry, "{\"error\":\"missing\"}");
    }

    [TestMethod]
    public async Task Failure_Response_PassesResponseThroughUnchanged()
    {
        var stub = new StubHandler(HttpStatusCode.InternalServerError, "boom");
        using var client = new HttpClient(new DebugHttpHandler(stub) { Log = _ => { } });

        var response = await client.GetAsync("https://example.test/");

        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.AreEqual("boom", await response.Content.ReadAsStringAsync());
    }

    [TestMethod]
    public async Task Multiple_Failures_LoggedSeparately()
    {
        var captured = new List<string>();
        var stub = new StubHandler(HttpStatusCode.BadGateway, "x");
        using var client = new HttpClient(new DebugHttpHandler(stub) { Log = captured.Add });

        await client.GetAsync("https://example.test/a");
        await client.GetAsync("https://example.test/b");

        Assert.AreEqual(2, captured.Count);
        StringAssert.Contains(captured[0], "/a");
        StringAssert.Contains(captured[1], "/b");
    }

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _status;
        private readonly string _body;

        public StubHandler(HttpStatusCode status, string body)
        {
            _status = status;
            _body = body;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(_status)
            {
                Content = new StringContent(_body ?? string.Empty),
                RequestMessage = request,
            });
    }
}
