using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class NotificationPermissionServiceTests
{
    [TestMethod]
    public async Task BaseImplementation_ReturnsGrantedByDefault()
    {
        var service = new NotificationPermissionService();

        Assert.AreEqual(NotificationPermissionStatus.Granted, await service.IsGrantedAsync());
        Assert.AreEqual(NotificationPermissionStatus.Granted, await service.EnsurePermissionAsync());
    }

    [TestMethod]
    public async Task Subclass_CanOverrideStatus()
    {
        var service = new ConfigurableService(NotificationPermissionStatus.Denied);

        Assert.AreEqual(NotificationPermissionStatus.Denied, await service.IsGrantedAsync());
        Assert.AreEqual(NotificationPermissionStatus.Denied, await service.EnsurePermissionAsync());
    }

    [TestMethod]
    public async Task OpenSettings_NoSettingsUri_ReturnsFalse()
    {
        var service = new NoSettingsUriService();

        Assert.IsFalse(await service.OpenSettingsAsync());
    }

    [TestMethod]
    public async Task OpenSettings_DispatchesUri_ToLauncher()
    {
        var service = new CapturingService(new Uri("ms-settings:notifications"));

        await service.OpenSettingsAsync();

        Assert.AreEqual("ms-settings:notifications", service.LastLaunchedUri?.OriginalString);
    }

    private sealed class ConfigurableService : NotificationPermissionService
    {
        private readonly NotificationPermissionStatus _status;

        public ConfigurableService(NotificationPermissionStatus status) => _status = status;

        public override Task<NotificationPermissionStatus> IsGrantedAsync() =>
            Task.FromResult(_status);
    }

    private sealed class NoSettingsUriService : NotificationPermissionService
    {
        protected override Uri? GetSettingsUri() => null;
    }

    private sealed class CapturingService : NotificationPermissionService
    {
        private readonly Uri _uri;

        public Uri? LastLaunchedUri { get; private set; }

        public CapturingService(Uri uri) => _uri = uri;

        protected override Uri? GetSettingsUri() => _uri;

        public override Task<bool> OpenSettingsAsync()
        {
            // Bypass the real Launcher to keep the test free of platform calls.
            LastLaunchedUri = GetSettingsUri();
            return Task.FromResult(LastLaunchedUri is not null);
        }
    }
}
