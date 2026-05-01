using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class FakeStoreServiceTests
{
    [TestMethod]
    public async Task DefaultsToPro_ForDevConvenience()
    {
        var service = new FakeStoreService();

        Assert.IsTrue(await service.HasProAsync());
    }

    [TestMethod]
    public async Task StartsAsPro_False_ReturnsFalseUntilPurchase()
    {
        var service = new FakeStoreService(startsAsPro: false);

        Assert.IsFalse(await service.HasProAsync());
    }

    [TestMethod]
    public async Task TryPurchaseProAsync_FlipsHasProToTrue()
    {
        var service = new FakeStoreService(startsAsPro: false);

        var purchaseResult = await service.TryPurchaseProAsync();

        Assert.IsTrue(purchaseResult);
        Assert.IsTrue(await service.HasProAsync());
    }

    [TestMethod]
    public async Task GetPriceAsync_ReturnsConfiguredFakePrice()
    {
        var service = new FakeStoreService(fakePrice: "$1.99");

        Assert.AreEqual("$1.99", await service.GetPriceAsync());
    }

    [TestMethod]
    public async Task TryRestorePurchasesAsync_ReflectsCurrentProState()
    {
        var service = new FakeStoreService(startsAsPro: false);
        Assert.IsFalse(await service.TryRestorePurchasesAsync());

        await service.TryPurchaseProAsync();

        Assert.IsTrue(await service.TryRestorePurchasesAsync());
    }
}

[TestClass]
public class ProStoreServiceTests
{
    private static readonly ProStoreService Service = new();

    [TestMethod]
    public async Task HasProAsync_AlwaysTrue() => Assert.IsTrue(await Service.HasProAsync());

    [TestMethod]
    public async Task TryPurchaseProAsync_AlwaysTrue() => Assert.IsTrue(await Service.TryPurchaseProAsync());

    [TestMethod]
    public async Task GetPriceAsync_ReturnsNull() => Assert.IsNull(await Service.GetPriceAsync());

    [TestMethod]
    public async Task TryRestorePurchasesAsync_AlwaysTrue() => Assert.IsTrue(await Service.TryRestorePurchasesAsync());
}

[TestClass]
public class StoreServiceTests
{
    [TestMethod]
    public void Ctor_NullOptions_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new StoreService(null!));
    }

    [TestMethod]
    public async Task NonWindowsBuild_ThrowsPlatformNotSupported()
    {
        // The library project compiles StoreService for several TFMs. The test binary
        // references the net10.0 (cross-platform) build where WINDOWS is not defined,
        // so every method throws regardless of the host OS.
        var service = new StoreService(new StoreServiceOptions { ProSkuId = "9P0" });

        await Assert.ThrowsExactlyAsync<PlatformNotSupportedException>(service.HasProAsync);
        await Assert.ThrowsExactlyAsync<PlatformNotSupportedException>(service.TryPurchaseProAsync);
        await Assert.ThrowsExactlyAsync<PlatformNotSupportedException>(service.GetPriceAsync);
        await Assert.ThrowsExactlyAsync<PlatformNotSupportedException>(service.TryRestorePurchasesAsync);
    }
}
