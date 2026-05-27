using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Converters;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class IntToTimeComponentStringConverterTests
{
    private static readonly IntToTimeComponentStringConverter Converter = new();

    [TestMethod]
    public void Convert_SingleDigit_ZeroPaddedToTwo()
    {
        Assert.AreEqual("05", Converter.Convert(5, typeof(string), null!, null!));
    }

    [TestMethod]
    public void Convert_TwoDigits_NotPadded()
    {
        Assert.AreEqual("42", Converter.Convert(42, typeof(string), null!, null!));
    }

    [TestMethod]
    public void Convert_Zero_ReturnsDoubleZero()
    {
        Assert.AreEqual("00", Converter.Convert(0, typeof(string), null!, null!));
    }

    [TestMethod]
    public void Convert_ThreeDigits_NotTruncated()
    {
        // Format "00" widens but does not truncate.
        Assert.AreEqual("123", Converter.Convert(123, typeof(string), null!, null!));
    }

    [TestMethod]
    public void Convert_Negative_KeepsSign()
    {
        // The "00" format preserves negative sign and pads digits.
        Assert.AreEqual("-05", Converter.Convert(-5, typeof(string), null!, null!));
    }

    [TestMethod]
    public void Convert_Long_Coerced()
    {
        Assert.AreEqual("07", Converter.Convert(7L, typeof(string), null!, null!));
    }

    [TestMethod]
    public void Convert_Double_CoercedThroughIConvertible()
    {
        Assert.AreEqual("09", Converter.Convert(9.4, typeof(string), null!, null!));
    }

    [TestMethod]
    public void ConvertBack_NotSupported()
    {
        Assert.Throws<NotSupportedException>(() =>
            Converter.ConvertBack("05", typeof(int), null!, null!));
    }
}
