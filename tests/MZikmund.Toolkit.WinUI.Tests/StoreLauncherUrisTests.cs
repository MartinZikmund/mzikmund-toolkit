using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class StoreLauncherUrisTests
{
    private static readonly StoreLauncherOptions Full = new()
    {
        WindowsProductId = "9P1234567890",
        AndroidPackageName = "com.example.app",
        AppleAppId = "1234567890",
        PublisherName = "Contoso",
    };

    [TestMethod]
    public void Rate_Windows_BuildsReviewProtocolUri()
    {
        var uri = StoreLauncherUris.Rate(StorePlatform.Windows, Full);

        Assert.AreEqual("ms-windows-store://review/?ProductId=9P1234567890", uri?.OriginalString);
    }

    [TestMethod]
    public void Rate_Android_BuildsMarketDetailsUri()
    {
        var uri = StoreLauncherUris.Rate(StorePlatform.Android, Full);

        Assert.AreEqual("market://details?id=com.example.app", uri?.OriginalString);
    }

    [TestMethod]
    public void Rate_Apple_BuildsItmsAppsUriWithReviewAction()
    {
        var uri = StoreLauncherUris.Rate(StorePlatform.Apple, Full);

        Assert.AreEqual("itms-apps://itunes.apple.com/app/id1234567890?action=write-review", uri?.OriginalString);
    }

    [TestMethod]
    public void Rate_Unsupported_ReturnsNull()
    {
        Assert.IsNull(StoreLauncherUris.Rate(StorePlatform.Unsupported, Full));
    }

    [TestMethod]
    public void Rate_MissingPlatformIdentifier_ReturnsNull()
    {
        var noWindows = new StoreLauncherOptions { AndroidPackageName = "x" };

        Assert.IsNull(StoreLauncherUris.Rate(StorePlatform.Windows, noWindows));
    }

    [TestMethod]
    public void Listing_Windows_UsesPdpProtocol()
    {
        var uri = StoreLauncherUris.Listing(StorePlatform.Windows, Full);

        Assert.AreEqual("ms-windows-store://pdp/?ProductId=9P1234567890", uri?.OriginalString);
    }

    [TestMethod]
    public void Listing_Apple_OmitsReviewAction()
    {
        var uri = StoreLauncherUris.Listing(StorePlatform.Apple, Full);

        Assert.AreEqual("itms-apps://itunes.apple.com/app/id1234567890", uri?.OriginalString);
    }

    [TestMethod]
    public void Listing_MissingId_ReturnsNull()
    {
        Assert.IsNull(StoreLauncherUris.Listing(StorePlatform.Windows, new StoreLauncherOptions()));
    }

    [TestMethod]
    public void MoreApps_Windows_BuildsPublisherProtocol()
    {
        var uri = StoreLauncherUris.MoreApps(StorePlatform.Windows, Full);

        Assert.AreEqual("ms-windows-store://publisher/?name=Contoso", uri?.OriginalString);
    }

    [TestMethod]
    public void MoreApps_Android_BuildsMarketSearch()
    {
        var uri = StoreLauncherUris.MoreApps(StorePlatform.Android, Full);

        Assert.AreEqual("market://search?q=pub:Contoso", uri?.OriginalString);
    }

    [TestMethod]
    public void MoreApps_Apple_BuildsItunesSearch()
    {
        var uri = StoreLauncherUris.MoreApps(StorePlatform.Apple, Full);

        Assert.AreEqual("itms-apps://itunes.apple.com/search?term=Contoso", uri?.OriginalString);
    }

    [TestMethod]
    public void MoreApps_PublisherNameWithSpace_IsUrlEncoded()
    {
        var options = new StoreLauncherOptions { PublisherName = "Acme & Co" };
        var uri = StoreLauncherUris.MoreApps(StorePlatform.Windows, options);

        // Uri.EscapeDataString encodes "&" → "%26" and space → "%20".
        Assert.AreEqual("ms-windows-store://publisher/?name=Acme%20%26%20Co", uri?.OriginalString);
    }

    [TestMethod]
    public void MoreApps_MissingPublisherName_ReturnsNull()
    {
        var options = new StoreLauncherOptions { WindowsProductId = "x" };

        Assert.IsNull(StoreLauncherUris.MoreApps(StorePlatform.Windows, options));
    }
}
