namespace MZikmund.Toolkit.SourceGenerators;

/// <summary>
/// Filtering rules used by <see cref="PackageInfoGenerator"/> to identify "user-facing"
/// referenced assemblies (i.e. third-party deps suitable for an About / Third-party
/// software dialog) vs framework / SDK / source-generator assemblies that should be hidden.
/// </summary>
/// <remarks>
/// Lives in its own class — without a Roslyn base — so it can be unit-tested from
/// outside the generator's host process without dragging in Microsoft.CodeAnalysis at
/// runtime.
/// </remarks>
public static class PackageInfoFilter
{
    /// <summary>
    /// Returns <see langword="true"/> when <paramref name="assemblyName"/> looks like a
    /// user-facing third-party dependency.
    /// </summary>
    public static bool IsUserFacing(string assemblyName)
    {
        if (string.IsNullOrEmpty(assemblyName))
        {
            return false;
        }

        // Framework + BCL roots
        if (assemblyName.StartsWith("System.", System.StringComparison.Ordinal) ||
            assemblyName == "System" ||
            assemblyName == "mscorlib" ||
            assemblyName == "netstandard" ||
            assemblyName.StartsWith("Microsoft.NETCore.", System.StringComparison.Ordinal) ||
            assemblyName.StartsWith("Microsoft.Win32.", System.StringComparison.Ordinal) ||
            assemblyName.StartsWith("Windows.", System.StringComparison.Ordinal))
        {
            return false;
        }

        // SDKs and tooling
        if (assemblyName.EndsWith(".SourceGenerators", System.StringComparison.Ordinal) ||
            assemblyName.EndsWith(".SourceGenerator", System.StringComparison.Ordinal) ||
            assemblyName.EndsWith(".Analyzers", System.StringComparison.Ordinal) ||
            assemblyName.EndsWith(".Analyzer", System.StringComparison.Ordinal))
        {
            return false;
        }

        return true;
    }
}
