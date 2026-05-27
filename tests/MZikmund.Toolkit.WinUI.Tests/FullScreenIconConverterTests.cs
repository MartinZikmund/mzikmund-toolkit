using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Converters;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class FullScreenIconConverterTests
{
    private static readonly FullScreenIconConverter Converter = new();

    [TestMethod]
    public void Convert_True_ReturnsBackToWindow()
    {
        Assert.AreEqual(Symbol.BackToWindow, Converter.Convert(true, typeof(Symbol), null!, null!));
    }

    [TestMethod]
    public void Convert_False_ReturnsFullScreen()
    {
        Assert.AreEqual(Symbol.FullScreen, Converter.Convert(false, typeof(Symbol), null!, null!));
    }

    [TestMethod]
    public void Convert_Null_ReturnsFullScreen()
    {
        Assert.AreEqual(Symbol.FullScreen, Converter.Convert(null!, typeof(Symbol), null!, null!));
    }

    [TestMethod]
    public void Convert_NonBool_ReturnsFullScreen()
    {
        // Non-bool inputs are treated as "not full screen".
        Assert.AreEqual(Symbol.FullScreen, Converter.Convert("yes", typeof(Symbol), null!, null!));
    }

    [TestMethod]
    public void ConvertBack_BackToWindow_ReturnsTrue()
    {
        Assert.AreEqual(true, Converter.ConvertBack(Symbol.BackToWindow, typeof(bool), null!, null!));
    }

    [TestMethod]
    public void ConvertBack_FullScreen_ReturnsFalse()
    {
        Assert.AreEqual(false, Converter.ConvertBack(Symbol.FullScreen, typeof(bool), null!, null!));
    }
}
