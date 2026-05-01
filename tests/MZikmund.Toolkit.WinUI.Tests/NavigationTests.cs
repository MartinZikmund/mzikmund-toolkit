using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.Navigation;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class NavigationInfoAttributeTests
{
    [TestMethod]
    public void Ctor_NullSection_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new NavigationInfoAttribute(null!));
    }

    [TestMethod]
    public void Ctor_DefaultsTransitionToDefault()
    {
        var attr = new NavigationInfoAttribute("main");

        Assert.AreEqual("main", attr.SectionTag);
        Assert.AreEqual(NavigationTransition.Default, attr.Transition);
    }

    [TestMethod]
    public void Ctor_AcceptsCustomTransition()
    {
        var attr = new NavigationInfoAttribute("settings", NavigationTransition.FromBottom);

        Assert.AreEqual("settings", attr.SectionTag);
        Assert.AreEqual(NavigationTransition.FromBottom, attr.Transition);
    }

    [TestMethod]
    public void Attribute_DiscoverableViaReflection()
    {
        var attr = typeof(TaggedPage).GetCustomAttributes(typeof(NavigationInfoAttribute), false);

        Assert.AreEqual(1, attr.Length);
        Assert.AreEqual("demo", ((NavigationInfoAttribute)attr[0]).SectionTag);
    }

    [NavigationInfo("demo")]
    private sealed class TaggedPage
    {
    }
}

[TestClass]
public class NavigationServiceTransitionTests
{
    // BuildTransitionInfo round-trips can't be exercised in the headless test runner
    // because instantiating WinUI's NavigationTransitionInfo subclasses fails with
    // "Ref assembly" (the reference assemblies have no implementation). The default-
    // returns-null branch is still safe to test since it doesn't instantiate anything.

    [TestMethod]
    public void BuildTransitionInfo_Default_ReturnsNull()
    {
        Assert.IsNull(NavigationService.BuildTransitionInfo(NavigationTransition.Default));
    }

    [TestMethod]
    public void Ctor_NullProvider_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new NavigationService(null!));
    }

    [TestMethod]
    public void Navigate_NullPageType_Throws()
    {
        var service = new NavigationService(new WindowShellProvider());

        Assert.Throws<ArgumentNullException>(() => service.Navigate(null!));
    }

    [TestMethod]
    public void Navigate_BeforeShellRegistered_Throws()
    {
        // No shell registered → InvalidOperationException with a helpful message.
        var service = new NavigationService(new WindowShellProvider());

        Assert.Throws<InvalidOperationException>(() => service.Navigate(typeof(object)));
    }

    [TestMethod]
    public void GoBack_BeforeShellRegistered_ReturnsFalse()
    {
        var service = new NavigationService(new WindowShellProvider());

        Assert.IsFalse(service.GoBack());
    }
}
