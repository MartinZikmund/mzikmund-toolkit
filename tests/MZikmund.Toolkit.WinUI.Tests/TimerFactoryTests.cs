using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class FakeTimerFactoryTests
{
    [TestMethod]
    public void CreateTimer_ReturnsStoppedTimer_WithRequestedInterval()
    {
        var factory = new FakeTimerFactory();

        var timer = factory.CreateTimer(TimeSpan.FromSeconds(5));

        Assert.IsFalse(timer.IsRunning);
        Assert.AreEqual(TimeSpan.FromSeconds(5), timer.Interval);
    }

    [TestMethod]
    public void CreateTimer_TracksAllCreatedTimers()
    {
        var factory = new FakeTimerFactory();

        var a = factory.CreateTimer(TimeSpan.FromSeconds(1));
        var b = factory.CreateTimer(TimeSpan.FromSeconds(2));

        Assert.AreEqual(2, factory.Timers.Count);
        Assert.AreSame(a, factory.Timers[0]);
        Assert.AreSame(b, factory.Timers[1]);
    }

    [TestMethod]
    public void Start_FlipsIsRunning()
    {
        var timer = (FakeTimer)new FakeTimerFactory().CreateTimer(TimeSpan.FromSeconds(1));

        timer.Start();

        Assert.IsTrue(timer.IsRunning);
    }

    [TestMethod]
    public void Stop_FlipsIsRunningOff()
    {
        var timer = (FakeTimer)new FakeTimerFactory().CreateTimer(TimeSpan.FromSeconds(1));
        timer.Start();

        timer.Stop();

        Assert.IsFalse(timer.IsRunning);
    }

    [TestMethod]
    public void RaiseTick_WhenRunning_FiresTickEvent()
    {
        var timer = (FakeTimer)new FakeTimerFactory().CreateTimer(TimeSpan.FromSeconds(1));
        var ticks = 0;
        timer.Tick += (_, _) => ticks++;
        timer.Start();

        timer.RaiseTick();

        Assert.AreEqual(1, ticks);
    }

    [TestMethod]
    public void RaiseTick_WhenStopped_DoesNotFire()
    {
        var timer = (FakeTimer)new FakeTimerFactory().CreateTimer(TimeSpan.FromSeconds(1));
        var ticks = 0;
        timer.Tick += (_, _) => ticks++;

        timer.RaiseTick();

        Assert.AreEqual(0, ticks);
    }

    [TestMethod]
    public void RaiseTick_Count_FiresMultipleTimes()
    {
        var timer = (FakeTimer)new FakeTimerFactory().CreateTimer(TimeSpan.FromSeconds(1));
        var ticks = 0;
        timer.Tick += (_, _) => ticks++;
        timer.Start();

        timer.RaiseTick(count: 5);

        Assert.AreEqual(5, ticks);
    }
}
