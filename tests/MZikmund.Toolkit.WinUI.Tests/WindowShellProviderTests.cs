using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Infrastructure;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class WindowShellProviderTests
{
    [TestMethod]
    public void Shell_BeforeSetShell_Throws()
    {
        var provider = new WindowShellProvider();

        Assert.Throws<InvalidOperationException>(() => _ = provider.Shell);
    }

    [TestMethod]
    public void SetShell_Null_Throws()
    {
        var provider = new WindowShellProvider();

        Assert.Throws<ArgumentNullException>(() => provider.SetShell(null!));
    }

    [TestMethod]
    public void SetShell_RegistersInstance_AndShellReturnsSame()
    {
        var provider = new WindowShellProvider();
        var shell = new StubShell();

        provider.SetShell(shell);

        Assert.AreSame(shell, provider.Shell);
    }

    [TestMethod]
    public void SetShell_Replace_OverwritesPreviousInstance()
    {
        var provider = new WindowShellProvider();
        var first = new StubShell();
        var second = new StubShell();

        provider.SetShell(first);
        provider.SetShell(second);

        Assert.AreSame(second, provider.Shell);
    }

    private sealed class StubShell : IWindowShell
    {
        public object? ViewModel => null;

        public XamlRoot XamlRoot => null!;

        public IServiceProvider ServiceProvider => null!;

        public DispatcherQueue DispatcherQueue => null!;

        public Frame RootFrame => null!;

        public void SetTitleBar(UIElement titleBar)
        {
        }
    }
}
