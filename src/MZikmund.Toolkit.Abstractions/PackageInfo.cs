#if NETSTANDARD2_0
// Polyfill for records in netstandard2.0
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}
#endif

namespace MZikmund.Toolkit.Abstractions
{
    /// <summary>
    /// Represents information about a third-party package dependency.
    /// </summary>
    /// <param name="Name">The name of the package.</param>
    /// <param name="Version">The version of the package.</param>
    /// <param name="Url">The URL to the package on NuGet.org.</param>
    public record PackageInfo(string Name, string Version, string Url);
}
