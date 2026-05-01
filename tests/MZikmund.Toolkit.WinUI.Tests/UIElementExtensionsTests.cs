using Microsoft.UI.Xaml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Extensions;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class UIElementExtensionsTests
{
    // The visual-tree walk relies on VisualTreeHelper, which requires a running XAML
    // host. Headless tests can only cover the null-arg guards; happy-path lookup is
    // verified manually through the sample gallery page.

    [TestMethod]
    public void GetServiceProvider_NullElement_Throws()
    {
        UIElement? element = null;

        Assert.Throws<ArgumentNullException>(() => element!.GetServiceProvider());
    }

    [TestMethod]
    public void TryGetServiceProvider_NullElement_Throws()
    {
        UIElement? element = null;

        Assert.Throws<ArgumentNullException>(() => element!.TryGetServiceProvider());
    }
}
