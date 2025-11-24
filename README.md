# MZikmund Toolkit

Reusable services and tools for cross-platform apps made with Uno Platform and WinUI.

## Features

### Services

- **Preferences Service** (`IPreferences`): Store and retrieve application settings with support for both simple and complex types (serialized as JSON)
- **Dialog Coordinator** (`IDialogCoordinator`): Coordinate ContentDialog display to ensure only one dialog is shown at a time, preventing WinUI exceptions

### Extensions

- **Package Version Extensions**: Helper extensions for formatting `PackageVersion` objects to string representations

### Infrastructure

- **XamlRoot Provider** (`IXamlRootProvider`): Provides centralized access to the XamlRoot instance for dialogs and popups

## Sample Application

Check out the [Sample Gallery App](samples/MZikmund.Toolkit.Sample) to see interactive demos of all toolkit features.

The sample app is a gallery-style application similar to WinUI Gallery that demonstrates each feature with:
- Interactive examples
- Code samples
- Detailed descriptions

## Installation

```bash
dotnet add package MZikmund.Toolkit.WinUI
```

## Usage Examples

### Preferences Service

```csharp
using MZikmund.Toolkit.WinUI.Services;

// Initialize
var preferences = new Preferences();

// Simple preference
preferences.Set("username", "John");
var username = preferences.Get("username", string.Empty);

// Complex preference
var person = new Person { Name = "John", Age = 30 };
preferences.SetComplex("user", person);
var savedPerson = preferences.GetComplex("user", new Person());
```

### Dialog Coordinator

```csharp
using MZikmund.Toolkit.WinUI.Services;

// Initialize
var coordinator = new DialogCoordinator();

// Show a dialog - it will be queued if another is showing
var dialog = new ContentDialog
{
    Title = "Example Dialog",
    Content = "This is an example",
    PrimaryButtonText = "OK",
    XamlRoot = this.XamlRoot
};
var result = await coordinator.ShowAsync(dialog);
```

### Package Version Extensions

```csharp
using MZikmund.Toolkit.WinUI.Extensions;

var package = Package.Current;
var version = package.Id.Version;

// Format full version (e.g., "1.2.3.4")
string fullVersion = version.ToVersionString();

// Format short version (e.g., "1.2")
string shortVersion = version.ToVersionString(majorMinorOnly: true);
```

## Platform Support

- Windows (WinAppSDK)
- Android
- iOS
- macCatalyst
- WebAssembly
- Linux/macOS (Skia Desktop)

## Building

```bash
# Restore workloads
dotnet workload restore

# Build
dotnet build

# Run tests (if available)
dotnet test
```

## License

See [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.