# MZikmund.Toolkit.SourceGenerators

This project contains source generators for the MZikmund.Toolkit.

## PackageInfoGenerator

A source generator that automatically creates a list of third-party package dependencies at build time. This is used by the `ThirdPartySoftwareDialog` in `MZikmund.Toolkit.WinUI` to display package attribution.

### How to Enable

The generator is **disabled by default**. To enable it in your project, add the following property to your `.csproj` file:

```xml
<PropertyGroup>
  <EnableThirdPartySoftwareGenerator>true</EnableThirdPartySoftwareGenerator>
</PropertyGroup>
```

### What It Does

When enabled, the generator:
1. Reads package information from `Directory.Packages.props`
2. Extracts additional package information from compilation metadata
3. Filters out framework and internal packages
4. Generates a `GeneratedPackageInfo` class with a `GetPackages()` method

### Usage

After enabling the generator, you can use the `ThirdPartySoftwareDialog` to display the package list:

```csharp
using MZikmund.Toolkit.WinUI.Dialogs;

// The generator creates GeneratedPackageInfo in your app's root namespace
var dialog = new ThirdPartySoftwareDialog(GeneratedPackageInfo.GetPackages())
{
    XamlRoot = this.Content.XamlRoot
};
await dialog.ShowAsync();
```

The generator creates a `GeneratedPackageInfo` class in your application's root namespace (matching your assembly name) with a `GetPackages()` method returning `List<PackageInfo>`. 

**Note:** You must also reference the `MZikmund.Toolkit.Abstractions` package in your project, as it contains the `PackageInfo` model used by the generated code.

The dialog will display all detected third-party packages with links to their NuGet pages.

### Customization

The generator uses smart filtering to exclude framework packages. If you need to customize the filtering logic, you can modify the `ShouldIncludePackage` method in `PackageInfoGenerator.cs`.
