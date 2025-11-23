using System.Collections.Generic;

namespace MZikmund.Toolkit.WinUI.Dialogs;

/// <summary>
/// Dialog that displays third-party software dependencies used by the application.
/// </summary>
public sealed partial class ThirdPartySoftwareDialog : ContentDialog
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ThirdPartySoftwareDialog"/> class.
	/// </summary>
	public ThirdPartySoftwareDialog()
	{
		this.InitializeComponent();

		// Initialize the list of packages from generated source if available
		// When EnableThirdPartySoftwareGenerator is false, use an empty list
		PackagesList.ItemsSource = GetPackages();
	}

	private static List<PackageInfo> GetPackages()
	{
#if ENABLE_THIRD_PARTY_SOFTWARE_GENERATOR
		return GeneratedPackageInfo.GetPackages();
#else
		return new List<PackageInfo>();
#endif
	}
}

/// <summary>
/// Represents information about a third-party package dependency.
/// </summary>
/// <param name="Name">The name of the package.</param>
/// <param name="Version">The version of the package.</param>
/// <param name="Url">The URL to the package on NuGet.org.</param>
public record PackageInfo(string Name, string Version, string Url);
