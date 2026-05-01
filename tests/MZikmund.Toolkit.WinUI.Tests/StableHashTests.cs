using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Helpers;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class StableHashTests
{
    [TestMethod]
    public void FromGuid_IsDeterministic()
    {
        var guid = new Guid("11111111-2222-3333-4444-555555555555");

        var first = StableHash.FromGuid(guid);
        var second = StableHash.FromGuid(guid);

        Assert.AreEqual(first, second);
    }

    [TestMethod]
    public void FromGuid_KnownValue_HasExpectedHash()
    {
        var guid = new Guid("11111111-2222-3333-4444-555555555555");
        var bytes = guid.ToByteArray();
        var expected = BitConverter.ToInt32(bytes, 0)
                     ^ BitConverter.ToInt32(bytes, 4)
                     ^ BitConverter.ToInt32(bytes, 8)
                     ^ BitConverter.ToInt32(bytes, 12);

        Assert.AreEqual(expected, StableHash.FromGuid(guid));
    }

    [TestMethod]
    public void FromGuid_EmptyGuid_ReturnsZero()
    {
        Assert.AreEqual(0, StableHash.FromGuid(Guid.Empty));
    }

    [TestMethod]
    public void FromGuid_DifferentGuids_GenerallyProduceDifferentHashes()
    {
        var a = StableHash.FromGuid(Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"));
        var b = StableHash.FromGuid(Guid.Parse("11111111-2222-3333-4444-555555555555"));

        Assert.AreNotEqual(a, b);
    }

    [TestMethod]
    public void FromString_IsDeterministic()
    {
        var first = StableHash.FromString("notification-key");
        var second = StableHash.FromString("notification-key");

        Assert.AreEqual(first, second);
    }

    [TestMethod]
    public void FromString_NullOrEmpty_ReturnsZero()
    {
        Assert.AreEqual(0, StableHash.FromString(null));
        Assert.AreEqual(0, StableHash.FromString(string.Empty));
    }

    [TestMethod]
    public void FromString_DifferentInputs_GenerallyProduceDifferentHashes()
    {
        Assert.AreNotEqual(
            StableHash.FromString("alpha"),
            StableHash.FromString("beta"));
    }

    [TestMethod]
    public void FromString_HandlesNonAsciiUtf8()
    {
        var first = StableHash.FromString("připomínka");
        var second = StableHash.FromString("připomínka");

        Assert.AreEqual(first, second);
        Assert.AreNotEqual(0, first);
    }

    [TestMethod]
    public void FromString_LengthNotMultipleOfFour_StillStable()
    {
        var first = StableHash.FromString("abc");
        var second = StableHash.FromString("abc");

        Assert.AreEqual(first, second);
    }

    [TestMethod]
    public void FromString_LongInput_StillStable()
    {
        var input = new string('x', 10_000);

        Assert.AreEqual(StableHash.FromString(input), StableHash.FromString(input));
    }
}
