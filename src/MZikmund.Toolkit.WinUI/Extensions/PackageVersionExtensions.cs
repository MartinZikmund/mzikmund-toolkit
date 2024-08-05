namespace MZikmund.Toolkit.WinUI.Extensions;

/// <summary>
/// Extensions for PackageVersion.
/// </summary>
public static class PackageVersionExtensions
{
    /// <summary>
    /// Returns a string representation of the version.
    /// </summary>
    /// <param name="version">Version.</param>
    /// <param name="majorMinorOnly">Whether to return only major and minor version.</param>
    /// <returns>Version string</returns>
    public static string ToVersionString(this PackageVersion version, bool majorMinorOnly = false) => majorMinorOnly ?
        $"{version.Major}.{version.Minor}" :
        $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
}