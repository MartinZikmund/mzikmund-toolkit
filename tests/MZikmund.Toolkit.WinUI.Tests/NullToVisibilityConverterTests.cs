using Microsoft.UI.Xaml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Converters;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class NullToVisibilityConverterTests
{
    [TestMethod]
    public void Convert_NonNull_ReturnsVisible()
    {
        var converter = new NullToVisibilityConverter();

        Assert.AreEqual(Visibility.Visible, converter.Convert("hi", typeof(Visibility), null!, null!));
    }

    [TestMethod]
    public void Convert_Null_ReturnsCollapsed()
    {
        var converter = new NullToVisibilityConverter();

        Assert.AreEqual(Visibility.Collapsed, converter.Convert(null!, typeof(Visibility), null!, null!));
    }

    [TestMethod]
    public void Convert_Inverted_NullReturnsVisible()
    {
        var converter = new NullToVisibilityConverter { Invert = true };

        Assert.AreEqual(Visibility.Visible, converter.Convert(null!, typeof(Visibility), null!, null!));
    }

    [TestMethod]
    public void Convert_Inverted_NonNullReturnsCollapsed()
    {
        var converter = new NullToVisibilityConverter { Invert = true };

        Assert.AreEqual(Visibility.Collapsed, converter.Convert("anything", typeof(Visibility), null!, null!));
    }

    [TestMethod]
    public void Convert_EmptyString_TreatedAsNonNull()
    {
        // The converter checks reference null only — empty strings are still "have value".
        var converter = new NullToVisibilityConverter();

        Assert.AreEqual(Visibility.Visible, converter.Convert(string.Empty, typeof(Visibility), null!, null!));
    }

    [TestMethod]
    public void ConvertBack_NotSupported()
    {
        var converter = new NullToVisibilityConverter();

        Assert.Throws<NotSupportedException>(() =>
            converter.ConvertBack(Visibility.Visible, typeof(object), null!, null!));
    }
}
