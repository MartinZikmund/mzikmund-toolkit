using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

// LocalSettings is null in a plain net10.0 host, so these cover the JSON contracts, not the storage.
[TestClass]
public class PreferencesTests
{
    [TestMethod]
    public void ResolvedFromDI_IgnoresRegisteredJsonSerializerOptions()
    {
        ServiceCollection services = new();
        services.AddSingleton(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        services.AddSingleton<IPreferences, Preferences>();
        using var provider = services.BuildServiceProvider();

        AssertRoundTrips((Preferences)provider.GetRequiredService<IPreferences>());
    }

    [TestMethod]
    public void Default_RoundTripsComplexValue() => AssertRoundTrips(new Preferences());

    [TestMethod]
    public void WithTypeInfoResolver_RoundTripsComplexValue() =>
        AssertRoundTrips(new Preferences(PreferencesTestContext.Default));

    [TestMethod]
    public void NullTypeInfoResolver_Throws() =>
        Assert.Throws<ArgumentNullException>(() => new Preferences(null!));

    private static void AssertRoundTrips(Preferences preferences)
    {
        var typeInfo = preferences.GetTypeInfo<StoredValue>();

        var json = JsonSerializer.Serialize(new StoredValue(42), typeInfo);

        Assert.AreEqual(new StoredValue(42), JsonSerializer.Deserialize(json, typeInfo));
    }
}

public record StoredValue(int Number);

[JsonSerializable(typeof(StoredValue))]
public partial class PreferencesTestContext : JsonSerializerContext
{
}
