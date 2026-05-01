using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;
using MZikmund.Toolkit.WinUI.ViewModels;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class LoadingIndicatorTests
{
    [TestMethod]
    public void Ctor_NullShellViewModel_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new LoadingIndicator(null!));
    }

    [TestMethod]
    public void BeginLoading_FlipsIsLoadingTrue_AndSetsStatus()
    {
        var shell = new StubShellViewModel();
        var indicator = new LoadingIndicator(shell);

        using var scope = indicator.BeginLoading("Saving…");

        Assert.IsTrue(indicator.IsLoading);
        Assert.IsTrue(shell.IsLoading);
        Assert.AreEqual("Saving…", indicator.StatusMessage);
    }

    [TestMethod]
    public void Dispose_LastScope_ClearsIsLoadingAndStatus()
    {
        var shell = new StubShellViewModel();
        var indicator = new LoadingIndicator(shell);
        var scope = indicator.BeginLoading("Saving…");

        scope.Dispose();

        Assert.IsFalse(indicator.IsLoading);
        Assert.IsNull(indicator.StatusMessage);
    }

    [TestMethod]
    public void NestedScopes_KeepIsLoadingTrueUntilLastDisposed()
    {
        var shell = new StubShellViewModel();
        var indicator = new LoadingIndicator(shell);

        var outer = indicator.BeginLoading("Outer");
        var inner = indicator.BeginLoading("Inner");
        Assert.IsTrue(indicator.IsLoading);

        inner.Dispose();
        Assert.IsTrue(indicator.IsLoading, "Outer scope still active.");

        outer.Dispose();
        Assert.IsFalse(indicator.IsLoading);
    }

    [TestMethod]
    public void DisposeTwice_OnSameScope_IsNoOp()
    {
        var shell = new StubShellViewModel();
        var indicator = new LoadingIndicator(shell);

        var scope = indicator.BeginLoading();
        scope.Dispose();
        scope.Dispose();

        Assert.IsFalse(indicator.IsLoading);
    }

    [TestMethod]
    public void BeginLoading_WithoutMessage_PreservesPriorMessage()
    {
        var shell = new StubShellViewModel();
        var indicator = new LoadingIndicator(shell);
        indicator.StatusMessage = "Pre-existing";

        using var scope = indicator.BeginLoading();

        Assert.AreEqual("Pre-existing", indicator.StatusMessage);
    }

    [TestMethod]
    public void StatusMessage_SetterRoutesToShellViewModel()
    {
        var shell = new StubShellViewModel();
        var indicator = new LoadingIndicator(shell);

        indicator.StatusMessage = "Updated";

        Assert.AreEqual("Updated", shell.LoadingStatusMessage);
    }

    private sealed class StubShellViewModel : IWindowShellViewModel
    {
        public string Title { get; set; } = string.Empty;

        public bool IsLoading { get; set; }

        public string? LoadingStatusMessage { get; set; }

        public ICommand GoBackCommand => throw new NotSupportedException();
    }
}
