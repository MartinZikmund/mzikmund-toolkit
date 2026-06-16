# Copilot Instructions for MZikmund.Toolkit

## Repository Overview

This is a **cross-platform .NET library** that provides reusable services and tools for applications built with **Uno Platform** and **WinUI 3**. The toolkit is designed to work across multiple platforms including Windows, Android, iOS, macOS (Catalyst), WebAssembly (WASM), and Desktop (Skia).

**Repository Stats:**
- Primary Language: C# (.NET 10)
- Project Type: NuGet library package
- Framework: Uno.Sdk 6.4.31 (latest stable)
- Target Frameworks: `net10.0`, `net10.0-ios`, `net10.0-maccatalyst`, `net10.0-android`, `net10.0-windows10.0.19041`, `net10.0-browserwasm`, `net10.0-desktop`
- Single Project: `MZikmund.Toolkit.WinUI` (library only, no test projects)

## Build Instructions

### Prerequisites

**CRITICAL:** This project MUST be built with **.NET 10 SDK** (specifically 10.0.100 or compatible) as specified in `src/global.json`.

### Build Steps (ALWAYS FOLLOW THIS ORDER)

1. **Navigate to the src directory:**
   ```bash
   cd src
   ```

2. **Restore workloads (Required - First Time or After SDK Change):**
   ```bash
   dotnet workload restore MZikmund.Toolkit.sln
   ```
   - Takes 60-120 seconds
   - Installs Android workload (required for `net10.0-android` target)
   - Installs WASM tools (required for `net10.0-browserwasm` target)
   - **Important:** Run this from the `src` directory where `global.json` is located to ensure .NET 10 SDK is used
   - **Note:** iOS/macOS/Windows targets require their respective operating systems. On Linux, only `net10.0`, `net10.0-android`, `net10.0-browserwasm`, and `net10.0-desktop` targets build. The CI workflow uses Windows to support all targets.

3. **Restore NuGet dependencies:**
   ```bash
   dotnet restore MZikmund.Toolkit.sln
   ```
   - Takes 5-10 seconds
   - Takes 5-10 seconds

4. **Build the solution:**
   ```bash
   dotnet build MZikmund.Toolkit.sln
   ```
   - Takes 10-15 seconds
   - Builds all target frameworks
   - Generates NuGet packages (.nupkg and .snupkg) in `MZikmund.Toolkit.WinUI/bin/Debug/`
   - **Expected Warnings:** XML documentation warnings (CS1570) in `IPreferences.cs` and `Preferences.cs` - these are existing issues in the codebase

5. **Clean the solution (when needed):**
   ```bash
   dotnet clean MZikmund.Toolkit.sln
   ```

### Common Build Issues and Workarounds

**Issue 1: SDK Version Mismatch**
- **Symptom:** Build fails with workload or SDK errors
- **Cause:** Using incorrect SDK version
- **Solution:** Always run commands from the `src` directory where `global.json` is located. The `global.json` file pins the SDK to 10.0.100

**Issue 2: NETSDK1147 Error - Workloads not installed**
- **Symptom:** Build fails with "To build this project, the following workloads must be installed: android"
- **Cause:** Workloads not installed for the current SDK version
- **Solution:** Run `dotnet workload restore MZikmund.Toolkit.sln` from the `src` directory

**Issue 3: Build warnings about XML comments**
- **Symptom:** CS1570 warnings about badly formed XML comments
- **Cause:** Extra closing `</summary>` tags in XML documentation
- **Solution:** These are existing issues. Your changes should not introduce new XML documentation warnings

## Testing

**No test projects exist in this repository.** The CI workflow has test steps commented out. Do not add test infrastructure unless specifically requested.

## GitHub Workflows and CI

The repository has two GitHub Actions workflows:

### 1. Build and Deploy (.github/workflows/dotnet.yml)

**Runs on:** Push to `main` and Pull Requests to `main`  
**Runner:** `windows-latest` (required for full Windows SDK support)

**Build Steps (in order):**
1. Checkout with full git history (`fetch-depth: 0`)
2. Setup .NET 10.0.x SDK
3. Install GitVersion 5.x (for versioning)
4. Determine version using GitVersion with `gitversion.yml` config
5. Restore workloads: `dotnet workload restore src/MZikmund.Toolkit.sln`
6. Restore dependencies: `dotnet restore src/MZikmund.Toolkit.sln`
7. Build: `dotnet build --no-restore src/MZikmund.Toolkit.sln /p:PackageVersion={version}`
8. Upload NuGet packages as artifacts

**Important Notes:**
- Test and Pack steps are commented out
- Publishing to NuGet only happens on `main` branch pushes
- Build must succeed without errors for PR to be accepted
- XML documentation warnings are acceptable (but don't add new ones)

### 2. TODO to Issue (.github/workflows/todo.yml)

- Automatically converts TODO comments in code to GitHub issues
- Runs on pushes to `main` branch

## Project Structure

```
/
├── .github/
│   ├── workflows/
│   │   ├── dotnet.yml          # Main CI/CD pipeline
│   │   └── todo.yml            # TODO to Issue automation
│   ├── dependabot.yml          # Dependency updates config
│   └── FUNDING.yml             # GitHub Sponsors info
├── src/
│   ├── Directory.Build.props    # Shared MSBuild properties
│   ├── Directory.Build.targets  # Shared MSBuild targets (empty)
│   ├── Directory.Packages.props # Central Package Management
│   ├── global.json              # CRITICAL: Pins SDK to 10.0.100
│   ├── MZikmund.Toolkit.sln     # Solution file
│   └── MZikmund.Toolkit.WinUI/  # Main library project
│       ├── MZikmund.Toolkit.WinUI.csproj  # Project file (uses Uno.Sdk)
│       ├── Extensions/
│       │   └── PackageVersionExtensions.cs
│       ├── Infrastructure/
│       │   └── IXamlRootProvider.cs     # Provides XamlRoot access
│       └── Services/
│           ├── IPreferences.cs           # Preferences interface
│           ├── Preferences.cs            # Preferences implementation
│           └── Dialogs/
│               ├── IDialogCoordinator.cs  # Dialog coordination interface
│               ├── DialogCoordinator.cs   # Dialog coordinator implementation
│               └── QueuedDialog.cs        # Queued dialog helper
├── gitversion.yml               # GitVersion configuration
├── .gitignore                   # Standard Visual Studio gitignore
├── LICENSE                      # License file
└── README.md                    # Minimal README (just says "template")
```

## Architecture and Key Components

### Library Purpose
Provides cross-platform services and utilities for Uno Platform / WinUI apps:

1. **Preferences Service** (`Services/IPreferences.cs`, `Services/Preferences.cs`)
   - Handles application settings/preferences
   - Supports both simple and complex (JSON-serialized) settings
   - Generic type support for type-safe preference access

2. **Dialog Coordinator** (`Services/Dialogs/`)
   - Coordinates ContentDialog display to prevent WinUI exceptions
   - Ensures only one dialog shown at a time
   - Queue-based dialog management

3. **XamlRoot Provider** (`Infrastructure/IXamlRootProvider.cs`)
   - Provides access to the XamlRoot instance for UI operations

### Build Configuration

**Directory.Build.props** sets:
- `ImplicitUsings`: enabled
- `Nullable`: enabled
- `ManagePackageVersionsCentrally`: true (Central Package Management)
- Suppressed warnings: NU1507, NETSDK1201, PRI257

**Package References:**
- `Microsoft.SourceLink.GitHub` (version 8.0.0) - for source linking in NuGet packages

### NuGet Package Configuration

The project generates NuGet packages on every build:
- Package ID: `MZikmund.Toolkit.WinUI`
- Author/Company: Martin Zikmund
- License: See LICENSE file in repository root
- Generates both regular (.nupkg) and symbol (.snupkg) packages
- Includes XML documentation

## Coding Conventions

- **Nullable Reference Types:** Enabled - use nullable annotations (`?`, `[MaybeNullWhen]`)
- **Implicit Usings:** Enabled - common namespaces automatically imported
- **XML Documentation:** Required for public APIs - use `<summary>`, `<param>`, `<returns>`, `<typeparam>` tags
  - **Warning:** Do NOT add extra closing `</summary>` tags (causes CS1570 warnings)
- **Namespace Style:** File-scoped namespaces preferred
- **Code Style:** Follow standard C# conventions

## Version Management

This project uses **GitVersion** with Mainline mode:
- **Main branch:** Tagged as `dev` with patch increment
- **Pull Requests:** Tagged as `PullRequest` with no increment
- **Stable branches:** Use `release/stable/*` pattern (note: gitversion.yml references 'master' as source, but actual branch is 'main')
- **Dev branches:** Use `dev/*` pattern with custom tag

Configuration in `gitversion.yml` at repository root.

## Dependencies

**NuGet Packages:** Managed centrally via `Directory.Packages.props`
- Microsoft.SourceLink.GitHub 8.0.0

**SDK Dependencies:**
- Uno.Sdk 6.4.31 (latest stable, specified in `global.json` under `msbuild-sdks` section)
- .NET 10 SDK (10.0.100 or compatible)

**Workload Dependencies:**
- `android` workload (for Android target)
- `wasm-tools-net8` workload (for WebAssembly target)

## Dependabot Configuration

Two package ecosystems monitored:
- NuGet packages: Weekly updates at 04:00 UTC
- NPM packages: Weekly updates at 04:00 UTC (though no JS/TS code exists currently)
- Max 10 open PRs per ecosystem
- Target branch: `main`

## Important Notes for Code Changes

1. **Always work from the `src` directory** when running dotnet commands to ensure .NET 10 SDK is used
2. **Do not add test projects** unless specifically requested - this is a library-only repository
3. **Maintain cross-platform compatibility** - code must work on all target frameworks
4. **Follow XML documentation standards** - public APIs must be documented
5. **Do not modify `global.json`** - SDK version is intentionally pinned to 10.0.100
6. **Preserve NoWarn suppressions** in Directory.Build.props - they address known issues
7. **Package generation is automatic** - happens on every build via `GeneratePackageOnBuild`
8. **Trust these instructions** - only search for additional information if something is unclear or these instructions prove incorrect

## Quick Command Reference

```bash
# Full build from scratch (run from repository root)
cd src
dotnet workload restore MZikmund.Toolkit.sln
dotnet restore MZikmund.Toolkit.sln
dotnet build MZikmund.Toolkit.sln

# Incremental build (after initial setup)
cd src
dotnet build MZikmund.Toolkit.sln

# Clean build
cd src
dotnet clean MZikmund.Toolkit.sln
dotnet build MZikmund.Toolkit.sln

# Check installed workloads
cd src
dotnet workload list
```

## Summary

This is a small, focused cross-platform library for Uno Platform/WinUI apps. The build process is straightforward but requires .NET 10 SDK and proper workload installation. Always run commands from the `src` directory, follow the build order, and maintain cross-platform compatibility in any code changes.
