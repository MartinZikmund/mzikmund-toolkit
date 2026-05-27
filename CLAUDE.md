# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository layout

The repo uses a **single root solution** (`MZikmund.Toolkit.slnx`) that covers all three projects:

- `src/MZikmund.Toolkit.WinUI` — the published library (`MZikmund.Toolkit.WinUI`).
- `samples/MZikmund.Toolkit.Sample/MZikmund.Toolkit.Sample` — gallery sample app.
- `tests/MZikmund.Toolkit.WinUI.Tests` — unit tests for the library, built on `MSTest.Sdk` (Microsoft Testing Platform).

SDK versions are pinned in the root `global.json` (`Uno.Sdk` and `MSTest.Sdk`). The library and samples target Uno 6.x; bumping Uno is a single edit in the root `global.json`.

Each project tree (`src/`, `samples/`, `tests/`) keeps its own `Directory.Packages.props`. The tests tree does not enable central package management because `MSTest.Sdk` manages its own dependency versions.

## New features must ship with a sample (and tests)

Every new feature added to `src/MZikmund.Toolkit.WinUI` must come with:

- A corresponding page or example in `samples/MZikmund.Toolkit.Sample` — the sample app is the gallery users browse to discover what the toolkit does.
- Unit tests in `tests/MZikmund.Toolkit.WinUI.Tests` whenever the feature is testable without a UI host (extensions, services, helpers).

PRs that add a service/extension/infrastructure piece without sample coverage are incomplete.

## Build & verify

```powershell
# Restore workloads (one-time per session)
dotnet workload restore MZikmund.Toolkit.slnx

# Build everything
dotnet build MZikmund.Toolkit.slnx

# Run tests (executable from the test build output)
tests/MZikmund.Toolkit.WinUI.Tests/bin/Debug/net10.0/MZikmund.Toolkit.WinUI.Tests.exe
```

Run `/verify-build` to build and test in one shot.

The tests project uses `MSTest.Sdk` and the Microsoft Testing Platform runner. Because `dotnet test` against MTP on .NET 10 SDK requires the new test experience opt-in, prefer running the produced test executable directly. `dotnet test` against the test project will surface a VSTest-mode error.

## Gotchas Claude tends to get wrong

- **Central Package Management is on for `src/` and `samples/`.** Add or update versions in `src/Directory.Packages.props` or `samples/MZikmund.Toolkit.Sample/Directory.Packages.props` — never put `Version="..."` on a `<PackageReference>` in a csproj. The tests tree does not use CPM (MSTest.Sdk handles its own versions).
- **GitVersion owns the package version.** CI computes the package version from git history; the `<Version>0.0.1</Version>` in `src/MZikmund.Toolkit.WinUI/MZikmund.Toolkit.WinUI.csproj` is a placeholder. Do not bump it manually to "release" a version.
- **Uno Single Project.** `UnoSingleProject=true` means one csproj produces all platform heads (Android/iOS/macCatalyst/Windows/WASM/Desktop). Use `#if __ANDROID__`, `#if HAS_UNO`, etc. for platform-specific code rather than splitting projects. The `TargetFrameworks` list in the library csproj is the source of truth for supported platforms.

## Conventions

- **Commits & PRs:** Conventional Commits (`feat:`, `fix:`, `chore:`, `docs:`, `refactor:`, `test:`). GitVersion uses the history to compute the next package version.
- **Code style:** governed by `.editorconfig` — 4-space indent, file-scoped namespaces, `I`-prefixed interfaces, `System.*` usings first, CRLF line endings. Nullable is enabled repo-wide.

## CI

`.github/workflows/dotnet.yml` builds `MZikmund.Toolkit.slnx` on `windows-latest` against .NET 10 SDK, packs the library, and pushes to NuGet.org from `main` (uses `NUGETAPIKEY` secret). PRs build but don't publish.
