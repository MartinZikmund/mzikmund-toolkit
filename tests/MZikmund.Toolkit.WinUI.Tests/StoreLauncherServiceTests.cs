using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class StoreLauncherServiceTests
{
    [TestMethod]
    public void Ctor_NullOptions_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new StoreLauncherService(null!));
    }

    [TestMethod]
    public async Task RateAppAsync_PassesPlatformUri_ToLauncher()
    {
        var service = new TestableStoreLauncherService(
            StorePlatform.Windows,
            new StoreLauncherOptions { WindowsProductId = "9P0" });

        await service.RateAppAsync();

        Assert.AreEqual("ms-windows-store://review/?ProductId=9P0", service.LastUri?.OriginalString);
    }

    [TestMethod]
    public async Task ShowAppListingAsync_PassesPlatformUri_ToLauncher()
    {
        var service = new TestableStoreLauncherService(
            StorePlatform.Apple,
            new StoreLauncherOptions { AppleAppId = "42" });

        await service.ShowAppListingAsync();

        Assert.AreEqual("itms-apps://itunes.apple.com/app/id42", service.LastUri?.OriginalString);
    }

    [TestMethod]
    public async Task MoreAppsByPublisherAsync_NoPublisher_ReturnsFalseAndDoesNotLaunch()
    {
        var service = new TestableStoreLauncherService(
            StorePlatform.Windows,
            new StoreLauncherOptions { WindowsProductId = "9P0" });

        var result = await service.MoreAppsByPublisherAsync();

        Assert.IsFalse(result);
        Assert.IsNull(service.LastUri);
    }

    [TestMethod]
    public async Task UnsupportedPlatform_AllMethodsReturnFalse()
    {
        var service = new TestableStoreLauncherService(
            StorePlatform.Unsupported,
            new StoreLauncherOptions { WindowsProductId = "9P0", PublisherName = "X" });

        Assert.IsFalse(await service.RateAppAsync());
        Assert.IsFalse(await service.ShowAppListingAsync());
        Assert.IsFalse(await service.MoreAppsByPublisherAsync());
        Assert.IsNull(service.LastUri);
    }

    private sealed class TestableStoreLauncherService : StoreLauncherService
    {
        private readonly StorePlatform _platform;

        public Uri? LastUri { get; private set; }

        public TestableStoreLauncherService(StorePlatform platform, StoreLauncherOptions options)
            : base(options)
        {
            _platform = platform;
        }

        protected override StorePlatform CurrentPlatform => _platform;

        protected override Task<bool> LaunchAsync(Uri? uri)
        {
            LastUri = uri;
            return Task.FromResult(uri is not null);
        }
    }
}
