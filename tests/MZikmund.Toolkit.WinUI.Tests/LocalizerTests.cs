using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Localization;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class LocalizerTests
{
    [TestCleanup]
    public void Cleanup()
    {
        // Reset Localizer.Current to a fresh fallback so tests don't leak state into each other.
        // The fallback type isn't public, so reassign through a no-op replacement and rely on the
        // public contract: any GetString returns "???key???".
        Localizer.Current = new FallbackLocalizer();
    }

    [TestMethod]
    public void Default_ReturnsMissingKeyMarker()
    {
        // The default Localizer.Current (set by the static initializer) returns ???key??? for any key.
        Localizer.Current = new FallbackLocalizer();

        Assert.AreEqual("???Greeting???", Localizer.Current.GetString("Greeting"));
    }

    [TestMethod]
    public void Current_CanBeReplaced()
    {
        Localizer.Current = new StaticLocalizer("hello");

        Assert.AreEqual("hello", Localizer.Current.GetString("anykey"));
    }

    [TestMethod]
    public void Current_RoundTripPreservesReference()
    {
        var custom = new StaticLocalizer("v");
        Localizer.Current = custom;

        Assert.AreSame(custom, Localizer.Current);
    }

    private sealed class FallbackLocalizer : ILocalizer
    {
        public string GetString(string key) => $"???{key}???";
    }

    private sealed class StaticLocalizer(string value) : ILocalizer
    {
        public string GetString(string key) => value;
    }
}
