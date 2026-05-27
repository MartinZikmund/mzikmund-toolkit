using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.SourceGenerators;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class PackageInfoGeneratorTests
{
    [TestMethod]
    public void IsUserFacing_FrameworkAssembly_ReturnsFalse()
    {
        Assert.IsFalse(PackageInfoFilter.IsUserFacing("System.Runtime"));
        Assert.IsFalse(PackageInfoFilter.IsUserFacing("System.Collections"));
        Assert.IsFalse(PackageInfoFilter.IsUserFacing("System"));
        Assert.IsFalse(PackageInfoFilter.IsUserFacing("mscorlib"));
        Assert.IsFalse(PackageInfoFilter.IsUserFacing("netstandard"));
    }

    [TestMethod]
    public void IsUserFacing_NetCoreOrWinAssembly_ReturnsFalse()
    {
        Assert.IsFalse(PackageInfoFilter.IsUserFacing("Microsoft.NETCore.App"));
        Assert.IsFalse(PackageInfoFilter.IsUserFacing("Windows.Foundation"));
        Assert.IsFalse(PackageInfoFilter.IsUserFacing("Microsoft.Win32.Registry"));
    }

    [TestMethod]
    public void IsUserFacing_AnalyzerOrSourceGenerator_ReturnsFalse()
    {
        Assert.IsFalse(PackageInfoFilter.IsUserFacing("Foo.SourceGenerators"));
        Assert.IsFalse(PackageInfoFilter.IsUserFacing("Bar.Analyzers"));
        Assert.IsFalse(PackageInfoFilter.IsUserFacing("Baz.Analyzer"));
    }

    [TestMethod]
    public void IsUserFacing_NullOrEmpty_ReturnsFalse()
    {
        Assert.IsFalse(PackageInfoFilter.IsUserFacing(null!));
        Assert.IsFalse(PackageInfoFilter.IsUserFacing(string.Empty));
    }

    [TestMethod]
    public void IsUserFacing_RegularPackage_ReturnsTrue()
    {
        Assert.IsTrue(PackageInfoFilter.IsUserFacing("Newtonsoft.Json"));
        Assert.IsTrue(PackageInfoFilter.IsUserFacing("CommunityToolkit.Mvvm"));
        Assert.IsTrue(PackageInfoFilter.IsUserFacing("Uno.UI"));
    }

    [TestMethod]
    public void IsUserFacing_DoesNotMisidentifySystemPrefix()
    {
        // "System" exact-match excluded; "Systemic" does not match the family rule.
        Assert.IsTrue(PackageInfoFilter.IsUserFacing("Systemic"));
    }
}
