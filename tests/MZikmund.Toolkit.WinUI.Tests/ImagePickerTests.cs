using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class ImagePickerOptionsTests
{
    [TestMethod]
    public void Defaults_AreReasonable()
    {
        var options = new ImagePickerOptions();

        Assert.AreEqual("Images", options.TargetSubfolder);
        CollectionAssert.AreEqual(
            new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" },
            options.AllowedExtensions.ToList());
    }

    [TestMethod]
    public void InitOnly_AppliesCustomValues()
    {
        var options = new ImagePickerOptions
        {
            TargetSubfolder = "Avatars",
            AllowedExtensions = [".jpg", ".png"],
        };

        Assert.AreEqual("Avatars", options.TargetSubfolder);
        CollectionAssert.AreEqual(new[] { ".jpg", ".png" }, options.AllowedExtensions.ToList());
    }
}

[TestClass]
public class ImagePickerServiceTests
{
    [TestMethod]
    public void BuildCopiedFileName_PreservesExtension()
    {
        var name = ImagePickerService.BuildCopiedFileName(@"C:\Pictures\dog.JPG");

        Assert.IsTrue(name.EndsWith(".JPG", StringComparison.Ordinal));
        Assert.AreEqual(36, name.Length); // 32 hex + ".JPG"
    }

    [TestMethod]
    public void BuildCopiedFileName_NoExtension_ReturnsBareGuid()
    {
        var name = ImagePickerService.BuildCopiedFileName(@"C:\NoDot");

        Assert.AreEqual(32, name.Length); // raw "N"-format Guid
        Assert.IsFalse(name.Contains('.'));
    }

    [TestMethod]
    public void BuildCopiedFileName_TwoCalls_ProduceDifferentNames()
    {
        var first = ImagePickerService.BuildCopiedFileName("a.jpg");
        var second = ImagePickerService.BuildCopiedFileName("a.jpg");

        Assert.AreNotEqual(first, second);
    }

    [TestMethod]
    public async Task CopyToLocalFolderAsync_NullSource_Throws()
    {
        var service = new ImagePickerService();

        await Assert.ThrowsAsync<ArgumentNullException>(() => service.CopyToLocalFolderAsync(null!));
    }
}
