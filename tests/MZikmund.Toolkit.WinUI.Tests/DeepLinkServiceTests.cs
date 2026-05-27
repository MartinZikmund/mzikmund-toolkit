using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class DeepLinkServiceTests
{
    [TestMethod]
    public void Enqueue_NullRequest_Throws()
    {
        var service = new DeepLinkService();

        Assert.Throws<ArgumentNullException>(() => service.Enqueue(null!));
    }

    [TestMethod]
    public void Replay_FiresRequestReceived_ForEachQueuedItem_InOrder()
    {
        var service = new DeepLinkService();
        var captured = new List<Uri>();
        service.RequestReceived += (_, request) => captured.Add(request.Uri);

        service.Enqueue(new DeepLinkRequest(new Uri("myapp://a")));
        service.Enqueue(new DeepLinkRequest(new Uri("myapp://b")));
        service.Enqueue(new DeepLinkRequest(new Uri("myapp://c")));

        var replayed = service.Replay();

        Assert.AreEqual(3, replayed);
        CollectionAssert.AreEqual(
            new[] { new Uri("myapp://a"), new Uri("myapp://b"), new Uri("myapp://c") },
            captured);
    }

    [TestMethod]
    public void Replay_OnEmptyQueue_ReturnsZeroAndDoesNotFire()
    {
        var service = new DeepLinkService();
        var fired = 0;
        service.RequestReceived += (_, _) => fired++;

        var replayed = service.Replay();

        Assert.AreEqual(0, replayed);
        Assert.AreEqual(0, fired);
    }

    [TestMethod]
    public void Replay_DrainsQueue_SoSecondReplayIsZero()
    {
        var service = new DeepLinkService();
        service.Enqueue(new DeepLinkRequest(new Uri("myapp://a")));
        service.Replay();

        Assert.AreEqual(0, service.Replay());
    }

    [TestMethod]
    public async Task TryDequeueAsync_ReturnsEnqueuedItem()
    {
        var service = new DeepLinkService();
        var request = new DeepLinkRequest(new Uri("myapp://reminder/42"));
        service.Enqueue(request);

        var dequeued = await service.TryDequeueAsync();

        Assert.AreSame(request, dequeued);
    }

    [TestMethod]
    public async Task TryDequeueAsync_PreservesArrivalOrder()
    {
        var service = new DeepLinkService();
        service.Enqueue(new DeepLinkRequest(new Uri("myapp://1")));
        service.Enqueue(new DeepLinkRequest(new Uri("myapp://2")));
        service.Enqueue(new DeepLinkRequest(new Uri("myapp://3")));

        var first = await service.TryDequeueAsync();
        var second = await service.TryDequeueAsync();
        var third = await service.TryDequeueAsync();

        Assert.AreEqual(new Uri("myapp://1"), first?.Uri);
        Assert.AreEqual(new Uri("myapp://2"), second?.Uri);
        Assert.AreEqual(new Uri("myapp://3"), third?.Uri);
    }

    [TestMethod]
    public async Task TryDequeueAsync_Cancelled_ReturnsNull()
    {
        var service = new DeepLinkService();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var result = await service.TryDequeueAsync(cts.Token);

        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task TryDequeueAsync_DoesNotFireRequestReceivedEvent()
    {
        // RequestReceived is the Replay() channel; async dequeue is independent.
        var service = new DeepLinkService();
        var fired = 0;
        service.RequestReceived += (_, _) => fired++;
        service.Enqueue(new DeepLinkRequest(new Uri("myapp://1")));

        await service.TryDequeueAsync();

        Assert.AreEqual(0, fired);
    }

    [TestMethod]
    public void Replay_AfterAsyncDequeue_OnlyReplaysRemaining()
    {
        var service = new DeepLinkService();
        service.Enqueue(new DeepLinkRequest(new Uri("myapp://1")));
        service.Enqueue(new DeepLinkRequest(new Uri("myapp://2")));

        // Synchronously consume one through the async channel and then replay.
        var t = service.TryDequeueAsync();
        Assert.IsTrue(t.IsCompletedSuccessfully);

        Assert.AreEqual(1, service.Replay());
    }
}
