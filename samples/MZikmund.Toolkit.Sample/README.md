# MZikmund Toolkit Sample Gallery

This is a gallery application that demonstrates the features of the MZikmund.Toolkit.WinUI library. The app is built with Uno Platform and runs on all supported targets.

## Features Demonstrated

The gallery includes samples for the following toolkit features:

### Services

- **Preferences Service**: Store and retrieve application settings with support for simple and complex types
- **Dialog Coordinator**: Coordinate ContentDialog display to ensure only one dialog is shown at a time

### Extensions

- **Package Version Extensions**: Format PackageVersion objects for display

### Infrastructure

- **XamlRoot Provider**: Centralized access to XamlRoot for dialogs and popups

## Building and Running

### Prerequisites

- .NET 10 SDK
- Uno Platform Workloads (automatically installed via `dotnet workload restore`)

### Build

To build all targets:

```bash
dotnet build
```

To build a specific target:

```bash
# Desktop (Skia)
dotnet build -f net10.0-desktop

# WebAssembly
dotnet build -f net10.0-browserwasm

# Android
dotnet build -f net10.0-android

# Windows (Windows only)
dotnet build -f net10.0-windows10.0.19041

# iOS (macOS only)
dotnet build -f net10.0-ios

# macCatalyst (macOS only)
dotnet build -f net10.0-maccatalyst
```

### Run

To run the desktop version:

```bash
dotnet run -f net10.0-desktop
```

To run the WASM version:

```bash
dotnet run -f net10.0-browserwasm
```

## Architecture

The sample app uses a NavigationView-based shell to provide easy access to all feature samples. Each sample is on a separate page with:

- Description of the feature
- Interactive demo
- Code samples

## Project Structure

```
MZikmund.Toolkit.Sample/
├── Pages/                           # Sample pages
│   ├── HomePage.xaml               # Landing page
│   ├── PreferencesSamplePage.xaml  # Preferences service sample
│   ├── DialogCoordinatorSamplePage.xaml  # Dialog coordinator sample
│   ├── PackageVersionSamplePage.xaml     # Package version extensions sample
│   └── XamlRootProviderSamplePage.xaml   # XamlRoot provider sample
├── Shell.xaml                      # NavigationView shell
├── App.xaml                        # Application entry point
└── MZikmund.Toolkit.Sample.csproj  # Project file
```
