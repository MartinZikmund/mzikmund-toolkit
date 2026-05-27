using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class DisplayRequestManagerTests
{
    [TestMethod]
    public void NewInstance_ActiveCountIsZero()
    {
        var sut = new DisplayRequestManager();

        Assert.AreEqual(0, sut.ActiveCount);
    }

    [TestMethod]
    public void Acquire_IncrementsActiveCount()
    {
        var sut = new DisplayRequestManager();

        sut.Acquire();

        Assert.AreEqual(1, sut.ActiveCount);
    }

    [TestMethod]
    public void Acquire_Multiple_StacksCount()
    {
        var sut = new DisplayRequestManager();

        sut.Acquire();
        sut.Acquire();
        sut.Acquire();

        Assert.AreEqual(3, sut.ActiveCount);
    }

    [TestMethod]
    public void Release_DecrementsActiveCount()
    {
        var sut = new DisplayRequestManager();
        sut.Acquire();
        sut.Acquire();

        sut.Release();

        Assert.AreEqual(1, sut.ActiveCount);
    }

    [TestMethod]
    public void Release_OnEmpty_IsNoOp()
    {
        var sut = new DisplayRequestManager();

        sut.Release();
        sut.Release();

        Assert.AreEqual(0, sut.ActiveCount);
    }

    [TestMethod]
    public void Clear_DropsAllAcquisitions()
    {
        var sut = new DisplayRequestManager();
        sut.Acquire();
        sut.Acquire();
        sut.Acquire();

        sut.Clear();

        Assert.AreEqual(0, sut.ActiveCount);
    }

    [TestMethod]
    public void AcquireRelease_PairedToZero_AllowsReacquisition()
    {
        var sut = new DisplayRequestManager();

        sut.Acquire();
        sut.Release();
        sut.Acquire();

        Assert.AreEqual(1, sut.ActiveCount);
    }
}

[TestClass]
public class LauncherServiceTests
{
    [TestMethod]
    public void LaunchUriAsync_NullUri_Throws()
    {
        var service = new LauncherService();

        Assert.Throws<ArgumentNullException>(() => service.LaunchUriAsync(null!));
    }
}
