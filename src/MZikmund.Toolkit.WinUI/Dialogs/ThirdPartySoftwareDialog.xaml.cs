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
	public ThirdPartySoftwareDialog() : this(new List<PackageInfo>())
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ThirdPartySoftwareDialog"/> class with the specified packages.
	/// </summary>
	/// <param name="packages">The list of third-party packages to display.</param>
	public ThirdPartySoftwareDialog(IEnumerable<PackageInfo> packages)
	{
		this.InitializeComponent();
		PackagesList.ItemsSource = packages;
	}
}

/// <summary>
/// Represents information about a third-party package dependency.
/// </summary>
/// <param name="Name">The name of the package.</param>
/// <param name="Version">The version of the package.</param>
/// <param name="Url">The URL to the package on NuGet.org.</param>
public record PackageInfo(string Name, string Version, string Url);
