using System.Collections.Generic;
using MZikmund.Toolkit.Abstractions;

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
