# MZikmund.Toolkit

Reusable services and tools for cross-platform apps made with Uno Platform and WinUI.

## Projects

### MZikmund.Toolkit.WinUI

A library containing reusable UI components, services, and infrastructure for Uno Platform and WinUI applications.

**Features:**
- Services for preferences, navigation, and more
- UI dialogs including `ThirdPartySoftwareDialog` for package attribution
- Cross-platform support for iOS, Android, macOS, Windows, and WebAssembly

### MZikmund.Toolkit.SourceGenerators

Source generators that enhance the toolkit functionality.

**Features:**
- `PackageInfoGenerator` - Automatically generates a list of third-party package dependencies at build time

## Using ThirdPartySoftwareDialog

To display third-party software attribution in your app:

1. Install the `MZikmund.Toolkit.WinUI` package
2. Enable the source generator in your `.csproj`:

```xml
<PropertyGroup>
  <EnableThirdPartySoftwareGenerator>true</EnableThirdPartySoftwareGenerator>
</PropertyGroup>
```

3. Display the dialog in your app:

```csharp
using MZikmund.Toolkit.WinUI.Dialogs;

var dialog = new ThirdPartySoftwareDialog
{
    XamlRoot = this.Content.XamlRoot
};
await dialog.ShowAsync();
```

The dialog will automatically display all detected third-party packages with links to their NuGet pages.

## Building

This project requires .NET 8.0 SDK and Uno Platform workloads:

```bash
dotnet workload restore
dotnet build
```

## License

See [LICENSE](LICENSE) file for details.