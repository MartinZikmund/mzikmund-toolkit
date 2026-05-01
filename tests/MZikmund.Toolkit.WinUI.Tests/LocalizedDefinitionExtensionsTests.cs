using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Localization;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class LocalizedDefinitionExtensionsTests
{
    private static readonly Definition Sample = new("Achievement_FirstSteps_Name", "Achievement_FirstSteps_Description");

    [TestCleanup]
    public void Cleanup() => Localizer.Current = new MapLocalizer(new());

    [TestMethod]
    public void GetName_WithExplicitLocalizer_ResolvesNameKey()
    {
        var localizer = new MapLocalizer(new() { ["Achievement_FirstSteps_Name"] = "First Steps" });

        Assert.AreEqual("First Steps", Sample.GetName(localizer));
    }

    [TestMethod]
    public void GetDescription_WithExplicitLocalizer_ResolvesDescriptionKey()
    {
        var localizer = new MapLocalizer(new() { ["Achievement_FirstSteps_Description"] = "Complete onboarding." });

        Assert.AreEqual("Complete onboarding.", Sample.GetDescription(localizer));
    }

    [TestMethod]
    public void GetName_WithoutArgument_UsesLocalizerCurrent()
    {
        Localizer.Current = new MapLocalizer(new() { ["Achievement_FirstSteps_Name"] = "Z static" });

        Assert.AreEqual("Z static", Sample.GetName());
    }

    [TestMethod]
    public void GetDescription_WithoutArgument_UsesLocalizerCurrent()
    {
        Localizer.Current = new MapLocalizer(new() { ["Achievement_FirstSteps_Description"] = "via Current" });

        Assert.AreEqual("via Current", Sample.GetDescription());
    }

    [TestMethod]
    public void GetDescription_EmptyDescriptionKey_ShortCircuitsToEmpty()
    {
        var noDesc = new Definition("name", string.Empty);
        var localizer = new ThrowingLocalizer();

        Assert.AreEqual(string.Empty, noDesc.GetDescription(localizer));
        Assert.AreEqual(string.Empty, noDesc.GetDescription(localizer: localizer));
    }

    [TestMethod]
    public void GetName_NullDefinition_Throws()
    {
        ILocalizedDefinition? def = null;

        Assert.Throws<ArgumentNullException>(() => def!.GetName(new MapLocalizer(new())));
    }

    [TestMethod]
    public void GetName_NullLocalizer_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Sample.GetName(localizer: null!));
    }

    private sealed record Definition(string NameKey, string DescriptionKey) : ILocalizedDefinition;

    private sealed class MapLocalizer(Dictionary<string, string> entries) : ILocalizer
    {
        public string GetString(string key) =>
            entries.TryGetValue(key, out var value) ? value : $"???{key}???";
    }

    private sealed class ThrowingLocalizer : ILocalizer
    {
        public string GetString(string key) => throw new InvalidOperationException("Should not be called.");
    }
}
