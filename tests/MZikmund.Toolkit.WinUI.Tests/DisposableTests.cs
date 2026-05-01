using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Disposables;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class DisposableTests
{
    [TestMethod]
    public void Create_NullAction_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => Disposable.Create(null!));
    }

    [TestMethod]
    public void Dispose_RunsActionOnce()
    {
        var count = 0;
        var sut = Disposable.Create(() => count++);

        sut.Dispose();

        Assert.AreEqual(1, count);
    }

    [TestMethod]
    public void Dispose_MultipleTimes_RunsActionExactlyOnce()
    {
        var count = 0;
        var sut = Disposable.Create(() => count++);

        sut.Dispose();
        sut.Dispose();
        sut.Dispose();

        Assert.AreEqual(1, count);
    }

    [TestMethod]
    public void Dispose_PropagatesExceptionFromAction()
    {
        var sut = Disposable.Create(() => throw new InvalidOperationException("boom"));

        Assert.Throws<InvalidOperationException>(() => sut.Dispose());
    }

    [TestMethod]
    public void Empty_DoesNothing()
    {
        var sut = Disposable.Empty;

        sut.Dispose();
        sut.Dispose();
        // No exception ⇒ pass.
    }

    [TestMethod]
    public void Empty_ReturnsSameInstance()
    {
        Assert.AreSame(Disposable.Empty, Disposable.Empty);
    }

    [TestMethod]
    public void UsingScope_RunsActionOnExit()
    {
        var count = 0;
        using (Disposable.Create(() => count++))
        {
            Assert.AreEqual(0, count);
        }

        Assert.AreEqual(1, count);
    }
}
