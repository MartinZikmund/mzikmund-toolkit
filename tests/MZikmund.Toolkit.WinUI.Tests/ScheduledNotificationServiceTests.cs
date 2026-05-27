using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class ScheduledNotificationTests
{
    [TestMethod]
    public void Record_StoresAllFields()
    {
        var when = new DateTimeOffset(2024, 5, 1, 9, 0, 0, TimeSpan.Zero);
        var n = new ScheduledNotification("id-1", "Title", "Body", when, "category");

        Assert.AreEqual("id-1", n.Id);
        Assert.AreEqual("Title", n.Title);
        Assert.AreEqual("Body", n.Body);
        Assert.AreEqual(when, n.DeliveryTime);
        Assert.AreEqual("category", n.Category);
    }

    [TestMethod]
    public void Record_CategoryDefaultsToNull()
    {
        var n = new ScheduledNotification("id-1", "t", "b", DateTimeOffset.UtcNow);

        Assert.IsNull(n.Category);
    }
}

[TestClass]
public class ScheduledNotificationServiceBaseTests
{
    [TestMethod]
    public async Task ScheduleAsync_NullNotification_Throws()
    {
        var service = new ScheduledNotificationService();

        await Assert.ThrowsAsync<ArgumentNullException>(() => service.ScheduleAsync(null!));
    }

    [TestMethod]
    public async Task ScheduleAsync_DefaultBase_ThrowsPlatformNotSupported()
    {
        var service = new ScheduledNotificationService();
        var notification = new ScheduledNotification("id-1", "t", "b", DateTimeOffset.UtcNow);

        await Assert.ThrowsAsync<PlatformNotSupportedException>(() => service.ScheduleAsync(notification));
    }

    [TestMethod]
    public async Task CancelAsync_NullId_Throws()
    {
        var service = new ScheduledNotificationService();

        await Assert.ThrowsAsync<ArgumentException>(() => service.CancelAsync(null!));
    }

    [TestMethod]
    public async Task CancelAllAsync_DefaultBase_ThrowsPlatformNotSupported()
    {
        var service = new ScheduledNotificationService();

        await Assert.ThrowsAsync<PlatformNotSupportedException>(() => service.CancelAllAsync());
    }

    [TestMethod]
    public async Task GetPendingAsync_DefaultBase_ThrowsPlatformNotSupported()
    {
        var service = new ScheduledNotificationService();

        await Assert.ThrowsAsync<PlatformNotSupportedException>(() => service.GetPendingAsync());
    }

    [TestMethod]
    public async Task Subclass_CanOverrideAllMethods()
    {
        var service = new InMemoryStub();
        var n = new ScheduledNotification("id-1", "t", "b", DateTimeOffset.UtcNow);

        await service.ScheduleAsync(n);
        var pending = await service.GetPendingAsync();
        Assert.AreEqual(1, pending.Count);

        await service.CancelAsync("id-1");
        Assert.AreEqual(0, (await service.GetPendingAsync()).Count);
    }

    private sealed class InMemoryStub : ScheduledNotificationService
    {
        private readonly Dictionary<string, ScheduledNotification> _pending = new();

        public override Task ScheduleAsync(ScheduledNotification notification)
        {
            _pending[notification.Id] = notification;
            return Task.CompletedTask;
        }

        public override Task CancelAsync(string id)
        {
            _pending.Remove(id);
            return Task.CompletedTask;
        }

        public override Task CancelAllAsync()
        {
            _pending.Clear();
            return Task.CompletedTask;
        }

        public override Task<IReadOnlyList<ScheduledNotification>> GetPendingAsync() =>
            Task.FromResult<IReadOnlyList<ScheduledNotification>>(_pending.Values.ToList());
    }
}
