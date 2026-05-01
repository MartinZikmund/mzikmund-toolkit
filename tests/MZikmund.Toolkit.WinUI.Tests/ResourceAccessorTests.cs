using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Resources;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class ResourceAccessorTests
{
    // The headless test host has no initialized Application.Current, so these tests
    // exercise the null-safety branches and argument validation. Happy-path lookups
    // are exercised manually through the sample gallery page.

    [TestMethod]
    public void GetResource_NullKey_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => ResourceAccessor.GetResource<object>(null!));
    }

    [TestMethod]
    public void TryGetResource_NullKey_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => ResourceAccessor.TryGetResource<object>(null!, out _));
    }

    [TestMethod]
    public void GetResource_WithoutApplication_ThrowsInvalidOperation()
    {
        Assert.Throws<InvalidOperationException>(() => ResourceAccessor.GetResource<object>("AnyKey"));
    }

    [TestMethod]
    public void TryGetResource_WithoutApplication_ReturnsFalse()
    {
        var result = ResourceAccessor.TryGetResource<object>("AnyKey", out var value);

        Assert.IsFalse(result);
        Assert.IsNull(value);
    }

    [TestMethod]
    public void TryGetResource_WithoutApplication_ValueTypeOutDefault()
    {
        var result = ResourceAccessor.TryGetResource<int>("AnyKey", out var value);

        Assert.IsFalse(result);
        Assert.AreEqual(0, value);
    }
}
