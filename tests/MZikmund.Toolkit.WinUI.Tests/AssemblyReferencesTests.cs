using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.Services;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class AssemblyReferencesTests
{
    // Pre-7.0 Uno assembly names (renamed without forwarders) and the Uno 5.2-built SkiaSharp views
    // that hang consuming apps at startup.
    [TestMethod]
    [DataRow("Uno")]
    [DataRow("Uno.UI.Toolkit")]
    [DataRow("SkiaSharp.Views.Windows")]
    public void Library_DoesNotReference(string assemblyName)
    {
        var references = typeof(AppRatingService).Assembly.GetReferencedAssemblies().Select(a => a.Name);

        CollectionAssert.DoesNotContain(references.ToList(), assemblyName);
    }
}
