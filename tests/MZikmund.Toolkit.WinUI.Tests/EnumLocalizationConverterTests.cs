using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Converters;
using MZikmund.Toolkit.WinUI.Localization;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class EnumLocalizationConverterTests
{
    private static readonly EnumLocalizationConverter Converter = new();

    [TestCleanup]
    public void Cleanup()
    {
        Localizer.Current = new FallbackLocalizer();
    }

    [TestMethod]
    public void Convert_KnownEnum_LocalizedThroughLocalizer()
    {
        Localizer.Current = new MapLocalizer(new()
        {
            ["OrderStatus_Pending"] = "Pending",
            ["OrderStatus_Shipped"] = "Shipped",
        });

        Assert.AreEqual("Shipped", Converter.Convert(OrderStatus.Shipped, typeof(string), null!, null!));
    }

    [TestMethod]
    public void Convert_UnknownKey_FallsBackToLocalizerFallback()
    {
        Localizer.Current = new FallbackLocalizer();

        Assert.AreEqual("???OrderStatus_Cancelled???",
            Converter.Convert(OrderStatus.Cancelled, typeof(string), null!, null!));
    }

    [TestMethod]
    public void Convert_Null_ReturnsEmptyString()
    {
        Assert.AreEqual(string.Empty, Converter.Convert(null!, typeof(string), null!, null!));
    }

    [TestMethod]
    public void Convert_NonEnum_FallsBackToToString()
    {
        Localizer.Current = new MapLocalizer(new());

        Assert.AreEqual("hello", Converter.Convert("hello", typeof(string), null!, null!));
    }

    [TestMethod]
    public void Convert_KeyConvention_UsesEnumTypeName()
    {
        var captured = new CaptureKey();
        Localizer.Current = captured;

        Converter.Convert(OrderStatus.Pending, typeof(string), null!, null!);

        Assert.AreEqual("OrderStatus_Pending", captured.LastKey);
    }

    [TestMethod]
    public void ConvertBack_NotSupported()
    {
        Assert.Throws<NotSupportedException>(() =>
            Converter.ConvertBack("Pending", typeof(OrderStatus), null!, null!));
    }

    private enum OrderStatus
    {
        Pending,
        Shipped,
        Cancelled,
    }

    private sealed class FallbackLocalizer : ILocalizer
    {
        public string GetString(string key) => $"???{key}???";
    }

    private sealed class MapLocalizer(Dictionary<string, string> entries) : ILocalizer
    {
        public string GetString(string key) =>
            entries.TryGetValue(key, out var value) ? value : $"???{key}???";
    }

    private sealed class CaptureKey : ILocalizer
    {
        public string? LastKey { get; private set; }

        public string GetString(string key)
        {
            LastKey = key;
            return key;
        }
    }
}
